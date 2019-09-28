// file:	Assets\Scripts\Components\BufferElements\UV.cs
//
// summary:	Implements the uv class
using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts.Components.BufferElements
{
    /// <summary>
    /// An UV coordinate component for creating dynamic buffers.
    /// 
    /// UV coordinates are used for texture unwrapping and
    /// represent the position of the given vertex in the mesh relative to the position on a square
    /// texture.
    /// </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    [InternalBufferCapacity(1)]
    public struct UV : IBufferElementData
    {
        /// <summary>   Implicit cast that converts the given UV to a Vector2. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="e">    An UV to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        public static implicit operator Vector2(UV e) { return e.Value; }

        /// <summary>   Implicit cast that converts the given Vector2 to an UV. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="e">    A Vector2 to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        public static implicit operator UV(Vector2 e) { return new UV(e); }

        /// <summary>   The UV coordinate for a certain vertex in a mesh. </summary>
        public Vector2 Value;

        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="value">    The UV coordinate for a certain vertex in a mesh. </param>
        public UV(Vector2 value)
        {
            Value = value;
        }
    }
}
