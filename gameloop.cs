using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Drawing;

namespace maze_iteration_1
{
    internal class gameloop
    {
        private DispatcherTimer timer;
        private DateTime previousTime;
        public bool running { get; private set; }

        public gameloop()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10); //120 fps interval roughly
            timer.Tick += onTick;
        }

        public void start()
        {
            running = true;

            previousTime = DateTime.Now;
            timer.Start();
        }
        public void stop()
        {
            running = false;
            previousTime = DateTime.Now;
            timer.Stop();
        }

        private void onTick(object sender, EventArgs e)
        {
            if (!running) { return; }

            TimeSpan gameTime = DateTime.Now - previousTime;
            previousTime = DateTime.Now;
            Application.Current.MainWindow.InvalidateVisual();
        }

        
    }
}
