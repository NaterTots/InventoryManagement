using System;
using System.ServiceModel.DomainServices.Client.ApplicationServices;

namespace InventoryManagement.Common
{
    public class LogoutOperationEventArgs : ResultsArgs
    {
        public LogoutOperationEventArgs(Exception ex)
            : base(ex)
        {
            LogoutOp = null;
        }

        public LogoutOperationEventArgs(LogoutOperation logoutOp)
            : base(null)
        {
            LogoutOp = logoutOp;
        }

        public LogoutOperationEventArgs(LogoutOperation logoutOp, Exception ex)
            : base(ex)
        {
            LogoutOp = logoutOp;
        }

        public LogoutOperation LogoutOp { get; private set; }
    }
}
