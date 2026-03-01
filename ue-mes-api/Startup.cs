using Autofac;
using Autofac.Configuration;
using Autofac.Core;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ue_mes_api.Configuration;
using ue_mes_api.Filters;
using ue_mes_api.Middleware;
using ue_mes_api.Modules;
using ue_mes_api.SignalR;
using ue_mes_data.Authorization;
using ue_mes_data.Authorization.Interface;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_data.Global;
using ue_mes_data.Global.Interface;

/*
 * This project have been originally created from ASP.Net Core RESTful Service Template.
 * Getting started guide: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki/Getting-Started-Guide
 * More information about configuring project: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki
 */
namespace ue_mes_api
{
    public class Startup
    {
        IConfiguration Configuration { get; }
        IHostEnvironment HostEnvironment { get; }

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            HostEnvironment = env;

            // See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#content-formatting
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Include,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = env.IsDevelopment() ? Formatting.Indented : Formatting.None
                };
                settings.Converters.Add(new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy()});
                return settings;
            };
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors()
                // Add useful interface for accessing the ActionContext outside a controller.
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                // Add useful interface for accessing the HttpContext outside a controller.
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                // Add useful interface for accessing the IUrlHelper outside a controller.
                .AddScoped<IUrlHelper>(x => x
                    .GetRequiredService<IUrlHelperFactory>()
                    .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext))
                .AddMvcCore(options =>
                {
                    options.Filters.Add(new ValidateModelFilter()); // Validate model. See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#model-validation
                    options.Filters.Add(new CacheControlFilter());  // Add "Cache-Control" header. See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#cache-control
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.WriteIndented = HostEnvironment.IsDevelopment();
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                })
                .AddApiExplorer();

            services
                .AddAutoMapper(typeof(Startup)) // Check out Configuration/AutoMapperProfiles/DefaultProfile to do actual configuration. See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#automapper
                .AddSwagger();                  // Check out Configuration/DependenciesConfig.cs/AddSwagger to do actual configuration. See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#documenting-api

            services.AddAuthentication()
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthenticationSettings:JwtSecret"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    cfg.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/orderItemUnit"))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddRouting();
            services.AddControllers(
                options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true
                );
            services.AddScoped<IAuthenticationMiddleware, AuthenticationMiddleware>();

            #region Data Layer Registration

            // Authorization schema
            services.AddSingleton<IRoleData, RoleData>();
            services.AddSingleton<IUserData, UserData>();

            // dbo schema
            services.AddSingleton<IBillOfMaterialData, BillOfMaterialData>();
            services.AddSingleton<IBillOfMaterialPartData, BillOfMaterialPartData>();
            services.AddSingleton<IBillOfProcessData, BillOfProcessData>();
            services.AddSingleton<IBillOfProcessProcessData, BillOfProcessProcessData>();
            services.AddSingleton<IBillOfProcessProcessWorkElementData, BillOfProcessProcessWorkElementData>();
            services.AddSingleton<IDefectCategoryData, DefectCategoryData>();
            services.AddSingleton<IDefectData, DefectData>();
            services.AddSingleton<IInventoryItemData, InventoryItemData>();
            services.AddSingleton<IInventoryItemScrapData, InventoryItemScrapData>();
            services.AddSingleton<IInventoryItemTypeData, InventoryItemTypeData>();
            services.AddSingleton<IInventoryLocationData, InventoryLocationData>();
            services.AddSingleton<IInventoryLocationTypeData, InventoryLocationTypeData>();
            services.AddSingleton<IItemAttributeTypeData, ItemAttributeTypeData>();
            services.AddSingleton<IOrderData, OrderData>();
            services.AddSingleton<IOrderItemData, OrderItemData>();
            services.AddSingleton<IOrderItemUnitConsumptionData, OrderItemUnitConsumptionData>();
            services.AddSingleton<IOrderItemUnitData, OrderItemUnitData>();
            services.AddSingleton<IOrderItemUnitDataCollectionData, OrderItemUnitDataCollectionData>();
            services.AddSingleton<IOrderItemUnitScrapData, OrderItemUnitScrapData>();
            services.AddSingleton<IOrderItemUnitWorkElementHistoryData, OrderItemUnitWorkElementHistoryData>();
            services.AddSingleton<IPartData, PartData>();
            services.AddSingleton<IPartItemAttributeTypeData, PartItemAttributeTypeData>();
            services.AddSingleton<ISupplierData, SupplierData>();
            services.AddSingleton<IUnitOfMeasureData, UnitOfMeasureData>();
            services.AddSingleton<IWorkElementStatusData, WorkElementStatusData>();
            services.AddSingleton<IWorkElementTypeAttributeData, WorkElementTypeAttributeData>();
            services.AddSingleton<IWorkElementTypeData, WorkElementTypeData>();

            // Global schema
            services.AddSingleton<ICoaterData, CoaterData>();
            services.AddSingleton<IEquipmentData, EquipmentData>();
            services.AddSingleton<IProcessData, ProcessData>();
            services.AddSingleton<ISiteData, SiteData>();

            #endregion

            services.AddHealthChecks();

            services.AddSignalR();
            services.AddSettings();
        }

        /// <summary>
        /// Configure Autofac DI-container
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <remarks>
        /// ConfigureContainer is where you can register things directly
        /// with Autofac. This runs after ConfigureServices so the things
        /// here will override registrations made in ConfigureServices.
        /// Don't build the container; that gets done for you.
        /// 
        /// See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#dependency-injection
        /// </remarks>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Add things to the Autofac ContainerBuilder.
            builder.RegisterModule<DefaultModule>();
            builder.RegisterModule(new ConfigurationModule(Configuration));
        }

        /// <summary>
        /// Configure Autofac DI-container for production
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <remarks>
        /// This only gets called if your environment is Production. The
        /// default ConfigureContainer won't be automatically called if this
        /// one is called.
        /// 
        /// See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#dependency-injection
        /// </remarks>
        public void ConfigureProductionContainer(ContainerBuilder builder)
        {
            ConfigureContainer(builder);

            // Add things to the ContainerBuilder that are only for the
            // production environment.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILogger<Startup> logger)
        {
            app.UseSerilogRequestLogging();

            // Use an exception handler middleware before any other handlers
            // See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#unhandled-exceptions-handling
            app.UseExceptionHandler();

            app.UseRouting();

            // See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#cross-origin-resource-sharing-cors-and-preflight-requests
            app.UseCors(builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                //.AllowAnyOrigin() // <-- Comment this line out and uncomment two lines below to use with SignalR
                .AllowCredentials()
                  .SetIsOriginAllowed(origin => true)
                );

            app.UseOptionsVerbHandler()    // Options verb handler must be added after CORS. See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#cross-origin-resource-sharing-cors-and-preflight-requests
               .UseSwaggerWithOptions();   // Check out Configuration/MiddlewareConfig.cs/UseSwaggerWithOptions to do actual configuration. See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#documenting-api

            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHealthChecks(Constants.Health.EndPoint); // See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#health-checks
                endpoints.MapHub<OrderItemUnitHub>("/hubs/orderItemUnit");
            });
            
            logger.LogInformation("Server configuration is completed");
            var addr = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(addr))
            {
                var uri = new Uri(new Uri(addr), "swagger");
                logger.LogInformation("Open {uri} to browse the server API", uri);
            }
        }
    }
}
