using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private StringValue currentScene;
    private string currentTheme;
    public Sound[] sounds;
    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake ()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.playOnAwake = s.playOnAwake;
        }
        currentTheme = currentScene.element;
        Play(currentScene.element + " Theme");
    }

    private void Update()
    {
        if (currentScene.element != currentTheme)
        {
            Debug.Log(currentScene.element + " Theme");
            stopAllThemes();
            Play(currentScene.element + " Theme");
            currentTheme = currentScene.element;
        }
    }

    public AudioSource findAudioSource(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
        }
        return s.source;
    }

    public bool isPlaying(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
        }
        if (s.source.isPlaying)
            return true;
        else return 
            false;
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        changeVolumePitch(s, s.volume, s.pitch);
        //Too lazy to trim audio files
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
        }
        else 
        {
            if (s.name.Contains("breakingvase"))
            {
                s.source.time = 0.3f;
            }
            else if (s.name == "damage1")
            {
                s.source.time = 0.4f;
            }
            else if (s.name == "damage2")
            {
                s.source.time = 0.7f;
            }
            else if (s.name == "Rock-smash")
            {
                s.source.time = 0.2f;
            }
            else if (s.name == "ding")
            {
                s.source.time = 0.3f;
            }


            s.source.Play();
        }
    }

    public void PlayWithSettings (string name, float volume, float pitch)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        //Too lazy to trim audio files
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
        }
        else
        {
            changeVolumePitch(s, s.volume, s.pitch);
            if (s.name.Contains("breakingvase"))
            {
                s.source.time = 0.3f;
            }
            else if (s.name == "damage1")
            {
                s.source.time = 0.4f;
            }
            else if (s.name == "damage2")
            {
                s.source.time = 0.7f;
            }
            else if (s.name == "Rock-smash")
            {
                s.source.time = 0.2f;
            }
            else if (s.name == "ding")
            {
                s.source.time = 0.3f;
            }

            changeVolumePitch(s, volume, pitch);

            s.source.Play();
        }
    }

    private void changeVolumePitch(Sound s, float volume, float pitch)
    {
        s.source.volume = volume;
        s.source.pitch = pitch;
    }

    public void Stop (string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
        }
        s.source.Stop();
    }

    public void stopAllThemes ()
    {
        foreach (Sound s in sounds)
        {
            if (s.name.Contains("Theme"))
            {
                s.source.Stop();
            }
        }
    }

    public void PlayRandomPitch (string name, float low, float high)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
        }
        s.source.pitch = UnityEngine.Random.Range(low, high);
        s.source.Play();
    }

    public void startLoop (string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
        }
        s.source.loop = true;
    }

    public void stopLoop(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
        }
        s.source.loop = true;
    }

    //public void playSceneTheme(string currentScene)
    //{
    //    Sound currentTheme = Array.Find(sounds, Sound => Sound.name == "Auburn Theme");

    //    if (currentScene == "Auburn")
    //    {
    //        Sound s = Array.Find(sounds, Sound => Sound.name == "Auburn Theme");
    //        if (s == null)
    //        {
    //            Debug.LogWarning("Sound: " + name + " not found");
    //        }
    //        currentTheme = s;
    //    }

    //    if (currentScene == "Home")
    //    {
    //        Sound s = Array.Find(sounds, Sound => Sound.name == "Home Theme");
    //        if (s == null)
    //        {
    //            Debug.LogWarning("Sound: " + name + " not found");
    //        }
    //        currentTheme = s;
    //    }

    //    if (currentScene == "Dungeon")
    //    {
    //        Sound s = Array.Find(sounds, Sound => Sound.name == "Dungeon Theme");
    //        if (s == null)
    //        {
    //            Debug.LogWarning("Sound: " + name + " not found");
    //        }
    //        currentTheme = s;
    //    }

    //    if (currentScene == "Rosewood_Forest")
    //    {
    //        Sound s = Array.Find(sounds, Sound => Sound.name == "Rosewood_Forest Theme");
    //        if (s == null)
    //        {
    //            Debug.LogWarning("Sound: " + name + " not found");
    //        }
    //        currentTheme = s;
    //    }
    //    currentTheme.source.Play();
    //}
}
