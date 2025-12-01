namespace Arkanoid.Core
{
    using Arkanoid.Models;
    using System.Diagnostics;

    // Главный класс игрового движка
    public class GameEngine
    {
        public GameObjects GameObjects { get; private set; }
        public GameField GameField { get; private set; }
        public GameState GameState { get; private set; }
        public LevelManager LevelManager { get; private set; }
        public int TargetFPS { get; set; } = 60;
        private bool _isRunning;

        public GameEngine()
        {
            GameField = new GameField();
            GameState = new GameState();
            LevelManager = new LevelManager();
            GameObjects = new GameObjects();
            _isRunning = false;
        }

        public GameEngine(float fieldWidth, float fieldHeight)
        {
            GameField = new GameField(fieldWidth, fieldHeight);
            GameState = new GameState();
            LevelManager = new LevelManager();
            GameObjects = new GameObjects();
            _isRunning = false;
        }

        // Инициализирует игру
        public void Initialize()
        {
            // Создаем мяч и платформу в начальных позициях
            var centerX = GameField.Width / 2;
            var paddleY = GameField.Height - 3;

            GameObjects.Paddle = new Paddle(centerX, paddleY);
            GameObjects.Ball = new Ball();
            GameObjects.Ball.Reset(centerX, paddleY);

            // Сбрасываем состояние игры
            GameState.Reset();

            // Загружаем первый уровень
            LevelManager.Reset();
            LevelManager.LoadLevel(1, GameObjects);

            _isRunning = false;
        }

        // Обновляет состояние игры за один кадр
        public void Update(float deltaTime)
        {
            if (!GameState.IsGameRunning)
            {
                return;
            }

            // Обновляем позицию мяча
            if (GameObjects.Ball.IsActive)
            {
                GameObjects.Ball.Update(deltaTime);
            }

            // Обновляем бонусы (если они реализованы)
            // TODO: Добавить обновление бонусов после реализации PowerUp

            // Проверяем все коллизии
            CheckCollisions();

            // Обрабатываем уничтоженные кирпичи
            ProcessDestroyedBricks();

            // Проверяем выход мяча за нижнюю границу (потеря жизни)
            CheckBallOutOfBounds();

            // Проверяем завершение уровня
            CheckLevelComplete();
        }

        // Проверяет все коллизии в игре
        private void CheckCollisions()
        {
            if (!GameObjects.Ball.IsActive)
            {
                return;
            }

            var ball = GameObjects.Ball;
            var paddle = GameObjects.Paddle;

            // Проверка столкновения мяча с платформой
            if (CollisionSystem.HandleBallPaddleCollision(ball, paddle))
            {
                // Коллизия обработана в HandleBallPaddleCollision
            }

            // Проверка столкновения мяча с кирпичами
            foreach (var brick in GameObjects.Bricks)
            {
                if (!brick.IsDestroyed && CollisionSystem.CheckBallBrickCollision(ball, brick))
                {
                    if (CollisionSystem.HandleBallBrickCollision(ball, brick))
                    {
                        // Кирпич получил урон
                        brick.Hit();
                        break; // Обрабатываем только одно столкновение за кадр
                    }
                }
            }

            // Проверка столкновения мяча со стенами
            CollisionSystem.HandleBallWallCollision(ball, GameField);
        }

        // Обрабатывает уничтоженные кирпичи
        private void ProcessDestroyedBricks()
        {
            // Удаляем все разрушенные кирпичи
            int removedCount = GameObjects.RemoveDestroyedBricks();

            // TODO: Создание бонусов при разрушении кирпичей
            // Это будет реализовано после полной реализации PowerUp
        }

        // Проверяет выход мяча за нижнюю границу поля
        private void CheckBallOutOfBounds()
        {
            if (!GameObjects.Ball.IsActive)
            {
                return;
            }

            if (CollisionSystem.CheckBallOutOfBounds(GameObjects.Ball, GameField))
            {
                // Мяч упал вниз - теряем жизнь
                bool continueGame = GameState.LoseLife();

                if (continueGame)
                {
                    // Игра продолжается - перезапускаем мяч
                    ResetBall();
                }
                else
                {
                    // Игра окончена
                    CheckGameOver();
                }
            }
        }

        // Проверяет завершение уровня
        private void CheckLevelComplete()
        {
            if (LevelManager.IsLevelComplete(GameObjects))
            {
                // Уровень завершен
                GameState.CompleteLevel();
            }
        }

        // Проверяет условия завершения игры
        public bool CheckGameOver()
        {
            if (GameState.Lives <= 0)
            {
                GameState.GameOver();
                return true;
            }

            return false;
        }

        // Переходит на следующий уровень
        public bool NextLevel()
        {
            if (!LevelManager.HasMoreLevels())
            {
                // Все уровни пройдены - победа
                GameState.Victory();
                return false;
            }

            // Переходим на следующий уровень
            LevelManager.NextLevel();
            int nextLevel = LevelManager.CurrentLevel;

            // Загружаем новый уровень
            LevelManager.LoadLevel(nextLevel, GameObjects);

            // Сбрасываем позиции мяча и платформы
            ResetBall();
            ResetPaddle();

            // Возвращаемся в состояние игры
            GameState.TransitionTo(GameStateType.Playing);

            return true;
        }

        // Сбрасывает мяч в начальное положение над платформой
        private void ResetBall()
        {
            float paddleX = GameObjects.Paddle.Position.X;
            float paddleY = GameObjects.Paddle.Position.Y;
            GameObjects.Ball.Reset(paddleX, paddleY);
        }

        // Сбрасывает платформу в центр нижней части поля
        private void ResetPaddle()
        {
            float centerX = GameField.Width / 2;
            float paddleY = GameField.Height - 3;
            GameObjects.Paddle.Position = new Vector2(centerX, paddleY);
            GameObjects.Paddle.ResetSize();
        }

        // Обновляет бонусы (падающие бонусы)
        public void UpdatePowerUps(float deltaTime)
        {
            // TODO: Реализовать после полной реализации PowerUp
            // foreach (var powerUp in GameObjects.PowerUps)
            // {
            //     powerUp.Update(deltaTime);
            // }
        }

        // Запускает игровой цикл
        public void Run()
        {
            _isRunning = true;
            var stopwatch = Stopwatch.StartNew();
            var lastTime = stopwatch.ElapsedMilliseconds;
            float targetFrameTime = 1000f / TargetFPS;

            while (_isRunning)
            {
                var currentTime = stopwatch.ElapsedMilliseconds;
                var deltaTime = (currentTime - lastTime) / 1000f; // Конвертируем в секунды
                lastTime = currentTime;

                // Ограничиваем максимальный deltaTime для предотвращения больших скачков
                if (deltaTime > 0.1f)
                {
                    deltaTime = 0.1f;
                }

                // Обновляем игру
                Update(deltaTime);

                // Обрабатываем переходы между состояниями
                ProcessStateTransitions();

                // Контроль FPS
                var frameTime = stopwatch.ElapsedMilliseconds - currentTime;
                if (frameTime < targetFrameTime)
                {
                    var sleepTime = (int)(targetFrameTime - frameTime);
                    System.Threading.Thread.Sleep(sleepTime);
                }
            }
        }

        // Обрабатывает переходы между состояниями игры
        private void ProcessStateTransitions()
        {
            switch (GameState.CurrentState)
            {
                case GameStateType.LevelComplete:
                    // Автоматически переходим на следующий уровень
                    if (LevelManager.HasMoreLevels())
                    {
                        NextLevel();
                    }
                    else
                    {
                        GameState.Victory();
                    }
                    break;

                case GameStateType.GameOver:
                case GameStateType.Victory:
                    // Игра завершена - останавливаем цикл
                    _isRunning = false;
                    break;
            }
        }

        // Останавливает игровой цикл
        public void Stop()
        {
            _isRunning = false;
        }

        // Начинает новую игру
        public void StartNewGame()
        {
            Initialize();
            GameState.StartGame();
        }

        // Ставит игру на паузу
        public void Pause()
        {
            GameState.Pause();
        }

        // Возобновляет игру с паузы
        public void Resume()
        {
            GameState.Resume();
        }

        // Запускает мяч в игру
        public void LaunchBall(float angle = 0)
        {
            if (GameState.CurrentState == GameStateType.Playing)
            {
                GameObjects.Ball.Launch(angle);
            }
        }

        public override string ToString()
        {
            return $"GameEngine(State: {GameState.CurrentState}, " +
                   $"Level: {LevelManager.CurrentLevel}, " +
                   $"Lives: {GameState.Lives}, " +
                   $"Running: {_isRunning})";
        }
    }
}

