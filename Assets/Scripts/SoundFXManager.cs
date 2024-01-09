using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] AudioSource soundFXClip;

    private void Awake()
    {
        if(instance == null) 
        {
            instance = this;
        }
    }

    public void PlaySoundFX(AudioClip audioClip, Transform spawnPoint, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXClip, spawnPoint.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume= volume;

        audioSource.Play();

        float audioLength = audioSource.clip.length;

        Destroy(audioSource, audioLength);
    }
}
