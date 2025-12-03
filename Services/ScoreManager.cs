namespace Arkanoid.Services
{
    /// <summary>
    /// Класс управления очками
    /// </summary>
    public class ScoreManager
    {
        private const string HighScoreFileName = "highscore.txt";

        /// <summary>
        /// Текущий счет
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// Рекордный счет
        /// </summary>
        public int HighScore { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ScoreManager()
        {
            Score = 0;
            HighScore = 0;
            LoadHighScore();
        }

        /// <summary>
        /// Добавляет очки к счету
        /// </summary>
        /// <param name="points">Количество очков для добавления</param>
        public void AddScore(int points)
        {
            if (points > 0)
            {
                Score += points;

                // Обновляем рекорд если текущий счет больше
                if (Score > HighScore)
                {
                    HighScore = Score;
                    SaveHighScore();
                }
            }
        }

        /// <summary>
        /// Сбрасывает счет
        /// </summary>
        public void Reset()
        {
            Score = 0;
        }

        /// <summary>
        /// Сохраняет рекордный счет в файл
        /// </summary>
        public void SaveHighScore()
        {
            try
            {
                File.WriteAllText(HighScoreFileName, HighScore.ToString());
            }
            catch (Exception)
            {
                // Игнорируем ошибки записи в файл
            }
        }

        /// <summary>
        /// Загружает рекордный счет из файла
        /// </summary>
        public void LoadHighScore()
        {
            try
            {
                if (File.Exists(HighScoreFileName))
                {
                    string content = File.ReadAllText(HighScoreFileName);
                    if (int.TryParse(content, out int loadedScore))
                    {
                        HighScore = loadedScore;
                    }
                }
            }
            catch (Exception)
            {
                // Игнорируем ошибки чтения файла
                HighScore = 0;
            }
        }
    }
}


