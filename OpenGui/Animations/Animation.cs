﻿using OpenGui.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Animations
{
    public abstract class Animation : FrameRunner
    {
        int currentDuration;
        bool stopped = false;
        public int Duration
        {
            get;
            private set;
        }
        protected ReactiveObject ReactiveObject
        {
            get;
            private set;
        }

        public Animation(ReactiveObject objectToAnimate, int durationMilliseconds)
        {
            currentDuration = 0;
            Duration = durationMilliseconds;
            ReactiveObject = objectToAnimate;
        }

        protected abstract float ChangeFunction(int time);
        protected abstract void UpdateValue(float value);

        public override bool Stoped()
        {
            return stopped || currentDuration >= Duration;
        }

        public override void Update(float deltaTime)
        {
            currentDuration += (int)(deltaTime * 1000);
            var value = ChangeFunction(currentDuration);
            UpdateValue(value);
        }

        public void Stop()
        {
            stopped = true;
        }
    }
}
