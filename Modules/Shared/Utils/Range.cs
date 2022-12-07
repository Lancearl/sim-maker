// Todo - swap this out for System.Range when the Godot .NET version gets updated to 8.0 or above.
public class Range
{
    private readonly float minValue;
    private readonly float maxValue;

    public Range(float minValue, float maxValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    public bool InRange(float value) => value >= minValue && value <= maxValue;

    public override string ToString() => $"({minValue},{maxValue})";
}
