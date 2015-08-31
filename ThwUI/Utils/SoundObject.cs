using System;

namespace ThW.UI.Utils
{
    internal class SoundObject : UIObject
    {
        public SoundObject(UIEngine engine, String name) : base("SoundObject", name)
        {
            this.engine = engine;
            this.missing = null == name;
        }

        public void Play()
        {
            if (false == this.missing)
            {
                if (null == this.soundEffect)
                {
                    if (null != this.engine.Audio)
                    {
                        this.soundEffect = this.engine.Audio.LoadSound(this.Name);
                    }
                }

                if (null == this.soundEffect)
                {
                    this.missing = true;
                }
                else
                {
                    this.engine.Audio.PlaySound(this.soundEffect);
                }
            }
        }

        internal int AddRef()
        {
            this.referenceCount++;

            return this.referenceCount;
        }

        internal int Release()
        {
            this.referenceCount--;

            return this.referenceCount;
        }

        private bool missing = false;
        private ISoundEffect soundEffect = null;
        private int referenceCount = 0;
        private UIEngine engine = null;
    }
}
