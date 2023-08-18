using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private List<AudioSource> musicSource, effectSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(int index)
    {
        if (index >= 0 && index < effectSource.Count)
        {
            AudioSource source = effectSource[index];
            source.Play();
        }
    }

    public void ChangeMusicVolume(float value){
        for(int index = 0; index != musicSource.Count; index++){
            musicSource[index].volume = value;
        }
    }

    public void ChangeEffectVolume(float value){
        for(int index = 0; index != effectSource.Count; index++){
            effectSource[index].volume = value;
        }
    }

    public void ToggleMusic(){
        for(int index = 0; index != musicSource.Count; index++){
            musicSource[index].mute = !musicSource[index].mute;
        }
    }

    public void ToggleEffect(){
        for(int index = 0; index != effectSource.Count; index++){
            effectSource[index].mute = !effectSource[index].mute;
        }
    }
}
