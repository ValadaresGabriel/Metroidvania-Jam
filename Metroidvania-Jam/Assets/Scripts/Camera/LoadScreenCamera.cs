using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScreenCamera : MonoBehaviour
{
    public static LoadScreenCamera Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
