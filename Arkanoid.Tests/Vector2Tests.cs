using Arkanoid.Models;
using Xunit;

namespace Arkanoid.Tests
{
    public class Vector2Tests
    {
        [Fact]
        public void Constructor_Default_CreatesZeroVector()
        {
            // Подготовка и действие
            var vector = new Vector2();

            // Проверка
            Assert.Equal(0, vector.X);
            Assert.Equal(0, vector.Y);
        }

        [Fact]
        public void Constructor_WithParameters_SetsCoordinates()
        {
            // Подготовка и действие
            var vector = new Vector2(5.5f, 10.2f);

            // Проверка
            Assert.Equal(5.5f, vector.X);
            Assert.Equal(10.2f, vector.Y);
        }

        [Theory]
        [InlineData(3, 4, 5)]
        [InlineData(5, 12, 13)]
        [InlineData(0, 0, 0)]
        [InlineData(1, 0, 1)]
        [InlineData(0, 1, 1)]
        public void Length_CalculatesCorrectly(float x, float y, float expectedLength)
        {
            // Подготовка
            var vector = new Vector2(x, y);

            // Действие
            var length = vector.Length();

            // Проверка
            Assert.Equal(expectedLength, length, 0.001f);
        }

        [Fact]
        public void Normalize_NonZeroVector_ReturnsUnitVector()
        {
            // Подготовка
            var vector = new Vector2(3, 4);

            // Действие
            var normalized = vector.Normalize();

            // Проверка
            Assert.Equal(1.0f, normalized.Length(), 0.001f);
            Assert.Equal(0.6f, normalized.X, 0.001f);
            Assert.Equal(0.8f, normalized.Y, 0.001f);
        }

        [Fact]
        public void Normalize_ZeroVector_ReturnsZeroVector()
        {
            // Подготовка
            var vector = new Vector2(0, 0);

            // Действие
            var normalized = vector.Normalize();

            // Проверка
            Assert.Equal(0, normalized.X);
            Assert.Equal(0, normalized.Y);
        }

        [Fact]
        public void Dot_CalculatesDotProduct()
        {
            // Подготовка
            var vector1 = new Vector2(2, 3);
            var vector2 = new Vector2(4, 5);

            // Действие
            var dotProduct = vector1.Dot(vector2);

            // Проверка
            Assert.Equal(23, dotProduct); // 2*4 + 3*5 = 8 + 15 = 23
        }

        [Fact]
        public void Operator_Addition_AddsVectors()
        {
            // Подготовка
            var vector1 = new Vector2(2, 3);
            var vector2 = new Vector2(4, 5);

            // Действие
            var result = vector1 + vector2;

            // Проверка
            Assert.Equal(6, result.X);
            Assert.Equal(8, result.Y);
        }

        [Fact]
        public void Operator_Subtraction_SubtractsVectors()
        {
            // Подготовка
            var vector1 = new Vector2(5, 7);
            var vector2 = new Vector2(2, 3);

            // Действие
            var result = vector1 - vector2;

            // Проверка
            Assert.Equal(3, result.X);
            Assert.Equal(4, result.Y);
        }

        [Fact]
        public void Operator_MultiplyVectorByScalar_MultipliesCoordinates()
        {
            // Подготовка
            var vector = new Vector2(2, 3);
            float scalar = 5;

            // Действие
            var result = vector * scalar;

            // Проверка
            Assert.Equal(10, result.X);
            Assert.Equal(15, result.Y);
        }

        [Fact]
        public void Operator_MultiplyScalarByVector_MultipliesCoordinates()
        {
            // Подготовка
            var vector = new Vector2(2, 3);
            float scalar = 5;

            // Действие
            var result = scalar * vector;

            // Проверка
            Assert.Equal(10, result.X);
            Assert.Equal(15, result.Y);
        }

        [Fact]
        public void Operator_DivideVectorByScalar_DividesCoordinates()
        {
            // Подготовка
            var vector = new Vector2(10, 15);
            float scalar = 5;

            // Действие
            var result = vector / scalar;

            // Проверка
            Assert.Equal(2, result.X);
            Assert.Equal(3, result.Y);
        }

        [Fact]
        public void Operator_DivideByZero_ReturnsZeroVector()
        {
            // Подготовка
            var vector = new Vector2(10, 15);
            float scalar = 0;

            // Действие
            var result = vector / scalar;

            // Проверка
            Assert.Equal(0, result.X);
            Assert.Equal(0, result.Y);
        }

        [Fact]
        public void Clone_CreatesNewVectorWithSameValues()
        {
            // Подготовка
            var original = new Vector2(5, 10);

            // Действие
            var cloned = original.Clone();

            // Проверка
            Assert.Equal(original.X, cloned.X);
            Assert.Equal(original.Y, cloned.Y);
            Assert.NotSame(original, cloned);
        }

        [Fact]
        public void ToString_ReturnsFormattedString()
        {
            // Подготовка
            var vector = new Vector2(5.123f, 10.456f);

            // Действие
            var result = vector.ToString();

            // Проверка
            // Формат: (5,12, 10,46) - с запятой как разделителем десятичных
            Assert.Contains("5", result);
            Assert.Contains("10", result);
            Assert.Contains(",", result);
        }
    }
}

