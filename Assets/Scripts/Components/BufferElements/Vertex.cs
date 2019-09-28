﻿// file:	Assets\Scripts\Components\BufferElements\Vertex.cs
//
// summary:	Implements the vertex class
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts.Components.BufferElements
{
    /// <summary>   A vertex. </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    [InternalBufferCapacity(1)]
    public struct Vertex : IBufferElementData
    {
        /// <summary>   Implicit cast that converts the given Vertex to a Vector3. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="e">    A Vertex to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        public static implicit operator Vector3(Vertex e) { return e.Value; }

        /// <summary>   Implicit cast that converts the given Vector3 to a Vertex. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="e">    A Vector3 to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        public static implicit operator Vertex(Vector3 e) { return new Vertex(e); }

        /// <summary>   The value. </summary>
        public Vector3 Value;

        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="value">    The value. </param>
        public Vertex(Vector3 value) {
            Value = value;
        }
    }
}