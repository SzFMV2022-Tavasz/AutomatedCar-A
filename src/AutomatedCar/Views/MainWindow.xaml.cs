namespace AutomatedCar.Views
{
    using AutomatedCar.Models;
    using AutomatedCar.ViewModels;
    using Avalonia.Controls;
    using Avalonia.Input;
    using Avalonia.Markup.Xaml;

    public class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            World.Instance.ScrollViewerForFocus = this.Get<CourseDisplayView>("courseDisplay").Get<ScrollViewer>("scrollViewer");
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            Keyboard.Keys.Add(e.Key);
            base.OnKeyDown(e);

            MainWindowViewModel viewModel = (MainWindowViewModel)this.DataContext;

            if (Keyboard.IsKeyDown(Key.Up))
            {
                viewModel.CourseDisplay.KeyUp();
            }

            if (Keyboard.IsKeyDown(Key.Down))
            {
                viewModel.CourseDisplay.KeyDown();
            }

            if (Keyboard.IsKeyDown(Key.Left))
            {
                viewModel.CourseDisplay.KeyLeft();
            }

            if (Keyboard.IsKeyDown(Key.Right))
            {
                viewModel.CourseDisplay.KeyRight();
            }

            if (Keyboard.IsKeyDown(Key.PageUp))
            {
                viewModel.CourseDisplay.PageUp();
            }

            if (Keyboard.IsKeyDown(Key.PageDown))
            {
                viewModel.CourseDisplay.PageDown();
            }

            if (Keyboard.IsKeyDown(Key.D1))
            {
                viewModel.CourseDisplay.ToggleDebug();
            }

            if (Keyboard.IsKeyDown(Key.D2))
            {
                viewModel.CourseDisplay.ToggleCamera();
            }

            if (Keyboard.IsKeyDown(Key.D3))
            {
                viewModel.CourseDisplay.ToggleRadar();
            }

            if (Keyboard.IsKeyDown(Key.D4))
            {
                viewModel.CourseDisplay.ToggleUltrasonic();
            }

            if (Keyboard.IsKeyDown(Key.D5))
            {
                viewModel.CourseDisplay.ToggleRotation();
            }

            if (Keyboard.IsKeyDown(Key.F1))
            {
                new HelpWindow().Show();
                Keyboard.Keys.Remove(Key.F1);
            }

            if (Keyboard.IsKeyDown(Key.F5))
            {
                viewModel.NextControlledCar();
                Keyboard.Keys.Remove(Key.F5);
            }

            if (Keyboard.IsKeyDown(Key.F6))
            {
                viewModel.PrevControlledCar();
                Keyboard.Keys.Remove(Key.F5);
            }

            if (Keyboard.IsKeyDown(Key.R))
            {
                viewModel.CourseDisplay.GearReverse();
                Keyboard.Keys.Remove(Key.R);
            }

            if (Keyboard.IsKeyDown(Key.P))
            {
                viewModel.CourseDisplay.GearPark();
                Keyboard.Keys.Remove(Key.P);
            }

            if (Keyboard.IsKeyDown(Key.N))
            {
                viewModel.CourseDisplay.GearNeutral();
                Keyboard.Keys.Remove(Key.N);
            }

            if (Keyboard.IsKeyDown(Key.D))
            {
                viewModel.CourseDisplay.GearDrive();
                Keyboard.Keys.Remove(Key.D);
            }

            if (Keyboard.IsKeyDown(Key.C))
            {
                viewModel.CourseDisplay.ACC();
                Keyboard.Keys.Remove(Key.C);
            }

            if (Keyboard.IsKeyDown(Key.OemPlus))
            {
                viewModel.CourseDisplay.ACCPlus();
                Keyboard.Keys.Remove(Key.OemPlus);
            }

            if (Keyboard.IsKeyDown(Key.OemMinus))
            {
                viewModel.CourseDisplay.ACCMinus();
                Keyboard.Keys.Remove(Key.OemMinus);
            }

            if (Keyboard.IsKeyDown(Key.T))
            {
                viewModel.CourseDisplay.CycleDistance();
                Keyboard.Keys.Remove(Key.OemMinus);
            }

            var scrollViewer = this.Get<CourseDisplayView>("courseDisplay").Get<ScrollViewer>("scrollViewer");
            viewModel.CourseDisplay.FocusCar(scrollViewer);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {

            MainWindowViewModel viewModel = (MainWindowViewModel)this.DataContext;

            if (Keyboard.IsKeyDown(Key.Up))
            {
                viewModel.CourseDisplay.GasRelease();
            }

            if (Keyboard.IsKeyDown(Key.Down))
            {
                viewModel.CourseDisplay.BreakRelease();
            }

            Keyboard.Keys.Remove(e.Key);
            base.OnKeyUp(e);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}