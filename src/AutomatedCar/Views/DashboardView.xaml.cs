namespace AutomatedCar.Views
{
    using Avalonia.Controls;
    using Avalonia.Controls.Primitives;
    using Avalonia.Controls.Templates;
    using Avalonia.Interactivity;
    using Avalonia.Layout;
    using Avalonia.LogicalTree;
    using Avalonia.Markup.Xaml;
    using Avalonia.Media;
    using Avalonia.VisualTree;
    using System;
    using System.Diagnostics;
    using System.Linq;

    public class DashboardView : UserControl
    {
        public DashboardView()
        {
            this.InitializeComponent();
        }

        private bool isOn = false;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Button_On_Off(object? sender, RoutedEventArgs e)
        {
            // todo
            // hardcoded
           var myButtonBorder = (Border)this.FindControl<ContentControl>("Dashboard").GetLogicalChildren().ToList()[0].LogicalChildren[11].LogicalChildren[25].LogicalChildren[0].LogicalChildren[0];
           var labelText = (AccessText)myButtonBorder.GetLogicalChildren().ToList()[0].LogicalChildren[0];

           if (this.isOn)
           {
               labelText.DataContext = "Off";
               myButtonBorder.HorizontalAlignment = HorizontalAlignment.Left;
           }
           else
           {
               labelText.DataContext = "On";
               myButtonBorder.HorizontalAlignment = HorizontalAlignment.Right;
           }

           this.isOn = !this.isOn;
        }
    }
}