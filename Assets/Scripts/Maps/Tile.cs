using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using UnityEngine;
using WorldGeneration;

namespace Maps {
    /// <summary>
    /// Tiles are the map used for world generation.
    /// </summary>
    [Serializable, ComVisible(false)]
    public class Tile : Map {
        private readonly int layer;
        private readonly World world;
        private Terrain terrain;

        private readonly Coordinates coordinatesRelativeToParent;
        private readonly Coordinates coordinatesOfOriginTile;

        public int Layer {
            get {
                return layer;
            }
        }
        public Tile Parent { get; private set; }



        public Tile(Coordinates coordinates, Tile parent, World world, int layer) : base(coordinates) {
            if(world == null) {
                throw new ArgumentNullException("Tile must exist in a world");
            }
            Parent = parent;
            this.world = world;
            this.layer = layer;

            coordinatesRelativeToParent = coordinates;
            if (parent != null) {
                Coordinates parentCoords = parent.Coordinates;
                coordinates = new Coordinates(2 * parentCoords.X - parentCoords.Y, parentCoords.X + 3 * parentCoords.Y);
                coordinates = coordinates.Add(coordinatesRelativeToParent);
            }
            this.coordinates = coordinates;

            coordinatesOfOriginTile = coordinates;
            for(int i = 0; i < layer; i++) {
                coordinatesOfOriginTile = new Coordinates(2 * coordinatesOfOriginTile.X - coordinatesOfOriginTile.Y, coordinatesOfOriginTile.X + 3 * coordinatesOfOriginTile.Y);
            }

            AddToLayer();
        }

        protected Tile(SerializationInfo info, StreamingContext context) : base(info, context) {
            layer = (int)info.GetValue("layer", typeof(int));
            world = (World)info.GetValue("world", typeof(World));
            Parent = (Tile)info.GetValue("parent", typeof(Tile));
        }


        public override void Add(Coordinates coordinates, Map submap) {
            if(submap.GetType() != typeof(Tile)) {
                throw new ArgumentException("Tiles may only add other Tiles.");
            }
            Tile subtile = (Tile) submap;
            if(subtile.Layer != layer - 1) {
                throw new ArgumentException("Tile layer heirarchy broken.");
            }
            subtile.SetParent(this);
            base.Add(coordinates, submap);
        }

        public override void Add(Map submap) {
            Add(submap.Coordinates, submap);
        }



        private void AddToLayer() {
            if (!world.Layers.ContainsKey(layer)) {
                world.Layers.Add(layer, new Map());
            }
            world.Layers[layer].Add(Coordinates, this);
        }

        protected void SetParent(Tile parent) {
            this.Parent = parent;
        }

        public void Delete() {
            if (layer > 0) {
                ((Tile)this[Directions.O]).Delete();
                foreach (Coordinates direction in new Directions()) {
                    ((Tile)this[direction]).Delete();
                }
            }
            world.Layers[layer].Remove(this.Coordinates);
        }



        public void GenerateMap() {
            if(layer <= 0) {
                return;
            }
            AddSubtile(Directions.O);
            foreach (Coordinates direction in new Directions()) {
                AddSubtile(direction);
            }
        }

        public void GenerateMap(Tile center) {
            Add(Directions.O, center);
            foreach(Coordinates direction in new Directions()) {
                AddSubtile(direction);
            }
        }

        private void AddSubtile(Coordinates direction) {
            Tile subTile = new Tile(direction, this, world, layer - 1);
            subTile.GenerateMap();
            Add(subTile);
        }


        private static readonly float WIDTH_MULTIPLE = Mathf.Sqrt(3);
        private static readonly float HEIGHT_MULTIPLE = 2f;
        /// <summary>
        /// Returns the location of the hex in the worldspace.
        /// </summary>
        /// <returns></returns>
        public Vector3 Position() {
            float height = HEIGHT_MULTIPLE;
            float width = WIDTH_MULTIPLE;
            float posX = width * (coordinatesOfOriginTile.X + (coordinatesOfOriginTile.Y / 2f));
            float posY = 0;
            float posZ = height * 3/4 * coordinatesOfOriginTile.Y;

            return new Vector3(posX, posY, posZ);
        }



        public override bool Equals(object obj) {
            var tile = obj as Tile;
            return tile != null &&
                   base.Equals(obj) &&
                   layer == tile.layer &&
                   EqualityComparer<World>.Default.Equals(world, tile.world) &&
                   EqualityComparer<Tile>.Default.Equals(Parent, tile.Parent);
        }

        public override int GetHashCode() {
            var hashCode = 642277390;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + layer.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<World>.Default.GetHashCode(world);
            hashCode = hashCode * -1521134295 + EqualityComparer<Tile>.Default.GetHashCode(Parent);
            return hashCode;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);
            info.AddValue("layer", layer);
            info.AddValue("world", world);
            info.AddValue("parent", Parent);
        }

        public override string ToString() {
            return Layers.GetName(layer) + ": (" + Coordinates.X + ", " + Coordinates.Y + ")";
        }
    }
}