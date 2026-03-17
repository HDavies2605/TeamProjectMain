using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    ///<summary>
    ///Scriptable object template for crteating objects to interact with on the map
    /// </summary>
    /// 

    public class InteractableDataSO : ScriptableObject
    {
        [Header("Text")]
        public string IOName;

        [Header("Visual")]
        public Sprite IOSprite;
    }
}