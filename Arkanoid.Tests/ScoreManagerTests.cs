using Arkanoid.Services;
using System.IO;
using Xunit;

namespace Arkanoid.Tests
{
    public class ScoreManagerTests : IDisposable
    {
        private const string TestHighScoreFile = "test_highscore.txt";

        public ScoreManagerTests()
        {
            // Удаляем тестовый файл перед каждым тестом
            if (File.Exists(TestHighScoreFile))
            {
                File.Delete(TestHighScoreFile);
            }
        }

        public void Dispose()
        {
            // Удаляем тестовый файл после каждого теста
            if (File.Exists(TestHighScoreFile))
            {
                File.Delete(TestHighScoreFile);
            }
        }

        [Fact]
        public void Constructor_InitializesWithZeroScore()
        {
            // Подготовка & Act
            var scoreManager = new ScoreManager();

            // Проверка
            Assert.Equal(0, scoreManager.Score);
        }

        [Fact]
        public void AddScore_PositivePoints_IncreasesScore()
        {
            // Подготовка
            var scoreManager = new ScoreManager();
            int points = 100;

            // Действие
            scoreManager.AddScore(points);

            // Проверка
            Assert.Equal(points, scoreManager.Score);
        }

        [Fact]
        public void AddScore_MultipleTimes_AccumulatesScore()
        {
            // Подготовка
            var scoreManager = new ScoreManager();

            // Действие
            scoreManager.AddScore(10);
            scoreManager.AddScore(20);
            scoreManager.AddScore(30);

            // Проверка
            Assert.Equal(60, scoreManager.Score);
        }

        [Fact]
        public void AddScore_NegativePoints_DoesNotChangeScore()
        {
            // Подготовка
            var scoreManager = new ScoreManager();
            int initialScore = scoreManager.Score;

            // Действие
            scoreManager.AddScore(-10);

            // Проверка
            Assert.Equal(initialScore, scoreManager.Score);
        }

        [Fact]
        public void AddScore_ZeroPoints_DoesNotChangeScore()
        {
            // Подготовка
            var scoreManager = new ScoreManager();
            int initialScore = scoreManager.Score;

            // Действие
            scoreManager.AddScore(0);

            // Проверка
            Assert.Equal(initialScore, scoreManager.Score);
        }

        [Fact]
        public void AddScore_NewHighScore_UpdatesHighScore()
        {
            // Подготовка
            var scoreManager = new ScoreManager();
            int points = 1000;

            // Действие
            scoreManager.AddScore(points);

            // Проверка
            Assert.Equal(points, scoreManager.HighScore);
        }

        [Fact]
        public void AddScore_BelowHighScore_DoesNotUpdateHighScore()
        {
            // Подготовка
            var scoreManager = new ScoreManager();
            scoreManager.AddScore(1000); // Устанавливаем рекорд
            int initialHighScore = scoreManager.HighScore;
            scoreManager.Reset(); // Сбрасываем счет, но рекорд остается
            int newScore = 500;

            // Действие
            scoreManager.AddScore(newScore);

            // Проверка
            Assert.Equal(500, scoreManager.Score); // Общий счет
            Assert.Equal(initialHighScore, scoreManager.HighScore); // Рекорд не изменился (остался 1000)
        }

        [Fact]
        public void Reset_SetsScoreToZero()
        {
            // Подготовка
            var scoreManager = new ScoreManager();
            scoreManager.AddScore(100);

            // Действие
            scoreManager.Reset();

            // Проверка
            Assert.Equal(0, scoreManager.Score);
        }

        [Fact]
        public void Reset_DoesNotResetHighScore()
        {
            // Подготовка
            var scoreManager = new ScoreManager();
            scoreManager.AddScore(1000); // Устанавливаем рекорд

            // Действие
            scoreManager.Reset();

            // Проверка
            Assert.Equal(0, scoreManager.Score);
            Assert.Equal(1000, scoreManager.HighScore);
        }

        [Fact]
        public void SaveHighScore_CreatesFile()
        {
            // Подготовка
            var scoreManager = new ScoreManager();
            scoreManager.AddScore(500);

            // Действие
            scoreManager.SaveHighScore();

            // Проверка
            Assert.True(File.Exists("highscore.txt"));
        }

        [Fact]
        public void LoadHighScore_ExistingFile_LoadsHighScore()
        {
            // Подготовка
            File.WriteAllText("highscore.txt", "750");
            var scoreManager = new ScoreManager();

            // Действие
            scoreManager.LoadHighScore();

            // Проверка
            Assert.Equal(750, scoreManager.HighScore);

            // Очистка
            if (File.Exists("highscore.txt"))
            {
                File.Delete("highscore.txt");
            }
        }

        [Fact]
        public void LoadHighScore_NonExistentFile_KeepsZeroHighScore()
        {
            // Подготовка
            if (File.Exists("highscore.txt"))
            {
                File.Delete("highscore.txt");
            }
            var scoreManager = new ScoreManager();

            // Действие
            scoreManager.LoadHighScore();

            // Проверка
            Assert.Equal(0, scoreManager.HighScore);
        }

        [Fact]
        public void LoadHighScore_InvalidFileContent_KeepsZeroHighScore()
        {
            // Подготовка
            File.WriteAllText("highscore.txt", "invalid");
            var scoreManager = new ScoreManager();

            // Действие
            scoreManager.LoadHighScore();

            // Проверка
            Assert.Equal(0, scoreManager.HighScore);

            // Очистка
            if (File.Exists("highscore.txt"))
            {
                File.Delete("highscore.txt");
            }
        }
    }
}

