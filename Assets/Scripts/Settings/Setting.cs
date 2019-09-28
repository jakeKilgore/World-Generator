// file:	Assets\Scripts\Settings\Setting.cs
//
// summary:	Implements the setting class
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Settings
{
    /// <summary>   A scriptable object for making editable settings in the Unity editor. </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    public class Setting : ScriptableObject
    {
        /// <summary>   True to foldout. </summary>
        [HideInInspector]
        public bool foldout = true;
    }
}