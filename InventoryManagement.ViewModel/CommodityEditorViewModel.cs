using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using InventoryManagement.Data.Web;
using InventoryManagement.Common;

namespace InventoryManagement.ViewModel
{
    [Export(ViewModelTypes.CommodityEditorViewModel, typeof(ViewModelBase))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CommodityEditorViewModel : ViewModelBase
    {
        #region Private Data Members

        private IInventoryManagementModel _inventoryManagementModel;
        private Commodity _currentCommodityCache;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public CommodityEditorViewModel(IInventoryManagementModel inventoryManagementModel)
        {
            _inventoryManagementModel = inventoryManagementModel;

            //event handling
            _inventoryManagementModel.GetVendorsComplete += _inventoryManagementModel_GetVendorsComplete;
            _inventoryManagementModel.GetCommodityTypesComplete += _inventoryManagementModel_GetCommodityTypesComplete;
            _inventoryManagementModel.GetUnitOfMeasuresComplete += _inventoryManagementModel_GetUnitOfMeasuresComplete;

            _currentCommodityCache = null;

            VendorEntries = null;
            _inventoryManagementModel.GetVendorsAsync();
            CommodityTypeEntries = null;
            _inventoryManagementModel.GetCommodityTypesAsync();
            UnitOfMeasureEntries = null;
            _inventoryManagementModel.GetUnitOfMeasuresAsync();

            // register for EditCommodity
            AppMessages.EditCommodityMessage.Register(this, OnEditCommodityMessage);
        }

        #endregion

        #region Public Properties

        private Commodity _currentCommodity;

        public Commodity CurrentCommodity
        {
            get { return _currentCommodity; }
            private set
            {
                if (!ReferenceEquals(_currentCommodity, value))
                {
                    _currentCommodity = value;
                    RaisePropertyChanged("CurrentCommodity");
                }
            }
        }

        private IEnumerable<Vendor> _vendorEntries;

        public IEnumerable<Vendor> VendorEntries
        {
            get { return _vendorEntries; }
            private set
            {
                if (!ReferenceEquals(_vendorEntries, value))
                {
                    _vendorEntries = value;
                    RaisePropertyChanged("VendorEntries");
                }
            }
        }

        private IEnumerable<CommodityType> _commodityTypeEntries;

        public IEnumerable<CommodityType> CommodityTypeEntries
        {
            get { return _commodityTypeEntries; }
            private set
            {
                if (!ReferenceEquals(_commodityTypeEntries, value))
                {
                    _commodityTypeEntries = value;
                    RaisePropertyChanged("CommodityTypeEntries");
                }
            }
        }

        private IEnumerable<UnitOfMeasure> _unitOfMeasureEntries;

        public IEnumerable<UnitOfMeasure> UnitOfMeasureEntries
        {
            get { return _unitOfMeasureEntries; }
            private set
            {
                if (!ReferenceEquals(_unitOfMeasureEntries, value))
                {
                    _unitOfMeasureEntries = value;
                    RaisePropertyChanged("UnitOfMeasureEntries");
                }
            }
        }

        #endregion Public Properties

        #region Private Methods

        void _inventoryManagementModel_GetUnitOfMeasuresComplete(object sender, DataObjectResultsArgs<UnitOfMeasure> e)
        {
 	        if (!e.HasError)
            {
                UnitOfMeasureEntries = e.Results.OrderBy(g => g.UnitOfMeasureTag);
                // check whether IssueTypeEntries is populated after CurrentIssue
                AssignCurrentCommodity(null);
            }
            else
            {
                // notify user if there is any error
                AppMessages.RaiseErrorMessage.Send(e.Error);
            }
        }

        void _inventoryManagementModel_GetCommodityTypesComplete(object sender, DataObjectResultsArgs<CommodityType> e)
        {
 	        if (!e.HasError)
            {
                CommodityTypeEntries = e.Results.OrderBy(g => g.TypeCode);
                // check whether IssueTypeEntries is populated after CurrentIssue
                AssignCurrentCommodity(null);
            }
            else
            {
                // notify user if there is any error
                AppMessages.RaiseErrorMessage.Send(e.Error);
            }
        }

        void _inventoryManagementModel_GetVendorsComplete(object sender, DataObjectResultsArgs<Vendor> e)
        {
        	if (!e.HasError)
            {
                VendorEntries = e.Results.OrderBy(g => g.VendorName);
                // check whether IssueTypeEntries is populated after CurrentIssue
                AssignCurrentCommodity(null);
            }
            else
            {
                // notify user if there is any error
                AppMessages.RaiseErrorMessage.Send(e.Error);
            }
        }

        private void OnEditCommodityMessage(Commodity editCommodity)
        {
            if (editCommodity == null) return;
            // check whether this issue is read-only or not
            //TODO: will this ever apply?
            //AppMessages.ReadOnlyIssueMessage.Send(editIssue.IsIssueReadOnly());

            AssignCurrentCommodity(editCommodity);
        }

        /// <summary>
        /// Assign edit commodity only after all ComboBox are ready
        /// </summary>
        /// <param name="editIssue"></param>
        private void AssignCurrentCommodity(Commodity editCommodity)
        {
            if (editCommodity != null)
            {
                // this call is coming from OnEditIssueMessage()
                if (CommodityTypeEntries != null && VendorEntries != null &&
                    UnitOfMeasureEntries != null)
                {
                    // if all ComboBox are ready, we set CurrentCommodity
                    CurrentCommodity = editCommodity;
                    _currentCommodityCache = null;
                }
                else
                    _currentCommodityCache = editCommodity;
            }
            else
            {
                // this call is coming from one of the complete event handlers
                if (_currentCommodityCache != null && CommodityTypeEntries != null && 
                    VendorEntries != null && UnitOfMeasureEntries != null)
                {
                    // if all ComboBox are ready, we set CurrentCommodity
                    CurrentCommodity = _currentCommodityCache;
                    _currentCommodityCache = null;
                }
            }
        }

        #endregion

        #region ICleanup
        public override void Cleanup()
        {
            if (_inventoryManagementModel != null)
            {
                // unregister all events
                _inventoryManagementModel.GetVendorsComplete -= _inventoryManagementModel_GetVendorsComplete;
                _inventoryManagementModel.GetCommodityTypesComplete -= _inventoryManagementModel_GetCommodityTypesComplete;
                _inventoryManagementModel.GetUnitOfMeasuresComplete -= _inventoryManagementModel_GetUnitOfMeasuresComplete;
                _inventoryManagementModel = null;
            }
            // set properties back to null
            CurrentCommodity = null;
            VendorEntries = null;
            CommodityTypeEntries = null;
            UnitOfMeasureEntries = null;
            // unregister any messages for this ViewModel
            base.Cleanup();
        }
        #endregion ICleanup
    }
}