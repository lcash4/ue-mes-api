using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo.Interface
{
    public interface ISupplierData
    {
        /// <summary>
        /// Simply return a supplier by the supplied ID
        /// </summary>
        /// <param name="supplierId">ID of the supplier to search</param>
        /// <returns></returns>
        public Supplier GetSupplierById(int supplierId);

        /// <summary>
        /// Simply return a supplier by the supplied name
        /// </summary>
        /// <param name="supplierName">name of the supplier to search</param>
        /// <returns></returns>
        public Supplier GetSupplierByName(string supplierName);

        /// <summary>
        /// Simply return all suppliers for use in a client list or dropdown
        /// </summary>
        /// <param name="includeInactive">Only include inactive when set to true.  Defaults to false</param>
        /// <returns></returns>
        public List<Supplier> GetAllSuppliers(bool includeInactive = false);

        /// <summary>
        /// Add a new supplier
        /// </summary>
        /// <param name="supplier"></param>
        public void AddSupplier(Supplier supplier);
    }
}
