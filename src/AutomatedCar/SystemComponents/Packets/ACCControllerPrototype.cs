namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;

    public class ACCControllerPrototype:ReactiveObject
    {

        private bool isOn = false;

        public bool IsOn
        {
            get => this.isOn;
            set => this.RaiseAndSetIfChanged(ref this.isOn, value);
        }

        public void TurnOnOrOff()
        {
            this.IsOn = !this.IsOn;
        }
    }
}