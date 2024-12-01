using TreasureMap.Core.Enums;
using TreasureMap.Core.Models;
using TreasureMap.Core.Services;

namespace TreasureMap.Tests.Services
{
    public class GameEngineTests
    {
        [Fact]
        public void RunSimulation_WithBasicMovement_UpdatesPosition()
        {
            // Arrange
            var map = new Map(3, 3);
            var adventurer = new Adventurer("Test", new Position(0, 0), Orientation.South, "A");
            var adventurers = new List<Adventurer> { adventurer };
            map.AddAdventurer(adventurer);
            var engine = new GameEngine();

            // Act
            engine.RunSimulation(map, adventurers);

            // Assert
            Assert.Equal(new Position(0, 1), adventurer.Position);
            Assert.Equal(Orientation.South, adventurer.Orientation);
        }

        [Fact]
        public void RunSimulation_WithMountainBlocking_StaysInPlace()
        {
            // Arrange
            var map = new Map(3, 3);
            map.AddMountain(new Position(0, 1));
            var adventurer = new Adventurer("Test", new Position(0, 0), Orientation.South, "A");
            var adventurers = new List<Adventurer> { adventurer };
            map.AddAdventurer(adventurer);
            var engine = new GameEngine();

            // Act
            engine.RunSimulation(map, adventurers);

            // Assert
            Assert.Equal(new Position(0, 0), adventurer.Position);
        }

        [Fact]
        public void RunSimulation_WithTreasureCollection_UpdatesTreasureCount()
        {
            // Arrange
            var map = new Map(3, 3);
            map.AddTreasures(new Position(0, 1), 1);
            var adventurer = new Adventurer("Test", new Position(0, 0), Orientation.South, "A");
            var adventurers = new List<Adventurer> { adventurer };
            map.AddAdventurer(adventurer);
            var engine = new GameEngine();

            // Act
            engine.RunSimulation(map, adventurers);

            // Assert
            Assert.Equal(1, adventurer.CollectedTreasures);
        }


        [Fact]
        public void RunSimulation_WithMultipleAdventurers_MovesInOrder()
        {
            // Arrange
            var map = new Map(3, 3);
            var adventurer1 = new Adventurer("Test1", new Position(0, 0), Orientation.East, "A");
            var adventurer2 = new Adventurer("Test2", new Position(2, 0), Orientation.South, "A"); // Placé en (2,0) au lieu de (1,0)
            var adventurers = new List<Adventurer> { adventurer1, adventurer2 };
            map.AddAdventurer(adventurer1);
            map.AddAdventurer(adventurer2);
            var engine = new GameEngine();

            // Act
            engine.RunSimulation(map, adventurers);

            // Assert
            Assert.Equal(new Position(1, 0), adventurer1.Position); // Peut maintenant avancer vers l'Est
            Assert.Equal(new Position(2, 1), adventurer2.Position); // Peut avancer vers le Sud
        }

        [Fact]
        public void RunSimulation_WithRotations_UpdatesOrientation()
        {
            // Arrange
            var map = new Map(3, 3);
            var adventurer = new Adventurer("Test", new Position(1, 1), Orientation.North, "GD");
            var adventurers = new List<Adventurer> { adventurer };
            map.AddAdventurer(adventurer);
            var engine = new GameEngine();

            // Act
            engine.RunSimulation(map, adventurers);

            // Assert
            Assert.Equal(Orientation.North, adventurer.Orientation);
            Assert.Equal(new Position(1, 1), adventurer.Position);
        }

        [Fact]
        public void RunSimulation_WithBoundaryLimit_StaysInBounds()
        {
            // Arrange
            var map = new Map(2, 2);
            var adventurer = new Adventurer("Test", new Position(0, 0), Orientation.North, "A");
            var adventurers = new List<Adventurer> { adventurer };
            map.AddAdventurer(adventurer);
            var engine = new GameEngine();

            // Act
            engine.RunSimulation(map, adventurers);

            // Assert
            Assert.Equal(new Position(0, 0), adventurer.Position);
        }

        [Fact]
        public void RunSimulation_WithComplexSequence_ExecutesCorrectly()
        {
            // Arrange
            var map = new Map(3, 3);
            var adventurer = new Adventurer("Test", new Position(1, 1), Orientation.North, "AADADAGA");
            var adventurers = new List<Adventurer> { adventurer };
            map.AddAdventurer(adventurer);
            var engine = new GameEngine();

            // Act
            engine.RunSimulation(map, adventurers);

            // Assert
            Assert.Equal(new Position(2, 1), adventurer.Position);
            Assert.Equal(Orientation.East, adventurer.Orientation);
        }

        [Fact]
        public void RunSimulation_WithMultipleTreasures_CollectsOneAtATime()
        {
            // Arrange
            var map = new Map(3, 3);
            map.AddTreasures(new Position(0, 1), 2);
            // Séquence : avance, avance, tourne droite, tourne droite, avance
            var adventurer = new Adventurer("Test", new Position(0, 0), Orientation.South, "AADDA");
            var adventurers = new List<Adventurer> { adventurer };
            map.AddAdventurer(adventurer);
            var engine = new GameEngine();

            // Act
            engine.RunSimulation(map, adventurers);

            // Assert
            Assert.Equal(2, adventurer.CollectedTreasures);
        }
    }
}