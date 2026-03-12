using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "InteractibleObject", menuName = "Scriptable Objects/InteractibleObject")]
    public class InteractibleObject : ScriptableObject
    {
        [Header("Description")]
        public string description;
    }
}