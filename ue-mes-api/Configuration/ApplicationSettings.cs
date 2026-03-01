using Contrib.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ue_mes_api.Configuration
{
    public static class ApplicationSettings
    {
        public static void AddSettings(this IServiceCollection services)
        {
            // Uses AutoBind and SubstituteVariables from https://github.com/drwatson1/configuration-extensions project
            services.AddOptions<Settings.Products>()
                .AutoBind()
                .SubstituteVariables();

            services.AddOptions<Settings.WorkElements>()
                .AutoBind()
                .SubstituteVariables();

            services.AddOptions<Settings.AuthenticationSettings>()
                .AutoBind()
                .SubstituteVariables();
        }
    }
}
