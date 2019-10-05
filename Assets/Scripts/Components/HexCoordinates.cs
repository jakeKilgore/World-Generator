////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Assets\Scripts\Components\Coordinates.cs
//
// summary:	Implements the coordinates class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
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
        private readonly int column;
        /// <summary>   The row. </summary>
        private readonly int row;
        /// <summary>   The offset. </summary>
        private readonly int offset;

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
            offset = -(column + row);
        }

        /// <summary>   Gets the column. </summary>
        ///
        /// <value> The column. </value>
        public int Column => column;

        /// <summary>   Gets the row. </summary>
        ///
        /// <value> The row. </value>
        public int Row => row;

        /// <summary>   Gets the offset. </summary>
        ///
        /// <value> The offset. </value>
        public int Offset => offset;

        /// <summary>   Convert this object into a string representation. </summary>
        ///
        /// <remarks>   The Vitulus, 10/2/2019. </remarks>
        ///
        /// <returns>   A string that represents this object. </returns>
        public override string ToString()
        {
            return "(" + Column + ", " + Row + ")";
        }
    }
}
