namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Helpers;

    public interface IGearShiftPacket
    {
        Gear CurrentGear { get; set; }

        Shifts CurrentShift { get; set; }

    }
}
