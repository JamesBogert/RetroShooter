using NUnit.Framework;
using SaintsField.Playa;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu (fileName = "SFXManagerSO",menuName = "AudioSO/SFXManager")]
public class SoundFXManager : ScriptableObject
{
    public static SoundFXManager instance;
    [SerializeField] private float pitchRange;

    [ShowInInspector] public Sound[] sounds;
    [SerializeField] private AudioSource source;

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }

    public void PlaySFXClip(string name, Transform spawnTransform)
    {
        Sound sound = FindSound(name);
        if (sound == null) return;

        AudioSource audioSource = Instantiate(source, spawnTransform.position, Quaternion.identity);
        audioSource.clip = sound.clip;
        audioSource.volume = sound.volume;
        audioSource.pitch = sound.pitch + Random.Range(-pitchRange, pitchRange);
        audioSource.Play();

        float length = audioSource.clip.length;

        Destroy(audioSource.gameObject, length);
    }

    Sound FindSound(string name)
    {
        foreach (Sound snd in sounds)
        {
            if (name == snd.soundName)
            {
                return snd;
            }
        }
        return null;
    }
}
