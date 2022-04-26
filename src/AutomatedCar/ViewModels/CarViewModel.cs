namespace AutomatedCar.ViewModels
{
    using AutomatedCar.Models;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Text;
    using System.Threading.Tasks;
    using SystemComponents;
    using SystemComponents.Packets;

    public class CarViewModel : WorldObjectViewModel
    {
        public AutomatedCar Car { get; set; }
        public ACCController Acc { get; set; }

        public ReactiveCommand<Unit, Unit> ACCButtonCommand { get; }
        public CarViewModel(AutomatedCar car) : base(car)
        {
            this.Car = car;
            this.Acc = this.Car.ACCController;
            ACCButtonCommand = ReactiveCommand.Create(this.TurnOnOff);
            
        }
        
        void TurnOnOff()
        {
            this.Acc.ControllerPacket.Enabled = !this.Acc.ControllerPacket.Enabled;
        }
    }
}
