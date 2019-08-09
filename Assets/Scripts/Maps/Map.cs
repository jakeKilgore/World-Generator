using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using UnityEngine;

namespace Maps {
    /// <summary>
    /// Serves as a template for the tile hierarchy.
    /// </summary>
    /// <typeparam name="TParent">Parent map type</typeparam>
    /// <typeparam name="TChild">Child map type</typeparam>
    [Serializable, ComVisible(false)]
    public class Map : Dictionary<Coordinates, Map> {
        protected Coordinates coordinates;

        public Coordinates Coordinates {
            get {
                return coordinates;
            }
        }


        public Map(Coordinates coordinates) : base() {
            if (coordinates == null) {
                throw new ArgumentNullException("Coordinates may not be null.");
            }
            this.coordinates = coordinates;
        }

        public Map() : this(Directions.O) { }

        protected Map(SerializationInfo info, StreamingContext context) : base(info, context) {
            coordinates = (Coordinates)info.GetValue("coordinates", typeof(Coordinates));
        }


        public new virtual void Add(Coordinates coordinates, Map submap) {
            if (submap == null) {
                throw new ArgumentNullException();
            }
            base.Add(coordinates, submap);
        }
        public virtual void Add(Map submap) {
            Add(submap.Coordinates, submap);
        }



        public override bool Equals(object obj) {
            var map = obj as Map;
            return map != null &&
                   EqualityComparer<int>.Default.Equals(Count, map.Count) &&
                   EqualityComparer<Coordinates>.Default.Equals(coordinates, map.coordinates);
        }

        public override int GetHashCode() {
            var hashCode = -1219080764;
            hashCode = hashCode * -1521134295 + EqualityComparer<int>.Default.GetHashCode(Count);
            hashCode = hashCode * -1521134295 + EqualityComparer<Coordinates>.Default.GetHashCode(coordinates);
            return hashCode;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);
            info.AddValue("coordinates", coordinates);
        }

        public override string ToString() {
            return "Map: (" + coordinates.X + ", " + coordinates.Y + ")";
        }
    }
}