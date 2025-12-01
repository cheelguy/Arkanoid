namespace Arkanoid.Models
{
    /// <summary>
    /// Класс кирпича, который должен разрушить игрок
    /// </summary>
    public class Brick
    {
        /// <summary>
        /// Позиция левого верхнего угла кирпича
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Ширина кирпича
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Высота кирпича
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Текущее здоровье кирпича
        /// </summary>
        public int Health { get; private set; }

        /// <summary>
        /// Максимальное здоровье кирпича
        /// </summary>
        public int MaxHealth { get; private set; }

        /// <summary>
        /// Тип кирпича
        /// </summary>
        public BrickType Type { get; set; }

        /// <summary>
        /// Данные типа кирпича (цвет, очки и т.д.)
        /// </summary>
        public BrickTypeData TypeData { get; private set; }

        /// <summary>
        /// Флаг, указывающий разрушен ли кирпич
        /// </summary>
        public bool IsDestroyed { get; private set; }

        /// <summary>
        /// Конструктор кирпича
        /// </summary>
        /// <param name="x">X координата</param>
        /// <param name="y">Y координата</param>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        /// <param name="type">Тип кирпича</param>
        public Brick(float x, float y, float width, float height, BrickType type)
        {
            Position = new Vector2(x, y);
            Width = width;
            Height = height;
            Type = type;
            TypeData = BrickTypeData.GetData(type);
            MaxHealth = TypeData.MaxHealth;
            Health = MaxHealth;
            IsDestroyed = false;
        }

        /// <summary>
        /// Конструктор кирпича с позицией в виде вектора
        /// </summary>
        /// <param name="position">Позиция</param>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        /// <param name="type">Тип кирпича</param>
        public Brick(Vector2 position, float width, float height, BrickType type)
        {
            Position = position;
            Width = width;
            Height = height;
            Type = type;
            TypeData = BrickTypeData.GetData(type);
            MaxHealth = TypeData.MaxHealth;
            Health = MaxHealth;
            IsDestroyed = false;
        }

        /// <summary>
        /// Наносит урон кирпичу при попадании мяча
        /// </summary>
        /// <returns>True если кирпич разрушен, иначе false</returns>
        public bool Hit()
        {
            // Неразрушимые кирпичи не получают урона
            if (Type == BrickType.Unbreakable)
            {
                return false;
            }

            Health--;

            if (Health <= 0)
            {
                IsDestroyed = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Получает границы кирпича для проверки коллизий
        /// </summary>
        /// <returns>Границы в формате (left, top, right, bottom)</returns>
        public (float left, float top, float right, float bottom) GetBounds()
        {
            return (
                Position.X,              // left
                Position.Y,              // top
                Position.X + Width,      // right
                Position.Y + Height      // bottom
            );
        }

        /// <summary>
        /// Получает центр кирпича
        /// </summary>
        /// <returns>Позиция центра кирпича</returns>
        public Vector2 GetCenter()
        {
            return new Vector2(
                Position.X + Width / 2,
                Position.Y + Height / 2
            );
        }

        /// <summary>
        /// Проверяет, должен ли выпасть бонус при разрушении этого кирпича
        /// </summary>
        /// <returns>True если бонус должен выпасть</returns>
        public bool ShouldDropPowerUp()
        {
            if (Type == BrickType.Unbreakable || !IsDestroyed)
            {
                return false;
            }

            // Используем Random для определения выпадения бонуса
            Random random = new Random();
            return random.NextDouble() < TypeData.PowerUpDropChance;
        }

        /// <summary>
        /// Получает текущий цвет кирпича в зависимости от его здоровья
        /// </summary>
        /// <returns>Цвет для отрисовки</returns>
        public ConsoleColor GetCurrentColor()
        {
            return BrickTypeData.GetHealthColor(Health, MaxHealth, TypeData.Color);
        }

        /// <summary>
        /// Получает символ для отрисовки кирпича
        /// </summary>
        /// <returns>Символ кирпича</returns>
        public char GetSymbol()
        {
            return TypeData.Symbol;
        }

        /// <summary>
        /// Получает количество очков за разрушение кирпича
        /// </summary>
        /// <returns>Количество очков</returns>
        public int GetPoints()
        {
            return TypeData.Points;
        }

        /// <summary>
        /// Проверяет, пересекается ли точка с кирпичом
        /// </summary>
        /// <param name="point">Точка для проверки</param>
        /// <returns>True если точка внутри кирпича</returns>
        public bool ContainsPoint(Vector2 point)
        {
            var bounds = GetBounds();
            return point.X >= bounds.left && point.X <= bounds.right &&
                   point.Y >= bounds.top && point.Y <= bounds.bottom;
        }

        /// <summary>
        /// Получает информацию о кирпиче в виде строки
        /// </summary>
        public override string ToString()
        {
            return $"Brick({Type}, HP: {Health}/{MaxHealth}, Pos: {Position})";
        }
    }
}

