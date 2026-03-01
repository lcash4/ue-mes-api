using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo
{
    public class SupplierData : DbContextBase, ISupplierData
    {
        #region AutoMapper Config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.Supplier, Supplier>()
                .ReverseMap();
        });
        private IMapper mapper;

        #endregion

        #region Constructor
        public SupplierData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// Simply return a supplier by the supplied ID
        /// </summary>
        /// <param name="supplierId">ID of the supplier to search</param>
        /// <returns></returns>
        public Supplier GetSupplierById(int supplierId)
        {
            Supplier supplier = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var supplierRecord = mesProductionContext.Suppliers
                    .Where(supplier => supplier.SupplierId == supplierId)
                    .FirstOrDefault();

                if (supplierRecord != null)
                {
                    supplier = mapper.Map<Model.Supplier, Supplier>(supplierRecord);
                }
            }
            return supplier;
        }

        /// <summary>
        /// Simply return a supplier by the supplied name
        /// </summary>
        /// <param name="supplierName">name of the supplier to search</param>
        /// <returns></returns>
        public Supplier GetSupplierByName(string supplierName)
        {
            Supplier supplier = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var supplierRecord = mesProductionContext.Suppliers
                    .Where(supplier => supplier.Name == supplierName)
                    .FirstOrDefault();

                if (supplierRecord != null)
                {
                    supplier = mapper.Map<Model.Supplier, Supplier>(supplierRecord);
                }
            }
            return supplier;
        }

        /// <summary>
        /// Simply return all suppliers for use in a client list or dropdown
        /// </summary>
        /// <param name="includeInactive">Only include inactive when set to true.  Defaults to false</param>
        /// <returns></returns>
        public List<Supplier> GetAllSuppliers(bool includeInactive = false)
        {
            List<Supplier> suppliers = new List<Supplier>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var supplierRecords = mesProductionContext.Suppliers
                    .Where (supplier => supplier.IsActive || (!supplier.IsActive && includeInactive))
                    .OrderBy(supplier => supplier.Name).ToList();

                if (supplierRecords != null)
                {
                    suppliers = mapper.Map<List<Model.Supplier>, List<Supplier>>(supplierRecords);
                }
            }
            return suppliers;
        }

        /// <summary>
        /// Add a new supplier
        /// </summary>
        /// <param name="supplier"></param>
        public void AddSupplier(Supplier supplier)
        {
            var supplierRecord = mapper.Map<Model.Supplier>(supplier);
            if (supplierRecord != null)
            {
                // Set the Last Modified By if present
                supplierRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(supplier.LastModifiedBy) ? supplier.LastModifiedBy : null;

                using (var mesProductionContext = new Model.MesProductionContext())
                {
                    mesProductionContext.Suppliers.Add(supplierRecord);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        #endregion
    }
}
