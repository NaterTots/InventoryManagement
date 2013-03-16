using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InventoryManagement.Data.Web;

using NHibernate;
using NHibernate.Criterion;

namespace InventoryManagement.Test
{
    public class Program
    {
        static void Main()
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                foreach (Commodity ct in session.CreateCriteria(typeof(Commodity))
                    .List<Commodity>().OrderBy(g => g.PartNumber))
                {
                    Console.WriteLine(string.Format("{0} ({1}): {2}", ct.PartNumber, ct.InventoryID, ct.PartDescription));
                }
            }

            Console.Read();
        }
    }
}