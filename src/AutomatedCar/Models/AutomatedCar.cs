namespace AutomatedCar.Models
{
    using Avalonia.Media;
    using SystemComponents;

    public class AutomatedCar : Car
    {
        private VirtualFunctionBus virtualFunctionBus;
        private PowerTrain powerTrain;
        public AutomaticGearShift carShift;
        private SteeringWheel steeringWheel;
        private Pedal pedal;

        public AutomatedCar(int x, int y, string filename)
            : base(x, y, filename)
        {
            this.virtualFunctionBus = new VirtualFunctionBus();
            this.ZIndex = 10;
            this.powerTrain = new PowerTrain(this.virtualFunctionBus, this);
            this.carShift = new AutomaticGearShift(this.virtualFunctionBus);
            this.steeringWheel = new SteeringWheel(this.virtualFunctionBus);
            this.pedal = new Pedal(this.virtualFunctionBus, this);
        }

        public VirtualFunctionBus VirtualFunctionBus { get => this.virtualFunctionBus; }

        public SteeringWheel SteeringWheel { get => this.steeringWheel; }

        public Pedal Pedal { get => this.pedal; }

        public int Revolution { get; set; }

        public int Velocity { get; set; }

        public PolylineGeometry Geometry { get; set; }

        /// <summary>Starts the automated cor by starting the ticker in the Virtual Function Bus, that cyclically calls the system components.</summary>
        public void Start()
        {
            this.virtualFunctionBus.Start();
        }

        /// <summary>Stops the automated cor by stopping the ticker in the Virtual Function Bus, that cyclically calls the system components.</summary>
        public void Stop()
        {
            this.virtualFunctionBus.Stop();
        }
    }
}