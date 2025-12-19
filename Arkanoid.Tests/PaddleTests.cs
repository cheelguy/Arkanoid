using Arkanoid.Models;
using Xunit;

namespace Arkanoid.Tests
{
    public class PaddleTests
    {
        [Fact]
        public void Constructor_WithParameters_SetsProperties()
        {
            // Подготовка и действие
            var paddle = new Paddle(40, 27, 10, 1, 30);

            // Проверка
            Assert.Equal(40, paddle.Position.X);
            Assert.Equal(27, paddle.Position.Y);
            Assert.Equal(10, paddle.Width);
            Assert.Equal(1, paddle.Height);
            Assert.Equal(30, paddle.Speed);
        }

        [Fact]
        public void Constructor_Default_SetsDefaultValues()
        {
            // Подготовка и действие
            var paddle = new Paddle();

            // Проверка
            Assert.Equal(40, paddle.Position.X);
            Assert.Equal(27, paddle.Position.Y);
            Assert.Equal(10, paddle.Width);
            Assert.Equal(1, paddle.Height);
            Assert.Equal(30, paddle.Speed);
        }

        [Fact]
        public void Move_Right_MovesPaddleRight()
        {
            // Подготовка
            var paddle = new Paddle(40, 27, 10, 1, 30);
            float initialX = paddle.Position.X;
            float deltaTime = 0.1f;
            float fieldWidth = 80;

            // Действие
            paddle.Move(1, deltaTime, fieldWidth);

            // Проверка
            Assert.True(paddle.Position.X > initialX);
        }

        [Fact]
        public void Move_Left_MovesPaddleLeft()
        {
            // Подготовка
            var paddle = new Paddle(40, 27, 10, 1, 30);
            float initialX = paddle.Position.X;
            float deltaTime = 0.1f;
            float fieldWidth = 80;

            // Действие
            paddle.Move(-1, deltaTime, fieldWidth);

            // Проверка
            Assert.True(paddle.Position.X < initialX);
        }

        [Fact]
        public void Move_ZeroDirection_DoesNotMove()
        {
            // Подготовка
            var paddle = new Paddle(40, 27, 10, 1, 30);
            float initialX = paddle.Position.X;
            float deltaTime = 0.1f;
            float fieldWidth = 80;

            // Действие
            paddle.Move(0, deltaTime, fieldWidth);

            // Проверка
            Assert.Equal(initialX, paddle.Position.X);
        }

        [Fact]
        public void Move_RightBoundary_StopsAtBoundary()
        {
            // Подготовка
            var paddle = new Paddle(75, 27, 10, 1, 30);
            float fieldWidth = 80;
            float deltaTime = 1.0f; // Большой шаг времени

            // Действие
            paddle.Move(1, deltaTime, fieldWidth);

            // Проверка
            // Платформа не должна выйти за правую границу
            Assert.True(paddle.GetRight() <= fieldWidth);
        }

        [Fact]
        public void Move_LeftBoundary_StopsAtBoundary()
        {
            // Подготовка
            var paddle = new Paddle(5, 27, 10, 1, 30);
            float fieldWidth = 80;
            float deltaTime = 1.0f; // Большой шаг времени

            // Действие
            paddle.Move(-1, deltaTime, fieldWidth);

            // Проверка
            // Платформа не должна выйти за левую границу
            Assert.True(paddle.GetLeft() >= 0);
        }

        [Fact]
        public void GetBounds_ReturnsCorrectBounds()
        {
            // Подготовка
            var paddle = new Paddle(40, 27, 10, 1, 30);

            // Действие
            var bounds = paddle.GetBounds();

            // Проверка
            Assert.Equal(35, bounds.left); // 40 - 10/2
            Assert.Equal(26.5f, bounds.top); // 27 - 1/2
            Assert.Equal(45, bounds.right); // 40 + 10/2
            Assert.Equal(27.5f, bounds.bottom); // 27 + 1/2
        }

        [Fact]
        public void GetLeft_ReturnsLeftBoundary()
        {
            // Подготовка
            var paddle = new Paddle(40, 27, 10, 1, 30);

            // Действие
            float left = paddle.GetLeft();

            // Проверка
            Assert.Equal(35, left);
        }

        [Fact]
        public void GetRight_ReturnsRightBoundary()
        {
            // Подготовка
            var paddle = new Paddle(40, 27, 10, 1, 30);

            // Действие
            float right = paddle.GetRight();

            // Проверка
            Assert.Equal(45, right);
        }

        [Fact]
        public void GetTop_ReturnsTopBoundary()
        {
            // Подготовка
            var paddle = new Paddle(40, 27, 10, 1, 30);

            // Действие
            float top = paddle.GetTop();

            // Проверка
            Assert.Equal(26.5f, top);
        }

        [Fact]
        public void GetBottom_ReturnsBottomBoundary()
        {
            // Подготовка
            var paddle = new Paddle(40, 27, 10, 1, 30);

            // Действие
            float bottom = paddle.GetBottom();

            // Проверка
            Assert.Equal(27.5f, bottom);
        }

        [Fact]
        public void Expand_IncreasesWidth()
        {
            // Подготовка
            var paddle = new Paddle(40, 27, 10, 1, 30);
            float initialWidth = paddle.Width;
            float multiplier = 1.5f;

            // Действие
            paddle.Expand(multiplier);

            // Проверка
            Assert.Equal(initialWidth * multiplier, paddle.Width);
        }

        [Fact]
        public void Shrink_DecreasesWidth()
        {
            // Подготовка
            var paddle = new Paddle(40, 27, 10, 1, 30);
            float initialWidth = paddle.Width;
            float multiplier = 0.7f;

            // Действие
            paddle.Shrink(multiplier);

            // Проверка
            Assert.Equal(initialWidth * multiplier, paddle.Width);
        }

        [Fact]
        public void ResetSize_RestoresBaseWidth()
        {
            // Подготовка
            var paddle = new Paddle(40, 27, 10, 1, 30);
            float baseWidth = paddle.Width;
            paddle.Expand(1.5f);

            // Действие
            paddle.ResetSize();

            // Проверка
            Assert.Equal(baseWidth, paddle.Width);
        }

        [Fact]
        public void GetRelativeHitPosition_Center_ReturnsZero()
        {
            // Подготовка
            var paddle = new Paddle(40, 27, 10, 1, 30);

            // Действие
            float relative = paddle.GetRelativeHitPosition(40);

            // Проверка
            Assert.Equal(0, relative, 0.001f);
        }

        [Fact]
        public void GetRelativeHitPosition_LeftEdge_ReturnsNegative()
        {
            // Подготовка
            var paddle = new Paddle(40, 27, 10, 1, 30);

            // Действие
            float relative = paddle.GetRelativeHitPosition(35); // Левая граница

            // Проверка
            Assert.Equal(-1, relative, 0.1f);
        }

        [Fact]
        public void GetRelativeHitPosition_RightEdge_ReturnsPositive()
        {
            // Подготовка
            var paddle = new Paddle(40, 27, 10, 1, 30);

            // Действие
            float relative = paddle.GetRelativeHitPosition(45); // Правая граница

            // Проверка
            Assert.Equal(1, relative, 0.1f);
        }
    }
}

