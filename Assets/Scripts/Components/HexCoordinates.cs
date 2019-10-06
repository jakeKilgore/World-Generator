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
    [Serializable]
    public struct HexCoordinates : IComponentData, IEquatable<HexCoordinates>
    {
        /// <summary>   The column. </summary>
        public readonly int column;
        /// <summary>   The row. </summary>
        public readonly int row;
        /// <summary>   The offset. </summary>
        public readonly int offset;

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

        public static implicit operator int3(HexCoordinates coordinates) { return new int3(coordinates.column, coordinates.row, coordinates.offset); }
        public static implicit operator HexCoordinates(int3 coordinates) { return new HexCoordinates(coordinates.x, coordinates.y); }
        public static implicit operator HexCoordinates(int2 coordinates) { return new HexCoordinates(coordinates.x, coordinates.y); }

        /// <summary>   Tests if this HexCoordinates is considered equal to another. </summary>
        ///
        /// <remarks>   The Vitulus, 10/5/2019. </remarks>
        ///
        /// <param name="other">    The hexagonal coordinates to compare to this object. </param>
        ///
        /// <returns>   True if the objects are considered equal, false if they are not. </returns>
        public bool Equals(HexCoordinates other)
        {
            return column == other.column &&
                   row == other.row &&
                   offset == other.offset;
        }

        public override int GetHashCode()
        {
            var hashCode = 931164788;
            hashCode = hashCode * -1521134295 + column.GetHashCode();
            hashCode = hashCode * -1521134295 + row.GetHashCode();
            hashCode = hashCode * -1521134295 + offset.GetHashCode();
            return hashCode;
        }

        /// <summary>   Convert this object into a string representation. </summary>
        ///
        /// <remarks>   The Vitulus, 10/2/2019. </remarks>
        ///
        /// <returns>   A string that represents this object. </returns>
        public override string ToString()
        {
            return "(" + column + ", " + row + ")";
        }
    }
}
