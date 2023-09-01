using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")]

        public int itemID;

        public string itemName;

        public Sprite itemIcon;

        [TextArea]
        public string itemDescription;
    }
}
