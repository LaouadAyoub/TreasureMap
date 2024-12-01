using System;
using Xunit;
using TreasureMap.Core.Models;
using TreasureMap.Core.Enums;

namespace TreasureMap.Tests.Models
{
    public class MapTests
    {
        [Fact]
        public void Constructor_InitializesMap()
        {
            // Arrange & Act
            var map = new Map(3, 4);

            // Assert
            Assert.Equal(3, map.Width);
            Assert.Equal(4, map.Height);
        }

        [Fact]
        public void AddMountain_AddsMountainToCell()
        {
            // Arrange
            var map = new Map(3, 3);
            var position = new Position(1, 1);

            // Act
            map.AddMountain(position);

            // Assert
            Assert.Equal(CellType.Mountain, map.GetCell(position).Type);
        }

        [Fact]
        public void AddTreasures_AddsTreasuresToCell()
        {
            // Arrange
            var map = new Map(3, 3);
            var position = new Position(1, 1);

            // Act
            map.AddTreasures(position, 2);

            // Assert
            Assert.Equal(CellType.Treasure, map.GetCell(position).Type);
            Assert.Equal(2, map.GetCell(position).TreasureCount);
        }

        [Fact]
        public void GetCell_ReturnsCorrectCell()
        {
            // Arrange
            var map = new Map(3, 3);
            var position = new Position(1, 1);

            // Act
            var cell = map.GetCell(position);

            // Assert
            Assert.Equal(CellType.Plain, cell.Type);
        }

        [Fact]
        public void GetCell_ThrowsExceptionForInvalidPosition()
        {
            // Arrange
            var map = new Map(3, 3);
            var position = new Position(-1, 1);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => map.GetCell(position));
        }

        [Fact]
        public void AddAdventurer_AddsAdventurerToCell()
        {
            // Arrange
            var map = new Map(3, 3);
            var adventurer = new Adventurer("John", new Position(1, 1), Orientation.North, "");

            // Act
            map.AddAdventurer(adventurer);

            // Assert
            Assert.Equal(adventurer, map.GetCell(adventurer.Position).Adventurer);
        }

        [Fact]
        public void MoveAdventurer_MovesAdventurerToNewPosition()
        {
            // Arrange
            var map = new Map(3, 3);
            var adventurer = new Adventurer("John", new Position(1, 1), Orientation.North, "");
            map.AddAdventurer(adventurer);
            var newPosition = new Position(1, 0);

            // Act
            map.MoveAdventurer(adventurer.Position, newPosition);

            // Assert
            Assert.Null(map.GetCell(adventurer.Position).Adventurer);
            Assert.Equal(adventurer, map.GetCell(newPosition).Adventurer);
        }

        [Fact]
        public void MoveAdventurer_ThrowsExceptionWhenNoAdventurerAtSourcePosition()
        {
            // Arrange
            var map = new Map(3, 3);
            var position = new Position(1, 1);
            var newPosition = new Position(1, 0);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => map.MoveAdventurer(position, newPosition));
        }

        [Fact]
        public void IsPositionOccupied_ReturnsTrueWhenAdventurerOccupiesPosition()
        {
            // Arrange
            var map = new Map(3, 3);
            var adventurer = new Adventurer("John", new Position(1, 1), Orientation.North, "");
            map.AddAdventurer(adventurer);

            // Act
            bool result = map.IsPositionOccupied(adventurer.Position);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsPositionOccupied_ReturnsFalseWhenNoAdventurerAtPosition()
        {
            // Arrange
            var map = new Map(3, 3);
            var position = new Position(1, 1);

            // Act
            bool result = map.IsPositionOccupied(position);

            // Assert
            Assert.False(result);
        }
    }
}