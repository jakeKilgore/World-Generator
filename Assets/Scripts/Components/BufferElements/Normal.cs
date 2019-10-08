// file:	Assets\Scripts\Components\BufferElements\Normal.cs
//
// summary:	Implements the normal class
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Components.BufferElements
{
    /// <summary>
    /// A normal component for generating dynamic buffers.
    /// 
    /// Normals are vectors pointing in the direction of reflections. In Unity, vectors are stored
    /// per-vertex and used for lighting.
    /// </summary>
    ///
    /// <remarks>   The Vitulus, 10/1/2019. </remarks>
    [InternalBufferCapacity(1)]
    public struct Normal : IBufferElementData
    {
        /// <summary>   Implicit cast that converts the given Normal to a Vector3. </summary>
        ///
        /// <remarks>   The Vitulus, 10/1/2019. </remarks>
        ///
        /// <param name="e">    A Normal to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        public static implicit operator Vector3(Normal e) { return e.Value; }

        /// <summary>   Implicit cast that converts the given Normal to a float3. </summary>
        ///
        /// <remarks>   The Vitulus, 10/7/2019. </remarks>
        ///
        /// <param name="e">    A Normal to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        public static implicit operator float3(Normal e) { return e.Value; }

        /// <summary>   Implicit cast that converts the given Vector3 to a Normal. </summary>
        ///
        /// <remarks>   The Vitulus, 10/1/2019. </remarks>
        ///
        /// <param name="e">    A Vector3 to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        public static implicit operator Normal(Vector3 e) { return new Normal(e); }

        /// <summary>   Implicit cast that converts the given float3 to a Normal. </summary>
        ///
        /// <remarks>   The Vitulus, 10/7/2019. </remarks>
        ///
        /// <param name="e">    A float3 to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        public static implicit operator Normal(float3 e) { return new Normal(e); }

        /// <summary>   The normal vector of the vertex. </summary>
        public Vector3 Value;

        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   The Vitulus, 10/1/2019. </remarks>
        ///
        /// <param name="value">    The normal vector of the vertex. </param>
        public Normal(Vector3 value)
        {
            Value = value;
        }
    }
}