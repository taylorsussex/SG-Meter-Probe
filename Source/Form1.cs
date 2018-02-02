using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace SGMeterProbe
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            obsX = new Observer("Skullgirls", new int[3] { 0x003C9004, 0x2B8, 0x100 });
            obsY = new Observer("Skullgirls", new int[3] { 0x003C9004, 0x2B4, 0x100 });

            obsX.PropertyChanged += UpdateCursorX;
            obsY.PropertyChanged += UpdateCursorY;
            FormClosing += EndProbe;
        }

        private void EndProbe(object sender, FormClosingEventArgs e)
        {
            obsX.ProcessRunning = false;
            obsY.ProcessRunning = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            obsX.Begin();
            obsY.Begin();
        }

        private void UpdateCursorX(object sender, PropertyChangedEventArgs e)
        {
            cursorX.BeginInvoke((Action)delegate () { cursorX.Text = obsX.Text; });
        }

        private void UpdateCursorY(object sender, PropertyChangedEventArgs e)
        {
            cursorY.BeginInvoke((Action)delegate () { cursorY.Text = obsY.Text; });
        }

        private Observer obsX;
        private Observer obsY;
    }
}
