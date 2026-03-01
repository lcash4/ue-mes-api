using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.Global;

namespace ue_mes_data.Global.Interface
{
    public interface IProcessData
    {
        /// <summary>
        /// Return all production processes for the factory
        /// </summary>
        /// <returns></returns>
        public List<Process> GetProcesses();

        /// <summary>
        /// Return all production processes that are not already included in the provided bill of process
        /// </summary>
        /// <param name="localSiteName">The local site to filter to.  Not all processes exist in every site.</param>
        /// <param name="billOfProcessId">Bill of Process ID to filter</param>
        /// <returns></returns>
        public List<Process> GetProcessesNotInBillOfProcessForLocalSite(string localSiteName, int billOfProcessId);
    }
}
