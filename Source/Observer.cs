using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace SGMeterProbe
{
    class Observer : MemoryReader
    {
        public Observer(string processName, int[] offset) : base(processName, processName, false)
        {
            _offset = offset;
        }

        public void Begin()
        {
            NoErrors = true;
            getProcess();

            if (NoErrors)
            {
                pointer = findPointer(_offset);
                ProcessRunning = true;

                LoopThread = new Thread(() => WatchVariable());
                LoopThread.Start();
            }
        }

        private void WatchVariable()
        {
            while (ProcessRunning)
            {
                Text = readMemoryFloat(pointer).ToString();
                Thread.Sleep(50);
            }
        }

        protected override void GameNotOpen()
        {
            base.GameNotOpen();
            NoErrors = false;
        }

        public bool ProcessRunning = false;

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Thread LoopThread;
        private bool NoErrors;
        private int pointer;
        private int[] _offset;
    }
}
