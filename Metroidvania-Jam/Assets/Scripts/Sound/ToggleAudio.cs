using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleAudio : MonoBehaviour
{
    [SerializeField] private bool toggleMusic, toggleEffect;
    [SerializeField] private GameObject gameObject;    

    public void Toggle(){
        if(toggleMusic) SoundManager.Instance.ToggleMusic();
        if(toggleEffect) SoundManager.Instance.ToggleEffect();
        if(gameObject != null)
                gameObject.active = !gameObject.active;
    }
}
