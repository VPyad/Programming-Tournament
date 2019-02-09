using System;
using System.Collections.Generic;
using System.Text;

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
        void StatusChanged(ProcessResult processResult);
    }
}
