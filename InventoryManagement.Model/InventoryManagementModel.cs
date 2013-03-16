using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ServiceModel.DomainServices.Client;
using InventoryManagement.Common;
using InventoryManagement.Data.Web;

namespace InventoryManagement.Model
{
    [Export(typeof(IInventoryManagementModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class InventoryManagementModel : IInventoryManagementModel
    {
        #region Private Members
        private InventoryManagementContext _ctx;
        #endregion Private Members

        #region Protected

        protected InventoryManagementContext Context
        {
            get
            {
                if (_ctx == null)
                {
                    _ctx = new InventoryManagementContext();
                    _ctx.PropertyChanged += _ctx_PropertyChanged;
                }

                return _ctx;
            }
        }

        #endregion

        #region IInventoryManagementModel

        public void GetCommodityTypesAsync()
        {
            PerformQuery(Context.GetCommodityTypesQuery(), GetCommodityTypesComplete);
        }

        public void GetVendorsAsync()
        {
            PerformQuery(Context.GetVendorsQuery(), GetVendorsComplete);
        }

        public void GetUnitOfMeasuresAsync()
        {
            PerformQuery(Context.GetUnitOfMeasuresQuery(), GetUnitOfMeasuresComplete);
        }

        public void GetCommoditiesAsync()
        {
            PerformQuery(Context.GetCommoditiesQuery(), GetCommoditiesComplete);
        }

        public void GetWorkOrdersAsync()
        {
            PerformQuery(Context.GetAllWorkOrdersQuery(), GetWorkOrdersComplete);
        }

        public void GetOpenWorkOrdersAsync()
        {
            PerformQuery(Context.GetOpenWorkOrdersQuery(), GetOpenWorkOrdersComplete);
        }

        public void GetMeansOfPaymentsAsync()
        {
            PerformQuery(Context.GetMeansOfPaymentsQuery(), GetMeansOfPaymentsComplete);
        }

        public Commodity AddNewCommodity()
        {
            var g = new Commodity
            {
                InventoryID = 0,
                PartNumber = string.Empty,
                InventoryType = 0,
                PartDescription = string.Empty,
                UnitOfMeasureID = 0,
                ReorderLevel = 0,
                Vendor = 0,
                Discontinued = false
            };
            Context.Commodities.Add(g);
            return g;
        }

        public WorkOrder AddNewWorkOrder()
        {
            throw new NotImplementedException();
        }

        public Vendor AddNewVendor()
        {
            var g = new Vendor
            {
                VendorID = 0,
                VendorName = string.Empty,
                //TODO: more?
            };
            Context.Vendors.Add(g);
            return g;
        }

        public User AddNewUser()
        {
            throw new NotImplementedException();
        }

        public void RemoveUser(User user)
        {
            throw new NotImplementedException();
        }

        public void SaveChangesAsync()
        {
            Context.SubmitChanges(s =>
            {
                if (SaveChangesComplete != null)
                {
                    try
                    {
                        Exception ex = null;
                        if (s.HasError)
                        {
                            ex = s.Error;
                            s.MarkErrorAsHandled();
                        }
                        SaveChangesComplete(this, new SubmitOperationEventArgs(s, ex));
                    }
                    catch (Exception ex)
                    {
                        SaveChangesComplete(this, new SubmitOperationEventArgs(ex));
                    }
                }
            }, null);
        }

        public void RejectChanges()
        {
            Context.RejectChanges();
        }

        private bool _hasChanges;

        public bool HasChanges
        {
            get
            {
                return _hasChanges;
            }

            private set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged("HasChanges");
                }
            }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            private set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
        }

        public event EventHandler<DataObjectResultsArgs<CommodityType>> GetCommodityTypesComplete;
        public event EventHandler<DataObjectResultsArgs<Vendor>> GetVendorsComplete;
        public event EventHandler<DataObjectResultsArgs<UnitOfMeasure>> GetUnitOfMeasuresComplete;
        public event EventHandler<DataObjectResultsArgs<Commodity>> GetCommoditiesComplete;
        public event EventHandler<DataObjectResultsArgs<WorkOrder>> GetWorkOrdersComplete;
        public event EventHandler<DataObjectResultsArgs<WorkOrder>> GetOpenWorkOrdersComplete;
        public event EventHandler<DataObjectResultsArgs<MeansOfPayment>> GetMeansOfPaymentsComplete;
        public event EventHandler<SubmitOperationEventArgs> SaveChangesComplete;

        #endregion IInventoryManagementModel

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged

        #region Private Methods

        private void _ctx_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "HasChanges":
                    HasChanges = _ctx.HasChanges;
                    break;
                case "IsLoading":
                    IsBusy = _ctx.IsLoading;
                    break;
                case "IsSubmitting":
                    IsBusy = _ctx.IsSubmitting;
                    break;
            }
        }

        private void PerformQuery<T>(EntityQuery<T> qry, EventHandler<DataObjectResultsArgs<T>> evt) where T : Entity
        {

            Context.Load(qry, LoadBehavior.RefreshCurrent, r =>
            {
                if (evt != null)
                {
                    try
                    {
                        if (r.HasError)
                        {
                            evt(this, new DataObjectResultsArgs<T>(r.Error));
                            r.MarkErrorAsHandled();
                        }
                        else
                        {
                            evt(this, new DataObjectResultsArgs<T>(r.Entities));
                        }
                    }
                    catch (Exception ex)
                    {
                        evt(this, new DataObjectResultsArgs<T>(ex));
                    }
                }
            }, null);
        }

        #endregion Private Methods
    }
}
