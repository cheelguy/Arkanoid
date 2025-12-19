using Arkanoid.Models;
using Xunit;

namespace Arkanoid.Tests
{
    public class BallTests
    {
        [Fact]
        public void Constructor_WithParameters_SetsProperties()
        {
            // Подготовка
            var position = new Vector2(10, 20);
            var velocity = new Vector2(1, -1);
            float radius = 1.5f;

            // Действие
            var ball = new Ball(position, velocity, radius);

            // Проверка
            Assert.Equal(position.X, ball.Position.X);
            Assert.Equal(position.Y, ball.Position.Y);
            Assert.Equal(velocity.X, ball.Velocity.X);
            Assert.Equal(velocity.Y, ball.Velocity.Y);
            Assert.Equal(radius, ball.Radius);
            Assert.True(ball.IsActive);
        }

        [Fact]
        public void Constructor_Default_SetsDefaultValues()
        {
            // Подготовка & Act
            var ball = new Ball();

            // Проверка
            Assert.Equal(40, ball.Position.X);
            Assert.Equal(25, ball.Position.Y);
            Assert.Equal(0, ball.Velocity.X);
            Assert.Equal(-1, ball.Velocity.Y);
            Assert.Equal(0.5f, ball.Radius);
            Assert.Equal(12f, ball.Speed);
            Assert.False(ball.IsActive);
        }

        [Fact]
        public void Update_ActiveBall_MovesPosition()
        {
            // Подготовка
            var ball = new Ball(new Vector2(10, 10), new Vector2(5, 3), 0.5f);
            ball.IsActive = true;
            ball.Speed = 10f;
            ball.Velocity = new Vector2(5, 3).Normalize() * ball.Speed;
            var initialPosition = ball.Position.Clone();
            float deltaTime = 0.1f;

            // Действие
            ball.Update(deltaTime);

            // Проверка
            Assert.NotEqual(initialPosition.X, ball.Position.X);
            Assert.NotEqual(initialPosition.Y, ball.Position.Y);
        }

        [Fact]
        public void Update_InactiveBall_DoesNotMove()
        {
            // Подготовка
            var ball = new Ball(new Vector2(10, 10), new Vector2(5, 3), 0.5f);
            ball.IsActive = false;
            var initialPosition = ball.Position.Clone();
            float deltaTime = 0.1f;

            // Действие
            ball.Update(deltaTime);

            // Проверка
            Assert.Equal(initialPosition.X, ball.Position.X);
            Assert.Equal(initialPosition.Y, ball.Position.Y);
        }

        [Fact]
        public void Reflect_WithNormal_ReflectsVelocity()
        {
            // Подготовка
            var ball = new Ball(new Vector2(10, 10), new Vector2(1, 1), 0.5f);
            ball.Speed = 10f;
            ball.Velocity = new Vector2(1, 1).Normalize() * ball.Speed;
            var normal = new Vector2(0, 1); // Нормаль вверх
            var initialVelocity = ball.Velocity.Clone();

            // Действие
            ball.Reflect(normal);

            // Проверка
            // При отражении от нормали (0,1) скорость должна измениться
            // Скорость должна сохраниться
            Assert.Equal(ball.Speed, ball.Velocity.Length(), 0.1f);
            // Y компонент должен измениться (отразиться)
            Assert.NotEqual(initialVelocity.Y, ball.Velocity.Y);
        }

        [Fact]
        public void ReflectHorizontal_ReversesXComponent()
        {
            // Подготовка
            var ball = new Ball(new Vector2(10, 10), new Vector2(5, 3), 0.5f);
            var initialVelocityX = ball.Velocity.X;
            var initialVelocityY = ball.Velocity.Y;

            // Действие
            ball.ReflectHorizontal();

            // Проверка
            Assert.Equal(-initialVelocityX, ball.Velocity.X);
            Assert.Equal(initialVelocityY, ball.Velocity.Y);
        }

