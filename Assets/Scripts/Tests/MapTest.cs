using NUnit.Framework;
using Maps;
using System.Collections.Generic;

[TestFixture]
public class MapTest {
    
    [Test]
    public void When_SubmapAdded_Expect_MapContainsSubmap() {
        Map map = new Map();
        Map submap = new Map();
        map.Add(submap);
        Assert.That(map, Contains.Key(submap.Coordinates), "Map does not recognize submap key.");
        Assert.That(map, Contains.Value(submap), "Map does not recognize submap value.");
        Assert.That(submap, Is.SameAs(map[submap.Coordinates]), "Submap is not the same object as the submap inside of map.");
    }

    [Test]
    public void When_SubmapAdded_Expect_CountIncrements() {
        Map map = new Map();
        Map submap = new Map();
        Assert.That(map.Count, Is.Zero, "Map count initialized at non-zero number.");
        map.Add(submap);
        Assert.That(map.Count, Is.EqualTo(1), "Map count did not increment.");
    }

    [Test]
    public void When_IdenticalMapsCompared_Expect_MapsAreEqual() {
        Map map = new Map(Directions.O);
        Map isEqual = new Map(Directions.O);
        Map isNotEqual = new Map(Directions.E);
        Assert.That(map, Is.Not.SameAs(isEqual), "Maps refer to same object.");
        Assert.That(map, Is.Not.SameAs(isNotEqual), "Maps refer to same object.");
        Assert.That(map.Equals(isEqual), "Equivalent maps are not equal.");
        Assert.That(!map.Equals(isNotEqual), "Non-equivalent maps are equal.");
    }
}
