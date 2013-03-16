using System;
using System.Collections.Generic;
using System.ServiceModel.DomainServices.Client;

namespace InventoryManagement.Common
{
    public class EntityResultsArgs<T> : ResultsArgs where T : Entity
    {
        public EntityResultsArgs(Exception ex)
            : base(ex)
        {
        }

        public EntityResultsArgs(IEnumerable<T> results)
            : base(null)
        {
            Results = results;
        }

        public IEnumerable<T> Results { get; private set; }
    }
}
