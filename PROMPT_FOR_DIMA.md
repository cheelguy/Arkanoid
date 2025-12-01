# ПРОМПТ ДЛЯ ДИМЫ - РЕАЛИЗАЦИЯ ИГРОВОЙ ЛОГИКИ И СИСТЕМ

Скопируйте этот промпт и вставьте в AI ассистент (Cursor/Claude/GPT) для выполнения вашей части работы.

---

## ПРОМПТ:

Привет! Я работаю над проектом Arkanoid (консольная игра на C#). 

**Контекст:**
- Репозиторий: https://github.com/cheelguy/Arkanoid.git
- Часть Никиты (игровые объекты и физика) УЖЕ ГОТОВА и запушена
- Мне нужно реализовать часть Димы: игровая логика и системы

**Что уже готово:**
1. Models/Vector2.cs - класс векторов с операциями (полностью реализован)
2. Models/Ball.cs - класс мяча с методами Reflect, Update (полностью реализован)
3. Models/Paddle.cs - класс платформы с методом Move, GetBounds (полностью реализован)
4. Models/Brick.cs - класс кирпича с методом Hit, GetBounds (полностью реализован)
5. Models/BrickType.cs - типы кирпичей с данными (полностью реализован)
6. Models/GameObjects.cs - контейнер объектов (полностью реализован)
7. Core/GameField.cs - игровое поле с границами (полностью реализован)
8. Arkanoid.csproj - проект настроен для .NET 9.0

**Что нужно реализовать (МОЯ ЧАСТЬ - ДИМА):**

### 1. Core/GameState.cs
Управление состоянием игры:
- Enum GameStateType: Menu, Playing, Paused, GameOver, Victory, LevelComplete
- Класс GameState:
  - Свойства: CurrentState, Lives, CurrentLevel, IsGameRunning
  - Метод ChangeState(GameStateType newState) - смена состояния с валидацией
  - Метод Reset() - сброс к начальному состоянию
  - Начальные значения: Lives = 3, CurrentLevel = 1

### 2. Data/LevelData.cs
Данные уровней:
- Класс LevelConfig:
  - Свойства: LevelNumber, BrickLayout (List<Brick>), Difficulty
  - Метод для создания конфигурации уровня
- Класс LevelData:
  - Статический метод GetLevel(int levelNumber) -> LevelConfig
  - Минимум 3 готовых уровня:
    - Уровень 1: простая сетка из Normal кирпичей (5 рядов x 10 колонок)
    - Уровень 2: микс Normal и Strong кирпичей (6 рядов x 10 колонок)
    - Уровень 3: включает VeryStrong и Unbreakable кирпичи (7 рядов x 10 колонок)
  - Размер кирпича: Width = 7, Height = 2
  - Отступы между кирпичами: 1 символ
  - Начальная позиция: X = 1, Y = 3

### 3. Core/LevelManager.cs
Управление уровнями:
- Свойства: CurrentLevel (int), TotalLevels (int)
- Метод LoadLevel(int levelNumber, GameObjects gameObjects):
  - Очистить существующие кирпичи
  - Загрузить конфигурацию из LevelData
  - Добавить кирпичи в gameObjects
- Метод IsLevelComplete(GameObjects gameObjects) -> bool:
  - Проверить что все разрушаемые кирпичи уничтожены
  - Использовать gameObjects.HasActiveBricks()
- Метод NextLevel() - увеличить номер уровня
- Метод HasMoreLevels() -> bool

### 4. Core/CollisionSystem.cs
Система проверки коллизий (САМАЯ ВАЖНАЯ ЧАСТЬ):
- Статический класс с методами проверки

#### Метод CheckBallPaddleCollision(Ball ball, Paddle paddle) -> bool:
- Получить границы платформы через paddle.GetBounds()
- Проверить пересечение мяча (ball.Position +/- ball.Radius) с платформой
- Если есть коллизия:
  - Вычислить относительную позицию попадания: paddle.GetRelativeHitPosition(ball.Position.X)
  - Изменить угол отскока на основе позиции (центр - прямо, края - под углом)
  - Использовать ball.Reflect() с вычисленной нормалью
  - Вернуть true
- Иначе вернуть false

#### Метод CheckBallBrickCollision(Ball ball, List<Brick> bricks) -> (bool hit, Brick hitBrick):
- Перебрать все неразрушенные кирпичи
- Для каждого кирпича:
  - Получить границы через brick.GetBounds()
  - Проверить пересечение с мячом
  - Если есть коллизия:
    - Определить сторону столкновения (верх/низ/лево/право)
    - Вызвать ball.ReflectVertical() или ball.ReflectHorizontal()
    - Вызвать brick.Hit()
    - Вернуть (true, brick)
- Если коллизий нет, вернуть (false, null)

#### Метод CheckBallWallCollision(Ball ball, GameField field):
- Проверить левую/правую границы: field.IsOutOfLeftBound() / IsOutOfRightBound()
  - Если да: ball.ReflectHorizontal()
- Проверить верхнюю границу: field.IsOutOfTopBound()
  - Если да: ball.ReflectVertical()
- НЕ проверять нижнюю границу (там потеря жизни)

#### Метод CheckBallOutOfBounds(Ball ball, GameField field) -> bool:
- Проверить field.IsOutOfBottomBound(ball.Position.Y)
- Вернуть true если мяч улетел вниз

#### Метод CheckPowerUpPaddleCollision(PowerUp powerUp, Paddle paddle) -> bool:
- Получить границы платформы
- Проверить пересечение бонуса с платформой
- Вернуть true если есть коллизия

### 5. Core/GameEngine.cs (интеграция всего - делать В ПОСЛЕДНЮЮ ОЧЕРЕДЬ)
Главный класс игрового цикла:
- Поля: GameObjects, GameField, GameState, LevelManager, ScoreManager (если есть)
- Метод Initialize():
  - Создать мяч и платформу в начальных позициях
  - Загрузить первый уровень
  - Сбросить состояние
- Метод Update(float deltaTime):
  - Обновить позиции объектов (ball.Update, powerUps.Update)
  - Проверить все коллизии через CollisionSystem
  - Обработать уничтоженные кирпичи (удалить, создать бонусы)
  - Проверить подбор бонусов
  - Проверить выход мяча за пределы (потеря жизни)
  - Проверить завершение уровня
- Метод CheckGameOver() -> bool:
  - Если Lives <= 0: ChangeState(GameOver), вернуть true
- Метод NextLevel():
  - LevelManager.LoadLevel(++level)
  - Сбросить позиции мяча и платформы
- Метод Run():
  - Основной игровой цикл
  - Использовать DateTime для deltaTime
  - Целевой FPS: 30-60

**ВАЖНО ДЛЯ КОЛЛИЗИЙ:**
1. Изучи методы в Ball.cs: Reflect(), ReflectHorizontal(), ReflectVertical()
2. Изучи методы GetBounds() в Paddle.cs и Brick.cs
3. Изучи методы IsOutOfBounds() в GameField.cs
4. Коллизия мяча - это проверка пересечения круга (ball.Position, ball.Radius) с прямоугольником

**ВАЖНЫЕ ТРЕБОВАНИЯ:**
1. Все комментарии на русском языке БЕЗ эмодзи
2. Все Console.WriteLine БЕЗ эмодзи
3. Использовать namespace Arkanoid.Core, Arkanoid.Data
4. Код должен компилироваться: `dotnet build`
5. Начать с простых классов (GameState, LevelData) и закончить сложными (CollisionSystem, GameEngine)

**ПОСЛЕ РЕАЛИЗАЦИИ:**
1. Проверить компиляцию: `dotnet build`
2. Закоммитить: `git add . && git commit -m "Реализована часть Димы: игровая логика и системы"`
3. Запушить: `git push`

Начни с реализации GameState и LevelData, затем LevelManager, потом CollisionSystem, и в самом конце GameEngine.

---

## КОНЕЦ ПРОМПТА

После вставки этого промпта AI должен реализовать всю твою часть автоматически.

