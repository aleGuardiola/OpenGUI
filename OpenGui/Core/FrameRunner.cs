using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Core
{
    public abstract class FrameRunner
    {
        bool wasRunning = false;
        bool started = false;
        bool stopped = false;

        public event EventHandler Start;
        public event EventHandler Stop;        
        public abstract void Update(float deltaTime);

        /// <summary>
        /// return true if stopped running.
        /// </summary>
        /// <returns></returns>
        protected virtual bool Stoped()
        {
            return stopped;
        }

        /// <summary>
        /// Called before the first Update call.
        /// </summary>
        public virtual void Initialize()
        {
            started = true;
            wasRunning = true;
            Start?.Invoke(this, new EventArgs());
        }

        public bool IsStop()
        {
            var value = Stoped();
            if(value && wasRunning)
            {
                Stop?.Invoke(this, new EventArgs());
                wasRunning = false;
            }
                        
            return value;
        }

        /// <summary>
        /// Stop running.
        /// </summary>
        public void ForceStop()
        {
            stopped = true;
        }
        
        /// <summary>
        /// Return true if the is running.
        /// </summary>
        /// <returns></returns>
        public bool IsRunning()
        {
            return !Stoped() && started;
        }
        
    }
}
