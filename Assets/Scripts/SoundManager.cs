using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public AudioClip BackgroundMusic;

    public AudioSource SoundEffectsSource;

    public static SoundManager Instance;

    public AudioClip PaperFlip;
    public AudioClip PickUp;
    
    void Awake() {
        Instance = this;
    }
    
    
    public void PlaySound(AudioClip sound, float volume) {
        SoundEffectsSource.PlayOneShot(sound, volume);
    }
}
