// file:	Assets\Scripts\Components\Vertex.cs
//
// summary:	Implements the neighbors class
using Unity.Entities;

namespace Assets.Scripts.Components
{
    /// <summary>   A neighbors. </summary>
    ///
    /// <remarks>   The Vitulus, 10/2/2019. </remarks>
    public struct Neighbors : IComponentData
    {
        /// <summary>   The east. </summary>
        private Entity east;
        /// <summary>   The north. </summary>
        private Entity north;
        /// <summary>   The north west. </summary>
        private Entity northWest;
        /// <summary>   The south. </summary>
        private Entity south;
        /// <summary>   The south east. </summary>
        private Entity southEast;
        /// <summary>   The west. </summary>
        private Entity west;

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

        /// <summary>   Gets the east. </summary>
        ///
        /// <value> The east. </value>
        public Entity East { get => east; set => east = value; }

        /// <summary>   Gets the north. </summary>
        ///
        /// <value> The north. </value>
        public Entity North { get => north; set => north = value; }

        /// <summary>   Gets the north west. </summary>
        ///
        /// <value> The north west. </value>
        public Entity NorthWest { get => northWest; set => northWest = value; }

        /// <summary>   Gets the south. </summary>
        ///
        /// <value> The south. </value>
        public Entity South { get => south; set => south = value; }

        /// <summary>   Gets the south east. </summary>
        ///
        /// <value> The south east. </value>
        public Entity SouthEast { get => southEast; set => southEast = value; }

        /// <summary>   Gets the west. </summary>
        ///
        /// <value> The west. </value>
        public Entity West { get => west; set => west = value; }
    }
}
