using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct Coordinates : IComponentData {
    public int column;
    public int row;
    public int offset;
}
