using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class SceneSoundManager : MonoBehaviour
    {
        [SerializeField] private int musicIndex;

        private void Start()
        {
            SoundManager.Instance.PlayMusic(musicIndex);
        }
    }
}
