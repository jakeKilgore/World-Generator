using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts.Components.BufferElements
{
    [InternalBufferCapacity(3)]
    public struct Triangle : IBufferElementData
    {
        public static implicit operator int(Triangle e) { return e.Value; }
        public static implicit operator Triangle(int e) { return new Triangle(e); }

        public int Value;

        public Triangle(int value) {
            Value = value;
        }
    }
}