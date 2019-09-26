using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts.Components.BufferElements
{
    [InternalBufferCapacity(1)]
    public struct UV : IBufferElementData
    {
        public static implicit operator Vector2(UV e) { return e.Value; }
        public static implicit operator UV(Vector2 e) { return new UV(e); }

        public Vector2 Value;

        public UV(Vector2 value)
        {
            Value = value;
        }
    }
}
