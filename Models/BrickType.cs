namespace Arkanoid.Models
{
    /// <summary>
    /// Перечисление типов кирпичей
    /// </summary>
    public enum BrickType
    {
        /// <summary>
        /// Обычный кирпич - 1 удар для разрушения
        /// </summary>
        Normal,

        /// <summary>
        /// Прочный кирпич - 2 удара для разрушения
        /// </summary>
        Strong,

        /// <summary>
        /// Очень прочный кирпич - 3 удара для разрушения
        /// </summary>
        VeryStrong,

        /// <summary>
        /// Неразрушимый кирпич - невозможно разрушить
        /// </summary>
        Unbreakable
    }

    /// <summary>
    /// Класс с характеристиками типов кирпичей
    /// </summary>
    public class BrickTypeData
    {
        /// <summary>
        /// Тип кирпича
        /// </summary>
        public BrickType Type { get; set; }

        /// <summary>
        /// Максимальное здоровье кирпича
        /// </summary>
        public int MaxHealth { get; set; }

        /// <summary>
        /// Количество очков за разрушение
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Цвет кирпича для отрисовки
        /// </summary>
        public ConsoleColor Color { get; set; }

        /// <summary>
        /// Шанс выпадения бонуса (0.0 - 1.0)
        /// </summary>
        public float PowerUpDropChance { get; set; }

        /// <summary>
        /// Символ для отрисовки кирпича
        /// </summary>
        public char Symbol { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public BrickTypeData(BrickType type, int maxHealth, int points, ConsoleColor color, 
                            float powerUpDropChance, char symbol = '#')
        {
            Type = type;
            MaxHealth = maxHealth;
            Points = points;
            Color = color;
            PowerUpDropChance = powerUpDropChance;
            Symbol = symbol;
        }

        /// <summary>
        /// Получает данные для конкретного типа кирпича
        /// </summary>
        /// <param name="type">Тип кирпича</param>
        /// <returns>Данные типа кирпича</returns>
        public static BrickTypeData GetData(BrickType type)
        {
            return type switch
            {
                BrickType.Normal => new BrickTypeData(
                    BrickType.Normal,
                    maxHealth: 1,
                    points: 10,
                    color: ConsoleColor.Green,
                    powerUpDropChance: 0.2f,
                    symbol: '#'
                ),
                BrickType.Strong => new BrickTypeData(
                    BrickType.Strong,
                    maxHealth: 2,
                    points: 20,
                    color: ConsoleColor.Yellow,
                    powerUpDropChance: 0.4f,
                    symbol: '='
                ),
                BrickType.VeryStrong => new BrickTypeData(
                    BrickType.VeryStrong,
                    maxHealth: 3,
                    points: 30,
                    color: ConsoleColor.Red,
                    powerUpDropChance: 0.5f,
                    symbol: '@'
                ),
                BrickType.Unbreakable => new BrickTypeData(
                    BrickType.Unbreakable,
                    maxHealth: int.MaxValue,
                    points: 0,
                    color: ConsoleColor.DarkGray,
                    powerUpDropChance: 0.0f,
                    symbol: 'X'
                ),
                _ => new BrickTypeData(
                    BrickType.Normal,
                    maxHealth: 1,
                    points: 10,
                    color: ConsoleColor.Green,
                    powerUpDropChance: 0.2f,
                    symbol: '#'
                )
            };
        }

        /// <summary>
        /// Получает цвет для текущего состояния здоровья кирпича
        /// </summary>
        /// <param name="currentHealth">Текущее здоровье</param>
        /// <param name="maxHealth">Максимальное здоровье</param>
        /// <param name="baseColor">Базовый цвет</param>
        /// <returns>Цвет для отрисовки</returns>
        public static ConsoleColor GetHealthColor(int currentHealth, int maxHealth, ConsoleColor baseColor)
        {
            // Для поврежденных кирпичей показываем более темные оттенки
            if (currentHealth < maxHealth)
            {
                return baseColor switch
                {
                    ConsoleColor.Green => ConsoleColor.DarkGreen,
                    ConsoleColor.Yellow => ConsoleColor.DarkYellow,
                    ConsoleColor.Red => ConsoleColor.DarkRed,
                    _ => baseColor
                };
            }
            return baseColor;
        }
    }
}

