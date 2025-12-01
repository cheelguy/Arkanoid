# ПРОМПТ ДЛЯ КОСТИ - РЕАЛИЗАЦИЯ ИНТЕРФЕЙСА И БОНУСОВ

Скопируйте этот промпт и вставьте в AI ассистент (Cursor/Claude/GPT) для выполнения вашей части работы.

---

## ПРОМПТ:

Привет! Я работаю над проектом Arkanoid (консольная игра на C#). 

**Контекст:**
- Репозиторий: https://github.com/cheelguy/Arkanoid.git
- Часть Никиты (игровые объекты и физика) УЖЕ ГОТОВА и запушена
- Мне нужно реализовать часть Кости: интерфейс, ввод/вывод и бонусы

**Что уже готово:**
1. Models/Vector2.cs - класс векторов (полностью реализован)
2. Models/Ball.cs - класс мяча (полностью реализован)
3. Models/Paddle.cs - класс платформы (полностью реализован)
4. Models/Brick.cs - класс кирпича (полностью реализован)
5. Models/BrickType.cs - типы кирпичей (полностью реализован)
6. Models/GameObjects.cs - контейнер объектов (полностью реализован)
7. Core/GameField.cs - игровое поле (полностью реализован)
8. Arkanoid.csproj - проект настроен для .NET 9.0

**Что есть в виде заглушек (нужно заменить):**
- Models/PowerUp.cs - только базовая структура (Position, IsActive)
- Models/PowerUpType.cs - только enum с типами
- program.cs - только Main с заглушкой

**Что нужно реализовать (МОЯ ЧАСТЬ - КОСТЯ):**

### 1. Models/PowerUpType.cs
Дополнить функционалом аналогично BrickTypeData:
- Enum типов: ExpandPaddle, ShrinkPaddle, SpeedUp, SlowDown, ExtraLife, MultiBall (опционально)
- Класс PowerUpTypeData с характеристиками:
  - Type (PowerUpType)
  - Duration (float) - длительность эффекта в секундах
  - Color (ConsoleColor) - цвет для отрисовки
  - Symbol (char) - символ для отрисовки
  - Description (string) - описание эффекта
- Статический метод GetData(PowerUpType type) возвращающий данные

### 2. Models/PowerUp.cs
Полная реализация падающего бонуса:
- Свойства: Position, Type, FallSpeed, IsActive, Width, Height
- Конструктор с параметрами
- Метод Update(float deltaTime) - падение вниз со скоростью FallSpeed
- Метод Apply(GameObjects gameObjects) - применение эффекта к объектам:
  - ExpandPaddle: увеличить ширину платформы
  - ShrinkPaddle: уменьшить ширину платформы
  - SpeedUp: увеличить скорость мяча
  - SlowDown: уменьшить скорость мяча
  - ExtraLife: добавить жизнь (передать через параметр или событие)
- Метод GetBounds() - для проверки коллизий
- Метод GetTypeData() - получить данные типа

### 3. Services/InputHandler.cs
Класс обработки ввода с клавиатуры:
- Метод GetPaddleDirection() -> float:
  - Возвращает -1 для движения влево (←)
  - Возвращает +1 для движения вправо (→)
  - Возвращает 0 если клавиши не нажаты
  - Использовать Console.KeyAvailable и Console.ReadKey(true)
- Метод IsPausePressed() -> bool (Space или P)
- Метод IsQuitPressed() -> bool (Esc)
- Метод IsStartPressed() -> bool (Enter)
- Неблокирующее чтение клавиш

### 4. Services/ConsoleRenderer.cs
Класс отрисовки в консоли:
- Метод Render(GameObjects objects, int lives, int score, int level):
  - Очистить экран (Console.Clear() или буферизация)
  - Отрисовать все объекты
  - Отрисовать UI
- Метод DrawBall(Ball ball):
  - Символ 'O' или '●'
  - Цвет: White
- Метод DrawPaddle(Paddle paddle):
  - Символы '═══' по ширине платформы
  - Цвет: Cyan
- Метод DrawBrick(Brick brick):
  - Использовать brick.GetSymbol() и brick.GetCurrentColor()
  - Отрисовка прямоугольника по размеру кирпича
- Метод DrawPowerUp(PowerUp powerUp):
  - Использовать Symbol из PowerUpTypeData
  - Использовать Color из PowerUpTypeData
- Метод DrawUI(int lives, int score, int level):
  - Верхняя строка: "Lives: X | Score: XXX | Level: X"
  - Цвет: Yellow
- Метод DrawMenu():
  - Название игры
  - Инструкции по управлению
  - "Нажмите Enter для старта"
- Метод DrawGameOver(bool isVictory, int finalScore):
  - Если победа: "VICTORY!"
  - Если проигрыш: "GAME OVER"
  - Финальный счет
  - "Нажмите Enter для выхода"
- Использовать Console.SetCursorPosition(x, y) для отрисовки без мерцания

### 5. Services/ScoreManager.cs
Класс управления очками:
- Свойства: Score (int), HighScore (int)
- Метод AddScore(int points) - добавить очки
- Метод Reset() - сброс счета
- Метод SaveHighScore() - сохранение в файл "highscore.txt"
- Метод LoadHighScore() - загрузка из файла
- Конструктор загружает рекорд автоматически

### 6. Services/SoundManager.cs
Упрощенный класс для звуков:
- Свойство IsSoundEnabled (bool) - включены ли звуки
- Метод PlayBounce() - Console.Beep(800, 100)
- Метод PlayBrickDestroy() - Console.Beep(600, 150)
- Метод PlayPowerUp() - Console.Beep(1000, 200)
- Метод PlayGameOver() - Console.Beep(300, 500)
- Метод PlayLevelComplete() - последовательность из 3 звуков
- Все методы проверяют IsSoundEnabled перед воспроизведением
- Обернуть Beep в try-catch (может не работать на некоторых системах)

### 7. program.cs
Точка входа с полным игровым циклом:
```csharp
using Arkanoid.Core;
using Arkanoid.Models;
using Arkanoid.Services;

namespace Arkanoid
{
    class Program
    {
        static void Main(string[] args)
        {
            // Настройка консоли
            Console.CursorVisible = false;
            Console.SetWindowSize(80, 30);
            
            // Создание объектов
            var renderer = new ConsoleRenderer();
            var input = new InputHandler();
            var scoreManager = new ScoreManager();
            var soundManager = new SoundManager();
            
            // Игровые объекты (пока без GameEngine - его делает Дима)
            var gameObjects = new GameObjects();
            var field = new GameField();
            
            // TODO: Когда Дима реализует GameEngine, заменить на:
            // var gameEngine = new GameEngine(gameObjects, field, scoreManager, soundManager);
            // gameEngine.Run();
            
            // Пока временный код для демонстрации
            renderer.DrawMenu();
            Console.ReadKey();
            
            Console.WriteLine("Часть Кости реализована!");
            Console.WriteLine("Ожидается реализация GameEngine от Димы для запуска игры");
            Console.ReadKey();
        }
    }
}
```

**ВАЖНЫЕ ТРЕБОВАНИЯ:**
1. Все комментарии на русском языке БЕЗ эмодзи
2. Все Console.WriteLine БЕЗ эмодзи
3. Использовать namespace Arkanoid.Models, Arkanoid.Services
4. Код должен компилироваться: `dotnet build`
5. Изучить готовые классы Никиты для понимания структуры (особенно BrickTypeData как пример)

**ПОСЛЕ РЕАЛИЗАЦИИ:**
1. Проверить компиляцию: `dotnet build`
2. Закоммитить: `git add . && git commit -m "Реализована часть Кости: интерфейс, ввод/вывод и бонусы"`
3. Запушить: `git push`

Начни с реализации всех файлов по порядку. Используй готовые классы Никиты как примеры хорошего кода.

---

## КОНЕЦ ПРОМПТА

После вставки этого промпта AI должен реализовать всю твою часть автоматически.

