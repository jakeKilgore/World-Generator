using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.WorldGenerator.Tile.Component {

    /// <summary>   A component representing a cubic coordinate system for hexagonal tiles. </summary>
    /// <remarks>   More can be read about this system at: https://www.redblobgames.com/grids/hexagons/. </remarks>
    [Serializable]
    public struct Coordinates : IComponentData
    {
        private readonly int column;
        private readonly int row;
        private readonly int offset;

        /// <summary>   The x-coordinate of the tile. </summary>
        public readonly int Column => column;

        /// <summary>   The y-coordinate of the tile. </summary>
        public readonly int Row => row;

        ///<summary>    The z-coordinate of the tile. </summary>
        ///<remarks>    To be a valid tile, the offset must be the additive inverse of the sum of the column and row. </remarks>
        public readonly int Offset => offset;

        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="column">   The column. </param>
        /// <param name="row">      The row. </param>
        public Coordinates(int column, int row)
        {
            this.column = column;
            this.row = row;
            offset = -(column + row);
        }

        public static explicit operator int3(Coordinates coordinates) => new int3(coordinates.column, coordinates.row, coordinates.offset);
        public static explicit operator int2(Coordinates coordinates) => new int2(coordinates.column, coordinates.row);
        public static explicit operator Coordinates(int3 coordinates) => new Coordinates(coordinates.x, coordinates.y);
        public static explicit operator Coordinates(int2 coordinates) => new Coordinates(coordinates.x, coordinates.y);

        public override bool Equals(object other) => other is Coordinates c && (c.Column, c.Row, c.Offset).Equals((Column, Row, Offset));
        public override int GetHashCode() => (Column, Row, Offset).GetHashCode();
        public override string ToString() => "(" + column + ", " + row + ")";
    }
}
