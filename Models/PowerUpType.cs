namespace Arkanoid.Models
{
    /// <summary>
    /// Перечисление типов бонусов
    /// </summary>
    public enum PowerUpType
    {
        /// <summary>
        /// Увеличение размера платформы
        /// </summary>
        ExpandPaddle,

        /// <summary>
        /// Уменьшение размера платформы
        /// </summary>
        ShrinkPaddle,

        /// <summary>
        /// Увеличение скорости мяча
        /// </summary>
        SpeedUp,

        /// <summary>
        /// Уменьшение скорости мяча
        /// </summary>
        SlowDown,

        /// <summary>
        /// Дополнительная жизнь
        /// </summary>
        ExtraLife,

        /// <summary>
        /// Множественный мяч (опционально)
        /// </summary>
        MultiBall
    }

    /// <summary>
    /// Класс с характеристиками типов бонусов
    /// </summary>
    public class PowerUpTypeData
    {
        /// <summary>
        /// Тип бонуса
        /// </summary>
        public PowerUpType Type { get; set; }

        /// <summary>
        /// Длительность эффекта в секундах
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// Цвет для отрисовки
        /// </summary>
        public ConsoleColor Color { get; set; }

        /// <summary>
        /// Символ для отрисовки
        /// </summary>
        public char Symbol { get; set; }

        /// <summary>
        /// Описание эффекта
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public PowerUpTypeData(PowerUpType type, float duration, ConsoleColor color, 
                              char symbol, string description)
        {
            Type = type;
            Duration = duration;
            Color = color;
            Symbol = symbol;
            Description = description;
        }

        /// <summary>
        /// Получает данные для конкретного типа бонуса
        /// </summary>
        /// <param name="type">Тип бонуса</param>
        /// <returns>Данные типа бонуса</returns>
        public static PowerUpTypeData GetData(PowerUpType type)
        {
            return type switch
            {
                PowerUpType.ExpandPaddle => new PowerUpTypeData(
                    PowerUpType.ExpandPaddle,
                    duration: 10.0f,
                    color: ConsoleColor.Green,
                    symbol: 'E',
                    description: "Увеличивает размер платформы"
                ),
                PowerUpType.ShrinkPaddle => new PowerUpTypeData(
                    PowerUpType.ShrinkPaddle,
                    duration: 10.0f,
                    color: ConsoleColor.Red,
                    symbol: 'S',
                    description: "Уменьшает размер платформы"
                ),
                PowerUpType.SpeedUp => new PowerUpTypeData(
                    PowerUpType.SpeedUp,
                    duration: 8.0f,
                    color: ConsoleColor.Yellow,
                    symbol: 'F',
                    description: "Увеличивает скорость мяча"
                ),
                PowerUpType.SlowDown => new PowerUpTypeData(
                    PowerUpType.SlowDown,
                    duration: 8.0f,
                    color: ConsoleColor.Cyan,
                    symbol: 'L',
                    description: "Уменьшает скорость мяча"
                ),
                PowerUpType.ExtraLife => new PowerUpTypeData(
                    PowerUpType.ExtraLife,
                    duration: 0.0f,
                    color: ConsoleColor.Magenta,
                    symbol: '+',
                    description: "Добавляет дополнительную жизнь"
                ),
                PowerUpType.MultiBall => new PowerUpTypeData(
                    PowerUpType.MultiBall,
                    duration: 0.0f,
                    color: ConsoleColor.Blue,
                    symbol: 'M',
                    description: "Создает дополнительный мяч"
                ),
                _ => new PowerUpTypeData(
                    PowerUpType.ExpandPaddle,
                    duration: 10.0f,
                    color: ConsoleColor.Green,
                    symbol: 'E',
                    description: "Увеличивает размер платформы"
                )
            };
        }
    }
}

