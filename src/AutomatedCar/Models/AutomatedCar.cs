namespace AutomatedCar.Models
{
    using Avalonia.Media;
    using global::AutomatedCar.SystemComponents.Sensors;
    using SystemComponents;

    public class AutomatedCar : Car
    {
        private VirtualFunctionBus virtualFunctionBus;
        private PowerTrain powerTrain;
        public AutomaticGearShift carShift;
        private SteeringWheel steeringWheel;
        private Sensor radar;
        private Sensor camera;
        private Pedal pedal;
        private Sensor radar;
        private Sensor camera;
        private HitBox hitbox;
        public Sensor TempSen { get
            {
                return radar;
            } }

        public AutomatedCar(int x, int y, string filename)
            : base(x, y, filename)
        {
            this.virtualFunctionBus = new VirtualFunctionBus();
            this.ZIndex = 10;
            this.steeringWheel = new SteeringWheel(this.virtualFunctionBus, this);
            this.powerTrain = new PowerTrain(this.virtualFunctionBus, this);
            this.carShift = new AutomaticGearShift(this.virtualFunctionBus);
            this.steeringWheel = new SteeringWheel(this.virtualFunctionBus);
            this.camera = new Camera(World.Instance, this.virtualFunctionBus);
            this.radar = new Radar(World.Instance, this.virtualFunctionBus);
            this.pedal = new Pedal(this.virtualFunctionBus, this);
            this.camera = new Camera(World.Instance, this.virtualFunctionBus);
            this.radar = new Radar(World.Instance, this.virtualFunctionBus);
            this.hitbox = new HitBox(World.Instance, this.virtualFunctionBus);
            
        }

        public VirtualFunctionBus VirtualFunctionBus { get => this.virtualFunctionBus; }

        public Pedal Pedal { get => this.pedal; }

        public int Revolution { get; set; }

        public int Velocity { get; set; }

        public PolylineGeometry Geometry { get; set; }

        public void StreeringInputKey(int rotation)
        {
            this.steeringWheel.RotateWheelByInputRotation(rotation);
        }

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