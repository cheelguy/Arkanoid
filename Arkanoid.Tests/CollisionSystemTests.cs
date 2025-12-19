using Arkanoid.Core;
using Arkanoid.Models;
using Xunit;

namespace Arkanoid.Tests
{
    public class CollisionSystemTests
    {
        [Fact]
        public void CheckBallPaddleCollision_ActiveBallColliding_ReturnsTrue()
        {
            // Подготовка
            var ball = new Ball(new Vector2(40, 26.5f), new Vector2(0, 1), 0.5f);
            ball.IsActive = true;
            var paddle = new Paddle(40, 27, 10, 1, 30);

            // Действие
            bool collision = CollisionSystem.CheckBallPaddleCollision(ball, paddle);

            // Проверка
            Assert.True(collision);
        }

        [Fact]
        public void CheckBallPaddleCollision_InactiveBall_ReturnsFalse()
        {
            // Подготовка
            var ball = new Ball(new Vector2(40, 26), new Vector2(0, 1), 0.5f);
            ball.IsActive = false;
            var paddle = new Paddle(40, 27, 10, 1, 30);

            // Действие
            bool collision = CollisionSystem.CheckBallPaddleCollision(ball, paddle);

            // Проверка
            Assert.False(collision);
        }

        [Fact]
        public void CheckBallPaddleCollision_BallFarAway_ReturnsFalse()
        {
            // Подготовка
            var ball = new Ball(new Vector2(10, 10), new Vector2(0, 1), 0.5f);
            ball.IsActive = true;
            var paddle = new Paddle(40, 27, 10, 1, 30);

            // Действие
            bool collision = CollisionSystem.CheckBallPaddleCollision(ball, paddle);

            // Проверка
            Assert.False(collision);
        }

        [Fact]
        public void HandleBallPaddleCollision_Collision_ReflectsBall()
        {
            // Подготовка
            var ball = new Ball(new Vector2(40, 26.3f), new Vector2(0, 1), 0.5f);
            ball.IsActive = true;
            ball.Speed = 10f;
            ball.Velocity = new Vector2(0, 1) * ball.Speed;
            var paddle = new Paddle(40, 27, 10, 1, 30);
            var initialVelocityY = ball.Velocity.Y;

            // Действие
            bool handled = CollisionSystem.HandleBallPaddleCollision(ball, paddle);

            // Проверка
            Assert.True(handled);
            // Мяч должен отразиться вверх
            Assert.True(ball.Velocity.Y < 0);
        }

        [Fact]
        public void CheckBallBrickCollision_ActiveBallColliding_ReturnsTrue()
        {
            // Подготовка
            var ball = new Ball(new Vector2(10, 10), new Vector2(0, -1), 0.5f);
            ball.IsActive = true;
            var brick = new Brick(8, 8, 4, 2, BrickType.Normal);

            // Действие
            bool collision = CollisionSystem.CheckBallBrickCollision(ball, brick);

            // Проверка
            Assert.True(collision);
        }

        [Fact]
        public void CheckBallBrickCollision_DestroyedBrick_ReturnsFalse()
        {
            // Подготовка
            var ball = new Ball(new Vector2(10, 10), new Vector2(0, -1), 0.5f);
            ball.IsActive = true;
            var brick = new Brick(8, 8, 4, 2, BrickType.Normal);
            brick.Hit(); // Разрушаем кирпич

            // Действие
            bool collision = CollisionSystem.CheckBallBrickCollision(ball, brick);

            // Проверка
            Assert.False(collision);
        }

        [Fact]
        public void CheckBallBrickCollision_BallFarAway_ReturnsFalse()
        {
            // Подготовка
            var ball = new Ball(new Vector2(50, 50), new Vector2(0, -1), 0.5f);
            ball.IsActive = true;
            var brick = new Brick(8, 8, 4, 2, BrickType.Normal);

            // Действие
            bool collision = CollisionSystem.CheckBallBrickCollision(ball, brick);

            // Проверка
            Assert.False(collision);
        }

        [Fact]
        public void HandleBallBrickCollision_Collision_ReflectsBall()
        {
            // Подготовка
            var ball = new Ball(new Vector2(10, 9), new Vector2(0, -1), 0.5f);
            ball.IsActive = true;
            ball.Speed = 10f;
            ball.Velocity = new Vector2(0, -1) * ball.Speed;
            var brick = new Brick(8, 8, 4, 2, BrickType.Normal);
            var initialVelocityY = ball.Velocity.Y;

            // Действие
            bool handled = CollisionSystem.HandleBallBrickCollision(ball, brick);

            // Проверка
            Assert.True(handled);
            // Мяч должен отразиться вниз
            Assert.True(ball.Velocity.Y > 0);
        }

        [Fact]
        public void CheckBallWallCollision_LeftWall_ReturnsTrue()
        {
            // Подготовка
            var field = new GameField(80, 30);
            var ball = new Ball(new Vector2(0.3f, 15), new Vector2(-1, 0), 0.5f);
            ball.IsActive = true;

            // Действие
            bool collision = CollisionSystem.CheckBallWallCollision(ball, field);

            // Проверка
            Assert.True(collision);
        }

        [Fact]
        public void CheckBallWallCollision_RightWall_ReturnsTrue()
        {
            // Подготовка
            var field = new GameField(80, 30);
            var ball = new Ball(new Vector2(79.7f, 15), new Vector2(1, 0), 0.5f);
            ball.IsActive = true;

            // Действие
            bool collision = CollisionSystem.CheckBallWallCollision(ball, field);

            // Проверка
            Assert.True(collision);
        }

        [Fact]
        public void CheckBallWallCollision_TopWall_ReturnsTrue()
        {
            // Подготовка
            var field = new GameField(80, 30);
            var ball = new Ball(new Vector2(40, 0.3f), new Vector2(0, -1), 0.5f);
            ball.IsActive = true;

            // Действие
            bool collision = CollisionSystem.CheckBallWallCollision(ball, field);

            // Проверка
            Assert.True(collision);
        }

        [Fact]
        public void HandleBallWallCollision_LeftWall_ReflectsHorizontally()
        {
            // Подготовка
            var field = new GameField(80, 30);
            var ball = new Ball(new Vector2(0.3f, 15), new Vector2(-1, 0), 0.5f);
            ball.IsActive = true;
            ball.Speed = 10f;
            ball.Velocity = new Vector2(-1, 0) * ball.Speed;
            var initialVelocityX = ball.Velocity.X;

            // Действие
            bool handled = CollisionSystem.HandleBallWallCollision(ball, field);

            // Проверка
            Assert.True(handled);
            Assert.True(ball.Velocity.X > 0); // Должен отразиться вправо
        }

        [Fact]
        public void CheckBallOutOfBounds_BallBelowBottom_ReturnsTrue()
        {
            // Подготовка
            var field = new GameField(80, 30);
            var ball = new Ball(new Vector2(40, 31), new Vector2(0, 1), 0.5f);
            ball.IsActive = true;

            // Действие
            bool outOfBounds = CollisionSystem.CheckBallOutOfBounds(ball, field);

            // Проверка
            Assert.True(outOfBounds);
        }

        [Fact]
        public void CheckBallOutOfBounds_BallAboveBottom_ReturnsFalse()
        {
            // Подготовка
            var field = new GameField(80, 30);
            var ball = new Ball(new Vector2(40, 25), new Vector2(0, 1), 0.5f);
            ball.IsActive = true;

            // Действие
            bool outOfBounds = CollisionSystem.CheckBallOutOfBounds(ball, field);

            // Проверка
            Assert.False(outOfBounds);
        }
    }
}

