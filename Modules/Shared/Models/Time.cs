using Godot;
using System;

/// <summary>
/// A class for storing and processing in game time.
/// </summary>
public class Time
{
    private int _hour;
    public int Hour
    {
        get => _hour;
        set
        {
            if (value < 0)
                _hour = Day.Hours - 1;
            else if (value < Day.Hours)
                _hour = value;
            else
                _hour = 0;
        }
    }

    private int _minute;
    public int Minute
    {
        get => _minute;
        set
        {
            if (value < 0)
            {
                _minute = Constants.HourLength - 1;
                Hour--;
            }
            else if (value < Constants.HourLength)
                _minute = value;
            else
            {
                _minute = 0;
                Hour++;
            }
        }
    }

    public Time() { }

    public Time(int hour, int minute)
    {
        Hour = hour;
        Minute = minute;
    }

    public Time(string time)
    {
        try
        {
            string[] splitTime = time.Split(":");
            Hour = Convert.ToInt32(splitTime[0]);
            Minute = Convert.ToInt32(splitTime[1]);
        }
        catch
        {
            Hour = 0;
            Minute = 0;
        }
    }

    public override string ToString()
    {
        string h = Hour > 9 ? Hour.ToString() : "0" + Hour.ToString();
        string m = Minute > 9 ? Minute.ToString() : "0" + Minute.ToString();
        return $"{h}:{m}";
    }

    public override bool Equals(object obj) => base.Equals(obj);

    public override int GetHashCode() => base.GetHashCode();

    public static bool operator ==(Time a, Time b) => a.Hour == b.Hour && a.Minute == b.Minute;

    public static bool operator !=(Time a, Time b) => a.Hour != b.Hour || a.Minute != b.Minute;

    public static bool operator >(Time a, Time b) => (a.Hour > b.Hour) || (a.Minute > b.Minute && a.Hour >= b.Hour);

    public static bool operator <(Time a, Time b) => (a.Hour < b.Hour) || (a.Minute < b.Minute && a.Hour <= b.Hour);

    public static bool operator >=(Time a, Time b) => (a.Hour >= b.Hour) || (a.Minute >= b.Minute && a.Hour >= b.Hour);

    public static bool operator <=(Time a, Time b) => (a.Hour <= b.Hour) || (a.Minute <= b.Minute && a.Hour <= b.Hour);

    public static Time DeepClone(Time a) => new Time
    {
        Hour = a.Hour,
        Minute = a.Minute,
    };
}