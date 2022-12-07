// Todo - this is pretty cheeky - it stores a Date and Time in the game world, but will conflict with C# definition - rename if conflicting.
public class DateTime
{
    public Date Date { get; set; }
    public Time Time { get; set; }

    public override string ToString() => $"{Date.ToString()}, {Date.Season.Name} - {Time.ToString()}";
}

