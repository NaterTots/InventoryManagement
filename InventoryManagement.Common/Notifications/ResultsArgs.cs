using System;

namespace InventoryManagement.Common
{
    public class ResultsArgs : EventArgs
    {
        public ResultsArgs(Exception ex)
        {
            Error = ex;
        }

        public Exception Error { get; private set; }

        public bool HasError
        {
            get { return Error != null; }
        }
    }
}
