// file:	Assets\Scripts\Settings\Setting.cs
//
// summary:	Implements the setting class
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Settings
{
    /// <summary>   A setting. </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    public class Setting : ScriptableObject
    {
        /// <summary>   True to foldout. </summary>
        [HideInInspector]
        public bool foldout = true;
    }
}