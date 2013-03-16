using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.ServiceModel.DomainServices.EntityFramework;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;

using NHibernate;
using NHibernate.Criterion;

namespace InventoryManagement.Data.Web
{
    [EnableClientAccess]
    public class InventoryManagementService : DomainService
    {
        #region Commodity Types

        public IEnumerable<CommodityType> GetCommodityTypes()
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                return session.CreateCriteria(typeof(CommodityType))
                    .List<CommodityType>().OrderBy(g => g.TypeCode);
            }
        }

        [Insert]
        public void InsertCommodityType(CommodityType commodityType)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                session.SaveOrUpdate(commodityType);
                session.Flush();
            }
        }

        [Update]
        public void UpdateCommodityType(CommodityType commodityType)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                session.SaveOrUpdate(commodityType);
                session.Flush();
            }
        }

        [Delete]
        public void DeleteCommodityType(CommodityType commodityType)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                session.Delete(commodityType);
                session.Flush();
            }
        }

        #endregion Commodity Types

        #region Commodities

        public IEnumerable<Commodity> GetCommodities()
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                return session.CreateCriteria(typeof(Commodity))
                    .List<Commodity>().OrderBy(g => g.PartNumber);
            }
        }

        [Insert]
        public void InsertCommodity(Commodity commodity)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                session.SaveOrUpdate(commodity);
                session.Flush();
            }
        }

        [Update]
        public void UpdateCommodity(Commodity commodity)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                session.Update(commodity);
                session.Flush();
            }
        }

        [Delete]
        public void DeleteCommodity(Commodity commodity)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                session.Delete(commodity);
                session.Flush();
            }
        }

        #endregion Commodities

        #region MeansOfPayment

        public IEnumerable<MeansOfPayment> GetMeansOfPayments()
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                return session.CreateCriteria(typeof(MeansOfPayment))
                    .List<MeansOfPayment>().OrderBy(g => g.PaymentType);
            }
        }

        [Insert]
        public void InsertMeansOfPayment(MeansOfPayment mop)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                session.SaveOrUpdate(mop);
                session.Flush();
            }
        }

        [Update]
        public void UpdateMeansOfPayment(MeansOfPayment mop)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                session.Update(mop);
                session.Flush();
            }
        }

        [Delete]
        public void DeleteMeansOfPayment(MeansOfPayment mop)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                session.Delete(mop);
                session.Flush();
            }
        }

        #endregion MeansOfPayment

        #region UnitOfMeasure

        public IEnumerable<UnitOfMeasure> GetUnitOfMeasures()
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                return session.CreateCriteria(typeof(UnitOfMeasure))
                    .List<UnitOfMeasure>().OrderBy(g => g.UnitOfMeasureTag);
            }
        }

        [Insert]
        public void InsertUnitOfMeasure(UnitOfMeasure uom)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                session.SaveOrUpdate(uom);
                session.Flush();
            }
        }

        [Update]
        public void UpdateUnitOfMeasure(UnitOfMeasure uom)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                session.Update(uom);
                session.Flush();
            }
        }

        [Delete]
        public void DeleteUnitOfMeasure(UnitOfMeasure uom)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                session.Delete(uom);
                session.Flush();
            }
        }

        #endregion UnitOfMeasure

        #region Users

        public User GetUser(string userName)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                return session.CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("UserName", userName))
                    .UniqueResult<User>();
            }
        }

        [Insert]
        public void InsertUser(User user)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                session.SaveOrUpdate(user);
                session.Flush();
            }
        }

        #endregion Users

        #region Vendors

        public IEnumerable<Vendor> GetVendors()
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                return session.CreateCriteria(typeof(Vendor))
                    .List<Vendor>().OrderBy(g => g.VendorName);
            }
        }

        [Insert]
        public void InsertVendor(Vendor vendor)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                session.SaveOrUpdate(vendor);
                session.Flush();
            }
        }

        [Update]
        public void UpdateVendor(Vendor vendor)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                session.SaveOrUpdate(vendor);
                session.Flush();
            }
        }

        [Delete]
        public void DeleteVendor(Vendor vendor)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                session.Delete(vendor);
                session.Flush();
            }
        }

        #endregion Vendors

        #region Work Orders

        public IEnumerable<WorkOrder> GetAllWorkOrders()
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                return session.CreateCriteria(typeof(WorkOrder))
                    .List<WorkOrder>().OrderBy(g => g.JobID);
            }
        }

        public IEnumerable<WorkOrder> GetOpenWorkOrders()
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                return session.CreateCriteria(typeof(WorkOrder))
                    .Add(Restrictions.Eq("JobStatus", (int)WorkOrder.WorkOrderStatus.Open))
                    .List<WorkOrder>().OrderBy(g => g.JobID);
            }
        }

        [Insert]
        public void InsertWorkOrder(WorkOrder workOrder)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                session.SaveOrUpdate(workOrder);
                session.Flush();
            }
        }

        [Update]
        public void UpdateWorkOrder(WorkOrder workOrder)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                session.SaveOrUpdate(workOrder);
                session.Flush();
            }
        }

        [Delete]
        public void DeleteWorkOrder(WorkOrder workOrder)
        {
            using (ISession session = HibernateProvider.Factory.OpenSession())
            {
                session.Delete(workOrder);
                session.Flush();
            }
        }
        
        #endregion Work Orders
    }
}

        