using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using InventoryManagement.Common;
using InventoryManagement.Data.Web;

namespace InventoryManagement.ViewModel
{
    [Export(ViewModelTypes.VendorsViewModel, typeof(ViewModelBase))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class VendorsViewModel : ViewModelBase
    {
        #region Private Members
        private IInventoryManagementModel _inventoryManagementModel;
        #endregion Private Members

        #region Constructor
        [ImportingConstructor]
        public VendorsViewModel(IInventoryManagementModel inventoryManagementModel)
        {
            _inventoryManagementModel = inventoryManagementModel;

            // set up event handling
            _inventoryManagementModel.SaveChangesComplete += _inventoryManagementModel_SaveChangesComplete;
            _inventoryManagementModel.GetVendorsComplete += _inventoryManagementModel_GetVendorsComplete;
            _inventoryManagementModel.PropertyChanged += _inventoryManagementModel_PropertyChanged;

            // load all vendors
            _inventoryManagementModel.GetVendorsAsync();
        }

        #endregion Constructor

        #region Public Properties

        private IEnumerable<Vendor> _allVendors;

        private CollectionViewSource _allVendorsSource;

        public CollectionViewSource VendorsSource
        {
            get { return _allVendorsSource; }
            private set
            {
                if (!ReferenceEquals(_allVendorsSource, value))
                {
                    _allVendorsSource = value;
                    RaisePropertyChanged("VendorsSource");
                }
            }
        }

        private Vendor _currentVendor;

        public Vendor CurrentVendor
        {
            get { return _currentVendor; }
            private set
            {
                if (!ReferenceEquals(_currentVendor, value))
                {
                    _currentVendor = value;
                    RaisePropertyChanged("CurrentVendor");
                }
            }
        }

        #endregion Public Properties

        #region Public Commands

        private RelayCommand<SelectionChangedEventArgs> _selectionChangedCommand;

        public RelayCommand<SelectionChangedEventArgs> SelectionChangedCommand
        {
            get
            {
                if (_selectionChangedCommand == null)
                {
                    _selectionChangedCommand = new RelayCommand<SelectionChangedEventArgs>(
                        OnSelectionChangedCommand);
                }
                return _selectionChangedCommand;
            }
        }

        private void OnSelectionChangedCommand(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                // cancel any changes before editing another issue
                if (_inventoryManagementModel.HasChanges)
                {
                    _inventoryManagementModel.RejectChanges();
                }
                var enumerator = e.AddedItems.GetEnumerator();
                enumerator.MoveNext();
                CurrentVendor = (Vendor)enumerator.Current;
                // edit the new selected vendor
                AppMessages.EditVendorMessage.Send(CurrentVendor);
            }
        }

        private RelayCommand _submitChangeCommand;

        public RelayCommand SubmitChangeCommand
        {
            get
            {
                if (_submitChangeCommand == null)
                {
                    _submitChangeCommand = new RelayCommand(
                        OnSubmitChangeCommand,
                        () => (_inventoryManagementModel != null) && (_inventoryManagementModel.HasChanges));
                }
                return _submitChangeCommand;
            }
        }

        private void OnSubmitChangeCommand()
        {
            try
            {
                if (!_inventoryManagementModel.IsBusy)
                {
                    if (CurrentVendor != null)
                    {
                        /* TODO: validate vendor before saving
                        // this should trigger validation even if the Title is not changed and is null
                        if (string.IsNullOrWhiteSpace(CurrentIssue.Title))
                            CurrentIssue.Title = string.Empty;

                        // set ResolutionDate and ResolvedByID based on ResolutionID
                        if (CurrentIssue.ResolutionID == null || CurrentIssue.ResolutionID == 0)
                        {
                            CurrentIssue.ResolutionDate = null;
                            CurrentIssue.ResolvedByID = null;
                        }
                        else
                        {
                            if (CurrentIssue.ResolutionDate == null)
                                CurrentIssue.ResolutionDate = DateTime.Now;
                            if (CurrentIssue.ResolvedByID == null)
                                CurrentIssue.ResolvedByID = WebContext.Current.User.Identity.Name;
                        }

                        if (CurrentCommodity.TryValidateObject()
                            && CurrentIssue.TryValidateProperty("IssueID")
                            && CurrentIssue.TryValidateProperty("Title"))
                        {
                            _issueVisionModel.SaveChangesAsync();
                        } */

                        _inventoryManagementModel.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // notify user if there is any error
                AppMessages.RaiseErrorMessage.Send(ex);
            }
        }

        private RelayCommand _cancelChangeCommand;

        public RelayCommand CancelChangeCommand
        {
            get
            {
                if (_cancelChangeCommand == null)
                {
                    _cancelChangeCommand = new RelayCommand(
                        OnCancelChangeCommand,
                        () => (_inventoryManagementModel != null) && (_inventoryManagementModel.HasChanges));
                }
                return _cancelChangeCommand;
            }
        }

        private void OnCancelChangeCommand()
        {
            try
            {
                if (!_inventoryManagementModel.IsBusy)
                {
                    // ask to confirm canceling the current issue in edit
                    var dialogMessage = new DialogMessage(
                        this,
                        CommonResources.CancelCurrentVendorMessageBoxText,
                        s =>
                        {
                            if (s == MessageBoxResult.OK)
                            {
                                // if confirmed, cancel this issue
                                _inventoryManagementModel.RejectChanges();
                            }
                        })
                    {
                        Button = MessageBoxButton.OKCancel,
                        Caption = CommonResources.ConfirmMessageBoxCaption
                    };

                    AppMessages.PleaseConfirmMessage.Send(dialogMessage);
                }
            }
            catch (Exception ex)
            {
                // notify user if there is any error
                AppMessages.RaiseErrorMessage.Send(ex);
            }
        }

        #endregion Public Commands

        #region Private Methods

        void _inventoryManagementModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("HasChanges"))
            {
                SubmitChangeCommand.RaiseCanExecuteChanged();
                CancelChangeCommand.RaiseCanExecuteChanged();
            }
        }

        void _inventoryManagementModel_GetVendorsComplete(object sender, DataObjectResultsArgs<Vendor> e)
        {
            if (!e.HasError)
            {
                // cancel any changes before setting VendorsSource
                if (_inventoryManagementModel.HasChanges)
                {
                    _inventoryManagementModel.RejectChanges();
                }

                _allVendors = e.Results.OrderBy(g => g.VendorName);

                VendorsSource = new CollectionViewSource { Source = _allVendors };
                VendorsSource.View.CurrentChanging += View_CurrentChanging;

                // set the first row as the current vendor
                if (_allVendors.Count() >= 1)
                {
                    var enumerator = _allVendors.GetEnumerator();
                    enumerator.MoveNext();
                    CurrentVendor = enumerator.Current;

                    AppMessages.EditVendorMessage.Send(CurrentVendor);
                }
            }
            else
            {
                // notify user if there is any error
                AppMessages.RaiseErrorMessage.Send(e.Error);
            }
        }

        void _inventoryManagementModel_SaveChangesComplete(object sender, SubmitOperationEventArgs e)
        {
            if (e.HasError)
            {
                // notify user if there is any error
                AppMessages.RaiseErrorMessage.Send(e.Error);
            }
        }

        private void View_CurrentChanging(object sender, CurrentChangingEventArgs e)
        {
            MessageBoxResult theResult = MessageBoxResult.OK;

            if (_inventoryManagementModel.HasChanges)
            {
                // ask to confirm canceling the current issue in edit
                var dialogMessage = new DialogMessage(
                    this,
                    CommonResources.CancelCurrentVendorMessageBoxText,
                    s => theResult = s)
                {
                    Button = MessageBoxButton.OKCancel,
                    Caption = CommonResources.ConfirmMessageBoxCaption
                };

                AppMessages.PleaseConfirmMessage.Send(dialogMessage);

                if (theResult == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        #endregion Private Methods

        #region ICleanup

        public override void Cleanup()
        {
            if (_inventoryManagementModel != null)
            {
                // unregister all events
                _inventoryManagementModel.SaveChangesComplete -= _inventoryManagementModel_SaveChangesComplete;
                _inventoryManagementModel.GetVendorsComplete -= _inventoryManagementModel_GetVendorsComplete;
                _inventoryManagementModel.PropertyChanged -= _inventoryManagementModel_PropertyChanged;
                _inventoryManagementModel = null;
            }
            // set properties back to null
            _allVendors = null;
            CurrentVendor = null;
            // unregister any messages for this ViewModel
            base.Cleanup();
        }

        #endregion ICleanup
    }
}