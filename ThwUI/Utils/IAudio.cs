using System;

namespace ThW.UI.Utils
{
    public interface IAudio
    {
        ISoundEffect LoadSound(String name);
        void PlaySound(ISoundEffect sound);
    }
}
