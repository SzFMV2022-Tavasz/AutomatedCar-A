using System.Collections.ObjectModel;
using AutomatedCar.Models;
using System.Linq;

using ReactiveUI;

namespace AutomatedCar.ViewModels
{
    using Avalonia.Controls;
    using Models;
    using System;
    using Visualization;

    public class CourseDisplayViewModel : ViewModelBase
    {
        public ObservableCollection<WorldObjectViewModel> WorldObjects { get; } = new ObservableCollection<WorldObjectViewModel>();

        private Avalonia.Vector offset;

        public CourseDisplayViewModel(World world)
        {
            this.WorldObjects = new ObservableCollection<WorldObjectViewModel>(world.WorldObjects.Select(wo => new WorldObjectViewModel(wo)));
            this.Width = world.Width;
            this.Height = world.Height;

            world.ViewModelFocus = this;
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public Avalonia.Vector Offset
        {
            get => this.offset;
            set => this.RaiseAndSetIfChanged(ref this.offset, value);
        }

        private DebugStatus debugStatus = new DebugStatus();

        public DebugStatus DebugStatus
        {
            get => this.debugStatus;
            set => this.RaiseAndSetIfChanged(ref this.debugStatus, value);
        }

        public void KeyUp()
        {
            //World.Instance.ControlledCar.Y -= 5;
            World.Instance.ControlledCar.VirtualFunctionBus.PedalPacket.GasPressed = true;
        }

        public void KeyDown()
        {
            //World.Instance.ControlledCar.Y += 5;
            World.Instance.ControlledCar.VirtualFunctionBus.PedalPacket.BreakPressed = true;
        }

        public void KeyLeft()
        {
            World.Instance.ControlledCar.StreeringInputKey(-10);
            World.Instance.ControlledCar.VirtualFunctionBus.SteeringWheelPacket.IsBeingRotated = World.Instance.ControlledCar.VirtualFunctionBus.SteeringWheelPacket.WheelRotation < -20;
        }

        public void KeyRight()
        {
            World.Instance.ControlledCar.StreeringInputKey(10);
            World.Instance.ControlledCar.VirtualFunctionBus.SteeringWheelPacket.IsBeingRotated = World.Instance.ControlledCar.VirtualFunctionBus.SteeringWheelPacket.WheelRotation > 20;
        }

        public void KeyLeftUp()
        {
            World.Instance.ControlledCar.VirtualFunctionBus.SteeringWheelPacket.IsBeingRotated = false;
        }

        public void KeyRightUp()
        {
            World.Instance.ControlledCar.VirtualFunctionBus.SteeringWheelPacket.IsBeingRotated = false;
        }

        public void PageUp()
        {
        }

        public void PageDown()
        {
        }

        public void ToggleDebug()
        {
            this.debugStatus.Enabled = !this.debugStatus.Enabled;
        }

        public void ToggleCamera()
        {
            this.DebugStatus.Camera = !this.DebugStatus.Camera;
        }

        public void ToggleRadar()
        {
            // World.Instance.DebugStatus.Radar = !World.Instance.DebugStatus.Radar;
        }

        public void ToggleUltrasonic()
        {
            //World.Instance.DebugStatus.Ultrasonic = !World.Instance.DebugStatus.Ultrasonic;
        }

        public void ToggleRotation()
        {
            //World.Instance.DebugStatus.Rotate = !World.Instance.DebugStatus.Rotate;
        }

        public void GearReverse()
        {
            if (World.Instance.ControlledCar.VirtualFunctionBus.PowerTrainPacket.RPM <= 1000 && World.Instance.ControlledCar.VirtualFunctionBus.PowerTrainPacket.Speed == 0)
            {
                World.Instance.ControlledCar.carShift.ShiftPacket.CurrentGear = Helpers.Gear.Reverse;
                World.Instance.ControlledCar.carShift.ShiftPacket.GearState = Helpers.Gear.Reverse.ToString();
            }

        }
        public void GearNeutral()
        {
            World.Instance.ControlledCar.carShift.ShiftPacket.CurrentGear = Helpers.Gear.Neutral;
            World.Instance.ControlledCar.carShift.ShiftPacket.GearState = Helpers.Gear.Neutral.ToString();
        }

        public void GearPark()
        {
            if (World.Instance.ControlledCar.VirtualFunctionBus.PowerTrainPacket.RPM <= 1000 && World.Instance.ControlledCar.VirtualFunctionBus.PowerTrainPacket.Speed == 0)
            {
                World.Instance.ControlledCar.carShift.ShiftPacket.CurrentGear = Helpers.Gear.Park;
                World.Instance.ControlledCar.carShift.ShiftPacket.GearState = Helpers.Gear.Park.ToString();
            }
        }

        public void GearDrive()
        {
            if (World.Instance.ControlledCar.carShift.ShiftPacket.CurrentGear==Helpers.Gear.Neutral && World.Instance.ControlledCar.VirtualFunctionBus.PowerTrainPacket.Speed == 0 || World.Instance.ControlledCar.VirtualFunctionBus.PowerTrainPacket.RPM <= 1000 && World.Instance.ControlledCar.VirtualFunctionBus.PowerTrainPacket.Speed == 0)
            {
                World.Instance.ControlledCar.carShift.ShiftPacket.CurrentGear = Helpers.Gear.Drive;
                World.Instance.ControlledCar.carShift.ShiftPacket.GearState = World.Instance.ControlledCar.carShift.ShiftPacket.CurrentShift.ToString();
            }
        }

        public void BreakRelease()
        {
            World.Instance.ControlledCar.VirtualFunctionBus.PedalPacket.BreakPressed = false;
        }

        public void GasRelease()
        {
            World.Instance.ControlledCar.VirtualFunctionBus.PedalPacket.GasPressed = false;
        }

        public void FocusCar(ScrollViewer scrollViewer)
        {
            var offsetX = World.Instance.ControlledCar.X - (scrollViewer.Viewport.Width / 2);
            var offsetY = World.Instance.ControlledCar.Y - (scrollViewer.Viewport.Height / 2);
            this.Offset = new Avalonia.Vector(offsetX, offsetY);
        }
    }
}