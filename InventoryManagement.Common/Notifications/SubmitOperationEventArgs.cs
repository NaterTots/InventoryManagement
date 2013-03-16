using System;
using System.ServiceModel.DomainServices.Client;

namespace InventoryManagement.Common
{
    public class SubmitOperationEventArgs : ResultsArgs
    {
        public SubmitOperationEventArgs(Exception ex)
            : base(ex)
        {
            SubmitOp = null;
        }

        public SubmitOperationEventArgs(SubmitOperation submitOp)
            : base(null)
        {
            SubmitOp = submitOp;
        }

        public SubmitOperationEventArgs(SubmitOperation submitOp, Exception ex)
            : base(ex)
        {
            SubmitOp = submitOp;
        }

        public SubmitOperation SubmitOp { get; private set; }
    }
}