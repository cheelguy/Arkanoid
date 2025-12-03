using Arkanoid.Core;
using Arkanoid.Models;
using Arkanoid.Services;
using System.Diagnostics;

namespace Arkanoid
{
    /// <summary>
    /// Главный класс программы - точка входа
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Настройка консоли
            Console.CursorVisible = false;
            
            // SetWindowSize работает только на Windows
            try
            {
                if (OperatingSystem.IsWindows())
                {
                    Console.SetWindowSize(80, 30);
                }
            }
            catch
            {
                // Игнорируем ошибку на других платформах
            }

            // Создание игрового движка и сервисов
            var gameEngine = new GameEngine();
            var renderer = new ConsoleRenderer();
            var input = new InputHandler();
            var scoreManager = new ScoreManager();
            var soundManager = new SoundManager();

            // Инициализация игры
            gameEngine.Initialize();

            // Показываем меню
            renderer.DrawMenu();
            
            // Ждем нажатия Enter для старта
            bool waitingForStart = true;
            while (waitingForStart)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                    {
                        waitingForStart = false;
                        gameEngine.GameState.StartGame();
                    }
                    else if (key.Key == ConsoleKey.Escape)
                    {
                        return; // Выход из игры
                    }
                }
                Thread.Sleep(50);
            }

            // Запускаем мяч
            gameEngine.GameObjects.Ball.Launch(0);

            // Главный игровой цикл
            var stopwatch = Stopwatch.StartNew();
            float lastTime = 0;
            
            while (gameEngine.GameState.IsGameRunning || 
                   gameEngine.GameState.CurrentState == GameStateType.LevelComplete)
            {
                // Вычисляем deltaTime
                float currentTime = stopwatch.ElapsedMilliseconds / 1000f;
                float deltaTime = currentTime - lastTime;
                lastTime = currentTime;

                // Ограничиваем deltaTime для стабильности
                if (deltaTime > 0.1f) deltaTime = 0.1f;

                // Обрабатываем ввод
                float paddleDirection = input.GetPaddleDirection();
                gameEngine.GameObjects.Paddle.Move(paddleDirection, deltaTime, gameEngine.GameField.Width);

                // Проверяем паузу
                if (input.IsPausePressed())
                {
                    if (gameEngine.GameState.CurrentState == GameStateType.Playing)
                    {
                        gameEngine.GameState.Pause();
                    }
                    else if (gameEngine.GameState.CurrentState == GameStateType.Paused)
                    {
                        gameEngine.GameState.Resume();
                    }
                }

                // Проверяем выход
                if (input.IsQuitPressed())
                {
                    break;
                }

                // Обновляем игру (если не на паузе)
                if (gameEngine.GameState.CurrentState == GameStateType.Playing)
                {
                    // Сохраняем список кирпичей которые были разрушены ПЕРЕД обновлением
                    var bricksBeforeUpdate = gameEngine.GameObjects.Bricks
                        .Where(b => !b.IsDestroyed)
                        .Select(b => new { Brick = b, WasDestroyed = false })
                        .ToList();
                    
                    gameEngine.Update(deltaTime);
                    
                    // Проверяем какие кирпичи стали разрушенными и начисляем очки
                    foreach (var brickInfo in bricksBeforeUpdate)
                    {
                        if (brickInfo.Brick.IsDestroyed)
                        {
                            scoreManager.AddScore(brickInfo.Brick.GetPoints());
                        }
                    }
                    
                    // Если мяч неактивен (после потери жизни), запускаем его снова
                    if (!gameEngine.GameObjects.Ball.IsActive && gameEngine.GameState.Lives > 0)
                    {
                        Thread.Sleep(500); // Пауза перед новым запуском
                        gameEngine.GameObjects.Ball.Launch(0);
                    }
                }

                // Отрисовываем кадр
                try
                {
                    renderer.Render(
                        gameEngine.GameObjects, 
                        gameEngine.GameState.Lives,
                        scoreManager.Score,
                        gameEngine.GameState.CurrentLevel
                    );
                }
                catch
                {
                    // Игнорируем ошибки отрисовки
                }

                // Проверяем завершение уровня
                if (gameEngine.GameState.CurrentState == GameStateType.LevelComplete)
                {
                    Thread.Sleep(1000);
                    if (gameEngine.LevelManager.HasMoreLevels())
                    {
                        gameEngine.GameState.NextLevel(gameEngine.LevelManager.TotalLevels);
                        gameEngine.LevelManager.LoadLevel(gameEngine.GameState.CurrentLevel, gameEngine.GameObjects);
                        gameEngine.GameObjects.Ball.Reset(
                            gameEngine.GameObjects.Paddle.Position.X,
                            gameEngine.GameObjects.Paddle.Position.Y
                        );
                        gameEngine.GameObjects.Ball.Launch(0);
                    }
                    else
                    {
                        gameEngine.GameState.Victory();
                        break;
                    }
                }

                // Удаляем разрушенные кирпичи (очки уже начислены выше)
                gameEngine.GameObjects.Bricks.RemoveAll(b => b.IsDestroyed);

                // Ограничиваем FPS
                Thread.Sleep(16); // ~60 FPS
            }

            // Показываем финальный экран
            Console.Clear();
            bool isVictory = gameEngine.GameState.CurrentState == GameStateType.Victory;
            renderer.DrawGameOver(isVictory, scoreManager.Score);
            
            // Сохраняем рекорд
            if (scoreManager.Score > scoreManager.HighScore)
            {
                scoreManager.SaveHighScore();
            }

            Console.WriteLine("\nНажмите Enter для новой игры или Esc для выхода...");
            
            // Ждем решения игрока
            bool waitingForChoice = true;
            while (waitingForChoice)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                    {
                        // Перезапускаем игру
                        Main(args);
                        return;
                    }
                    else if (key.Key == ConsoleKey.Escape)
                    {
                        waitingForChoice = false;
                    }
                }
                Thread.Sleep(50);
            }
        }
    }
}

