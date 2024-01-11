using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    public enum SoundFXTypes 
    {
        HobDial, Sizzle, Chopping, BottlePops, EggCrack
    }

    [SerializeField] AudioSource soundFXClip;
    [Header("General SFX")]
    [SerializeField] AudioClip hobDial;
    [SerializeField] AudioClip[] sizzle;

    [Header("Ingredient SFX")]
    [SerializeField] AudioClip chopping;
    [SerializeField] AudioClip[] bottlePops;
    [SerializeField] AudioClip eggCrack;


    private void Awake()
    {
        if(instance == null) 
        {
            instance = this;
        }
    }

    public void PlaySoundFX(AudioClip audioClip, Vector3 spawnPoint)
    {
        AudioSource audioSource = Instantiate(soundFXClip, spawnPoint, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = PlayerPrefs.GetFloat("VolumeValue"); 

        audioSource.Play();

        float audioLength = audioSource.clip.length;

        Destroy(audioSource, audioLength);
    }

    public void PlayRandomSoundFX(AudioClip[] audioClip, Vector3 spawnPoint)
    {
        int rand = Random.Range(0, audioClip.Length);

        AudioSource audioSource = Instantiate(soundFXClip, spawnPoint, Quaternion.identity);

        audioSource.clip = audioClip[rand];

        audioSource.volume = PlayerPrefs.GetFloat("VolumeValue");

        audioSource.Play();

        float audioLength = audioSource.clip.length;

        Destroy(audioSource, audioLength);
    }

    public void AudioTrigger(SoundFXTypes audioType, Vector3 spawnPoint)
    {
        switch(audioType)
        {
            case SoundFXTypes.HobDial:
                PlaySoundFX(hobDial, spawnPoint);
                break;
            case SoundFXTypes.BottlePops:
                PlayRandomSoundFX(bottlePops, spawnPoint);
                break;
            case SoundFXTypes.Chopping:
                PlaySoundFX(chopping, spawnPoint);
                break;
            case SoundFXTypes.Sizzle:
                PlayRandomSoundFX(sizzle, spawnPoint);
                break;
            case SoundFXTypes.EggCrack:
                PlaySoundFX(eggCrack, spawnPoint);
                break;
        }
    }
}
