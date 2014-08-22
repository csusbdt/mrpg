using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenAl;
using System.Threading;

namespace Client
{
    class SoundSystem
    {
        #region Fields

        private static float[] listenerPosition;
        private static float volume = 0.5f;
        private static float musicVolume = 0.5f;
        private static float ambientSoundVolume = 0.5f;

        private static List<Music> playingMusic = new List<Music>();
        private static Dictionary<string, Sound> loadedSounds = new Dictionary<string, Sound>();
        private static Dictionary<string, SpatialSound> loadedSpatialSounds = new Dictionary<string, SpatialSound>();
        private static Dictionary<string, AmbientSound> loadedAmbientSounds = new Dictionary<string, AmbientSound>();

        //static int[] sourceNames = new int[1];

        #endregion

        #region Properties

        public static float[] ListenerPosition
        {
            set { listenerPosition = value; }
        }

        public static float Volume
        {
            set { volume = value; }
        }

        public static float MusicVolume
        {
            set { musicVolume = value; }
        }

        public static float AmbientSoundVolume
        {
            set { ambientSoundVolume = value; }
        }

        #endregion

        #region Life Cycle

        public static void Init()
        {
            Log.Write();
            Alut.alutGetError();
            int result = Alut.alutInit();
            if (result != Al.AL_TRUE)
            {
                int error = Alut.alutGetError();
                throw new Exception(Alut.alutGetErrorString(error));
            }
        }

        public static void Shutdown()
        {
            Alut.alutExit();
        }

        public static void Update(TimeSpan dt)
        {
        }

        #endregion

        #region Sound File Loading and Unloading

        // Game code may call the following to directly manage loading 
        // and unloading sound samples.

        static void LoadSound(string soundFile)
        {
            throw new NotImplementedException();
        }

        static void LoadSpatialSound(string soundFile)
        {
            throw new NotImplementedException();
        }

        static void LoadAmbientSound(string soundFile)
        {
            throw new NotImplementedException();
        }

        static void UnloadSound(string soundFile)
        {
            throw new NotImplementedException();
        }

        static void UnloadSpatialSound(string soundFile)
        {
            throw new NotImplementedException();
        }

        static void UnloadAmbientSound(string soundFile)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Tones

        public static void PlayTone(float cyclesPerSecond, float secondsOfDuration)
        {
            int bufferName = Alut.alutCreateBufferWaveform(
                Alut.ALUT_WAVEFORM_SINE, 
                cyclesPerSecond, 
                0, 
                secondsOfDuration);
            int[] sourceNames = new int[1];
            Al.alGenSources(1, sourceNames);
            Al.alSourcei(sourceNames[0], Al.AL_BUFFER, bufferName);
            Al.alSourcePlay(sourceNames[0]);
        }

        #endregion

        #region Simple Sounds

        public static void PlaySound(string soundFile)
        {
            Log.Write();
            //Al.alListener3f(AL_POSITION, 0, 0, 0);
            PlayTone(260, 1);
            //int bufferName = Alut.alutCreateBufferHelloWorld();
            //if (bufferName == Al.AL_NONE)
            //{
            //    int error = Alut.alutGetError();
            //    throw new Exception(Alut.alutGetErrorString(error));
            //}
            //int[] sourceNames = new int[1];
            //Al.alGenSources(1, sourceNames);
            //Al.alSourcei(sourceNames[0], Al.AL_BUFFER, bufferName);
            //Al.alSourcePlay(sourceNames[0]);
            //Al.alDeleteSources(1, sourceNames);
        }

        #endregion

        #region Spatial Sounds

        static void PlaySound(string soundFile, float[] position, float mindistance, float maxdistance)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Ambient Sounds

        static AmbientChannel playAmbientSound(string soundFile, float[] position, float mindistance, float maxdistance)
        {
            throw new NotImplementedException();
        }

        static void StopAmbientSound(AmbientChannel ambientChannel)
        {
            throw new NotImplementedException();
        }

        // Takes a value between 0 and 1.
        static void setAmbientSoundVolume(float volume)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Music

        // will fade out currently playing music if any
        static void PlayMusic(string musicFile)
        {
            throw new NotImplementedException();
        }

        static void StopMusic()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
