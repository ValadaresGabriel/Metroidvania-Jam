using UnityEngine;

namespace TS
{
    [CreateAssetMenu(menuName = "World Objective")]
    public class WorldObjective : ScriptableObject
    {
        public string title;
        [TextArea] public string description;
    }
}
