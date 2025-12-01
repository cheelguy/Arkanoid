namespace Arkanoid.Core
{
    using Arkanoid.Models;

    // Статический класс для обработки коллизий между игровыми объектами
    public static class CollisionSystem
    {
        // Проверяет столкновение мяча с платформой
        public static bool CheckBallPaddleCollision(Ball ball, Paddle paddle)
        {
            if (!ball.IsActive)
                return false;

            var paddleBounds = paddle.GetBounds();
            var ballPos = ball.Position;
            var ballRadius = ball.Radius;

            // Проверяем пересечение круга (мяч) с прямоугольником (платформа)
            // Находим ближайшую точку на прямоугольнике к центру круга
            float closestX = Math.Clamp(ballPos.X, paddleBounds.left, paddleBounds.right);
            float closestY = Math.Clamp(ballPos.Y, paddleBounds.top, paddleBounds.bottom);

            // Вычисляем расстояние от центра мяча до ближайшей точки
            float distanceX = ballPos.X - closestX;
            float distanceY = ballPos.Y - closestY;
            float distanceSquared = distanceX * distanceX + distanceY * distanceY;

            // Если расстояние меньше радиуса, произошло столкновение
            return distanceSquared < (ballRadius * ballRadius);
        }

        // Обрабатывает столкновение мяча с платформой и отражает мяч
        public static bool HandleBallPaddleCollision(Ball ball, Paddle paddle)
        {
            if (!CheckBallPaddleCollision(ball, paddle))
                return false;

            var paddleBounds = paddle.GetBounds();
            var ballPos = ball.Position;

            // Определяем, с какой стороны платформы произошло столкновение
            // Проверяем, находится ли мяч над платформой (основной случай)
            if (ballPos.Y < paddleBounds.top && ball.Velocity.Y > 0)
            {
                // Мяч ударился сверху платформы
                // Вычисляем нормаль отражения на основе позиции удара
                float relativeHitPosition = paddle.GetRelativeHitPosition(ballPos.X);
                
                // Угол отскока зависит от места удара (от -45 до +45 градусов)
                float angle = relativeHitPosition * (float)(Math.PI / 4); // -45° до +45°
                
                // Вычисляем нормаль для отражения
                Vector2 normal = new Vector2(
                    (float)Math.Sin(angle),
                    -(float)Math.Cos(angle)
                ).Normalize();

                ball.Reflect(normal);
                
                // Корректируем позицию мяча, чтобы он не застрял в платформе
                ball.Position = new Vector2(ballPos.X, paddleBounds.top - ball.Radius);
                
                return true;
            }
            // Если мяч ударился сбоку (редкий случай)
            else if (ballPos.X < paddleBounds.left && ball.Velocity.X > 0)
            {
                // Удар слева
                ball.ReflectHorizontal();
                ball.Position = new Vector2(paddleBounds.left - ball.Radius, ballPos.Y);
                return true;
            }
            else if (ballPos.X > paddleBounds.right && ball.Velocity.X < 0)
            {
                // Удар справа
                ball.ReflectHorizontal();
                ball.Position = new Vector2(paddleBounds.right + ball.Radius, ballPos.Y);
                return true;
            }

            return false;
        }

        // Проверяет столкновение мяча с кирпичом
        public static bool CheckBallBrickCollision(Ball ball, Brick brick)
        {
            if (!ball.IsActive || brick.IsDestroyed)
                return false;

            var brickBounds = brick.GetBounds();
            var ballPos = ball.Position;
            var ballRadius = ball.Radius;

            // Проверяем пересечение круга (мяч) с прямоугольником (кирпич)
            // Находим ближайшую точку на прямоугольнике к центру круга
            float closestX = Math.Clamp(ballPos.X, brickBounds.left, brickBounds.right);
            float closestY = Math.Clamp(ballPos.Y, brickBounds.top, brickBounds.bottom);

            // Вычисляем расстояние от центра мяча до ближайшей точки
            float distanceX = ballPos.X - closestX;
            float distanceY = ballPos.Y - closestY;
            float distanceSquared = distanceX * distanceX + distanceY * distanceY;

            // Если расстояние меньше радиуса, произошло столкновение
            return distanceSquared < (ballRadius * ballRadius);
        }

        // Обрабатывает столкновение мяча с кирпичом и отражает мяч
        public static bool HandleBallBrickCollision(Ball ball, Brick brick)
        {
            if (!CheckBallBrickCollision(ball, brick))
                return false;

            var brickBounds = brick.GetBounds();
            var ballPos = ball.Position;
            var ballRadius = ball.Radius;

            // Определяем, с какой стороны кирпича произошло столкновение
            // Вычисляем расстояния до каждой стороны
            float distToLeft = Math.Abs(ballPos.X - brickBounds.left);
            float distToRight = Math.Abs(ballPos.X - brickBounds.right);
            float distToTop = Math.Abs(ballPos.Y - brickBounds.top);
            float distToBottom = Math.Abs(ballPos.Y - brickBounds.bottom);

            // Находим минимальное расстояние - это сторона столкновения
            float minDist = Math.Min(Math.Min(distToLeft, distToRight), 
                                    Math.Min(distToTop, distToBottom));

            Vector2 normal;

            // Определяем нормаль отражения в зависимости от стороны столкновения
            if (minDist == distToLeft)
            {
                // Удар слева
                normal = new Vector2(-1, 0);
                ball.Position = new Vector2(brickBounds.left - ballRadius, ballPos.Y);
            }
            else if (minDist == distToRight)
            {
                // Удар справа
                normal = new Vector2(1, 0);
                ball.Position = new Vector2(brickBounds.right + ballRadius, ballPos.Y);
            }
            else if (minDist == distToTop)
            {
                // Удар сверху
                normal = new Vector2(0, -1);
                ball.Position = new Vector2(ballPos.X, brickBounds.top - ballRadius);
            }
            else // distToBottom
            {
                // Удар снизу
                normal = new Vector2(0, 1);
                ball.Position = new Vector2(ballPos.X, brickBounds.bottom + ballRadius);
            }

            // Отражаем мяч
            ball.Reflect(normal);

            return true;
        }

        // Проверяет столкновение мяча со стенами игрового поля
        public static bool CheckBallWallCollision(Ball ball, GameField field)
        {
            if (!ball.IsActive)
                return false;

            var ballPos = ball.Position;
            var ballRadius = ball.Radius;

            // Проверяем столкновение с левой стеной
            if (ballPos.X - ballRadius <= field.Left)
            {
                return true;
            }

            // Проверяем столкновение с правой стеной
            if (ballPos.X + ballRadius >= field.Right)
            {
                return true;
            }

            // Проверяем столкновение с верхней стеной
            if (ballPos.Y - ballRadius <= field.Top)
            {
                return true;
            }

            // Нижняя стена не проверяется здесь - это потеря жизни

            return false;
        }

        // Обрабатывает столкновение мяча со стенами и отражает мяч
        public static bool HandleBallWallCollision(Ball ball, GameField field)
        {
            if (!ball.IsActive)
                return false;

            var ballPos = ball.Position;
            var ballRadius = ball.Radius;
            bool collision = false;

            // Проверяем и обрабатываем столкновение с левой стеной
            if (ballPos.X - ballRadius <= field.Left)
            {
                ball.ReflectHorizontal();
                ball.Position = new Vector2(field.Left + ballRadius, ballPos.Y);
                collision = true;
            }
            // Проверяем и обрабатываем столкновение с правой стеной
            else if (ballPos.X + ballRadius >= field.Right)
            {
                ball.ReflectHorizontal();
                ball.Position = new Vector2(field.Right - ballRadius, ballPos.Y);
                collision = true;
            }

            // Проверяем и обрабатываем столкновение с верхней стеной
            if (ballPos.Y - ballRadius <= field.Top)
            {
                ball.ReflectVertical();
                ball.Position = new Vector2(ballPos.X, field.Top + ballRadius);
                collision = true;
            }

            return collision;
        }

        // Проверяет, упал ли мяч за нижнюю границу поля (потеря жизни)
        public static bool CheckBallOutOfBounds(Ball ball, GameField field)
        {
            if (!ball.IsActive)
                return false;

            // Мяч считается потерянным, если он упал ниже нижней границы
            return ball.Position.Y + ball.Radius > field.Bottom;
        }

    }
}

