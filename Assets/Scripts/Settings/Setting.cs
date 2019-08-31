using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Settings
{
    public class Setting : ScriptableObject
    {
        [HideInInspector]
        public bool foldout = true;
    }
}