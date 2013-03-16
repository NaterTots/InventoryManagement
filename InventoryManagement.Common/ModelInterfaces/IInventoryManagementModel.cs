using System;
using System.ComponentModel;
using InventoryManagement.Data;
using InventoryManagement.Data.Web;

namespace InventoryManagement.Common
{
    public interface IInventoryManagementModel : INotifyPropertyChanged
    {
        void GetCommodityTypesAsync();
        event EventHandler<DataObjectResultsArgs<CommodityType>> GetCommodityTypesComplete;
        void GetVendorsAsync();
        event EventHandler<DataObjectResultsArgs<Vendor>> GetVendorsComplete;
        void GetUnitOfMeasuresAsync();
        event EventHandler<DataObjectResultsArgs<UnitOfMeasure>> GetUnitOfMeasuresComplete;
        void GetCommoditiesAsync();
        event EventHandler<DataObjectResultsArgs<Commodity>> GetCommoditiesComplete;
        void GetWorkOrdersAsync();
        event EventHandler<DataObjectResultsArgs<WorkOrder>> GetWorkOrdersComplete;
        void GetOpenWorkOrdersAsync();
        event EventHandler<DataObjectResultsArgs<WorkOrder>> GetOpenWorkOrdersComplete;
        void GetMeansOfPaymentsAsync();
        event EventHandler<DataObjectResultsArgs<MeansOfPayment>> GetMeansOfPaymentsComplete;

        Commodity AddNewCommodity();
        WorkOrder AddNewWorkOrder();
        Vendor AddNewVendor();

        User AddNewUser();
        void RemoveUser(User user);

        void SaveChangesAsync();
        event EventHandler<SubmitOperationEventArgs> SaveChangesComplete;
        void RejectChanges();

        Boolean HasChanges { get; }
        Boolean IsBusy { get; }
    }
}