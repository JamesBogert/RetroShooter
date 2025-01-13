using SaintsField.Playa;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "SFXManagerSO",menuName = "AudioSO/SFXManager")]
public class SoundFXManager : ScriptableObject
{
    public static SoundFXManager instance;
    [SerializeField] private float pitchRange;

    [ShowInInspector] public Sound[] gunSounds;
    [ShowInInspector] public Sound[] enemySounds;
    [ShowInInspector] public Sound[] ambienceSounds;
    [ShowInInspector] public Sound[] playerSounds;
    [ShowInInspector] public Sound[] miscSounds;
    //list of arrays
    private List<Sound[]> sounds;
    [SerializeField] private AudioSource source;

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }

    public void PlaySFXClip(string name, Vector3 spawnPosition)
    {
        Sound sound = FindSound(name);
        if (sound == null) return;

        AudioSource audioSource = Instantiate(source, spawnPosition, Quaternion.identity);
        audioSource.clip = sound.clip;
        audioSource.volume = sound.volume;
        audioSource.pitch = sound.pitch + Random.Range(-pitchRange, pitchRange);
        audioSource.loop = sound.loop;
        audioSource.Play();

        float length = audioSource.clip.length;

        Destroy(audioSource.gameObject, length);
    }

    Sound FindSound(string name)
    {

       foreach (Sound snd in gunSounds)
       {
           if (name == snd.soundName)
           {
                    return snd;
           }
       }
       foreach (Sound snd in enemySounds)
       {
           if (name == snd.soundName)
           {
                    return snd;
           }
       }
       foreach (Sound snd in ambienceSounds)
       {
           if (name == snd.soundName)
           {
                    return snd;
           }
       }
        foreach (Sound snd in playerSounds)
        {
            if (name == snd.soundName)
            {
                return snd;
            }
        }
        foreach (Sound snd in miscSounds)
       {
           if (name == snd.soundName)
           {
                    return snd;
           }
       }

        return null;
    }
}
