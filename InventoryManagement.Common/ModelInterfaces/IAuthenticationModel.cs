using System;
using System.ComponentModel;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Security.Principal;

namespace InventoryManagement.Common
{
    public interface IAuthenticationModel : INotifyPropertyChanged
    {
        void LoadUserAsync();
        event EventHandler<LoadUserOperationEventArgs> LoadUserComplete;
        void LoginAsync(LoginParameters loginParameters);
        event EventHandler<LoginOperationEventArgs> LoginComplete;
        void LogoutAsync();
        event EventHandler<LogoutOperationEventArgs> LogoutComplete;

        IPrincipal User { get; }
        Boolean IsBusy { get; }
        Boolean IsLoadingUser { get; }
        Boolean IsLoggingIn { get; }
        Boolean IsLoggingOut { get; }
        Boolean IsSavingUser { get; }

        event EventHandler<AuthenticationEventArgs> AuthenticationChanged;
    }
}
