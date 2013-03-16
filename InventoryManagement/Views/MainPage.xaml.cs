using System;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using InventoryManagement.Common;

namespace InventoryManagement.Client
{
    public partial class MainPage : UserControl
    {
        #region Private Members
        private bool _noErrorMessage = true;
        #endregion Private Members

        #region Constructor
        public MainPage()
        {
            InitializeComponent();

            // register for ChangeScreenMessage
            AppMessages.ChangeScreenMessage.Register(this, OnChangeScreenMessage);
            // register for RaiseErrorMessage
            AppMessages.RaiseErrorMessage.Register(this, OnRaiseErrorMessage);
            // register for PleaseConfirmMessage
            AppMessages.PleaseConfirmMessage.Register(this, OnPleaseConfirmMessage);
            // register for StatusUpdateMessage
            AppMessages.StatusUpdateMessage.Register(this, OnStatusUpdateMessage);

            if (!ViewModelBase.IsInDesignModeStatic)
            {
                // Use MEF To load the View Model
                DataContext = App.Container.GetExportedValue<ViewModelBase>(
                    ViewModelTypes.MainPageViewModel);
            }
        }
        #endregion Constructor

        #region "ChangeScreenMessage"

        private void OnChangeScreenMessage(string changeScreen)
        {
            // call Cleanup() on the current screen before switching
            var currentScreen = mainPageContent.Content as ICleanup;
            if (currentScreen != null)
                currentScreen.Cleanup();

            // reset _noErrorMessage
            _noErrorMessage = true;

            switch (changeScreen)
            {
                case ViewTypes.HomeView:
                    mainPageContent.Content = new Home();
                    break;
                case ViewTypes.NewJob:
                    mainPageContent.Content = new NewWorkOrder();
                    break;
                case ViewTypes.OpenJobs:
                    mainPageContent.Content = new OpenWorkOrders();
                    break;
                case ViewTypes.AllJobs:
                    mainPageContent.Content = new AllWorkOrders();
                    break;
                case ViewTypes.Commodities:
                    mainPageContent.Content = new Commodities();
                    break;
                /* TODO:                
                case ViewTypes.MyProfileView:
                    mainPageContent.Content = new MyProfile();
                    break;
                case ViewTypes.UserMaintenanceView:
                    mainPageContent.Content = new UserMaintenance();
                    break; */
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion "ChangeScreenMessage"

        #region "RaiseErrorMessage"

        private void OnRaiseErrorMessage(Exception ex)
        {
            if (_noErrorMessage && ex != null)
            {
                ErrorWindow.CreateNew(ex);
                // logout after display the error message
                hyperlinkButton_Logout.Command.Execute(true);
                _noErrorMessage = false;
            }
        }

        #endregion "RaiseErrorMessage"

        #region "PleaseConfirmMessage"

        private static void OnPleaseConfirmMessage(DialogMessage dialogMessage)
        {
            if (dialogMessage != null)
            {
                MessageBoxResult result = MessageBox.Show(dialogMessage.Content,
                    dialogMessage.Caption, dialogMessage.Button);

                dialogMessage.ProcessCallback(result);
            }
        }

        #endregion "PleaseConfirmMessage"

        #region "StatusUpdateMessage"

        private static void OnStatusUpdateMessage(DialogMessage dialogMessage)
        {
            if (dialogMessage != null)
            {
                MessageBoxResult result = MessageBox.Show(dialogMessage.Content,
                    dialogMessage.Caption, dialogMessage.Button);

                dialogMessage.ProcessCallback(result);
            }
        }

        #endregion "StatusUpdateMessage"
    }
}
