using Xunit;
using TreasureMap.Core.Enums;
using TreasureMap.Core.Models;

namespace TreasureMap.Tests.Models
{
    public class CellTests
    {
        [Fact]
        public void Constructor_InitializesCell()
        {
            // Arrange & Act
            var cell = new Cell();

            // Assert
            Assert.Equal(CellType.Plain, cell.Type);
            Assert.Equal(0, cell.TreasureCount);
            Assert.Null(cell.Adventurer);
        }

        [Fact]
        public void SetAdventurer_SetsAdventurerOnPlainCell()
        {
            // Arrange
            var cell = new Cell();
            var adventurer = new Adventurer("John", new Position(0, 0), Orientation.North, "");

            // Act
            cell.SetAdventurer(adventurer);

            // Assert
            Assert.Equal(adventurer, cell.Adventurer);
        }

        [Fact]
        public void SetAdventurer_ThrowsExceptionOnMountainCell()
        {
            // Arrange
            var cell = new Cell();
            cell.ConvertToMountain();
            var adventurer = new Adventurer("John", new Position(0, 0), Orientation.North, "");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => cell.SetAdventurer(adventurer));
        }

        [Fact]
        public void ConvertToMountain_ConvertsPlainCellToMountain()
        {
            // Arrange
            var cell = new Cell();

            // Act
            cell.ConvertToMountain();

            // Assert
            Assert.Equal(CellType.Mountain, cell.Type);
        }

        [Fact]
        public void ConvertToMountain_ThrowsExceptionOnCellWithTreasures()
        {
            // Arrange
            var cell = new Cell();
            cell.AddTreasures(1);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => cell.ConvertToMountain());
        }

        [Fact]
        public void AddTreasures_AddsTreasuresToPlainCell()
        {
            // Arrange
            var cell = new Cell();

            // Act
            cell.AddTreasures(2);

            // Assert
            Assert.Equal(CellType.Treasure, cell.Type);
            Assert.Equal(2, cell.TreasureCount);
        }

        [Fact]
        public void AddTreasures_ThrowsExceptionOnMountainCell()
        {
            // Arrange
            var cell = new Cell();
            cell.ConvertToMountain();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => cell.AddTreasures(1));
        }

        [Fact]
        public void CollectTreasure_CollectsTreasureFromCell()
        {
            // Arrange
            var cell = new Cell();
            cell.AddTreasures(2);

            // Act
            bool result = cell.CollectTreasure();

            // Assert
            Assert.True(result);
            Assert.Equal(CellType.Treasure, cell.Type);
            Assert.Equal(1, cell.TreasureCount);
        }

        [Fact]
        public void CollectTreasure_ReturnsFalseWhenNoTreasuresLeft()
        {
            // Arrange
            var cell = new Cell();

            // Act
            bool result = cell.CollectTreasure();

            // Assert
            Assert.False(result);
            Assert.Equal(CellType.Plain, cell.Type);
            Assert.Equal(0, cell.TreasureCount);
        }
    }
}