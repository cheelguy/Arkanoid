namespace Arkanoid.Core
{
    /// <summary>
    /// Перечисление состояний игры
    /// </summary>
    public enum GameStateType
    {
        /// <summary>
        /// Главное меню
        /// </summary>
        Menu,

        /// <summary>
        /// Игра в процессе
        /// </summary>
        Playing,

        /// <summary>
        /// Игра на паузе
        /// </summary>
        Paused,

        /// <summary>
        /// Игра окончена (проигрыш)
        /// </summary>
        GameOver,

        /// <summary>
        /// Победа (все уровни пройдены)
        /// </summary>
        Victory,

        /// <summary>
        /// Уровень завершен
        /// </summary>
        LevelComplete
    }

    /// <summary>
    /// Класс управления состоянием игры
    /// Отвечает за отслеживание текущего состояния, жизней, уровня и валидацию переходов
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// Текущее состояние игры
        /// </summary>
        public GameStateType CurrentState { get; private set; }

        /// <summary>
        /// Количество жизней игрока
        /// </summary>
        public int Lives { get; private set; }

        /// <summary>
        /// Текущий уровень
        /// </summary>
        public int CurrentLevel { get; private set; }

        /// <summary>
        /// Флаг, указывающий запущена ли игра
        /// </summary>
        public bool IsGameRunning => CurrentState == GameStateType.Playing || 
                                     CurrentState == GameStateType.Paused;

        /// <summary>
        /// Константа начального количества жизней
        /// </summary>
        public const int InitialLives = 3;

        /// <summary>
        /// Константа начального уровня
        /// </summary>
        public const int InitialLevel = 1;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public GameState()
        {
            CurrentState = GameStateType.Menu;
            Lives = InitialLives;
            CurrentLevel = InitialLevel;
        }

        /// <summary>
        /// Конструктор с начальными параметрами
        /// </summary>
        /// <param name="initialLives">Начальное количество жизней</param>
        /// <param name="initialLevel">Начальный уровень</param>
        public GameState(int initialLives, int initialLevel = InitialLevel)
        {
            CurrentState = GameStateType.Menu;
            Lives = initialLives;
            CurrentLevel = initialLevel;
        }

        /// <summary>
        /// Начинает новую игру
        /// </summary>
        /// <param name="startLevel">Уровень, с которого начать</param>
        /// <returns>True если переход выполнен успешно</returns>
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

        /// <summary>
        /// Ставит игру на паузу
        /// </summary>
        /// <returns>True если переход выполнен успешно</returns>
        public bool Pause()
        {
            if (CurrentState != GameStateType.Playing)
            {
                return false;
            }

            CurrentState = GameStateType.Paused;
            return true;
        }

        /// <summary>
        /// Возобновляет игру с паузы
        /// </summary>
        /// <returns>True если переход выполнен успешно</returns>
        public bool Resume()
        {
            if (CurrentState != GameStateType.Paused)
            {
                return false;
            }

            CurrentState = GameStateType.Playing;
            return true;
        }

        /// <summary>
        /// Завершает текущий уровень
        /// </summary>
        /// <returns>True если переход выполнен успешно</returns>
        public bool CompleteLevel()
        {
            if (CurrentState != GameStateType.Playing)
            {
                return false;
            }

            CurrentState = GameStateType.LevelComplete;
            return true;
        }

        /// <summary>
        /// Переходит на следующий уровень
        /// </summary>
        /// <param name="maxLevels">Максимальное количество уровней</param>
        /// <returns>True если переход выполнен успешно</returns>
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

        /// <summary>
        /// Обрабатывает потерю жизни
        /// </summary>
        /// <returns>True если игра должна продолжиться, false если игра окончена</returns>
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

        /// <summary>
        /// Добавляет жизнь (для бонусов)
        /// </summary>
        public void AddLife()
        {
            Lives++;
        }

        /// <summary>
        /// Завершает игру с поражением
        /// </summary>
        public void GameOver()
        {
            CurrentState = GameStateType.GameOver;
        }

        /// <summary>
        /// Завершает игру с победой
        /// </summary>
        public void Victory()
        {
            CurrentState = GameStateType.Victory;
        }

        /// <summary>
        /// Возвращает в главное меню
        /// </summary>
        /// <returns>True если переход выполнен успешно</returns>
        public bool ReturnToMenu()
        {
            if (CurrentState == GameStateType.Menu)
            {
                return false;
            }

            CurrentState = GameStateType.Menu;
            return true;
        }

        /// <summary>
        /// Сбрасывает состояние игры к начальным значениям
        /// </summary>
        public void Reset()
        {
            CurrentState = GameStateType.Menu;
            Lives = InitialLives;
            CurrentLevel = InitialLevel;
        }

        /// <summary>
        /// Проверяет, можно ли перейти из текущего состояния в указанное
        /// </summary>
        /// <param name="targetState">Целевое состояние</param>
        /// <returns>True если переход допустим</returns>
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

        /// <summary>
        /// Безопасно переходит в указанное состояние с валидацией
        /// </summary>
        /// <param name="targetState">Целевое состояние</param>
        /// <returns>True если переход выполнен успешно</returns>
        public bool TransitionTo(GameStateType targetState)
        {
            if (!CanTransitionTo(targetState))
            {
                return false;
            }

            CurrentState = targetState;
            return true;
        }

        /// <summary>
        /// Получает строковое представление текущего состояния
        /// </summary>
        public override string ToString()
        {
            return $"GameState(State: {CurrentState}, Lives: {Lives}, Level: {CurrentLevel})";
        }
    }
}

