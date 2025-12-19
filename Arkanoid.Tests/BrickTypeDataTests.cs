using Arkanoid.Models;
using Xunit;

namespace Arkanoid.Tests
{
    public class BrickTypeDataTests
    {
        [Theory]
        [InlineData(BrickType.Normal, 1, 10, ConsoleColor.Green, 0.2f, '#')]
        [InlineData(BrickType.Strong, 2, 20, ConsoleColor.Yellow, 0.4f, '=')]
        [InlineData(BrickType.VeryStrong, 3, 30, ConsoleColor.Red, 0.5f, '@')]
        [InlineData(BrickType.Unbreakable, int.MaxValue, 0, ConsoleColor.DarkGray, 0.0f, 'X')]
        public void GetData_ReturnsCorrectData(BrickType type, int expectedHealth, int expectedPoints, 
            ConsoleColor expectedColor, float expectedDropChance, char expectedSymbol)
        {
            // Действие
            var data = BrickTypeData.GetData(type);

            // Проверка
            Assert.Equal(type, data.Type);
            Assert.Equal(expectedHealth, data.MaxHealth);
            Assert.Equal(expectedPoints, data.Points);
            Assert.Equal(expectedColor, data.Color);
            Assert.Equal(expectedDropChance, data.PowerUpDropChance);
            Assert.Equal(expectedSymbol, data.Symbol);
        }

        [Fact]
        public void GetHealthColor_FullHealth_ReturnsBaseColor()
        {
            // Подготовка
            var baseColor = ConsoleColor.Green;
            int currentHealth = 3;
            int maxHealth = 3;

            // Действие
            var color = BrickTypeData.GetHealthColor(currentHealth, maxHealth, baseColor);

            // Проверка
            Assert.Equal(baseColor, color);
        }

        [Fact]
        public void GetHealthColor_DamagedGreen_ReturnsDarkGreen()
        {
            // Подготовка
            var baseColor = ConsoleColor.Green;
            int currentHealth = 1;
            int maxHealth = 3;

            // Действие
            var color = BrickTypeData.GetHealthColor(currentHealth, maxHealth, baseColor);

            // Проверка
            Assert.Equal(ConsoleColor.DarkGreen, color);
        }

        [Fact]
        public void GetHealthColor_DamagedYellow_ReturnsDarkYellow()
        {
            // Подготовка
            var baseColor = ConsoleColor.Yellow;
            int currentHealth = 1;
            int maxHealth = 2;

            // Действие
            var color = BrickTypeData.GetHealthColor(currentHealth, maxHealth, baseColor);

            // Проверка
            Assert.Equal(ConsoleColor.DarkYellow, color);
        }

        [Fact]
        public void GetHealthColor_DamagedRed_ReturnsDarkRed()
        {
            // Подготовка
            var baseColor = ConsoleColor.Red;
            int currentHealth = 1;
            int maxHealth = 3;

            // Действие
            var color = BrickTypeData.GetHealthColor(currentHealth, maxHealth, baseColor);

            // Проверка
            Assert.Equal(ConsoleColor.DarkRed, color);
        }

        [Fact]
        public void GetHealthColor_UnknownColor_ReturnsBaseColor()
        {
            // Подготовка
            var baseColor = ConsoleColor.Blue;
            int currentHealth = 1;
            int maxHealth = 3;

            // Действие
            var color = BrickTypeData.GetHealthColor(currentHealth, maxHealth, baseColor);

            // Проверка
            Assert.Equal(baseColor, color);
        }

        [Fact]
        public void Constructor_SetsAllProperties()
        {
            // Подготовка и действие
            var data = new BrickTypeData(
                BrickType.Normal,
                maxHealth: 1,
                points: 10,
                color: ConsoleColor.Green,
                powerUpDropChance: 0.2f,
                symbol: '#'
            );

            // Проверка
            Assert.Equal(BrickType.Normal, data.Type);
            Assert.Equal(1, data.MaxHealth);
            Assert.Equal(10, data.Points);
            Assert.Equal(ConsoleColor.Green, data.Color);
            Assert.Equal(0.2f, data.PowerUpDropChance);
            Assert.Equal('#', data.Symbol);
        }
    }
}

