using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class WindWillRotation : MonoBehaviour
    {
        [SerializeField] private float velocity = 1f;

        void Update()
        {
            transform.Rotate(0, 0, velocity * Time.deltaTime);
        }
    }
}
