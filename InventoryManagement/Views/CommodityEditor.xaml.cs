using System;
using System.IO;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using InventoryManagement.Common;

namespace InventoryManagement.Client
{
    public partial class CommodityEditor : UserControl, ICleanup
    {
        #region Private Members

        private Lazy<ViewModelBase> _viewModelExport;

        #endregion Private Members

        #region Constructor

        public CommodityEditor()
        {
            InitializeComponent();

            if (!ViewModelBase.IsInDesignModeStatic)
            {
                // Use MEF To load the View Model
                _viewModelExport = App.Container.GetExport<ViewModelBase>(
                    ViewModelTypes.CommodityEditorViewModel);
                if (_viewModelExport != null) DataContext = _viewModelExport.Value;
            }
        }

        #endregion Constructor

        #region ICleanup

        public void Cleanup()
        {
            // call Cleanup on its ViewModel
            ((ICleanup)DataContext).Cleanup();
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
