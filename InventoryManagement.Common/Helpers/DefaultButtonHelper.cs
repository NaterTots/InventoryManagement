using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;

namespace InventoryManagement.Common
{
    /// <summary>
    /// Helper class to provide the functionality of a default button
    /// </summary>
    public static class DefaultButtonHelper
    {
        #region "DefaultButton Attached Property"

        public static DependencyProperty DefaultButtonProperty =
            DependencyProperty.RegisterAttached("DefaultButton",
                                                typeof(Button),
                                                typeof(DefaultButtonHelper),
                                                new PropertyMetadata(null, DefaultButtonChanged));

        /// <summary>
        /// Get method for the attached property DefaultButton
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Button GetDefaultButton(UIElement obj)
        {
            return (Button)obj.GetValue(DefaultButtonProperty);
        }

        /// <summary>
        /// Set method for the attached property DefaultButton
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="button"></param>
        public static void SetDefaultButton(UIElement obj, Button button)
        {
            obj.SetValue(DefaultButtonProperty, button);
        }

        #endregion "DefaultButton Attached Property"

        #region "Private Data & Method"

        private static bool fireClickEvent;

        private static void DefaultButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = d as UIElement;
            var button = e.NewValue as Button;
            if (uiElement != null && button != null)
            {
                // when KeyDown event fires with Key.Enter, set focus to the default button
                // and set fireClickEvent to true
                uiElement.KeyDown += (sender, arg) =>
                {
                    if (arg.Key == Key.Enter && button.IsEnabled)
                    {
                        fireClickEvent = true;
                        button.Focus();
                    }
                };

                // when the GetFocus event fires for  the default button, invoke the click event
                // when fireClickEvent is true
                button.GotFocus += (s, argument) =>
                {
                    // if fireClickEvent is true, this event is trigger by
                    // the KeyDown event with Key.Enter from uiElement
                    if (fireClickEvent)
                    {
                        var peer = new ButtonAutomationPeer(button);
                        if (peer.IsEnabled())
                        {
                            var invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                            if (invokeProv != null) invokeProv.Invoke();
                        }
                        // set fireClickEvent back to false
                        fireClickEvent = false;
                    }
                };
            }
        }

        #endregion "Private Data & Method"
    }
}
