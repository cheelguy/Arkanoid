namespace Arkanoid.Data
{
    using Arkanoid.Models;

    // Конфигурация уровня
    public class LevelConfig
    {
        public int LevelNumber { get; set; }
        public List<Brick> BrickLayout { get; set; }
        public int Difficulty { get; set; }

        public LevelConfig(int levelNumber, List<Brick> brickLayout, int difficulty)
        {
            LevelNumber = levelNumber;
            BrickLayout = brickLayout ?? new List<Brick>();
            Difficulty = difficulty;
        }
    }

    // Класс с данными уровней
    public static class LevelData
    {
        private const float BrickWidth = 7f;
        private const float BrickHeight = 2f;
        private const float BrickSpacing = 1f;
        private const float StartX = 1f;
        private const float StartY = 3f;
        private const int Columns = 10;
        public const int MaxLevels = 3;

        // Получает конфигурацию уровня по номеру
        public static LevelConfig GetLevel(int levelNumber)
        {
            return levelNumber switch
            {
                1 => CreateLevel1(),
                2 => CreateLevel2(),
                3 => CreateLevel3(),
                _ => CreateLevel1() // По умолчанию возвращаем первый уровень
            };
        }

        // Создает конфигурацию первого уровня (5 рядов x 10 колонок, Normal кирпичи)
        private static LevelConfig CreateLevel1()
        {
            var bricks = new List<Brick>();
            int rows = 5;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    float x = StartX + col * (BrickWidth + BrickSpacing);
                    float y = StartY + row * (BrickHeight + BrickSpacing);
                    
                    var brick = new Brick(x, y, BrickWidth, BrickHeight, BrickType.Normal);
                    bricks.Add(brick);
                }
            }

            return new LevelConfig(1, bricks, difficulty: 1);
        }

        // Создает конфигурацию второго уровня (6 рядов x 10 колонок, микс Normal и Strong)
        private static LevelConfig CreateLevel2()
        {
            var bricks = new List<Brick>();
            int rows = 6;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    float x = StartX + col * (BrickWidth + BrickSpacing);
                    float y = StartY + row * (BrickHeight + BrickSpacing);
                    
                    BrickType brickType;
                    // Первые 2 ряда - Strong, остальные - Normal
                    if (row < 2)
                    {
                        brickType = BrickType.Strong;
                    }
                    else
                    {
                        brickType = BrickType.Normal;
                    }
                    
                    var brick = new Brick(x, y, BrickWidth, BrickHeight, brickType);
                    bricks.Add(brick);
                }
            }

            return new LevelConfig(2, bricks, difficulty: 3);
        }

        // Создает конфигурацию третьего уровня (7 рядов x 10 колонок, VeryStrong и Unbreakable)
        private static LevelConfig CreateLevel3()
        {
            var bricks = new List<Brick>();
            int rows = 7;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    float x = StartX + col * (BrickWidth + BrickSpacing);
                    float y = StartY + row * (BrickHeight + BrickSpacing);
                    
                    BrickType brickType;
                    // Распределение типов кирпичей по рядам
                    if (row == 0)
                    {
                        // Первый ряд - Unbreakable (чередование)
                        brickType = (col % 2 == 0) ? BrickType.Unbreakable : BrickType.VeryStrong;
                    }
                    else if (row == 1)
                    {
                        // Второй ряд - VeryStrong
                        brickType = BrickType.VeryStrong;
                    }
                    else if (row < 3)
                    {
                        // Третий ряд - Strong
                        brickType = BrickType.Strong;
                    }
                    else
                    {
                        // Остальные ряды - Normal
                        brickType = BrickType.Normal;
                    }
                    
                    var brick = new Brick(x, y, BrickWidth, BrickHeight, brickType);
                    bricks.Add(brick);
                }
            }

            return new LevelConfig(3, bricks, difficulty: 5);
        }

        // Проверяет, существует ли уровень с указанным номером
        public static bool LevelExists(int levelNumber)
        {
            return levelNumber >= 1 && levelNumber <= MaxLevels;
        }

        // Получает общее количество кирпичей на уровне
        public static int GetBrickCount(int levelNumber)
        {
            var config = GetLevel(levelNumber);
            return config.BrickLayout.Count;
        }

        // Получает количество разрушаемых кирпичей на уровне
        public static int GetDestructibleBrickCount(int levelNumber)
        {
            var config = GetLevel(levelNumber);
            return config.BrickLayout.Count(brick => brick.Type != BrickType.Unbreakable);
        }
    }
}