        [Fact]
        public void ReflectVertical_ReversesYComponent()
        {
            // Подготовка
            var ball = new Ball(new Vector2(10, 10), new Vector2(5, 3), 0.5f);
            var initialVelocityX = ball.Velocity.X;
            var initialVelocityY = ball.Velocity.Y;

            // Действие
            ball.ReflectVertical();

            // Проверка
            Assert.Equal(initialVelocityX, ball.Velocity.X);
            Assert.Equal(-initialVelocityY, ball.Velocity.Y);
        }

        [Fact]
        public void SetSpeed_ChangesSpeedButKeepsDirection()
        {
            // Подготовка
            var ball = new Ball(new Vector2(10, 10), new Vector2(1, 1), 0.5f);
            ball.Speed = 10f;
            ball.Velocity = new Vector2(1, 1).Normalize() * ball.Speed;
            float newSpeed = 15f;
            var initialDirection = ball.Velocity.Normalize();

            // Действие
            ball.SetSpeed(newSpeed);

            // Проверка
            Assert.Equal(newSpeed, ball.Speed);
            Assert.Equal(newSpeed, ball.Velocity.Length(), 0.1f);
            // Направление должно сохраниться
            var newDirection = ball.Velocity.Normalize();
            Assert.Equal(initialDirection.X, newDirection.X, 0.1f);
            Assert.Equal(initialDirection.Y, newDirection.Y, 0.1f);
        }

        [Fact]
        public void IncreaseSpeed_IncreasesSpeedByPercentage()
        {
            // Подготовка
            var ball = new Ball(new Vector2(10, 10), new Vector2(1, 0), 0.5f);
            ball.Speed = 10f;
            ball.Velocity = new Vector2(1, 0) * ball.Speed;
            float initialSpeed = ball.Speed;
            float percentage = 20f; // +20%

            // Действие
            ball.IncreaseSpeed(percentage);

            // Проверка
            Assert.Equal(initialSpeed * 1.2f, ball.Speed, 0.1f);
        }

        [Fact]
        public void DecreaseSpeed_DecreasesSpeedByPercentage()
        {
            // Подготовка
            var ball = new Ball(new Vector2(10, 10), new Vector2(1, 0), 0.5f);
            ball.Speed = 10f;
            ball.Velocity = new Vector2(1, 0) * ball.Speed;
            float initialSpeed = ball.Speed;
            float percentage = 20f; // -20%

            // Действие
            ball.DecreaseSpeed(percentage);

            // Проверка
            Assert.Equal(initialSpeed * 0.8f, ball.Speed, 0.1f);
        }

        [Fact]
        public void Reset_SetsPositionAndDeactivates()
        {
            // Подготовка
            var ball = new Ball(new Vector2(10, 10), new Vector2(5, 3), 0.5f);
            ball.IsActive = true;
            float paddleX = 40f;
            float paddleY = 27f;

            // Действие
            ball.Reset(paddleX, paddleY);

            // Проверка
            Assert.Equal(paddleX, ball.Position.X);
            Assert.Equal(paddleY - 2, ball.Position.Y);
            Assert.Equal(0, ball.Velocity.X);
            Assert.Equal(-ball.Speed, ball.Velocity.Y);
            Assert.False(ball.IsActive);
        }

        [Fact]
        public void Launch_ActivatesBallAndSetsVelocity()
        {
            // Подготовка
            var ball = new Ball(new Vector2(10, 10), new Vector2(0, 0), 0.5f);
            ball.IsActive = false;
            ball.Speed = 10f;
            float angle = 0; // Вверх

            // Действие
            ball.Launch(angle);

            // Проверка
            Assert.True(ball.IsActive);
            Assert.Equal(ball.Speed, ball.Velocity.Length(), 0.1f);
            // При угле 0 скорость должна быть направлена вверх (отрицательный Y)
            Assert.True(ball.Velocity.Y < 0);
        }
    }
}

