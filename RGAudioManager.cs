using System;
using UnityEngine;
using RGUnityTools.GameSettings;

namespace RGUnityTools
{
    public class RGAudioManager : MonoBehaviour
    {
        public static RGAudioManager instance;

        public bool allowDuplicateNaming = false;

        public string defaultTrack;
        public Playlist[] playlists;

        #region UNITY_METHODS
        void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            foreach (Playlist p in playlists)
            {
                foreach (Sound s in p.sounds)
                {
                    s.source = gameObject.AddComponent<AudioSource>();
                    s.source.clip = s.clip;

                    s.source.pitch = s.pitch;
                    s.source.loop = s.loop;
                }
            }
        }

        void Start()
        {
            if (RGGameSettingsUI.Instance != null)
                RGGameSettingsUI.Instance.OnSettingsChanged += OnSettingsChanged;

            Play(defaultTrack);
        }
        #endregion

        #region CORE_METHODS
        public void Play(string audio)
        {
            foreach (Playlist p in playlists)
            {
                foreach (Sound s in p.sounds)
                {
                    if (s.name != audio) continue;
                    s.source.Play();
                    return;
                }
            }
        }

        public void Stop()
        {
            foreach (Playlist p in playlists)
            {
                foreach (Sound s in p.sounds)
                {
                    s.source.Stop();
                }
            }
        }

        public void Stop(string playlist)
        {
            foreach (Playlist p in playlists)
            {
                if (p.name != playlist) continue;
                foreach (Sound s in p.sounds)
                {
                    s.source.Stop();
                }
            }
        }

        public void Mute(bool mute = true)
        {
            foreach (Playlist p in playlists)
            {
                foreach (Sound s in p.sounds)
                {
                    s.source.mute = mute;
                }
            }
        }

        public void SetVolumeSettings(float newMasterVolume, float newMusicVolume, float newEffectsVolume)
        {
            foreach (Playlist p in playlists)
            {
                switch (p.type)
                {
                    case PlaylistType.Music:
                        foreach (Sound s in p.sounds)
                        {
                            if (newMusicVolume <= 1 && newMusicVolume >= 0)
                                s.source.volume = newMasterVolume * newMusicVolume * s.volume;
                        }
                        continue;
                    case PlaylistType.SoundFX:
                        foreach (Sound s in p.sounds)
                        {
                            if (newEffectsVolume <= 1 && newEffectsVolume >= 0)
                                s.source.volume = newMasterVolume * newEffectsVolume * s.volume;
                        }
                        continue;
                    default:
                        continue;
                }
            }
        }
        #endregion

        #region EVENTS
        void OnSettingsChanged(RGGameSettings settings)
        {
            SetVolumeSettings(settings.masterVolume, settings.musicVolume, settings.effectsVolume);
        }
        #endregion
    }

    [Serializable]
    public class Sound
    {
        public string name = "New Sound";
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = .8f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;
        public bool loop = false;
        [HideInInspector]
        public AudioSource source;
    }

    [Serializable]
    public class Playlist
    {
        public string name;
        public PlaylistType type;
        public Sound[] sounds;
    }

    public enum PlaylistType { Music, SoundFX, Speech }
}
