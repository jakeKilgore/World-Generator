// file:	Assets\Scripts\Components\BufferElements\Triangle.cs
//
// summary:	Implements the triangle class
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts.Components.BufferElements
{
    /// <summary>
    /// A triangle component for creating dynamic buffers.
    /// 
    /// Triangles are represented in the mesh creation system as integer indeces of a vertex array,
    /// with every group of three triangle components representing a full triangle.
    /// </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    [InternalBufferCapacity(3)]
    public struct TrianglePoint : IBufferElementData
    {
        /// <summary>   Implicit cast that converts the given Triangle to an int. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="e">    A Triangle to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        public static implicit operator int(TrianglePoint e) { return e.Value; }

        /// <summary>   Implicit cast that converts the given int to a Triangle. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="e">    An int to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        public static implicit operator TrianglePoint(int e) { return new TrianglePoint(e); }

        /// <summary>   The integer representing the index of a vertex array. </summary>
        public int Value;

        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="value">    The index in a vertex array representing a mesh. </param>
        public TrianglePoint(int value) {
            Value = value;
        }
    }
}
