using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using InventoryManagement.Common;

namespace InventoryManagement.Client
{
    public partial class LoginForm : UserControl
    {
        #region "Constructor"
        public LoginForm()
        {
            InitializeComponent();

            if (!ViewModelBase.IsInDesignModeStatic)
            {
                // Use MEF To load the View Model
                DataContext = App.Container.GetExportedValue<ViewModelBase>(
                    ViewModelTypes.LoginFormViewModel);
            }
        }
        #endregion "Constructor"

        #region "Event handlers"

        private void userNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // dynamically enable/disable error message
            if (!string.IsNullOrWhiteSpace(loginScreenErrorMessageTextBox.Text))
                loginScreenErrorMessageTextBox.Text = string.Empty;

            // dynamically enable/disable login button
            loginButton.IsEnabled = !(string.IsNullOrWhiteSpace(userNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(passwordPasswordBox.Password));
        }

        private void passwordPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // dynamically enable/disable error message
            if (!string.IsNullOrWhiteSpace(loginScreenErrorMessageTextBox.Text))
                loginScreenErrorMessageTextBox.Text = string.Empty;

            // dynamically enable/disable login button
            loginButton.IsEnabled = !(string.IsNullOrWhiteSpace(userNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(passwordPasswordBox.Password));
        }

        private void securityAnswerPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // dynamically enable/disable error message
            if (!string.IsNullOrWhiteSpace(resetPasswordScreenErrorMessageTextBox.Text))
                resetPasswordScreenErrorMessageTextBox.Text = string.Empty;

            // dynamically enable/disable reset password button
            resetPasswordButton.IsEnabled = !(string.IsNullOrWhiteSpace(newPasswordPasswordBox.Password) ||
                string.IsNullOrWhiteSpace(confirmPasswordPasswordBox.Password) ||
                string.IsNullOrEmpty(securityAnswerPasswordBox.Password));
        }

        private void newPasswordPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // update the ActualPassword every time password changes
            ActualPasswordTextBlock.Text = newPasswordPasswordBox.Password;

            // dynamically enable/disable error message
            if (!string.IsNullOrWhiteSpace(resetPasswordScreenErrorMessageTextBox.Text))
                resetPasswordScreenErrorMessageTextBox.Text = string.Empty;

            // dynamically enable/disable reset password button
            resetPasswordButton.IsEnabled = !(string.IsNullOrWhiteSpace(newPasswordPasswordBox.Password) ||
                string.IsNullOrWhiteSpace(confirmPasswordPasswordBox.Password) ||
                string.IsNullOrEmpty(securityAnswerPasswordBox.Password));
        }

        private void newPasswordPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // If confirm password is not empty, we need to re-validate confirm
            // password after leaving password field
            if (!string.IsNullOrEmpty(confirmPasswordPasswordBox.Password))
            {
                BindingExpression be = confirmPasswordPasswordBox.GetBindingExpression(PasswordBox.PasswordProperty);
                if (be != null)
                    be.UpdateSource();
            }
        }

        private void confirmPasswordPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // dynamically enable/disable error message
            if (!string.IsNullOrWhiteSpace(resetPasswordScreenErrorMessageTextBox.Text))
                resetPasswordScreenErrorMessageTextBox.Text = string.Empty;

            // dynamically enable/disable reset password button
            resetPasswordButton.IsEnabled = !(string.IsNullOrWhiteSpace(newPasswordPasswordBox.Password) ||
                string.IsNullOrWhiteSpace(confirmPasswordPasswordBox.Password) ||
                string.IsNullOrEmpty(securityAnswerPasswordBox.Password));
        }

        #endregion "Event handlers"
    }
}
