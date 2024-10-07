using System;

public class ThrowAction
{
    public Action Action { get; set; }
    public int? Duration { get; set; }
    public int Delay { get; set; }
    public bool InEffect { get; set; }

    public void Execute()
    {
        TimingFunctions.SetTimeout(() => {
            InEffect = true;
            Action();

            if (Duration.HasValue)
            {
                TimingFunctions.SetTimeout(() => {
                    InEffect = false;
                }, Duration.Value);
            }
        }, Delay);
    }
}