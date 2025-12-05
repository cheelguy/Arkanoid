namespace Arkanoid.Models
{
    /// <summary>
    /// Класс, представляющий мяч в игре
    /// Отвечает за движение и отражение мяча
    /// </summary>
    public class Ball
    {
        /// <summary>
        /// Текущая позиция мяча
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Вектор скорости мяча (направление и скорость)
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Радиус мяча для отрисовки и коллизий
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// Базовая скорость мяча
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Флаг активности мяча
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Конструктор мяча
        /// </summary>
        /// <param name="position">Начальная позиция</param>
        /// <param name="velocity">Начальная скорость</param>
        /// <param name="radius">Радиус мяча</param>
        public Ball(Vector2 position, Vector2 velocity, float radius = 0.5f)
        {
            Position = position;
            Velocity = velocity;
            Radius = radius;
            Speed = velocity.Length();
            IsActive = true;
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Ball()
        {
            Position = new Vector2(40, 25);
            Velocity = new Vector2(0, -1);
            Radius = 0.5f;
            Speed = 8f; // Уменьшена скорость для лучшей играбельности
            IsActive = false;
        }

        /// <summary>
        /// Обновляет позицию мяча на основе скорости и времени
        /// </summary>
        /// <param name="deltaTime">Время с последнего кадра</param>
        public void Update(float deltaTime)
        {
            if (!IsActive)
                return;

            // Обновляем позицию на основе скорости
            Position = Position + Velocity * deltaTime;
        }

        /// <summary>
        /// Отражает мяч от поверхности с заданной нормалью
        /// </summary>
        /// <param name="normal">Нормаль поверхности</param>
        public void Reflect(Vector2 normal)
        {
            // Формула отражения: V' = V - 2 * (V · N) * N
            // где V - скорость, N - нормаль
            float dotProduct = Velocity.Dot(normal);
            Velocity = Velocity - normal * (2 * dotProduct);

            // Нормализуем скорость и применяем текущую скорость
            Velocity = Velocity.Normalize() * Speed;
        }

        /// <summary>
        /// Отражает мяч по горизонтали (от боковых стен)
        /// </summary>
        public void ReflectHorizontal()
        {
            Velocity = new Vector2(-Velocity.X, Velocity.Y);
        }

        /// <summary>
        /// Отражает мяч по вертикали (от верхней стены)
        /// </summary>
        public void ReflectVertical()
        {
            Velocity = new Vector2(Velocity.X, -Velocity.Y);
        }

        /// <summary>
        /// Устанавливает новую скорость мяча, сохраняя направление
        /// </summary>
        /// <param name="newSpeed">Новая скорость</param>
        public void SetSpeed(float newSpeed)
        {
            Speed = newSpeed;
            Vector2 direction = Velocity.Normalize();
            Velocity = direction * Speed;
        }

        /// <summary>
        /// Увеличивает скорость мяча на заданный процент
        /// </summary>
        /// <param name="percentage">Процент увеличения (например, 10 для +10%)</param>
        public void IncreaseSpeed(float percentage)
        {
            Speed *= (1 + percentage / 100f);
            Vector2 direction = Velocity.Normalize();
            Velocity = direction * Speed;
        }

        /// <summary>
        /// Уменьшает скорость мяча на заданный процент
        /// </summary>
        /// <param name="percentage">Процент уменьшения (например, 10 для -10%)</param>
        public void DecreaseSpeed(float percentage)
        {
            Speed *= (1 - percentage / 100f);
            Vector2 direction = Velocity.Normalize();
            Velocity = direction * Speed;
        }

        /// <summary>
        /// Сбрасывает мяч в начальное положение над платформой
        /// </summary>
        /// <param name="paddleX">X координата платформы</param>
        /// <param name="paddleY">Y координата платформы</param>
        public void Reset(float paddleX, float paddleY)
        {
            Position = new Vector2(paddleX, paddleY - 2);
            Velocity = new Vector2(0, -Speed);
            IsActive = false;
        }

        /// <summary>
        /// Запускает мяч в движение
        /// </summary>
        /// <param name="angle">Угол запуска в радианах (0 - вверх)</param>
        public void Launch(float angle = 0)
        {
            float dirX = (float)Math.Sin(angle);
            float dirY = -(float)Math.Cos(angle);
            Velocity = new Vector2(dirX, dirY).Normalize() * Speed;
            IsActive = true;
        }
    }
}

