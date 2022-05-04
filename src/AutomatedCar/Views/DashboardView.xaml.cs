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

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}