using System;
using System.IO;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;

using InventoryManagement.Data.Web;

namespace InventoryManagement.Common
{
    /// <summary>
    /// class that defines all messages used in this application
    /// </summary>
    public static class AppMessages
    {
        enum MessageTypes
        {
            // MainPage View messages
            ChangeScreen,
            RaiseError,
            PleaseConfirm,
            StatusUpdate,

            // Inventory messages
            EditCommodity,
            EditVendor

        }

        public static class ChangeScreenMessage
        {
            public static void Send(string screenName)
            {
                Messenger.Default.Send(screenName, MessageTypes.ChangeScreen);
            }

            public static void Register(object recipient, Action<string> action)
            {
                Messenger.Default.Register(recipient, MessageTypes.ChangeScreen, action);
            }
        }

        public static class RaiseErrorMessage
        {
            public static void Send(Exception ex)
            {
                Messenger.Default.Send(ex, MessageTypes.RaiseError);
            }

            public static void Register(object recipient, Action<Exception> action)
            {
                Messenger.Default.Register(recipient, MessageTypes.RaiseError, action);
            }
        }

        public static class PleaseConfirmMessage
        {
            public static void Send(DialogMessage dialogMessage)
            {
                Messenger.Default.Send(dialogMessage, MessageTypes.PleaseConfirm);
            }

            public static void Register(object recipient, Action<DialogMessage> action)
            {
                Messenger.Default.Register(recipient, MessageTypes.PleaseConfirm, action);
            }
        }

        public static class StatusUpdateMessage
        {
            public static void Send(DialogMessage dialogMessage)
            {
                Messenger.Default.Send(dialogMessage, MessageTypes.StatusUpdate);
            }

            public static void Register(object recipient, Action<DialogMessage> action)
            {
                Messenger.Default.Register(recipient, MessageTypes.StatusUpdate, action);
            }
        }

        public static class EditCommodityMessage
        {
            public static void Send(Commodity commodity)
            {
                Messenger.Default.Send(commodity, MessageTypes.EditCommodity);
            }

            public static void Register(object recipient, Action<Commodity> action)
            {
                Messenger.Default.Register(recipient, MessageTypes.EditCommodity, action);
            }
        }

        public static class EditVendorMessage
        {
            public static void Send(Vendor vendor)
            {
                Messenger.Default.Send(vendor, MessageTypes.EditVendor);
            }

            public static void Register(object recipient, Action<Vendor> action)
            {
                Messenger.Default.Register(recipient, MessageTypes.EditVendor, action);
            }
        }
    }
}