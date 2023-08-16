using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private List<AudioSource> musicSource, effectSource;

    private void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    public void PlaySound(int index){
        if(index >= 0 && index < effectSource.Count){
            AudioSource source = effectSource[index];
            source.Play();
        }
    }
}
