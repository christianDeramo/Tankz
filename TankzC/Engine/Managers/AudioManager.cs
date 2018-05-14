using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Audio;

namespace TankzC
{
    static class AudioManager
    {
        static Dictionary<string, AudioClip> clips;

        public static void Init()
        {
            clips = new Dictionary<string, AudioClip>();
        }

        public static void AddClip(string name, string filePath)
        {
            clips.Add(name, new AudioClip(filePath));
        }

        public static AudioClip GetClip(string name)
        {
            if (clips.ContainsKey(name))
            {
                return clips[name];
            }
            return null;
        }

        public static void RemoveAll()
        {
            clips.Clear();
        }
        
    }
}
