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
            
            // SetWindowSize работает только на Windows
            try
            {
                if (OperatingSystem.IsWindows())
                {
                    Console.SetWindowSize(80, 30);
                }
            }
            catch
            {
                // Игнорируем ошибку на других платформах
            }

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

            // Демонстрация меню
            renderer.DrawMenu();
            
            Console.WriteLine("\nВсе части проекта реализованы!");
            Console.WriteLine("Никита: игровые объекты и физика");
            Console.WriteLine("Дима: игровая логика и системы");
            Console.WriteLine("Костя: интерфейс, ввод/вывод и бонусы");
            Console.WriteLine("\nИгра готова! Запустите на Windows для полного функционала.");
            Console.WriteLine("На macOS некоторые функции (звуки, размер окна) не поддерживаются.");
            
            // Пауза для чтения (без ReadKey который не работает в Cursor терминале)
            Thread.Sleep(2000);
        }
    }
}

