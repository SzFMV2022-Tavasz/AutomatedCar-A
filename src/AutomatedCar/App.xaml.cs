namespace AutomatedCar
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Numerics;
    using System.Reflection;
    using AutomatedCar.Models;
    using AutomatedCar.Models.NPC;
    using AutomatedCar.SystemComponents;
    using AutomatedCar.ViewModels;
    using AutomatedCar.Views;
    using Avalonia;
    using Avalonia.Controls.ApplicationLifetimes;
    using Avalonia.Markup.Xaml;
    using Avalonia.Media;
    using Newtonsoft.Json.Linq;

    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (this.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var world = this.CreateWorld();
                desktop.MainWindow = new MainWindow { DataContext = new MainWindowViewModel(world) };
            }

            base.OnFrameworkInitializationCompleted();
        }

        public World CreateWorld()
        {
            var world = World.Instance;

            //this.AddDummyCircleTo(world);

            world.PopulateFromJSON($"AutomatedCar.Assets.test_world.json");

            this.AddControlledCarsTo(world);
            
            this.LoadNPCsInto(world);

            return world;
        }

        private PolylineGeometry GetControlledCarBoundaryBox()
        {
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
    .GetManifestResourceStream($"AutomatedCar.Assets.worldobject_polygons.json"));
            string json_text = reader.ReadToEnd();
            dynamic stuff = JObject.Parse(json_text);
            var points = new List<Point>();
            foreach (var i in stuff["objects"][0]["polys"][0]["points"])
            {
                points.Add(new Point(i[0].ToObject<int>(), i[1].ToObject<int>()));
            }

            return new PolylineGeometry(points, false);
        }

        private void AddDummyCircleTo(World world)
        {
            var circle = new Circle(200, 200, "circle.png", 20);
            
            circle.Width = 40;
            circle.Height = 40;
            circle.ZIndex = 20;
            circle.Rotation = 45;

            world.AddObject(circle);
        }

        private AutomatedCar CreateControlledCar(int x, int y, int rotation, string filename)
        {
            var controlledCar = new Models.AutomatedCar(x, y, filename);
            
            controlledCar.Geometry = this.GetControlledCarBoundaryBox();
            controlledCar.RawGeometries.Add(controlledCar.Geometry);
            controlledCar.Geometries.Add(controlledCar.Geometry);
            controlledCar.RotationPoint = new System.Drawing.Point(54, 120);
            controlledCar.Rotation = rotation;

            controlledCar.Start();

            return controlledCar;
        }

        private void AddControlledCarsTo(World world)
        {
            var controlledCar = this.CreateControlledCar(480, 1425, 0, "car_1_white.png");
            var controlledCar2 = this.CreateControlledCar(4250, 1420, -90, "car_1_red.png");

            world.AddControlledCar(controlledCar);
            world.AddControlledCar(controlledCar2);
        }

        // hard-coded, test values
        private void LoadNPCsInto(World world)
        {
            var positions = new Vector2[] {
            new Vector2(264, 700), new Vector2(264, 530),
            new Vector2(264, 515), new Vector2(272, 467), new Vector2(290, 420), new Vector2(318, 372),
            new Vector2(357, 336), new Vector2(396, 304), new Vector2(437, 280), new Vector2(482, 268),
            new Vector2(520, 263),
            new Vector2(1000, 263), new Vector2(1001, 263),
            };

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] += new Vector2(130, 150);
            }

            var velocities = new float[positions.Length];
            for (int i = 0; i < velocities.Length; i++)
            {
                velocities[i] = 4.0f;
            }
            velocities[0] = 5f;
            velocities[velocities.Length - 3] = 5f;
            velocities[velocities.Length - 2] = 10f;
            velocities[velocities.Length - 1] = 100f;

            // data.Positions = new Vector2[] { new Vector2(10, 10), new Vector2(11, 10) };
            // data.Velocities = new float[] { 1, 1 };

            NPCCar car = new NPCCar(new NPCEngine(positions, velocities), 480, 1425, "car_1_white.png");
            car.RotationPoint = new System.Drawing.Point(54, 240);
            world.AddObject(car);
        }
    }
}