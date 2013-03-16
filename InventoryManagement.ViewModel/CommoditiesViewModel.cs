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
    [Export(ViewModelTypes.CommoditiesViewModel, typeof(ViewModelBase))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CommoditiesViewModel : ViewModelBase
    {
        #region Private Members
        private IInventoryManagementModel _inventoryManagementModel;
        #endregion Private Members

        #region Constructor
        [ImportingConstructor]
        public CommoditiesViewModel(IInventoryManagementModel inventoryManagementModel)
        {
            _inventoryManagementModel = inventoryManagementModel;

            // set up event handling
            _inventoryManagementModel.SaveChangesComplete += _inventoryManagementModel_SaveChangesComplete;
            _inventoryManagementModel.GetCommoditiesComplete += _inventoryManagementModel_GetCommoditiesComplete;
            _inventoryManagementModel.PropertyChanged += _inventoryManagementModel_PropertyChanged;

            // load all commodities
            _inventoryManagementModel.GetCommoditiesAsync();
        }
        #endregion Constructor

        #region Public Properties

        private IEnumerable<Commodity> _allCommodities;

        private CollectionViewSource _allCommoditiesSource;

        public CollectionViewSource CommoditiesSource
        {
            get { return _allCommoditiesSource; }
            private set
            {
                if (!ReferenceEquals(_allCommoditiesSource, value))
                {
                    _allCommoditiesSource = value;
                    RaisePropertyChanged("CommoditiesSource");
                }
            }
        }

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
                CurrentCommodity = (Commodity)enumerator.Current;
                // edit the new selected commodity
                AppMessages.EditCommodityMessage.Send(CurrentCommodity);
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
                    if (CurrentCommodity != null)
                    {
                        /* TODO: validate commodity before saving
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
                        CommonResources.CancelCurrentCommodityMessageBoxText,
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

        void _inventoryManagementModel_GetCommoditiesComplete(object sender, DataObjectResultsArgs<Commodity> e)
        {
            if (!e.HasError)
            {
                // cancel any changes before setting AllCommoditiesSource
                if (_inventoryManagementModel.HasChanges)
                {
                    _inventoryManagementModel.RejectChanges();
                }

                _allCommodities = e.Results.OrderBy(g => g.PartNumber).ThenBy(g => g.InventoryID);

                CommoditiesSource = new CollectionViewSource { Source = _allCommodities };
                CommoditiesSource.View.CurrentChanging += View_CurrentChanging;

                // set the first row as the current issue
                if (_allCommodities.Count() >= 1)
                {
                    var enumerator = _allCommodities.GetEnumerator();
                    enumerator.MoveNext();
                    CurrentCommodity = enumerator.Current;

                    AppMessages.EditCommodityMessage.Send(CurrentCommodity);
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
                    CommonResources.CancelCurrentCommodityMessageBoxText,
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
                _inventoryManagementModel.GetCommoditiesComplete -= _inventoryManagementModel_GetCommoditiesComplete;
                _inventoryManagementModel.PropertyChanged -= _inventoryManagementModel_PropertyChanged;
                _inventoryManagementModel = null;
            }
            // set properties back to null
            _allCommodities = null;
            CurrentCommodity = null;
            // unregister any messages for this ViewModel
            base.Cleanup();
        }

        #endregion ICleanup
    }
}