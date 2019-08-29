using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts.Components.BufferElements
{
    [InternalBufferCapacity(1)]
    public struct Vertex : IBufferElementData
    {
        public static implicit operator Vector3(Vertex e) { return e.Value; }
        public static implicit operator Vertex(Vector3 e) { return new Vertex(e); }

        public Vector3 Value;

        public Vertex(Vector3 value) {
            Value = value;
        }
    }
}