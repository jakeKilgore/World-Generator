////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Assets\Scripts\Components\Coordinates.cs
//
// summary:	Implements the coordinates class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Components {

    /// <summary>
    /// A component representing a cubic coordinate system for hexagonal tiles. More can be read
    /// about this system at: https://www.redblobgames.com/grids/hexagons/.
    /// </summary>
    ///
    /// <remarks>   The Vitulus, 8/15/2019. </remarks>
    public struct HexCoordinates : IComponentData
    {

        /// <summary>   The column. </summary>
        public int column;
        /// <summary>   The row. </summary>
        public int row;
        /// <summary>   The offset. </summary>
        public int offset;

        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="column">   The column. </param>
        /// <param name="row">      The row. </param>
        public HexCoordinates(int column, int row)
        {
            this.column = column;
            this.row = row;
            this.offset = -(column + row);
        }
    }
}
