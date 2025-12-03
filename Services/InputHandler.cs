namespace Arkanoid.Services
{
    /// <summary>
    /// Класс обработки ввода с клавиатуры
    /// </summary>
    public class InputHandler
    {
        /// <summary>
        /// Получает направление движения платформы
        /// Неблокирующее чтение - проверяет все доступные клавиши в буфере
        /// </summary>
        /// <returns>-1 для движения влево, +1 для движения вправо, 0 если клавиши не нажаты</returns>
        public float GetPaddleDirection()
        {
            if (!Console.KeyAvailable)
                return 0;

            float direction = 0;

            // Читаем все доступные клавиши в буфере (неблокирующее чтение)
            while (Console.KeyAvailable)
            {
                var keyInfo = Console.ReadKey(true);
                var key = keyInfo.Key;

                if (key == ConsoleKey.LeftArrow)
                {
                    direction = -1f;
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    direction = 1f;
                }
            }

            return direction;
        }

        /// <summary>
        /// Проверяет, нажата ли клавиша паузы
        /// </summary>
        /// <returns>True если нажата Space или P</returns>
        public bool IsPausePressed()
        {
            if (!Console.KeyAvailable)
                return false;

            var keyInfo = Console.ReadKey(true);
            var key = keyInfo.Key;

            return key == ConsoleKey.Spacebar || key == ConsoleKey.P;
        }

        /// <summary>
        /// Проверяет, нажата ли клавиша выхода
        /// </summary>
        /// <returns>True если нажата Esc</returns>
        public bool IsQuitPressed()
        {
            if (!Console.KeyAvailable)
                return false;

            var keyInfo = Console.ReadKey(true);
            var key = keyInfo.Key;

            return key == ConsoleKey.Escape;
        }

        /// <summary>
        /// Проверяет, нажата ли клавиша старта
        /// </summary>
        /// <returns>True если нажата Enter</returns>
        public bool IsStartPressed()
        {
            if (!Console.KeyAvailable)
                return false;

            var keyInfo = Console.ReadKey(true);
            var key = keyInfo.Key;

            return key == ConsoleKey.Enter;
        }

        /// <summary>
        /// Очищает буфер ввода
        /// </summary>
        public void ClearInputBuffer()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }
    }
}


