using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo.Interface
{
    public interface IWorkElementStatusData
    {
        /// <summary>
        /// Simply return all work element statuses for use in a client list or dropdown, or business logic
        /// </summary>
        /// <returns></returns>
        public List<WorkElementStatus> GetWorkElementStatuses();
    }
}
