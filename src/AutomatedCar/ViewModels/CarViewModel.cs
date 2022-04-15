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
    using SystemComponents.Packets;

    public class CarViewModel : WorldObjectViewModel
    {
        public AutomatedCar Car { get; set; }
        public ACCControllerPrototype Acc { get; set; }

        public ReactiveCommand<Unit, Unit> DoTheThing { get; }
        public CarViewModel(AutomatedCar car) : base(car)
        {
            this.Car = car;
            this.Acc = new ACCControllerPrototype();
            DoTheThing = ReactiveCommand.Create(DoSomeThing);
            
        }
        
        void DoSomeThing()
        {
            this.Acc.TurnOnOrOff();
        }
    }
}
