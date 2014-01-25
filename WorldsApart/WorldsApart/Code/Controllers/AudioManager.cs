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
        public static SoundEffect checkpoint;
        public static SoundEffect signal;

        public static SoundEffect pause;
        public static SoundEffect menuMove;
        public static SoundEffect menuSelect;

        static public void InitializeAudioManager(ContentManager cm)
        {
            audioEngine = new AudioEngine("Content//Music//music.xgs");
            waveBank = new WaveBank(audioEngine, "Content//Music//Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content//Music//Sound Bank.xsb");

            //TODO: initialize SoundEffects
            worldShatter = cm.Load<SoundEffect>("SFX/DimensionShatter");
            doorClose = cm.Load<SoundEffect>("SFX/DoorClose");
            doorOpen = cm.Load<SoundEffect>("SFX/DoorOpen");
            portal = cm.Load<SoundEffect>("SFX/EnterPortal");
            bridgeBarrier = cm.Load<SoundEffect>("SFX/HitBarrier");             
            objectDestroyed = cm.Load<SoundEffect>("SFX/ObjectDestroyed");      
            playerDie = cm.Load<SoundEffect>("SFX/PlayerDie");                 
            playerRevive = cm.Load<SoundEffect>("SFX/PlayerRevive");
            portalItem = cm.Load<SoundEffect>("SFX/PortalItemGet");
            switchActivate = cm.Load<SoundEffect>("SFX/SwitchActivate");        
            switchDeactivate = cm.Load<SoundEffect>("SFX/SwitchDeactivate");    
            bridgeBreaking = cm.Load<SoundEffect>("SFX/ObjectDestroyed");       //Change 
            playerJump = cm.Load<SoundEffect>("SFX/PlayerJump");                  
            playerLand = cm.Load<SoundEffect>("SFX/PlayerLand");                  
            playerPsyActivate = cm.Load<SoundEffect>("SFX/PsyActivate");        
            playerPsyDeactivate = cm.Load<SoundEffect>("SFX/PsyDeactivate");      
            checkpoint = worldShatter;                                          //Change?
            signal = bridgeBarrier;                                             //Do  -> Change?

            pause = cm.Load<SoundEffect>("SFX/pause");                          //Change
            //menuMove = cm.Load<SoundEffect>("SFX/menuBlip");
            menuMove = bridgeBarrier;
            //menuSelect = cm.Load<SoundEffect>("SFX/menuSelect");
            menuSelect = worldShatter;
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
