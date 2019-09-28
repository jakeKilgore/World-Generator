// file:	Assets\Scripts\Components\BufferElements\Triangle.cs
//
// summary:	Implements the triangle class
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts.Components.BufferElements
{
    /// <summary>   A triangle. </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    [InternalBufferCapacity(3)]
    public struct Triangle : IBufferElementData
    {
        /// <summary>   Implicit cast that converts the given Triangle to an int. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="e">    A Triangle to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        public static implicit operator int(Triangle e) { return e.Value; }

        /// <summary>   Implicit cast that converts the given int to a Triangle. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="e">    An int to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        public static implicit operator Triangle(int e) { return new Triangle(e); }

        /// <summary>   The value. </summary>
        public int Value;

        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="value">    The value. </param>
        public Triangle(int value) {
            Value = value;
        }
    }
}
