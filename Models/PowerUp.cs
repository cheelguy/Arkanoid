namespace Arkanoid.Models
{
    /// <summary>
    /// Класс бонуса (power-up) - падающий бонус, который может быть подобран платформой
    /// </summary>
    public class PowerUp
    {
        /// <summary>
        /// Позиция бонуса
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Тип бонуса
        /// </summary>
        public PowerUpType Type { get; set; }

        /// <summary>
        /// Скорость падения бонуса
        /// </summary>
        public float FallSpeed { get; set; }

        /// <summary>
        /// Флаг активности бонуса
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Ширина бонуса
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Высота бонуса
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Данные типа бонуса
        /// </summary>
        private PowerUpTypeData? _typeData;

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="position">Позиция бонуса</param>
        /// <param name="type">Тип бонуса</param>
        /// <param name="fallSpeed">Скорость падения</param>
        /// <param name="width">Ширина бонуса</param>
        /// <param name="height">Высота бонуса</param>
        public PowerUp(Vector2 position, PowerUpType type, float fallSpeed = 20f, float width = 1f, float height = 1f)
        {
            Position = position;
            Type = type;
            FallSpeed = fallSpeed;
            Width = width;
            Height = height;
            IsActive = true;
            _typeData = PowerUpTypeData.GetData(type);
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public PowerUp()
        {
            Position = new Vector2();
            Type = PowerUpType.ExpandPaddle;
            FallSpeed = 20f;
            Width = 1f;
            Height = 1f;
            IsActive = false;
            _typeData = null;
        }

        /// <summary>
        /// Обновляет позицию бонуса - падение вниз
        /// </summary>
        /// <param name="deltaTime">Время с последнего кадра</param>
        public void Update(float deltaTime)
        {
            if (!IsActive)
                return;

            // Падение вниз
            Position = new Vector2(Position.X, Position.Y + FallSpeed * deltaTime);
        }

        /// <summary>
        /// Применяет эффект бонуса к игровым объектам
        /// </summary>
        /// <param name="gameObjects">Игровые объекты</param>
        /// <returns>True если нужно добавить жизнь (для ExtraLife)</returns>
        public bool Apply(GameObjects gameObjects)
        {
            if (gameObjects == null || !IsActive)
                return false;

            switch (Type)
            {
                case PowerUpType.ExpandPaddle:
                    if (gameObjects.Paddle != null)
                    {
                        gameObjects.Paddle.Expand(1.5f);
                    }
                    break;

                case PowerUpType.ShrinkPaddle:
                    if (gameObjects.Paddle != null)
                    {
                        gameObjects.Paddle.Shrink(0.7f);
                    }
                    break;

                case PowerUpType.SpeedUp:
                    if (gameObjects.Ball != null)
                    {
                        gameObjects.Ball.IncreaseSpeed(20f);
                    }
                    break;

                case PowerUpType.SlowDown:
                    if (gameObjects.Ball != null)
                    {
                        gameObjects.Ball.DecreaseSpeed(20f);
                    }
                    break;

                case PowerUpType.ExtraLife:
                    // Возвращаем true, чтобы GameEngine мог добавить жизнь
                    return true;

                case PowerUpType.MultiBall:
                    // Опционально: создание дополнительного мяча
                    // Пока просто возвращаем false, так как это требует дополнительной логики
                    break;
            }

            return false;
        }

        /// <summary>
        /// Получает границы бонуса для проверки коллизий
        /// </summary>
        /// <returns>Границы в формате (left, top, right, bottom)</returns>
        public (float left, float top, float right, float bottom) GetBounds()
        {
            float halfWidth = Width / 2;
            float halfHeight = Height / 2;

            return (
                Position.X - halfWidth,  // left
                Position.Y - halfHeight, // top
                Position.X + halfWidth,  // right
                Position.Y + halfHeight  // bottom
            );
        }

        /// <summary>
        /// Получает данные типа бонуса
        /// </summary>
        /// <returns>Данные типа бонуса</returns>
        public PowerUpTypeData GetTypeData()
        {
            if (_typeData == null)
            {
                _typeData = PowerUpTypeData.GetData(Type);
            }
            return _typeData;
        }
    }
}

