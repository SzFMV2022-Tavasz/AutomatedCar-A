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

        /// <summary>
        /// Method to load npcs into world.
        /// </summary>
        /// <param name="world">World.</param>
        private void LoadNPCsInto(World world)
        {
            List<NPC> npcs = new List<NPC>();

            NPC car = this.CreateNPC(new System.Drawing.Point(54, 180), "../../../Assets/test_world_car.csv", "car_3_black.png");
            NPC ped = this.CreateNPC(new System.Drawing.Point(20, 36), "../../../Assets/test_world_pedestrian.csv", "man.png");
            car.WorldObjectType = WorldObjectType.Pedestrian;
            car.WorldObjectType = WorldObjectType.Car;

            car.Geometries.Add(this.GetNPCCarBoundaryBox());
            car.RawGeometries.Add(this.GetNPCCarBoundaryBox());

            ped.Geometries.Add(this.GetPedestrianBoundaryBox());
            ped.RawGeometries.Add(this.GetPedestrianBoundaryBox());

            world.AddObject(car);
            world.AddObject(ped);

            npcs.Add(car);
            npcs.Add(ped);

            NPCEngine engine = new NPCEngine(npcs);
            engine.Start();
        }

        /// <summary>
        /// Method to create an npc.
        /// </summary>
        /// <param name="rotationPoint">Npc's rotation point basesd on the image.</param>
        /// <param name="npcWayPoints">File containing the waypoints of the nps's path.</param>
        /// <param name="pictureName">Image of the npc.</param>
        /// <returns>Npc fully created..</returns>
        private NPC CreateNPC(System.Drawing.Point rotationPoint, string npcWayPoints, string pictureName)
        {
            NPCWaypointSerializer wps = new NPCWaypointSerializer();

            wps.Deserialize(npcWayPoints);

            var positions = new Vector2[wps.Data.Length];
            var velocities = new float[wps.Data.Length];

            for (int i = 0; i < wps.Data.Length; i++)
            {
                positions[i] = new Vector2(float.Parse(wps.Data[i][0]), float.Parse(wps.Data[i][1]));
                velocities[i] = float.Parse(wps.Data[i][2]);
            }

            NPC npc = new NPC(int.Parse(wps.Data[0][0]), int.Parse(wps.Data[0][1]), pictureName);

            npc.RotationPoint = rotationPoint;
            npc.NPCStatus.Positions = positions;
            npc.NPCStatus.Velocities = velocities;
            npc.NPCStatus.CurrentPosition = npc.NPCStatus.Positions[0];
            npc.NPCStatus.Rotations = new double[npc.NPCStatus.Positions.Length];

            for (int i = 0; i < npc.NPCStatus.Positions.Length; i++)
            {
                var vector = npc.NPCStatus.Positions[(i + 1) % npc.NPCStatus.Positions.Length] - npc.NPCStatus.Positions[i];
                var degree = Math.Atan2(vector.Y, vector.X);
                npc.NPCStatus.Rotations[i] = (degree * (180 / Math.PI)) + 90;
            }

            for (int i = 0; i < npc.NPCStatus.Velocities.Length; i++)
            {
                npc.NPCStatus.Velocities[i] *= 1000.0f / (60 * 60);
            }

            return npc;
        }

        /// <summary>
        /// Creating hitbox for the npc car.
        /// </summary>
        /// <returns>Hitbox.</returns>
        private PolylineGeometry GetNPCCarBoundaryBox()
        {
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
    .GetManifestResourceStream($"AutomatedCar.Assets.worldobject_polygons.json"));
            string json_text = reader.ReadToEnd();
            dynamic stuff = JObject.Parse(json_text);
            var points = new List<Point>();
            foreach (var i in stuff["objects"][6]["polys"][0]["points"])
            {
                points.Add(new Point(i[0].ToObject<int>(), i[1].ToObject<int>()));
            }

            return new PolylineGeometry(points, false);
        }

        /// <summary>
        /// Creating hitbox for the pedestrian.
        /// </summary>
        /// <returns>Hitbox.</returns>
        private PolylineGeometry GetPedestrianBoundaryBox()
        {
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
    .GetManifestResourceStream($"AutomatedCar.Assets.worldobject_polygons.json"));
            string json_text = reader.ReadToEnd();
            dynamic stuff = JObject.Parse(json_text);
            var points = new List<Point>();
            foreach (var i in stuff["objects"][28]["polys"][0]["points"])
            {
                points.Add(new Point(i[0].ToObject<int>(), i[1].ToObject<int>()));
            }

            return new PolylineGeometry(points, false);
        }
    }
}