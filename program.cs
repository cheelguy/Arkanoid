using Arkanoid.Core;
using Arkanoid.Models;
using Arkanoid.Services;

namespace Arkanoid
{
    /// <summary>
    /// Главный класс программы - точка входа
    /// </summary>
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

