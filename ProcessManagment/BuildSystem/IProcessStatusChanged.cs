using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagment.BuildSystem
{
    /// <summary>
    /// External system must implement this interface in order to be notificated then process changes lifecycle stage 
    /// </summary>
    public interface IProcessStatusChanged
    {
        /// <summary>
        /// Called then process changes lifecycle stage
        /// </summary>
        /// <param name="processResult"></param>
        Task StatusChanged(ProcessResult processResult);

        //Task StatusChangedAsync(ProcessResult processResult);
    }
}
