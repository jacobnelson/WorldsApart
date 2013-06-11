using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace WorldsApart.Code.Controllers
{
    class AudioManager
    {
        public static AudioEngine audioEngine;
        public static WaveBank waveBank;
        public static SoundBank soundBank;
        public static Cue cue;

        public static SoundEffect playerJump;
        public static SoundEffect playerLand;
        public static SoundEffect playerPsyActivate;
        public static SoundEffect playerPsyDeactivate;
        public static SoundEffect playerDie;
        public static SoundEffect playerRevive;
        public static SoundEffect switchActivate;
        public static SoundEffect switchDeactivate;
        public static SoundEffect doorOpen;
        public static SoundEffect doorClose;
        public static SoundEffect objectDestroyed;
        public static SoundEffect portalItem;
        public static SoundEffect portal;
        public static SoundEffect bridgeBarrier;
        public static SoundEffect bridgeBreaking;
        public static SoundEffect worldShatter;

        static public void InitializeAudioManager(ContentManager cm)
        {
            audioEngine = new AudioEngine("Content//Music//music.xgs");
            waveBank = new WaveBank(audioEngine, "Content//Music//Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content//Music//Sound Bank.xsb");

            //TODO: initialize SoundEffects
        }

        static public void StopMusic()
        {
            if (cue != null)
            {
                cue.Stop(AudioStopOptions.Immediate);
            }
        }

        static public void PlayMusic(string name)
        {
            StopMusic();

            cue = soundBank.GetCue(name);
            cue.Play();
        }
    }
}
