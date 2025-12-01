namespace Arkanoid.Core
{
    using Arkanoid.Data;
    using Arkanoid.Models;

    // Класс управления уровнями игры
    public class LevelManager
    {
        public int CurrentLevel { get; private set; }
        public int TotalLevels { get; private set; }

        public LevelManager()
        {
            CurrentLevel = 1;
            TotalLevels = LevelData.MaxLevels;
        }

        public LevelManager(int startLevel, int totalLevels = LevelData.MaxLevels)
        {
            CurrentLevel = startLevel;
            TotalLevels = totalLevels;
        }

        // Загружает уровень в игровые объекты
        public bool LoadLevel(int levelNumber, GameObjects gameObjects)
        {
            if (gameObjects == null)
            {
                return false;
            }

            if (!LevelData.LevelExists(levelNumber))
            {
                return false;
            }

            // Очищаем существующие кирпичи
            gameObjects.ClearBricks();

            // Загружаем конфигурацию уровня
            var levelConfig = LevelData.GetLevel(levelNumber);

            // Добавляем кирпичи в игровые объекты
            foreach (var brick in levelConfig.BrickLayout)
            {
                gameObjects.AddBrick(brick);
            }

            // Обновляем текущий уровень
            CurrentLevel = levelNumber;

            return true;
        }

        // Загружает текущий уровень
        public bool LoadCurrentLevel(GameObjects gameObjects)
        {
            return LoadLevel(CurrentLevel, gameObjects);
        }

        // Генерирует кирпичи для уровня
        public List<Brick> GenerateBricks(int level)
        {
            if (!LevelData.LevelExists(level))
            {
                return new List<Brick>();
            }

            var levelConfig = LevelData.GetLevel(level);
            return new List<Brick>(levelConfig.BrickLayout);
        }

        // Проверяет, завершен ли текущий уровень
        public bool IsLevelComplete(GameObjects gameObjects)
        {
            if (gameObjects == null)
            {
                return false;
            }

            // Уровень завершен, если не осталось активных разрушаемых кирпичей
            return !gameObjects.HasActiveBricks();
        }

        // Переходит на следующий уровень
        public bool NextLevel()
        {
            if (!HasMoreLevels())
            {
                return false;
            }

            CurrentLevel++;
            return true;
        }

        // Проверяет, есть ли еще уровни после текущего
        public bool HasMoreLevels()
        {
            return CurrentLevel < TotalLevels;
        }

        // Устанавливает текущий уровень
        public bool SetLevel(int levelNumber)
        {
            if (!LevelData.LevelExists(levelNumber))
            {
                return false;
            }

            CurrentLevel = levelNumber;
            return true;
        }

        // Сбрасывает менеджер уровней к начальному состоянию
        public void Reset()
        {
            CurrentLevel = 1;
        }

        // Получает конфигурацию текущего уровня
        public LevelConfig GetCurrentLevelConfig()
        {
            return LevelData.GetLevel(CurrentLevel);
        }

        // Получает количество кирпичей на текущем уровне
        public int GetCurrentLevelBrickCount()
        {
            return LevelData.GetBrickCount(CurrentLevel);
        }

        // Получает количество разрушаемых кирпичей на текущем уровне
        public int GetCurrentLevelDestructibleBrickCount()
        {
            return LevelData.GetDestructibleBrickCount(CurrentLevel);
        }

        // Получает сложность текущего уровня
        public int GetCurrentLevelDifficulty()
        {
            var config = GetCurrentLevelConfig();
            return config.Difficulty;
        }

        // Проверяет, является ли текущий уровень последним
        public bool IsLastLevel()
        {
            return CurrentLevel >= TotalLevels;
        }

        public override string ToString()
        {
            return $"LevelManager(Current: {CurrentLevel}/{TotalLevels}, " +
                   $"HasMore: {HasMoreLevels()})";
        }
    }
}

