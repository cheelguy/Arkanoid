using Arkanoid.Models;
using Xunit;

namespace Arkanoid.Tests
{
    public class BrickTests
    {
        [Fact]
        public void Constructor_WithParameters_SetsProperties()
        {
            // Подготовка и действие
            var brick = new Brick(10, 20, 5, 3, BrickType.Normal);

            // Проверка
            Assert.Equal(10, brick.Position.X);
            Assert.Equal(20, brick.Position.Y);
            Assert.Equal(5, brick.Width);
            Assert.Equal(3, brick.Height);
            Assert.Equal(BrickType.Normal, brick.Type);
            Assert.False(brick.IsDestroyed);
        }

        [Fact]
        public void Constructor_WithVectorPosition_SetsProperties()
        {
            // Подготовка
            var position = new Vector2(10, 20);

            // Действие
            var brick = new Brick(position, 5, 3, BrickType.Strong);

            // Проверка
            Assert.Equal(position.X, brick.Position.X);
            Assert.Equal(position.Y, brick.Position.Y);
            Assert.Equal(BrickType.Strong, brick.Type);
        }

        [Theory]
        [InlineData(BrickType.Normal, 1)]
        [InlineData(BrickType.Strong, 2)]
        [InlineData(BrickType.VeryStrong, 3)]
        public void Constructor_SetsCorrectHealth(BrickType type, int expectedHealth)
        {
            // Подготовка и действие
            var brick = new Brick(0, 0, 5, 3, type);

            // Проверка
            Assert.Equal(expectedHealth, brick.MaxHealth);
            Assert.Equal(expectedHealth, brick.Health);
        }

        [Fact]
        public void Hit_NormalBrick_DecreasesHealth()
        {
            // Подготовка
            var brick = new Brick(0, 0, 5, 3, BrickType.Normal);
            int initialHealth = brick.Health;

            // Действие
            bool destroyed = brick.Hit();

            // Проверка
            Assert.Equal(initialHealth - 1, brick.Health);
            Assert.True(destroyed); // Normal кирпич разрушается с одного удара
            Assert.True(brick.IsDestroyed);
        }

        [Fact]
        public void Hit_StrongBrick_TakesMultipleHits()
        {
            // Подготовка
            var brick = new Brick(0, 0, 5, 3, BrickType.Strong);

            // Действие & Assert
            bool firstHit = brick.Hit();
            Assert.False(firstHit);
            Assert.Equal(1, brick.Health);
            Assert.False(brick.IsDestroyed);

            bool secondHit = brick.Hit();
            Assert.True(secondHit);
            Assert.Equal(0, brick.Health);
            Assert.True(brick.IsDestroyed);
        }

        [Fact]
        public void Hit_UnbreakableBrick_DoesNotTakeDamage()
        {
            // Подготовка
            var brick = new Brick(0, 0, 5, 3, BrickType.Unbreakable);
            int initialHealth = brick.Health;

            // Действие
            bool destroyed = brick.Hit();

            // Проверка
            Assert.Equal(initialHealth, brick.Health);
            Assert.False(destroyed);
            Assert.False(brick.IsDestroyed);
        }

        [Fact]
        public void GetBounds_ReturnsCorrectBounds()
        {
            // Подготовка
            var brick = new Brick(10, 20, 5, 3, BrickType.Normal);

            // Действие
            var bounds = brick.GetBounds();

            // Проверка
            Assert.Equal(10, bounds.left);
            Assert.Equal(20, bounds.top);
            Assert.Equal(15, bounds.right);
            Assert.Equal(23, bounds.bottom);
        }

        [Fact]
        public void GetCenter_ReturnsCenterPosition()
        {
            // Подготовка
            var brick = new Brick(10, 20, 5, 3, BrickType.Normal);

            // Действие
            var center = brick.GetCenter();

            // Проверка
            Assert.Equal(12.5f, center.X);
            Assert.Equal(21.5f, center.Y);
        }

        [Fact]
        public void ShouldDropPowerUp_UnbreakableBrick_ReturnsFalse()
        {
            // Подготовка
            var brick = new Brick(0, 0, 5, 3, BrickType.Unbreakable);

            // Действие
            bool shouldDrop = brick.ShouldDropPowerUp();

            // Проверка
            Assert.False(shouldDrop);
        }

        [Fact]
        public void ShouldDropPowerUp_NotDestroyed_ReturnsFalse()
        {
            // Подготовка
            var brick = new Brick(0, 0, 5, 3, BrickType.Normal);
            // Кирпич не разрушен

            // Действие
            bool shouldDrop = brick.ShouldDropPowerUp();

            // Проверка
            Assert.False(shouldDrop);
        }

        [Fact]
        public void ContainsPoint_PointInside_ReturnsTrue()
        {
            // Подготовка
            var brick = new Brick(10, 20, 5, 3, BrickType.Normal);
            var point = new Vector2(12, 21);

            // Действие
            bool contains = brick.ContainsPoint(point);

            // Проверка
            Assert.True(contains);
        }

        [Fact]
        public void ContainsPoint_PointOutside_ReturnsFalse()
        {
            // Подготовка
            var brick = new Brick(10, 20, 5, 3, BrickType.Normal);
            var point = new Vector2(20, 30);

            // Действие
            bool contains = brick.ContainsPoint(point);

            // Проверка
            Assert.False(contains);
        }

        [Fact]
        public void GetPoints_ReturnsCorrectPoints()
        {
            // Подготовка
            var normalBrick = new Brick(0, 0, 5, 3, BrickType.Normal);
            var strongBrick = new Brick(0, 0, 5, 3, BrickType.Strong);
            var veryStrongBrick = new Brick(0, 0, 5, 3, BrickType.VeryStrong);

            // Действие & Assert
            Assert.Equal(10, normalBrick.GetPoints());
            Assert.Equal(20, strongBrick.GetPoints());
            Assert.Equal(30, veryStrongBrick.GetPoints());
        }

        [Fact]
        public void GetCurrentColor_ReturnsCorrectColor()
        {
            // Подготовка
            var brick = new Brick(0, 0, 5, 3, BrickType.Normal);

            // Действие
            var color = brick.GetCurrentColor();

            // Проверка
            Assert.Equal(System.ConsoleColor.Green, color);
        }

        [Fact]
        public void GetSymbol_ReturnsCorrectSymbol()
        {
            // Подготовка
            var brick = new Brick(0, 0, 5, 3, BrickType.Normal);

            // Действие
            var symbol = brick.GetSymbol();

            // Проверка
            Assert.Equal('#', symbol);
        }
    }
}

