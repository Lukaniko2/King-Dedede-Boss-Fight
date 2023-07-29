
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

/// <summary>
/// Singleton used to play sounds anywhere that they're required.
/// </summary>
public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private float lowerSoundSpeed;

    public List<Sound> Sounds = new List<Sound>();

    private bool musicCanPlay = true;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);

        //Loads in the sounds.
        foreach (Sound sound in Sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.looping;
        }
    }

    //Plays a sound of corresponding name.
    //canPlayMultiple indicates whether the same sound can be played multiple times.
    public void PlaySound(string name, bool canPlayMultiple = false)
    {
        Sound selectedSound = Sounds.Find(s => s.name == name);

        if (selectedSound == null)
        {
            Debug.LogWarning("Sound: " + name + "not found.");
            return;
        }

        if (!selectedSound.source.isPlaying || canPlayMultiple)
        {
            selectedSound.source.Play();
        }
    }

    //Stops the corresponding sound from playing.
    public void StopSound(string name)
    {
        Sound selectedSound = Sounds.Find(s => s.name == name);

        if (selectedSound == null)
        {
            Debug.LogWarning("Sound: " + name + "not found.");
            return;
        }

        if (selectedSound.source.isPlaying)
        {
            selectedSound.source.Stop();
        }
    }

    //Stops All background music from playing
    public void StopMusic()
    {
        foreach(Sound sound in Sounds)
        {
            if(sound.type == Sound.Type.music)
            {
                sound.source.Stop();
            }
        }
    }
    //Changes the volume of all sounds.
    public void SetGameVolume(float newVolume)
    {
        foreach (Sound sound in Sounds)
        {
            sound.source.volume = newVolume * sound.volume;
        }
    }

    //Stops all sounds from playing.
    public void StopAll()
    {
        foreach (Sound sound in Sounds)
        {
            sound.source.Stop();
        }
    }

    public void SetMusic(bool isOn)
    {
        musicCanPlay = isOn;
        if (isOn)
        {
            foreach (Sound sound in Sounds)
            {
                if(sound.type == Sound.Type.music)
                {
                    sound.source.volume = sound.volume;
                }
            }
        }
        else
        {
            foreach (Sound sound in Sounds)
            {
                if(sound.type == Sound.Type.music)
                {
                    sound.source.volume = 0.0f;
                }
            }
        }
    }

    public void LowerMusicVolume()
    {
        StartCoroutine(LoweringMusic());
    }

    IEnumerator<WaitForSeconds> LoweringMusic()
    {
        string bgMusicName = "bgm_bossMusic";

        Debug.Log("InC OUR");
        foreach (Sound sound in Sounds)
        {
            if (sound.type == Sound.Type.music)
            {
                Sound selectedSound = Sounds.Find(s => s.name == bgMusicName);

                while (selectedSound.source.volume > 0.0f)
                {
                    sound.source.volume -= Time.deltaTime;
                    yield return new WaitForSeconds(lowerSoundSpeed);
                }
                
                
            }
        }
     
    }
}
