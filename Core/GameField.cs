namespace Arkanoid.Core
{
    using Arkanoid.Models;

    /// <summary>
    /// Класс игрового поля
    /// Определяет размеры и границы игровой области
    /// </summary>
    public class GameField
    {
        /// <summary>
        /// Ширина игрового поля
        /// </summary>
        public float Width { get; private set; }

        /// <summary>
        /// Высота игрового поля
        /// </summary>
        public float Height { get; private set; }

        /// <summary>
        /// Левая граница поля
        /// </summary>
        public float Left { get; private set; }

        /// <summary>
        /// Верхняя граница поля
        /// </summary>
        public float Top { get; private set; }

        /// <summary>
        /// Правая граница поля
        /// </summary>
        public float Right { get; private set; }

        /// <summary>
        /// Нижняя граница поля
        /// </summary>
        public float Bottom { get; private set; }

        /// <summary>
        /// Стандартная ширина поля
        /// </summary>
        public const float DefaultWidth = 80f;

        /// <summary>
        /// Стандартная высота поля
        /// </summary>
        public const float DefaultHeight = 30f;

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="width">Ширина поля</param>
        /// <param name="height">Высота поля</param>
        public GameField(float width, float height)
        {
            Width = width;
            Height = height;
            Left = 0;
            Top = 0;
            Right = width;
            Bottom = height;
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public GameField()
        {
            Width = DefaultWidth;
            Height = DefaultHeight;
            Left = 0;
            Top = 0;
            Right = Width;
            Bottom = Height;
        }

        /// <summary>
        /// Проверяет, находится ли точка за пределами поля
        /// </summary>
        /// <param name="position">Позиция для проверки</param>
        /// <returns>True если позиция за пределами поля</returns>
        public bool IsOutOfBounds(Vector2 position)
        {
            return position.X < Left || position.X > Right ||
                   position.Y < Top || position.Y > Bottom;
        }

        /// <summary>
        /// Проверяет, находится ли точка за левой границей
        /// </summary>
        /// <param name="x">X координата</param>
        /// <returns>True если за левой границей</returns>
        public bool IsOutOfLeftBound(float x)
        {
            return x < Left;
        }

        /// <summary>
        /// Проверяет, находится ли точка за правой границей
        /// </summary>
        /// <param name="x">X координата</param>
        /// <returns>True если за правой границей</returns>
        public bool IsOutOfRightBound(float x)
        {
            return x > Right;
        }

        /// <summary>
        /// Проверяет, находится ли точка за верхней границей
        /// </summary>
        /// <param name="y">Y координата</param>
        /// <returns>True если за верхней границей</returns>
        public bool IsOutOfTopBound(float y)
        {
            return y < Top;
        }

        /// <summary>
        /// Проверяет, находится ли точка за нижней границей
        /// </summary>
        /// <param name="y">Y координата</param>
        /// <returns>True если за нижней границей</returns>
        public bool IsOutOfBottomBound(float y)
        {
            return y > Bottom;
        }

        /// <summary>
        /// Ограничивает позицию в пределах поля
        /// </summary>
        /// <param name="position">Позиция для ограничения</param>
        /// <returns>Позиция в пределах поля</returns>
        public Vector2 ClampPosition(Vector2 position)
        {
            float clampedX = Math.Clamp(position.X, Left, Right);
            float clampedY = Math.Clamp(position.Y, Top, Bottom);
            return new Vector2(clampedX, clampedY);
        }

        /// <summary>
        /// Ограничивает X координату в пределах поля
        /// </summary>
        /// <param name="x">X координата</param>
        /// <returns>X в пределах поля</returns>
        public float ClampX(float x)
        {
            return Math.Clamp(x, Left, Right);
        }

        /// <summary>
        /// Ограничивает Y координату в пределах поля
        /// </summary>
        /// <param name="y">Y координата</param>
        /// <returns>Y в пределах поля</returns>
        public float ClampY(float y)
        {
            return Math.Clamp(y, Top, Bottom);
        }

        /// <summary>
        /// Получает центр игрового поля
        /// </summary>
        /// <returns>Позиция центра</returns>
        public Vector2 GetCenter()
        {
            return new Vector2(Width / 2, Height / 2);
        }

        /// <summary>
        /// Проверяет, пересекается ли прямоугольник с границами поля
        /// </summary>
        /// <param name="position">Позиция левого верхнего угла</param>
        /// <param name="width">Ширина прямоугольника</param>
        /// <param name="height">Высота прямоугольника</param>
        /// <returns>True если есть пересечение</returns>
        public bool IntersectsWithBounds(Vector2 position, float width, float height)
        {
            return !(position.X + width < Left || position.X > Right ||
                     position.Y + height < Top || position.Y > Bottom);
        }

        /// <summary>
        /// Получает информацию о поле в виде строки
        /// </summary>
        public override string ToString()
        {
            return $"GameField({Width}x{Height})";
        }
    }
}

