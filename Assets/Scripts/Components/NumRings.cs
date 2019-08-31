using UnityEngine;
using System.Collections;
using Unity.Entities;

namespace Assets.Scripts.Components
{
    public struct NumRings : IComponentData
    {
        public int value;

        public NumRings(int value)
        {
            this.value = value;
        }
    }
}