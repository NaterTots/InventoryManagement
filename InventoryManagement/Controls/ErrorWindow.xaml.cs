﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace InventoryManagement.Client
{
    /// <summary>
    ///     Controls when a stack trace should be displayed on the
    ///     Error Window
    ///     
    ///     Defaults to <see cref="OnlyWhenDebuggingOrRunningLocally"/>
    /// </summary>
    public enum StackTracePolicy
    {
        /// <summary>
        ///   Stack trace is showed only when running with a debugger attached
        ///   or when running the app on the local machine. Use this to get
        ///   additional debug information you don't want the end users to see
        /// </summary>
        OnlyWhenDebuggingOrRunningLocally,

        /// <summary>
        /// Always show the stack trace, even if debugging
        /// </summary>
        Always,

        /// <summary>
        /// Never show the stack trace, even when debugging
        /// </summary>
        Never
    }

    public partial class ErrorWindow : ChildWindow
    {
        protected ErrorWindow(string message, string errorDetails)
        {
            InitializeComponent();
            IntroductoryText.Text = message;
            ErrorTextBox.Text = errorDetails;
        }

        #region Factory Shortcut Methods
        /// <summary>
        ///     Creates a new Error Window given an error message.
        ///     Current stack trace will be displayed if app is running under
        ///     debug or on the local machine
        /// </summary>
        public static void CreateNew(string message)
        {
            CreateNew(message, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        ///     Creates a new Error Window given an exception.
        ///     Current stack trace will be displayed if app is running under
        ///     debug or on the local machine
        ///     
        ///     The exception is converted onto a message using
        ///     <see cref="ConvertExceptionToMessage"/>
        /// </summary>
        public static void CreateNew(Exception exception)
        {
            CreateNew(exception, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        ///     Creates a new Error Window given an exception. The exception is converted onto a message using
        ///     <see cref="ConvertExceptionToMessage"/>
        ///     
        ///     <param name="policy">When to display the stack trace, see <see cref="StackTracePolicy"/></param>
        /// </summary>
        public static void CreateNew(Exception exception, StackTracePolicy policy)
        {
            string fullStackTrace = exception.StackTrace;

            // Account for nested exceptions
            Exception innerException = exception.InnerException;
            while (innerException != null)
            {
                fullStackTrace += "\nCaused by: " + innerException.Message + "\n\n" + innerException.StackTrace;
                innerException = innerException.InnerException;
            }

            CreateNew(ConvertExceptionToMessage(exception), fullStackTrace, policy);
        }

        /// <summary>
        ///     Creates a new Error Window given an error message.
        ///     
        ///     <param name="policy">When to display the stack trace, see <see cref="StackTracePolicy"/></param>
        /// </summary>
        public static void CreateNew(string message, StackTracePolicy policy)
        {
            CreateNew(message, new StackTrace().ToString(), policy);
        }
        #endregion

        #region Factory Methods
        /// <summary>
        ///     All other factory methods will result in a call to this one
        /// </summary>
        /// 
        /// <param name="message">Which message to display</param>
        /// <param name="stackTrace">The associated stack trace</param>
        /// <param name="policy">In which situations the stack trace should be appended to the message</param>
        private static void CreateNew(string message, string stackTrace, StackTracePolicy policy)
        {
            string errorDetails = string.Empty;

            if (policy == StackTracePolicy.Always ||
                policy == StackTracePolicy.OnlyWhenDebuggingOrRunningLocally && IsRunningUnderDebugOrLocalhost)
            {
                errorDetails = stackTrace ?? string.Empty;
            }

            var window = new ErrorWindow(message, errorDetails);
            window.Show();
        }
        #endregion

        #region Factory Helpers
        /// <summary>
        ///     Returns whether running under a dev environment, i.e., with a debugger attached or
        ///     with the server hosted on localhost
        /// </summary>
        private static bool IsRunningUnderDebugOrLocalhost
        {
            get
            {
                if (Debugger.IsAttached)
                {
                    return true;
                }
                if (Application.Current.Host.Source != null)
                {
                    string hostUrl = Application.Current.Host.Source.Host;
                    return hostUrl.Contains("::1") || hostUrl.Contains("localhost") || hostUrl.Contains("127.0.0.1");
                }
                return false;
            }
        }

        /// <summary>
        ///     Creates a user friendly message given an Exception. Currently this simply
        ///     takes the Exception.Message value, optionally  but you might want to change this to treat
        ///     some specific Exception classes differently
        /// </summary>
        private static string ConvertExceptionToMessage(Exception e)
        {
            return e.Message;
        }
        #endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

