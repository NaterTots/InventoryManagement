using System;
using System.Collections.Generic;
using System.ServiceModel.DomainServices.Client;

using InventoryManagement.Data.Web;

namespace InventoryManagement.Common
{
    public class DataObjectResultsArgs<T> : ResultsArgs //where T : DataObject
    {
        public DataObjectResultsArgs(Exception ex)
            : base(ex)
        {
        }

        public DataObjectResultsArgs(IEnumerable<T> results)
            : base(null)
        {
            Results = results;
        }

        public IEnumerable<T> Results { get; private set; }
    }
}
