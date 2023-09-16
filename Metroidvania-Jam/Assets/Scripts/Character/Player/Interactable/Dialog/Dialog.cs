using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class Dialog : ScriptableObject
    {
        public string owner;
        [TextArea] public string text;
        [SerializeField] private List<Dialog> dialogSequence;

        public List<Dialog> DialogSequence => dialogSequence;
    }
}
