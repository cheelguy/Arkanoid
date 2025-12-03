namespace Arkanoid.Services
{
    using Arkanoid.Models;

    /// <summary>
    /// Класс отрисовки в консоли
    /// </summary>
    public class ConsoleRenderer
    {
        /// <summary>
        /// Буфер для отрисовки без мерцания
        /// </summary>
        private char[,]? _buffer;

        /// <summary>
        /// Буфер цветов для отрисовки
        /// </summary>
        private ConsoleColor[,]? _colorBuffer;

        /// <summary>
        /// Ширина буфера
        /// </summary>
        private int _bufferWidth;

        /// <summary>
        /// Высота буфера
        /// </summary>
        private int _bufferHeight;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ConsoleRenderer()
        {
            // Используем безопасные размеры по умолчанию
            // На macOS размер окна может быть очень большим
            _bufferWidth = Math.Min(Console.WindowWidth, 120);
            _bufferHeight = Math.Min(Console.WindowHeight, 40);
            
            // Минимальные размеры
            if (_bufferWidth < 80) _bufferWidth = 80;
            if (_bufferHeight < 30) _bufferHeight = 30;
            
            InitializeBuffer();
        }

        /// <summary>
        /// Инициализирует буфер отрисовки
        /// </summary>
        private void InitializeBuffer()
        {
            _buffer = new char[_bufferWidth, _bufferHeight];
            _colorBuffer = new ConsoleColor[_bufferWidth, _bufferHeight];
            ClearBuffer();
        }

        /// <summary>
        /// Очищает буфер
        /// </summary>
        private void ClearBuffer()
        {
            if (_buffer == null || _colorBuffer == null)
                return;

            for (int y = 0; y < _bufferHeight; y++)
            {
                for (int x = 0; x < _bufferWidth; x++)
                {
                    _buffer[x, y] = ' ';
                    _colorBuffer[x, y] = ConsoleColor.White;
                }
            }
        }

        /// <summary>
        /// Отрисовывает все игровые объекты и UI
        /// </summary>
        /// <param name="objects">Игровые объекты</param>
        /// <param name="lives">Количество жизней</param>
        /// <param name="score">Счет</param>
        /// <param name="level">Уровень</param>
        public void Render(GameObjects objects, int lives, int score, int level)
        {
            // Обновляем размер буфера если консоль изменилась
            if (_bufferWidth != Console.WindowWidth || _bufferHeight != Console.WindowHeight)
            {
                _bufferWidth = Console.WindowWidth;
                _bufferHeight = Console.WindowHeight;
                InitializeBuffer();
            }

            ClearBuffer();

            // Отрисовываем объекты
            if (objects != null)
            {
                // Отрисовываем кирпичи
                foreach (var brick in objects.Bricks)
                {
                    if (brick != null && !brick.IsDestroyed)
                    {
                        DrawBrick(brick);
                    }
                }

                // Отрисовываем бонусы
                foreach (var powerUp in objects.PowerUps)
                {
                    if (powerUp != null && powerUp.IsActive)
                    {
                        DrawPowerUp(powerUp);
                    }
                }

                // Отрисовываем платформу
                if (objects.Paddle != null)
                {
                    DrawPaddle(objects.Paddle);
                }

                // Отрисовываем мяч
                if (objects.Ball != null && objects.Ball.IsActive)
                {
                    DrawBall(objects.Ball);
                }
            }

            // Отрисовываем UI
            DrawUI(lives, score, level);

            // Выводим буфер на экран
            FlushBuffer();
        }

        /// <summary>
        /// Выводит буфер на экран с поддержкой цветов
        /// </summary>
        private void FlushBuffer()
        {
            if (_buffer == null || _colorBuffer == null)
                return;

            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < _bufferHeight; y++)
            {
                for (int x = 0; x < _bufferWidth; x++)
                {
                    char c = _buffer[x, y];
                    ConsoleColor color = _colorBuffer[x, y];
                    Console.ForegroundColor = color;
                    Console.Write(c);
                }
            }

            Console.ResetColor();
        }

        /// <summary>
        /// Отрисовывает мяч
        /// </summary>
        /// <param name="ball">Мяч для отрисовки</param>
        public void DrawBall(Ball ball)
        {
            if (ball == null || _buffer == null || _colorBuffer == null)
                return;

            int x = (int)Math.Round(ball.Position.X);
            int y = (int)Math.Round(ball.Position.Y);

            if (x >= 0 && x < _bufferWidth && y >= 0 && y < _bufferHeight)
            {
                _buffer[x, y] = 'O';
                _colorBuffer[x, y] = ConsoleColor.White;
            }
        }

        /// <summary>
        /// Отрисовывает платформу
        /// </summary>
        /// <param name="paddle">Платформа для отрисовки</param>
        public void DrawPaddle(Paddle paddle)
        {
            if (paddle == null || _buffer == null || _colorBuffer == null)
                return;

            var bounds = paddle.GetBounds();
            int left = (int)Math.Round(bounds.left);
            int right = (int)Math.Round(bounds.right);
            int y = (int)Math.Round(paddle.Position.Y);

            for (int x = left; x < right && x < _bufferWidth; x++)
            {
                if (x >= 0 && y >= 0 && y < _bufferHeight)
                {
                    _buffer[x, y] = '=';
                    _colorBuffer[x, y] = ConsoleColor.Cyan;
                }
            }
        }

        /// <summary>
        /// Отрисовывает кирпич
        /// </summary>
        /// <param name="brick">Кирпич для отрисовки</param>
        public void DrawBrick(Brick brick)
        {
            if (brick == null || brick.IsDestroyed || _buffer == null || _colorBuffer == null)
                return;

            var bounds = brick.GetBounds();
            int left = (int)Math.Round(bounds.left);
            int top = (int)Math.Round(bounds.top);
            int right = (int)Math.Round(bounds.right);
            int bottom = (int)Math.Round(bounds.bottom);

            char symbol = brick.GetSymbol();
            ConsoleColor color = brick.GetCurrentColor();

            for (int y = top; y < bottom && y < _bufferHeight; y++)
            {
                for (int x = left; x < right && x < _bufferWidth; x++)
                {
                    if (x >= 0 && y >= 0)
                    {
                        _buffer[x, y] = symbol;
                        _colorBuffer[x, y] = color;
                    }
                }
            }
        }

        /// <summary>
        /// Отрисовывает бонус
        /// </summary>
        /// <param name="powerUp">Бонус для отрисовки</param>
        public void DrawPowerUp(PowerUp powerUp)
        {
            if (powerUp == null || !powerUp.IsActive || _buffer == null || _colorBuffer == null)
                return;

            var typeData = powerUp.GetTypeData();
            int x = (int)Math.Round(powerUp.Position.X);
            int y = (int)Math.Round(powerUp.Position.Y);

            if (x >= 0 && x < _bufferWidth && y >= 0 && y < _bufferHeight)
            {
                _buffer[x, y] = typeData.Symbol;
                _colorBuffer[x, y] = typeData.Color;
            }
        }

        /// <summary>
        /// Отрисовывает UI (жизни, счет, уровень)
        /// </summary>
        /// <param name="lives">Количество жизней</param>
        /// <param name="score">Счет</param>
        /// <param name="level">Уровень</param>
        public void DrawUI(int lives, int score, int level)
        {
            if (_buffer == null || _colorBuffer == null)
                return;

            string uiText = $"Lives: {lives} | Score: {score} | Level: {level}";

            for (int i = 0; i < uiText.Length && i < _bufferWidth; i++)
            {
                _buffer[i, 0] = uiText[i];
                _colorBuffer[i, 0] = ConsoleColor.Yellow;
            }
        }

        /// <summary>
        /// Отрисовывает меню
        /// </summary>
        public void DrawMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine("    ========================================");
            Console.WriteLine("              ARKANOID");
            Console.WriteLine("    ========================================");
            Console.WriteLine();
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("    Управление:");
            Console.ResetColor();
            Console.WriteLine("    <- ->  - Движение платформы");
            Console.WriteLine("    Space/P - Пауза");
            Console.WriteLine("    Esc    - Выход");
            Console.WriteLine("    Enter  - Старт");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("    Нажмите Enter для старта");
            Console.ResetColor();
        }

        /// <summary>
        /// Отрисовывает экран окончания игры
        /// </summary>
        /// <param name="isVictory">True если победа, false если проигрыш</param>
        /// <param name="finalScore">Финальный счет</param>
        public void DrawGameOver(bool isVictory, int finalScore)
        {
            Console.Clear();
            Console.WriteLine();

            if (isVictory)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("    ========================================");
                Console.WriteLine("              VICTORY!");
                Console.WriteLine("    ========================================");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("    ========================================");
                Console.WriteLine("            GAME OVER");
                Console.WriteLine("    ========================================");
            }

            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine($"    Финальный счет: {finalScore}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("    Нажмите Enter для выхода");
            Console.ResetColor();
        }

        /// <summary>
        /// Очищает экран
        /// </summary>
        public void Clear()
        {
            Console.Clear();
            if (_buffer != null)
            {
                ClearBuffer();
            }
        }
    }
}

