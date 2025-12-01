namespace Arkanoid.Models
{
    /// <summary>
    /// Класс платформы, которой управляет игрок
    /// </summary>
    public class Paddle
    {
        /// <summary>
        /// Позиция центра платформы
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Ширина платформы
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Высота платформы
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Скорость движения платформы
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Базовая ширина платформы (для сброса после power-up)
        /// </summary>
        private float _baseWidth;

        /// <summary>
        /// Конструктор платформы
        /// </summary>
        /// <param name="x">Начальная X координата</param>
        /// <param name="y">Начальная Y координата</param>
        /// <param name="width">Ширина платформы</param>
        /// <param name="height">Высота платформы</param>
        /// <param name="speed">Скорость движения</param>
        public Paddle(float x, float y, float width = 10f, float height = 1f, float speed = 30f)
        {
            Position = new Vector2(x, y);
            Width = width;
            Height = height;
            Speed = speed;
            _baseWidth = width;
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Paddle()
        {
            Position = new Vector2(40, 27);
            Width = 10f;
            Height = 1f;
            Speed = 30f;
            _baseWidth = Width;
        }

        /// <summary>
        /// Перемещает платформу в заданном направлении
        /// </summary>
        /// <param name="direction">Направление (-1 влево, +1 вправо, 0 нет движения)</param>
        /// <param name="deltaTime">Время с последнего кадра</param>
        /// <param name="fieldWidth">Ширина игрового поля для ограничения движения</param>
        public void Move(float direction, float deltaTime, float fieldWidth)
        {
            if (direction == 0)
                return;

            // Вычисляем новую позицию
            float newX = Position.X + direction * Speed * deltaTime;

            // Ограничиваем движение границами поля
            float halfWidth = Width / 2;
            if (newX - halfWidth < 0)
            {
                newX = halfWidth;
            }
            else if (newX + halfWidth > fieldWidth)
            {
                newX = fieldWidth - halfWidth;
            }

            Position = new Vector2(newX, Position.Y);
        }

        /// <summary>
        /// Получает границы платформы для проверки коллизий
        /// </summary>
        /// <returns>Границы в формате (left, top, right, bottom)</returns>
        public (float left, float top, float right, float bottom) GetBounds()
        {
            float halfWidth = Width / 2;
            float halfHeight = Height / 2;

            return (
                Position.X - halfWidth,  // left
                Position.Y - halfHeight,  // top
                Position.X + halfWidth,   // right
                Position.Y + halfHeight   // bottom
            );
        }

        /// <summary>
        /// Получает левую границу платформы
        /// </summary>
        public float GetLeft()
        {
            return Position.X - Width / 2;
        }

        /// <summary>
        /// Получает правую границу платформы
        /// </summary>
        public float GetRight()
        {
            return Position.X + Width / 2;
        }

        /// <summary>
        /// Получает верхнюю границу платформы
        /// </summary>
        public float GetTop()
        {
            return Position.Y - Height / 2;
        }

        /// <summary>
        /// Получает нижнюю границу платформы
        /// </summary>
        public float GetBottom()
        {
            return Position.Y + Height / 2;
        }

        /// <summary>
        /// Увеличивает размер платформы
        /// </summary>
        /// <param name="multiplier">Множитель увеличения</param>
        public void Expand(float multiplier = 1.5f)
        {
            Width = _baseWidth * multiplier;
        }

        /// <summary>
        /// Уменьшает размер платформы
        /// </summary>
        /// <param name="multiplier">Множитель уменьшения</param>
        public void Shrink(float multiplier = 0.7f)
        {
            Width = _baseWidth * multiplier;
        }

        /// <summary>
        /// Сбрасывает размер платформы к базовому
        /// </summary>
        public void ResetSize()
        {
            Width = _baseWidth;
        }

        /// <summary>
        /// Проверяет, находится ли точка над платформой
        /// Используется для определения угла отскока мяча
        /// </summary>
        /// <param name="x">X координата точки</param>
        /// <returns>Относительная позиция (-1 до 1, где 0 - центр)</returns>
        public float GetRelativeHitPosition(float x)
        {
            float halfWidth = Width / 2;
            float relativeX = (x - Position.X) / halfWidth;
            return Math.Clamp(relativeX, -1f, 1f);
        }
    }
}

