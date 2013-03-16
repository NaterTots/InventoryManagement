using System;
using System.ServiceModel.DomainServices.Client.ApplicationServices;

namespace InventoryManagement.Common
{
    public class LoginOperationEventArgs : ResultsArgs
    {
        public LoginOperationEventArgs(Exception ex)
            : base(ex)
        {
            LoginOp = null;
        }

        public LoginOperationEventArgs(LoginOperation loginOp)
            : base(null)
        {
            LoginOp = loginOp;
        }

        public LoginOperationEventArgs(LoginOperation loginOp, Exception ex)
            : base(ex)
        {
            LoginOp = loginOp;
        }

        public LoginOperation LoginOp { get; private set; }
    }
}
