using TreasureMap.Core.Enums;
using TreasureMap.Core.Models;
using TreasureMap.Core.Services;

namespace TreasureMap.Tests.Services
{
    public class MapWriterTests
    {
        [Fact]
        public void GenerateOutput_ValidMapAndAdventurers_GeneratesCorrectOutput()
        {
            // Arrange
            var map = new Map(3, 4);
            map.AddMountain(new Position(1, 0));
            map.AddMountain(new Position(2, 1));
            map.AddTreasures(new Position(0, 3), 2);
            map.AddTreasures(new Position(1, 3), 1);

            var adventurer = new Adventurer("Lara", new Position(1, 1), Orientation.South, "");
            adventurer.CollectTreasure();
            var adventurers = new List<Adventurer> { adventurer };

            var writer = new MapWriter();

            var expectedOutput = $"C - 3 - 4{Environment.NewLine}" +
                                 $"M - 1 - 0{Environment.NewLine}" +
                                 $"M - 2 - 1{Environment.NewLine}" +
                                 $"T - 0 - 3 - 2{Environment.NewLine}" +
                                 $"T - 1 - 3 - 1{Environment.NewLine}" +
                                 $"A - Lara - 1 - 1 - S - 1";

            // Act
            var output = writer.GenerateOutput(map, adventurers);

            // Assert
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void GenerateOutput_EmptyMap_GeneratesCorrectOutput()
        {
            // Arrange
            var map = new Map(3, 4);
            var adventurers = new List<Adventurer>();

            var writer = new MapWriter();

            var expectedOutput = "C - 3 - 4";

            // Act
            var output = writer.GenerateOutput(map, adventurers);

            // Assert
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void GenerateOutput_MultipleAdventurers_GeneratesCorrectOutput()
        {
            // Arrange
            var map = new Map(3, 4);
            var lara = new Adventurer("Lara", new Position(1, 1), Orientation.South, "");
            var indiana = new Adventurer("Indiana", new Position(2, 2), Orientation.East, "");
            indiana.CollectTreasure();
            indiana.CollectTreasure();
            var adventurers = new List<Adventurer> { lara, indiana };

            var writer = new MapWriter();

            var expectedOutput = $"C - 3 - 4{Environment.NewLine}" +
                                 $"A - Lara - 1 - 1 - S - 0{Environment.NewLine}" +
                                 $"A - Indiana - 2 - 2 - E - 2";

            // Act
            var output = writer.GenerateOutput(map, adventurers);

            // Assert
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void GenerateOutput_InvalidOrientation_ThrowsArgumentException()
        {
            // Arrange
            var map = new Map(3, 4);
            var adventurer = new Adventurer("Lara", new Position(1, 1), (Orientation)(-1), "");
            var adventurers = new List<Adventurer> { adventurer };

            var writer = new MapWriter();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => writer.GenerateOutput(map, adventurers));
        }
    }
}