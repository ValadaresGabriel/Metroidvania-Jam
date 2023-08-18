using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private bool music;

    private void Start() {
        if(music == true){
            SoundManager.Instance.ChangeMusicVolume(slider.value);
            slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMusicVolume(val));
        }else{
            SoundManager.Instance.ChangeEffectVolume(slider.value);
            slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeEffectVolume(val));
        }
    }
}
