using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThW.UI.Design;

namespace ThW.UI.Utils
{
    public delegate void PropertyAnimationEndedHandler();

    public abstract class PropertyAnimation
    {
        public abstract bool Animate(double dt);
    }

    public class LinearPropertyAnimation : PropertyAnimation
    {
        public LinearPropertyAnimation(int startValue, int endValue, double duration, SetValueHandler<int> propertySetter)
        {
            this.animating = (startValue != endValue);
            this.startPos = startValue;
            this.endPos = endValue;
            this.animationTime = (int)(duration * 1000);
            this.animationTimeElapsed = 0;
            this.propertySetter = propertySetter;
        }

        public override bool Animate(double dt)
        {
            if (true == this.animating)
            {
                int dx = this.endPos;

                this.animationTimeElapsed += (int)(dt * 1000);

                float t = (float)(this.animationTimeElapsed) / (float)(this.animationTime);

                if (t <= 1.0f)
                {
                    if (startPos != endPos)
                    {
                        //dx = (int)(startPos + (float)(endPos - startPos) * t);
                        //dx = f_aa(t, startPos, endPos, 1);

                        dx = (int)((float)startPos + (float)(endPos - startPos) * (1.0 - Math.Sin(Math.PI / 2 + t * Math.PI)) / 2);
                    }
                }
                else
                {
                    this.animating = false;
                }

                this.propertySetter(dx);

                //this.animating = (this.startPos != this.endPos);

                if (false == this.animating)
                {
                    if (null != this.AnimationEnded)
                    {
                        this.AnimationEnded();
                    }
                }
            }

            return !this.animating;
        }

        static int f_linear(float t, float b, float c, float d)
        {
            return 0;
        }

        static int f_aa(float t, float b, float c, float d)
        {
	        float ts = (t/d) * t;
	        float tc=ts*t;
	        return (int)(b+c*(66.1925*tc*ts + -183.48*ts*ts + 176.08*tc + -67.49*ts + 9.6975*t));
        }

        private int startPos = 0;
        private int endPos = 0;
        private bool animating = false;
        private int animationTime = 0;
        private int animationTimeElapsed = 0;
        private SetValueHandler<int> propertySetter = null;
        public event PropertyAnimationEndedHandler AnimationEnded = null;
    }
}
   