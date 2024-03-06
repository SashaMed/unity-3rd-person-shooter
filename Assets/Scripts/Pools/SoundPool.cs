using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPool : PoolBase
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource buttonAudioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip takeSounds;

    private float lastVolume = 0.75f;
    public bool IsSoundsMuted { get; private set; } = false;

    public static SoundPool SoundInstance { get; private set; }

    protected override void Awake()
    {
        if (SoundInstance != null)
        {
            Debug.LogError("More than one GameData. The newest one destroyed.");
        }
        SoundInstance = this;

    }


    public void PlayVFXSound(AudioClip sound, Vector3 pos)
    {
        if (!IsSoundsMuted)
        {

            var sourceGameObject = GetFromPool(pos);
            var selectedSource = sourceGameObject.GetComponent<AudioSource>();
            selectedSource.volume = lastVolume;
            selectedSource.clip = sound;
            selectedSource.pitch = Random.Range(0.8f, 1.5f);
            selectedSource.Play();
            StartCoroutine(ReturnAudioSourceToQueue(selectedSource));

        }
    }

    public void PlayVFXSound(AudioClip sound, Vector3 pos, float volume)
    {
        if (!IsSoundsMuted)
        {

            var sourceGameObject = GetFromPool(pos);
            var selectedSource = sourceGameObject.GetComponent<AudioSource>();
            selectedSource.volume = volume;
            selectedSource.clip = sound;
            selectedSource.pitch = Random.Range(0.8f, 1.5f);
            selectedSource.Play();
            StartCoroutine(ReturnAudioSourceToQueue(selectedSource));


        }
    }

    public void PlayButtonSound()
    {
        if (!IsSoundsMuted)
        {
            buttonAudioSource.pitch = 1;
            buttonAudioSource.clip = buttonSound;
            buttonAudioSource.Play();
        }
    }


    private IEnumerator ReturnAudioSourceToQueue(AudioSource audioSource)
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        AddToPool(audioSource.gameObject);
    }
}
