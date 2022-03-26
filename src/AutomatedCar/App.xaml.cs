namespace AutomatedCar
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Numerics;
    using System.Reflection;
    using AutomatedCar.Helpers;
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
            NPCWaypointSerializer cwp = new NPCWaypointSerializer();
            NPCWaypointSerializer pwp = new NPCWaypointSerializer();
            cwp.Deserialize("../../../Assets/teszt.csv");
            pwp.Deserialize("../../../Assets/testmap_pedestrian.csv");

            var cpositions = new Vector2[cwp.Data.Length];
            var cvelocities = new float[cwp.Data.Length];

            var ppositions = new Vector2[pwp.Data.Length];
            var pvelocities = new float[pwp.Data.Length];

            for (int i = 0; i < cwp.Data.Length; i++)
            {
                cpositions[i] = new Vector2(float.Parse(cwp.Data[i][0]), float.Parse(cwp.Data[i][1]));
                cvelocities[i] = float.Parse(cwp.Data[i][2]);
            }

            for (int i = 0; i < pwp.Data.Length; i++)
            {
                ppositions[i] = new Vector2(float.Parse(pwp.Data[i][0]), float.Parse(pwp.Data[i][1]));
                pvelocities[i] = float.Parse(pwp.Data[i][2]);
            }

            NPCCar car = new NPCCar(int.Parse(cwp.Data[0][0]), int.Parse(cwp.Data[0][1]), "car_1_white.png");
            car.RotationPoint = new System.Drawing.Point(54, 180);

            NPCPed ped = new NPCPed(int.Parse(pwp.Data[0][0]), int.Parse(pwp.Data[0][1]), "man.png");
            ped.RotationPoint = new System.Drawing.Point(20, 36);

            car.NPCStatus.Positions = cpositions;
            car.NPCStatus.Velocities = cvelocities;

            car.NPCStatus.CurrentPosition = car.NPCStatus.Positions[0];

            car.NPCStatus.Rotations = new double[car.NPCStatus.Positions.Length];
            for (int i = 0; i < car.NPCStatus.Positions.Length; i++)
            {
                var vector = car.NPCStatus.Positions[(i + 1) % car.NPCStatus.Positions.Length] - car.NPCStatus.Positions[i];

                var degree = Math.Atan2(vector.Y, vector.X);
                car.NPCStatus.Rotations[i] = (degree * (180 / Math.PI)) + 90;
            }

            ped.NPCStatus.Positions = ppositions;
            ped.NPCStatus.Velocities = pvelocities;

            ped.NPCStatus.CurrentPosition = ped.NPCStatus.Positions[0];

            ped.NPCStatus.Rotations = new double[ped.NPCStatus.Positions.Length];
            for (int i = 0; i < ped.NPCStatus.Positions.Length; i++)
            {
                var vector = ped.NPCStatus.Positions[(i + 1) % ped.NPCStatus.Positions.Length] - ped.NPCStatus.Positions[i];

                var degree = Math.Atan2(vector.Y, vector.X);
                ped.NPCStatus.Rotations[i] = (degree * (180 / Math.PI)) + 90;
            }

            world.AddObject(car);
            world.AddObject(ped);
            NPCEngine engine = new NPCEngine(new List<INPC>() { car, ped});
            engine.Start();
        }
    }
}