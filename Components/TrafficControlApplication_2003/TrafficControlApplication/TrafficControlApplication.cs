using System;
using System.Windows.Forms;

namespace TrafficControlApplication
{
    class TrafficControlApplication
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new ControlForm1());
        }
    }
}