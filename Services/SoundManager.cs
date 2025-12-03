namespace Arkanoid.Services
{
    /// <summary>
    /// Упрощенный класс для звуков
    /// </summary>
    public class SoundManager
    {
        /// <summary>
        /// Включены ли звуки
        /// </summary>
        public bool IsSoundEnabled { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public SoundManager()
        {
            IsSoundEnabled = true;
        }

        /// <summary>
        /// Воспроизводит звук отскока мяча
        /// </summary>
        public void PlayBounce()
        {
            if (!IsSoundEnabled)
                return;

            try
            {
                Console.Beep(800, 100);
            }
            catch (Exception)
            {
                // Игнорируем ошибки воспроизведения звука
            }
        }

        /// <summary>
        /// Воспроизводит звук разрушения кирпича
        /// </summary>
        public void PlayBrickDestroy()
        {
            if (!IsSoundEnabled)
                return;

            try
            {
                Console.Beep(600, 150);
            }
            catch (Exception)
            {
                // Игнорируем ошибки воспроизведения звука
            }
        }

        /// <summary>
        /// Воспроизводит звук подбора бонуса
        /// </summary>
        public void PlayPowerUp()
        {
            if (!IsSoundEnabled)
                return;

            try
            {
                Console.Beep(1000, 200);
            }
            catch (Exception)
            {
                // Игнорируем ошибки воспроизведения звука
            }
        }

        /// <summary>
        /// Воспроизводит звук окончания игры
        /// </summary>
        public void PlayGameOver()
        {
            if (!IsSoundEnabled)
                return;

            try
            {
                Console.Beep(300, 500);
            }
            catch (Exception)
            {
                // Игнорируем ошибки воспроизведения звука
            }
        }

        /// <summary>
        /// Воспроизводит звук завершения уровня
        /// </summary>
        public void PlayLevelComplete()
        {
            if (!IsSoundEnabled)
                return;

            try
            {
                // Последовательность из 3 звуков
                Console.Beep(523, 150); // C
                Thread.Sleep(50);
                Console.Beep(659, 150); // E
                Thread.Sleep(50);
                Console.Beep(784, 200); // G
            }
            catch (Exception)
            {
                // Игнорируем ошибки воспроизведения звука
            }
        }
    }
}


