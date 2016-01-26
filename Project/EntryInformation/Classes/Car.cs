
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using EntryInformation.Classes;

[Serializable]
public class Car
{
    private TimeSpan CarStopAtRed;
    public TimeSpan TotalRedTime = new TimeSpan();
    private bool WaitingAtRedLight;

    public virtual int RedLightTime
    {
        get;
        set;
    }

    public void SetTimeRed()
    {
        if (!WaitingAtRedLight)
        {
            WaitingAtRedLight = true;
            CarStopAtRed = DateTime.Now.TimeOfDay;
        }
    }

    public void CalculateRedTimeSpent()
    {
        if (WaitingAtRedLight)
        {
            TimeSpan difference = DateTime.Now.TimeOfDay - CarStopAtRed;
            TotalRedTime += difference;
            WaitingAtRedLight = false;
        }
    }

    public Car(Point location)
    {
        this.X = location.X;
        this.Y = location.Y;
        this.Direction = Direction.Straight;
    }

    public virtual DateTime EndTime
    {
        get;
        set;
    }

    public virtual int CarNumber
    {
        get;
        set;
    }

    public Direction Direction
    {
        get;
        set;
    }

    public virtual int RedStopCounter
    {
        get;
        set;
    }

    public int X
    {
        get;
        set;
    }

    public int Y
    {
        get;
        set;
    }

    public virtual int CrossingID
    {
        get;
        set;
    }

    public virtual void DrawCar()
    {
        throw new System.NotImplementedException();
    }

    public virtual void MoveToThePoint()
    {
        throw new System.NotImplementedException();
    }

    public virtual void CalculateRedlightTime()
    {
        throw new System.NotImplementedException();
    }
}

