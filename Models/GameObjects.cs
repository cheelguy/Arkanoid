namespace Arkanoid.Models
{
    /// <summary>
    /// Контейнер для всех игровых объектов
    /// Централизованное хранение мяча, платформы, кирпичей и бонусов
    /// </summary>
    public class GameObjects
    {
        /// <summary>
        /// Мяч игрока
        /// </summary>
        public Ball Ball { get; set; }

        /// <summary>
        /// Платформа игрока
        /// </summary>
        public Paddle Paddle { get; set; }

        /// <summary>
        /// Список всех кирпичей на поле
        /// </summary>
        public List<Brick> Bricks { get; set; }

        /// <summary>
        /// Список активных бонусов на поле
        /// </summary>
        public List<PowerUp> PowerUps { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public GameObjects()
        {
            Ball = new Ball();
            Paddle = new Paddle();
            Bricks = new List<Brick>();
            PowerUps = new List<PowerUp>();
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="ball">Мяч</param>
        /// <param name="paddle">Платформа</param>
        public GameObjects(Ball ball, Paddle paddle)
        {
            Ball = ball;
            Paddle = paddle;
            Bricks = new List<Brick>();
            PowerUps = new List<PowerUp>();
        }

        /// <summary>
        /// Добавляет кирпич в список
        /// </summary>
        /// <param name="brick">Кирпич для добавления</param>
        public void AddBrick(Brick brick)
        {
            if (brick != null)
            {
                Bricks.Add(brick);
            }
        }

        /// <summary>
        /// Добавляет несколько кирпичей в список
        /// </summary>
        /// <param name="bricks">Список кирпичей</param>
        public void AddBricks(IEnumerable<Brick> bricks)
        {
            if (bricks != null)
            {
                Bricks.AddRange(bricks);
            }
        }

        /// <summary>
        /// Удаляет кирпич из списка
        /// </summary>
        /// <param name="brick">Кирпич для удаления</param>
        public void RemoveBrick(Brick brick)
        {
            Bricks.Remove(brick);
        }

        /// <summary>
        /// Удаляет все разрушенные кирпичи
        /// </summary>
        /// <returns>Количество удаленных кирпичей</returns>
        public int RemoveDestroyedBricks()
        {
            int count = Bricks.RemoveAll(brick => brick.IsDestroyed);
            return count;
        }

        /// <summary>
        /// Добавляет бонус в список активных бонусов
        /// </summary>
        /// <param name="powerUp">Бонус для добавления</param>
        public void AddPowerUp(PowerUp powerUp)
        {
            if (powerUp != null)
            {
                PowerUps.Add(powerUp);
            }
        }

        /// <summary>
        /// Удаляет бонус из списка
        /// </summary>
        /// <param name="powerUp">Бонус для удаления</param>
        public void RemovePowerUp(PowerUp powerUp)
        {
            PowerUps.Remove(powerUp);
        }

        /// <summary>
        /// Удаляет все неактивные бонусы
        /// </summary>
        /// <returns>Количество удаленных бонусов</returns>
        public int RemoveInactivePowerUps()
        {
            int count = PowerUps.RemoveAll(powerUp => !powerUp.IsActive);
            return count;
        }

        /// <summary>
        /// Очищает все кирпичи
        /// </summary>
        public void ClearBricks()
        {
            Bricks.Clear();
        }

        /// <summary>
        /// Очищает все бонусы
        /// </summary>
        public void ClearPowerUps()
        {
            PowerUps.Clear();
        }

        /// <summary>
        /// Получает количество активных (неразрушенных) кирпичей
        /// </summary>
        /// <returns>Количество активных кирпичей</returns>
        public int GetActiveBrickCount()
        {
            return Bricks.Count(brick => !brick.IsDestroyed && brick.Type != BrickType.Unbreakable);
        }

        /// <summary>
        /// Получает количество разрушаемых кирпичей (исключая неразрушимые)
        /// </summary>
        /// <returns>Количество разрушаемых кирпичей</returns>
        public int GetDestructibleBrickCount()
        {
            return Bricks.Count(brick => brick.Type != BrickType.Unbreakable);
        }

        /// <summary>
        /// Проверяет, остались ли разрушаемые кирпичи на поле
        /// </summary>
        /// <returns>True если есть неразрушенные разрушаемые кирпичи</returns>
        public bool HasActiveBricks()
        {
            return Bricks.Any(brick => !brick.IsDestroyed && brick.Type != BrickType.Unbreakable);
        }

        /// <summary>
        /// Сбрасывает все объекты к начальному состоянию
        /// </summary>
        public void Reset()
        {
            Ball.Reset(Paddle.Position.X, Paddle.Position.Y);
            Paddle.ResetSize();
            ClearBricks();
            ClearPowerUps();
        }

        /// <summary>
        /// Получает общее количество объектов в контейнере
        /// </summary>
        /// <returns>Общее количество объектов</returns>
        public int GetTotalObjectCount()
        {
            return 1 + 1 + Bricks.Count + PowerUps.Count; // Ball + Paddle + Bricks + PowerUps
        }

        /// <summary>
        /// Получает строковое представление состояния объектов
        /// </summary>
        public override string ToString()
        {
            return $"GameObjects(Ball: {Ball.IsActive}, " +
                   $"Bricks: {Bricks.Count}, " +
                   $"Active: {GetActiveBrickCount()}, " +
                   $"PowerUps: {PowerUps.Count})";
        }
    }
}

