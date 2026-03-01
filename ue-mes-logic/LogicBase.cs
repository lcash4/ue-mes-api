using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_logic
{
    public class LogicBase : IDisposable
    {
        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);  // see Code Analysis CA1816
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        /// <remarks></remarks>
        protected virtual void Dispose(bool disposing)
        {

        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the <see cref="LogicBase"/> is reclaimed by garbage collection.
        /// </summary>
        /// <remarks></remarks>
        ~LogicBase()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        #endregion
    }
}
