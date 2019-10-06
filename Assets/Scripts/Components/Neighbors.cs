// file:	Assets\Scripts\Components\Vertex.cs
//
// summary:	Implements the neighbors class
using System;
using Unity.Entities;

namespace Assets.Scripts.Components
{
    /// <summary>   A neighbors. </summary>
    ///
    /// <remarks>   The Vitulus, 10/2/2019. </remarks>
    [Serializable]
    public struct Neighbors : IComponentData
    {
        /// <summary>   The east. </summary>
        public Entity east;
        /// <summary>   The north. </summary>
        public Entity north;
        /// <summary>   The north west. </summary>
        public Entity northWest;
        /// <summary>   The south. </summary>
        public Entity south;
        /// <summary>   The south east. </summary>
        public Entity southEast;
        /// <summary>   The west. </summary>
        public Entity west;

        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   The Vitulus, 10/2/2019. </remarks>
        ///
        /// <param name="east">         The east </param>
        /// <param name="north">        The north. </param>
        /// <param name="northWest">    The north west. </param>
        /// <param name="south">        The south. </param>
        /// <param name="southEast">    The south east. </param>
        /// <param name="west">         The west. </param>
        public Neighbors(Entity east, Entity north, Entity northWest, Entity south, Entity southEast, Entity west)
        {
            this.east = east;
            this.north = north;
            this.northWest = northWest;
            this.south = south;
            this.southEast = southEast;
            this.west = west;
        }
    }
}
