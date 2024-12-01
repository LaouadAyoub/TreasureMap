using Xunit;
using TreasureMap.Core.Enums;
using TreasureMap.Core.Models;
using TreasureMap.Core.Services;

namespace TreasureMap.Tests.Services
{
    public class MapParserTests
    {
        [Fact]
        public void ParseMap_ValidInput_CreatesMapAndAdventurers()
        {
            // Arrange
            var input = new[]
            {
                "C - 3 - 4",
                "M - 1 - 0",
                "M - 2 - 1",
                "T - 0 - 3 - 2",
                "T - 1 - 3 - 3",
                "A - Lara - 1 - 1 - S - AADADAGGA"
            };
            var parser = new MapParser();

            // Act
            var (map, adventurers) = parser.ParseMap(input);

            // Assert
            Assert.Equal(3, map.Width);
            Assert.Equal(4, map.Height);
            Assert.Equal(CellType.Mountain, map.GetCell(new Position(1, 0)).Type);
            Assert.Equal(CellType.Mountain, map.GetCell(new Position(2, 1)).Type);
            Assert.Equal(2, map.GetCell(new Position(0, 3)).TreasureCount);
            Assert.Equal(3, map.GetCell(new Position(1, 3)).TreasureCount);
            Assert.Single(adventurers);
            Assert.Equal("Lara", adventurers[0].Name);
            Assert.Equal(new Position(1, 1), adventurers[0].Position);
            Assert.Equal(Orientation.South, adventurers[0].Orientation);
        }

        [Fact]
        public void ParseMap_EmptyInput_ThrowsArgumentException()
        {
            // Arrange
            var input = new string[0];
            var parser = new MapParser();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => parser.ParseMap(input));
        }

        [Fact]
        public void ParseMap_InvalidMapDefinition_ThrowsArgumentException()
        {
            // Arrange
            var input = new[]
            {
                "M - 1 - 0",
                "T - 0 - 3 - 2",
                "A - Lara - 1 - 1 - S - AADADAGGA"
            };
            var parser = new MapParser();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => parser.ParseMap(input));
        }

        [Fact]
        public void ParseMap_InvalidMountainPosition_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var input = new[]
            {
                "C - 3 - 4",
                "M - 4 - 0"
            };
            var parser = new MapParser();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => parser.ParseMap(input));
        }

        [Fact]
        public void ParseMap_InvalidTreasurePosition_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var input = new[]
            {
                "C - 3 - 4",
                "T - 0 - 5 - 2"
            };
            var parser = new MapParser();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => parser.ParseMap(input));
        }

        [Fact]
        public void ParseMap_InvalidAdventurerOrientation_ThrowsArgumentException()
        {
            // Arrange
            var input = new[]
            {
                "C - 3 - 4",
                "A - Lara - 1 - 1 - X - AADADAGGA"
            };
            var parser = new MapParser();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => parser.ParseMap(input));
        }

        [Fact]
        public void ParseMap_MultipleAdventurers_CreatesAdventurersInOrder()
        {
            // Arrange
            var input = new[]
            {
                "C - 3 - 4",
                "A - Lara - 1 - 1 - S - AADADAGGA",
                "A - Indiana - 0 - 2 - E - GDAGADAA"
            };
            var parser = new MapParser();

            // Act
            var (_, adventurers) = parser.ParseMap(input);

            // Assert
            Assert.Equal(2, adventurers.Count);
            Assert.Equal("Lara", adventurers[0].Name);
            Assert.Equal("Indiana", adventurers[1].Name);
        }
    }
}