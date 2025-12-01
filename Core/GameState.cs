namespace Arkanoid.Core
{
    // Перечисление состояний игры
    public enum GameStateType
    {
        Menu,           // Главное меню
        Playing,         // Игра в процессе
        Paused,          // Игра на паузе
        GameOver,        // Игра окончена (проигрыш)
        Victory,         // Победа (все уровни пройдены)
        LevelComplete    // Уровень завершен
    }

    // Класс управления состоянием игры
    public class GameState
    {
        public GameStateType CurrentState { get; private set; }
        public int Lives { get; private set; }
        public int CurrentLevel { get; private set; }
        
        // Флаг, указывающий запущена ли игра
        public bool IsGameRunning => CurrentState == GameStateType.Playing || 
                                     CurrentState == GameStateType.Paused;

        public const int InitialLives = 3;
        public const int InitialLevel = 1;

        public GameState()
        {
            CurrentState = GameStateType.Menu;
            Lives = InitialLives;
            CurrentLevel = InitialLevel;
        }

        public GameState(int initialLives, int initialLevel = InitialLevel)
        {
            CurrentState = GameStateType.Menu;
            Lives = initialLives;
            CurrentLevel = initialLevel;
        }

        // Начинает новую игру
        public bool StartGame(int startLevel = InitialLevel)
        {
            if (CurrentState != GameStateType.Menu && 
                CurrentState != GameStateType.GameOver && 
                CurrentState != GameStateType.Victory)
            {
                return false;
            }

            CurrentState = GameStateType.Playing;
            Lives = InitialLives;
            CurrentLevel = startLevel;
            return true;
        }

        // Ставит игру на паузу
        public bool Pause()
        {
            if (CurrentState != GameStateType.Playing)
            {
                return false;
            }

            CurrentState = GameStateType.Paused;
            return true;
        }

        // Возобновляет игру с паузы
        public bool Resume()
        {
            if (CurrentState != GameStateType.Paused)
            {
                return false;
            }

            CurrentState = GameStateType.Playing;
            return true;
        }

        // Завершает текущий уровень
        public bool CompleteLevel()
        {
            if (CurrentState != GameStateType.Playing)
            {
                return false;
            }

            CurrentState = GameStateType.LevelComplete;
            return true;
        }

        // Переходит на следующий уровень
        public bool NextLevel(int maxLevels = int.MaxValue)
        {
            if (CurrentState != GameStateType.LevelComplete)
            {
                return false;
            }

            CurrentLevel++;

            // Проверяем, пройдены ли все уровни
            if (CurrentLevel > maxLevels)
            {
                CurrentState = GameStateType.Victory;
            }
            else
            {
                CurrentState = GameStateType.Playing;
            }

            return true;
        }

        // Обрабатывает потерю жизни
        public bool LoseLife()
        {
            if (CurrentState != GameStateType.Playing)
            {
                return false;
            }

            Lives--;

            if (Lives <= 0)
            {
                CurrentState = GameStateType.GameOver;
                return false;
            }

            // Игра продолжается, но мяч нужно перезапустить
            return true;
        }

        // Добавляет жизнь (для бонусов)
        public void AddLife()
        {
            Lives++;
        }

        // Завершает игру с поражением
        public void GameOver()
        {
            CurrentState = GameStateType.GameOver;
        }

        // Завершает игру с победой
        public void Victory()
        {
            CurrentState = GameStateType.Victory;
        }

        // Возвращает в главное меню
        public bool ReturnToMenu()
        {
            if (CurrentState == GameStateType.Menu)
            {
                return false;
            }

            CurrentState = GameStateType.Menu;
            return true;
        }

        // Сбрасывает состояние игры к начальным значениям
        public void Reset()
        {
            CurrentState = GameStateType.Menu;
            Lives = InitialLives;
            CurrentLevel = InitialLevel;
        }

        // Проверяет, можно ли перейти из текущего состояния в указанное
        public bool CanTransitionTo(GameStateType targetState)
        {
            return CurrentState switch
            {
                GameStateType.Menu => targetState == GameStateType.Playing,
                GameStateType.Playing => targetState == GameStateType.Paused || 
                                        targetState == GameStateType.LevelComplete || 
                                        targetState == GameStateType.GameOver || 
                                        targetState == GameStateType.Victory,
                GameStateType.Paused => targetState == GameStateType.Playing || 
                                       targetState == GameStateType.Menu,
                GameStateType.LevelComplete => targetState == GameStateType.Playing || 
                                              targetState == GameStateType.Victory,
                GameStateType.GameOver => targetState == GameStateType.Menu,
                GameStateType.Victory => targetState == GameStateType.Menu,
                _ => false
            };
        }

        // Безопасно переходит в указанное состояние с валидацией
        public bool TransitionTo(GameStateType targetState)
        {
            if (!CanTransitionTo(targetState))
            {
                return false;
            }

            CurrentState = targetState;
            return true;
        }

        public override string ToString()
        {
            return $"GameState(State: {CurrentState}, Lives: {Lives}, Level: {CurrentLevel})";
        }
    }
}

