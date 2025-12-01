namespace Arkanoid.Models
{
    /// <summary>
    /// Класс бонуса (power-up)
    /// TODO: Костя должен полностью реализовать этот класс
    /// Это временная заглушка для компиляции проекта
    /// </summary>
    public class PowerUp
    {
        /// <summary>
        /// Позиция бонуса
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Флаг активности бонуса
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public PowerUp()
        {
            Position = new Vector2();
            IsActive = false;
        }
    }
}

