using Xunit;
using TreasureMap.Core.Enums;
using TreasureMap.Core.Models;

namespace TreasureMap.Tests.Models
{
    public class AdventurerTests
    {
        [Fact]
        public void Constructor_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var name = "John";
            var position = new Position(0, 0);
            var orientation = Orientation.North;
            var movements = "AADAGA";

            // Act
            var adventurer = new Adventurer(name, position, orientation, movements);

            // Assert
            Assert.Equal(name, adventurer.Name);
            Assert.Equal(position, adventurer.Position);
            Assert.Equal(orientation, adventurer.Orientation);
            Assert.Equal(0, adventurer.CollectedTreasures);
            Assert.Equal(MovementType.Forward, adventurer.PeekNextMovement());
        }

        [Fact]
        public void GetNextPosition_ShouldReturnCorrectPosition()
        {
            // Arrange
            var adventurer = new Adventurer("John", new Position(1, 1), Orientation.North, "A");

            // Act
            var nextPosition = adventurer.GetNextPosition();

            // Assert
            Assert.Equal(new Position(1, 0), nextPosition);
        }

        [Fact]
        public void TurnLeft_ShouldUpdateOrientationCorrectly()
        {
            // Arrange
            var adventurer = new Adventurer("John", new Position(0, 0), Orientation.North, "");

            // Act
            adventurer.TurnLeft();

            // Assert
            Assert.Equal(Orientation.West, adventurer.Orientation);
        }

        [Fact]
        public void TurnRight_ShouldUpdateOrientationCorrectly()
        {
            // Arrange
            var adventurer = new Adventurer("John", new Position(0, 0), Orientation.North, "");

            // Act
            adventurer.TurnRight();

            // Assert
            Assert.Equal(Orientation.East, adventurer.Orientation);
        }

        [Fact]
        public void MoveTo_ShouldUpdatePositionCorrectly()
        {
            // Arrange
            var adventurer = new Adventurer("John", new Position(0, 0), Orientation.North, "");
            var newPosition = new Position(1, 1);

            // Act
            adventurer.MoveTo(newPosition);

            // Assert
            Assert.Equal(newPosition, adventurer.Position);
        }

        [Fact]
        public void CollectTreasure_ShouldIncrementCollectedTreasuresCount()
        {
            // Arrange
            var adventurer = new Adventurer("John", new Position(0, 0), Orientation.North, "");

            // Act
            adventurer.CollectTreasure();

            // Assert
            Assert.Equal(1, adventurer.CollectedTreasures);
        }
    }
}