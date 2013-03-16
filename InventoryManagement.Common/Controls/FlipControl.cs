using System.Windows;
using System.Windows.Controls;

namespace InventoryManagement.Common
{
    /// <summary>
    /// Custom control class with "Normal" and "Flipped" states
    /// </summary>
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates"),
    TemplateVisualState(Name = "Flipped", GroupName = "CommonStates")]
    public class FlipControl : Control
    {
        public FlipControl()
        {
            DefaultStyleKey = typeof(FlipControl);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateVisualState(false);
        }

        #region "Dependency Properties"

        public object FrontContent
        {
            get { return GetValue(FrontContentProperty); }
            set { SetValue(FrontContentProperty, value); }
        }

        public static readonly DependencyProperty FrontContentProperty =
            DependencyProperty.Register("FrontContent",
                                        typeof(object),
                                        typeof(FlipControl), null);

        public object BackContent
        {
            get { return GetValue(BackContentProperty); }
            set { SetValue(BackContentProperty, value); }
        }

        public static readonly DependencyProperty BackContentProperty =
            DependencyProperty.Register("BackContent",
                                        typeof(object),
                                        typeof(FlipControl), null);

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius",
                                        typeof(CornerRadius),
                                        typeof(FlipControl), null);

        public bool IsFlipped
        {
            get { return (bool)GetValue(IsFlippedProperty); }
            set { SetValue(IsFlippedProperty, value); }
        }

        public static readonly DependencyProperty IsFlippedProperty =
            DependencyProperty.Register("IsFlipped",
                                        typeof(bool),
                                        typeof(FlipControl),
                                        new PropertyMetadata(false, OnIsFlippedChanged));

        private static void OnIsFlippedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var flipControl = sender as FlipControl;

            if (flipControl != null)
            {
                flipControl.UpdateVisualState(true);
            }
        }

        #endregion "Dependency Properties"

        #region "Internal Method"
        internal void UpdateVisualState(bool useTransitions)
        {
            VisualStateManager.GoToState(this, IsFlipped ? "Flipped" : "Normal", useTransitions);
        }

        #endregion "Internal Method"
    }
}
