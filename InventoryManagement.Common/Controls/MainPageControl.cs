using System.Windows;
using System.Windows.Controls;

namespace InventoryManagement.Common
{
    /// <summary>
    /// Custom control class with "LoggedIn" and "LoggedOut" states
    /// </summary>
    [TemplateVisualState(Name = "LoggedIn", GroupName = "CommonStates"),
    TemplateVisualState(Name = "LoggedOut", GroupName = "CommonStates")]
    public class MainPageControl : Control
    {
        public MainPageControl()
        {
            DefaultStyleKey = typeof(MainPageControl);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateVisualState(false);
        }

        #region "Dependency Properties"

        public object TitleContent
        {
            get { return GetValue(TitleContentProperty); }
            set { SetValue(TitleContentProperty, value); }
        }

        public static readonly DependencyProperty TitleContentProperty =
            DependencyProperty.Register("TitleContent",
                                        typeof(object),
                                        typeof(MainPageControl), null);

        public object LoggedInMenuContent
        {
            get { return GetValue(LoggedInMenuContentProperty); }
            set { SetValue(LoggedInMenuContentProperty, value); }
        }

        public static readonly DependencyProperty LoggedInMenuContentProperty =
            DependencyProperty.Register("LoggedInMenuContent",
                                        typeof(object),
                                        typeof(MainPageControl), null);

        public object LoggedOutMenuContent
        {
            get { return GetValue(LoggedOutMenuContentProperty); }
            set { SetValue(LoggedOutMenuContentProperty, value); }
        }

        public static readonly DependencyProperty LoggedOutMenuContentProperty =
            DependencyProperty.Register("LoggedOutMenuContent",
                                        typeof(object),
                                        typeof(MainPageControl), null);

        public object LoginPageContent
        {
            get { return GetValue(LoginPageContentProperty); }
            set { SetValue(LoginPageContentProperty, value); }
        }

        public static readonly DependencyProperty LoginPageContentProperty =
            DependencyProperty.Register("LoginPageContent",
                                        typeof(object),
                                        typeof(MainPageControl), null);

        public object MainPageContent
        {
            get { return GetValue(MainPageContentProperty); }
            set { SetValue(MainPageContentProperty, value); }
        }

        public static readonly DependencyProperty MainPageContentProperty =
            DependencyProperty.Register("MainPageContent",
                                        typeof(object),
                                        typeof(MainPageControl), null);

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius",
                                        typeof(CornerRadius),
                                        typeof(MainPageControl), null);

        public bool IsLoggedIn
        {
            get { return (bool)GetValue(IsLoggedInProperty); }
            set { SetValue(IsLoggedInProperty, value); }
        }

        public static readonly DependencyProperty IsLoggedInProperty =
            DependencyProperty.Register("IsLoggedIn",
                                        typeof(bool),
                                        typeof(MainPageControl),
                                        new PropertyMetadata(false, OnIsLoggedInChanged));

        private static void OnIsLoggedInChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var mainPageControl = sender as MainPageControl;

            if (mainPageControl != null)
            {
                mainPageControl.UpdateVisualState(true);
            }
        }

        #endregion "Dependency Properties"

        #region "Internal Method"
        internal void UpdateVisualState(bool useTransitions)
        {
            VisualStateManager.GoToState(this, IsLoggedIn ? "LoggedIn" : "LoggedOut", useTransitions);
        }

        #endregion "Internal Method"
    }
}
