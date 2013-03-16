using System;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using InventoryManagement.Common;

namespace InventoryManagement.Client
{
    public partial class AllWorkOrders : UserControl, ICleanup
    {
        #region Private Members
        private const double MinimumWidth = 640;
        private Lazy<ViewModelBase> _viewModelExport;
        #endregion Private Members

        #region Constructor
        public AllWorkOrders()
        {
            InitializeComponent();
            // add the WorkOrderEditor
            workOrderEditorContentControl.Content = new WorkOrderEditor();
            // initialize the UserControl Width & Height
            Content_Resized(this, null);

            // register any AppMessages here

            if (!ViewModelBase.IsInDesignModeStatic)
            {
                // Use MEF To load the View Model
                _viewModelExport = App.Container.GetExport<ViewModelBase>(
                    ViewModelTypes.AllWorkOrdersViewModel);
                if (_viewModelExport != null) DataContext = _viewModelExport.Value;
            }
        }
        #endregion Constructor

        #region Privates

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.Host.Content.Resized += Content_Resized;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Application.Current.Host.Content.Resized -= Content_Resized;
        }

        private void Content_Resized(object sender, EventArgs e)
        {
            // stretch the UserControl to the current browser width if wider than MinimumWidth
            if (Application.Current.Host.Content.ActualWidth < MinimumWidth)
                Width = MinimumWidth;
            else
                Width = Application.Current.Host.Content.ActualWidth;
            // stretch the UserControl to the current brower Height - Title Height(42) - Menu Height(22)
            Height = Application.Current.Host.Content.ActualHeight - 42 - 22;
        }

        #endregion Privates

        #region ICleanup

        public void Cleanup()
        {
            // call Cleanup on its ViewModel
            ((ICleanup)DataContext).Cleanup();
            // call Cleanup on WorkOrderEditor
            var workOrderEditor = workOrderEditorContentControl.Content as ICleanup;
            if (workOrderEditor != null)
                workOrderEditor.Cleanup();
            workOrderEditorContentControl.Content = null;
            // cleanup itself
            Messenger.Default.Unregister(this);
            // set DataContext to null and call ReleaseExport()
            DataContext = null;
            App.Container.ReleaseExport(_viewModelExport);
            _viewModelExport = null;
        }

        #endregion ICleanup
    }
}

