using NUnit.Framework;
using Maps;
using System.Collections.Generic;
using Settings;
using WorldGeneration;

[TestFixture]
public class TileTest {

    [Test]
    public void When_SubtileAdded_Expect_TileContainsSubtile() {
        World world = new World(new MapSettings());
        Tile tile = new Tile(Directions.O, null, world, 1);
        Tile subtile = new Tile(Directions.O, null, world, 0);
        tile.Add(subtile);
        Assert.That(tile, Contains.Key(subtile.Coordinates), "Map does not recognize submap key.");
        Assert.That(tile, Contains.Value(subtile), "Map does not recognize submap value.");
        Assert.That(subtile, Is.SameAs(tile[subtile.Coordinates]), "Submap is not the same object as the submap inside of map.");
    }

    [Test]
    public void When_SubtileAdded_Expect_CountIncrements() {
        World world = new World(new MapSettings());
        Tile tile = new Tile(Directions.O, null, world, 1);
        Tile subtile = new Tile(Directions.O, null, world, 0);
        Assert.That(tile.Count, Is.Zero, "Map count initialized at non-zero number.");
        tile.Add(subtile);
        Assert.That(tile.Count, Is.EqualTo(1), "Map count did not increment.");
    }

    [Test]
    public void When_IdenticalTilesCompared_Expect_TilesAreEqual() {
        World world = new World(new MapSettings());
        Tile tile = new Tile(Directions.O, null, world, 0);
        Tile isEqual = new Tile(Directions.O, null, world, 0);
        Tile isNotEqual = new Tile(Directions.E, null, world, 0);
        Assert.That(tile, Is.Not.SameAs(isEqual), "Maps refer to same object.");
        Assert.That(tile, Is.Not.SameAs(isNotEqual), "Maps refer to same object.");
        Assert.That(tile.Equals(isEqual), "Equivalent maps are not equal.");
        Assert.That(!tile.Equals(isNotEqual), "Non-equivalent maps are equal.");
    }
}
