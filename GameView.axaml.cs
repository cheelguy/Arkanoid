using Arkanoid.Core;
using Arkanoid.Models;
using Arkanoid.Services;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arkanoid
{
    public partial class GameView : UserControl
    {
        private GameEngine? _gameEngine;
        private ScoreManager? _scoreManager;
        private DispatcherTimer? _gameTimer;
        private Dictionary<Brick, Rectangle> _brickShapes = new();
        private Dictionary<PowerUp, Ellipse> _powerUpShapes = new();
        private Rectangle? _paddleShape;
        private Ellipse? _ballShape;
        private bool _isLeftPressed = false;
        private bool _isRightPressed = false;
        private bool _isPaused = false;

        // Масштаб для отрисовки (игровое поле в пикселях)
        private const float ScaleX = 10f; // 1 игровая единица = 10 пикселей
        private const float ScaleY = 10f;

        public GameView()
        {
            InitializeComponent();
            InitializeGame();
            SetupInput();
        }

        private void InitializeGame()
        {
            _gameEngine = new GameEngine(80, 30);
            _scoreManager = new ScoreManager();
            _gameEngine.Initialize();

            // Настройка таймера игры
            _gameTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16) // ~60 FPS
            };
            _gameTimer.Tick += GameTimer_Tick;
        }

        private void SetupInput()
        {
            this.KeyDown += GameView_KeyDown;
            this.KeyUp += GameView_KeyUp;
            this.Focusable = true;
        }

        private void GameView_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Avalonia.Input.Key.Left:
                case Avalonia.Input.Key.A:
                    _isLeftPressed = true;
                    break;
                case Avalonia.Input.Key.Right:
                case Avalonia.Input.Key.D:
                    _isRightPressed = true;
                    break;
                case Avalonia.Input.Key.Enter:
                    HandleEnterKey();
                    break;
                case Avalonia.Input.Key.Space:
                case Avalonia.Input.Key.P:
                    TogglePause();
                    break;
                case Avalonia.Input.Key.Escape:
                    HandleEscapeKey();
                    break;
            }
        }

        private void GameView_KeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Avalonia.Input.Key.Left:
                case Avalonia.Input.Key.A:
                    _isLeftPressed = false;
                    break;
                case Avalonia.Input.Key.Right:
                case Avalonia.Input.Key.D:
                    _isRightPressed = false;
                    break;
            }
        }

        private void HandleEnterKey()
        {
            if (_gameEngine == null) return;

            if (MenuPanel?.IsVisible == true)
            {
                // Начало игры
                MenuPanel.IsVisible = false;
                _gameEngine.GameState.StartGame();
                _gameEngine.GameObjects.Ball.Launch(0);
                _gameTimer?.Start();
                this.Focus();
            }
            else if (GameOverPanel?.IsVisible == true)
            {
                // Новая игра
                RestartGame();
            }
        }

        private void HandleEscapeKey()
        {
            if (GameOverPanel?.IsVisible == true)
            {
                // Выход из игры
                Environment.Exit(0);
            }
        }

        private void TogglePause()
        {
            if (_gameEngine == null) return;

            if (_gameEngine.GameState.CurrentState == GameStateType.Playing)
            {
                _gameEngine.GameState.Pause();
                _isPaused = true;
            }
            else if (_gameEngine.GameState.CurrentState == GameStateType.Paused)
            {
                _gameEngine.GameState.Resume();
                _isPaused = false;
            }
        }

        private void RestartGame()
        {
            if (_gameEngine == null || _scoreManager == null) return;

            GameOverPanel!.IsVisible = false;
            MenuPanel!.IsVisible = true;
            _scoreManager.Reset();
            _gameEngine.Initialize();
            ClearGameCanvas();
        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            if (_gameEngine == null || _scoreManager == null) return;

            float deltaTime = 0.016f; // ~60 FPS

            // Обработка ввода
            float paddleDirection = 0;
            if (_isLeftPressed) paddleDirection = -1;
            if (_isRightPressed) paddleDirection = 1;

            _gameEngine.GameObjects.Paddle.Move(paddleDirection, deltaTime, _gameEngine.GameField.Width);

            // Обновление игры
            if (_gameEngine.GameState.CurrentState == GameStateType.Playing && !_isPaused)
            {
                var bricksBeforeUpdate = _gameEngine.GameObjects.Bricks
                    .Where(b => !b.IsDestroyed)
                    .ToList();

                _gameEngine.Update(deltaTime);

                // Начисление очков
                foreach (var brick in bricksBeforeUpdate)
                {
                    if (brick.IsDestroyed)
                    {
                        _scoreManager.AddScore(brick.GetPoints());
                    }
                }

                // Запуск мяча после потери жизни
                if (!_gameEngine.GameObjects.Ball.IsActive && _gameEngine.GameState.Lives > 0)
                {
                    var timer = new DispatcherTimer
                    {
                        Interval = TimeSpan.FromMilliseconds(500)
                    };
                    timer.Tick += (s, e) =>
                    {
                        timer.Stop();
                        if (_gameEngine != null)
                        {
                            _gameEngine.GameObjects.Ball.Launch(0);
                        }
                    };
                    timer.Start();
                }

                // Удаление разрушенных кирпичей
                _gameEngine.GameObjects.Bricks.RemoveAll(b => b.IsDestroyed);
            }

            // Проверка завершения уровня
            if (_gameEngine.GameState.CurrentState == GameStateType.LevelComplete)
            {
                if (_gameEngine.LevelManager.HasMoreLevels())
                {
                    _gameEngine.GameState.NextLevel(_gameEngine.LevelManager.TotalLevels);
                    _gameEngine.LevelManager.LoadLevel(_gameEngine.GameState.CurrentLevel, _gameEngine.GameObjects);
                    _gameEngine.GameObjects.Ball.Reset(
                        _gameEngine.GameObjects.Paddle.Position.X,
                        _gameEngine.GameObjects.Paddle.Position.Y
                    );
                    _gameEngine.GameObjects.Ball.Launch(0);
                }
                else
                {
                    _gameEngine.GameState.Victory();
                }
            }

            // Отрисовка
            Render();

            // Проверка окончания игры
            if (_gameEngine.GameState.CurrentState == GameStateType.GameOver ||
                _gameEngine.GameState.CurrentState == GameStateType.Victory)
            {
                _gameTimer?.Stop();
                ShowGameOver(_gameEngine.GameState.CurrentState == GameStateType.Victory);
            }
        }

        private void Render()
        {
            if (_gameEngine == null || _scoreManager == null || GameCanvas == null) return;

            // Обновление UI
            ScoreText!.Text = $"Счет: {_scoreManager.Score}";
            LivesText!.Text = $"Жизни: {_gameEngine.GameState.Lives}";
            LevelText!.Text = $"Уровень: {_gameEngine.GameState.CurrentLevel}";

            // Очистка canvas
            ClearGameCanvas();

            // Отрисовка кирпичей
            foreach (var brick in _gameEngine.GameObjects.Bricks)
            {
                if (!brick.IsDestroyed)
                {
                    DrawBrick(brick);
                }
            }

            // Отрисовка бонусов
            foreach (var powerUp in _gameEngine.GameObjects.PowerUps)
            {
                if (powerUp.IsActive)
                {
                    DrawPowerUp(powerUp);
                }
            }

            // Отрисовка платформы
            DrawPaddle(_gameEngine.GameObjects.Paddle);

            // Отрисовка мяча
            if (_gameEngine.GameObjects.Ball.IsActive)
            {
                DrawBall(_gameEngine.GameObjects.Ball);
            }
        }

        private void DrawBrick(Brick brick)
        {
            if (GameCanvas == null) return;

            if (!_brickShapes.ContainsKey(brick))
            {
                var rect = new Rectangle
                {
                    Fill = new SolidColorBrush(GetColorFromConsoleColor(brick.GetCurrentColor())),
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = 1
                };
                GameCanvas.Children.Add(rect);
                _brickShapes[brick] = rect;
            }

            var bounds = brick.GetBounds();
            var shape = _brickShapes[brick];
            Canvas.SetLeft(shape, bounds.left * ScaleX);
            Canvas.SetTop(shape, bounds.top * ScaleY);
            shape.Width = (bounds.right - bounds.left) * ScaleX;
            shape.Height = (bounds.bottom - bounds.top) * ScaleY;
        }

        private void DrawPaddle(Paddle paddle)
        {
            if (GameCanvas == null) return;

            if (_paddleShape == null)
            {
                _paddleShape = new Rectangle
                {
                    Fill = new SolidColorBrush(Colors.Cyan),
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = 1
                };
                GameCanvas.Children.Add(_paddleShape);
            }

            var bounds = paddle.GetBounds();
            Canvas.SetLeft(_paddleShape, bounds.left * ScaleX);
            Canvas.SetTop(_paddleShape, bounds.top * ScaleY);
            _paddleShape.Width = (bounds.right - bounds.left) * ScaleX;
            _paddleShape.Height = (bounds.bottom - bounds.top) * ScaleY;
        }

        private void DrawBall(Ball ball)
        {
            if (GameCanvas == null) return;

            if (_ballShape == null)
            {
                _ballShape = new Ellipse
                {
                    Fill = new SolidColorBrush(Colors.White),
                    Stroke = new SolidColorBrush(Colors.Gray),
                    StrokeThickness = 1
                };
                GameCanvas.Children.Add(_ballShape);
            }

            Canvas.SetLeft(_ballShape, (ball.Position.X - ball.Radius) * ScaleX);
            Canvas.SetTop(_ballShape, (ball.Position.Y - ball.Radius) * ScaleY);
            _ballShape.Width = ball.Radius * 2 * ScaleX;
            _ballShape.Height = ball.Radius * 2 * ScaleY;
        }

        private void DrawPowerUp(PowerUp powerUp)
        {
            if (GameCanvas == null) return;

            if (!_powerUpShapes.ContainsKey(powerUp))
            {
                var typeData = powerUp.GetTypeData();
                var ellipse = new Ellipse
                {
                    Fill = new SolidColorBrush(GetColorFromConsoleColor(typeData.Color)),
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = 1
                };
                GameCanvas.Children.Add(ellipse);
                _powerUpShapes[powerUp] = ellipse;
            }

            var shape = _powerUpShapes[powerUp];
            var bounds = powerUp.GetBounds();
            Canvas.SetLeft(shape, bounds.left * ScaleX);
            Canvas.SetTop(shape, bounds.top * ScaleY);
            shape.Width = powerUp.Width * ScaleX;
            shape.Height = powerUp.Height * ScaleY;
        }

        private void ClearGameCanvas()
        {
            if (GameCanvas == null) return;

            // Удаляем только игровые объекты, оставляя UI элементы
            var toRemove = new List<Avalonia.Controls.Control>();
            foreach (var child in GameCanvas.Children)
            {
                if (child is Rectangle || child is Ellipse)
                {
                    toRemove.Add(child);
                }
            }

            foreach (var item in toRemove)
            {
                GameCanvas.Children.Remove(item);
            }

            _brickShapes.Clear();
            _powerUpShapes.Clear();
            _paddleShape = null;
            _ballShape = null;
        }

        private void ShowGameOver(bool isVictory)
        {
            if (_gameEngine == null || _scoreManager == null) return;

            GameOverPanel!.IsVisible = true;
            GameOverText!.Text = isVictory ? "ПОБЕДА!" : "ИГРА ОКОНЧЕНА";
            GameOverText.Foreground = new SolidColorBrush(isVictory ? Colors.Green : Colors.Red);
            FinalScoreText!.Text = $"Финальный счет: {_scoreManager.Score}";

            if (_scoreManager.Score > _scoreManager.HighScore)
            {
                _scoreManager.SaveHighScore();
            }
        }

        private Color GetColorFromConsoleColor(ConsoleColor consoleColor)
        {
            return consoleColor switch
            {
                ConsoleColor.Black => Colors.Black,
                ConsoleColor.DarkBlue => Colors.DarkBlue,
                ConsoleColor.DarkGreen => Colors.DarkGreen,
                ConsoleColor.DarkCyan => Colors.DarkCyan,
                ConsoleColor.DarkRed => Colors.DarkRed,
                ConsoleColor.DarkMagenta => Colors.DarkMagenta,
                ConsoleColor.DarkYellow => Colors.Orange,
                ConsoleColor.Gray => Colors.Gray,
                ConsoleColor.DarkGray => Colors.DarkGray,
                ConsoleColor.Blue => Colors.Blue,
                ConsoleColor.Green => Colors.Green,
                ConsoleColor.Cyan => Colors.Cyan,
                ConsoleColor.Red => Colors.Red,
                ConsoleColor.Magenta => Colors.Magenta,
                ConsoleColor.Yellow => Colors.Yellow,
                ConsoleColor.White => Colors.White,
                _ => Colors.White
            };
        }
    }
}


