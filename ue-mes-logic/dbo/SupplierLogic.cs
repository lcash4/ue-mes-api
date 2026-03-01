using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_logic.dbo
{
    public class SupplierLogic : LogicBase
    {
        ISupplierData SupplierData { get; }

        #region Constructor

        public SupplierLogic(ISupplierData supplierData)
        {
            SupplierData = supplierData ?? throw new ArgumentNullException(nameof(supplierData));
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
            return SupplierData.GetSupplierById(supplierId);
        }

        /// <summary>
        /// Simply return a supplier by the supplied name
        /// </summary>
        /// <param name="supplierName">name of the supplier to search</param>
        /// <returns></returns>
        public Supplier GetSupplierByName(string supplierName)
        {
            return SupplierData.GetSupplierByName(supplierName);
        }

        /// <summary>
        /// Simply return all suppliers for use in a client list or dropdown
        /// </summary>
        /// <param name="includeInactive">Only include inactive when set to true.  Defaults to false</param>
        /// <returns></returns>
        public List<Supplier> GetAllSuppliers(bool includeInactive = false)
        {
            return SupplierData.GetAllSuppliers(includeInactive);
        }

        /// <summary>
        /// Add a new supplier
        /// </summary>
        /// <param name="supplier"></param>
        public void AddSupplier(Supplier supplier)
        {
            // Need to check if the inventory item already exists.  If so, return "Inventory for part {part number and revision} and serial number {serialNumber} at location {location} already exists" exception
            var existingSupplier = SupplierData.GetSupplierByName(supplier.Name);
            if (existingSupplier != null)
                throw new InvalidDataException(string.Format("Supplier with name {0} already exists", supplier.Name));

            SupplierData.AddSupplier(supplier);
        }

        #endregion
    }
}
