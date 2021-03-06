﻿
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EntryInformation;

[Serializable]
public class TrafficLight
{
    public System.Timers.Timer greenLightTimer { get; set; }
    public System.Timers.Timer yellowLightTimer { get; set; }
    private int feederID;
    private Crossing crossing;
    private Point redPOint;
    private Point greenPOint;
    public bool SensorClicked { get; set; }
    public bool Pauze { get; set; }//for pauze and stop
    private Simulation simulation;//needed to stop the simulation timer

    public TrafficLight(Crossing crossing, int greenLight, int feederID, Simulation simulation)
    {
        this.simulation = simulation;
        this.feederID = feederID;
        this.GreenLight = greenLight;
        this.crossing = crossing;

        greenLightTimer = new System.Timers.Timer();
        yellowLightTimer = new System.Timers.Timer();

        yellowLightTimer.Interval = (this.GreenLight * 100);
        yellowLightTimer.Elapsed += yellowLightTimer_Elapsed;

        greenLightTimer.Interval = (this.GreenLight*1000);
        greenLightTimer.Elapsed += greenLightTimer_Elapsed;

        switch (this.feederID)
        {
            case 1:
                redPOint = new Point(71,96);
                greenPOint = new Point(65, 96);
                break;
            case 2:
                redPOint = new Point(96, 71);
                greenPOint = new Point(96, 65);
                break;
            case 3:
                redPOint = new Point(123, 96);
                greenPOint = new Point(128, 96);
                break;
            case 4:
                redPOint = new Point(96, 123);
                greenPOint = new Point(96, 128);
                break;
        }
    }

    public void DrawGreenLight(PaintEventArgs e)
    {
        e.Graphics.FillEllipse(Brushes.Green, greenPOint.X, greenPOint.Y, 8, 8);
    }

    public void DrawRedLight(PaintEventArgs e)
    {
        e.Graphics.FillEllipse(Brushes.Red, redPOint.X, redPOint.Y, 8, 8);
    }

    void yellowLightTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        yellowLightTimer.Stop();
        if (!SensorClicked && !Pauze)
            crossing.Feeders[(feederID % 4)].trafficLight.greenLightTimer.Start();
        else if (SensorClicked)
        {
            (crossing as CrossingA).SensorTimer.Start();
            SensorClicked = false;
        }
        else if (Pauze)
        {
            simulation.PauzeCountDown--;
            if (simulation.PauzeCountDown == 0)
            {
                simulation.MoveCarsTimer.Stop();
                simulation.BeginInvoke(new MethodInvoker(delegate
                {
                    simulation.buttonStart.Enabled = true;
                }));
            }
            Pauze = false;
        }
    }

    void greenLightTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        greenLightTimer.Stop();
        
        yellowLightTimer.Start();
    }
	public int GreenLight
	{
		get;
		set;
	}

	public virtual bool State
	{
		get;
		set;
	}

	public virtual int Time
	{
		get;
		set;
	}

	public virtual void SetState()
	{
		throw new System.NotImplementedException();
	}

	public virtual void GetState()
	{
		throw new System.NotImplementedException();
	}

}

