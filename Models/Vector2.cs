namespace Arkanoid.Models
{
    /// <summary>
    /// Класс для работы с двумерными векторами
    /// Используется для представления позиций и скоростей в 2D пространстве
    /// </summary>
    public class Vector2
    {
        /// <summary>
        /// Координата X
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Координата Y
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Конструктор по умолчанию (0, 0)
        /// </summary>
        public Vector2()
        {
            X = 0;
            Y = 0;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Вычисляет длину вектора
        /// </summary>
        /// <returns>Длина вектора</returns>
        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        /// <summary>
        /// Нормализует вектор (делает его длину равной 1)
        /// </summary>
        /// <returns>Нормализованный вектор</returns>
        public Vector2 Normalize()
        {
            float length = Length();
            if (length > 0)
            {
                return new Vector2(X / length, Y / length);
            }
            return new Vector2(0, 0);
        }

        /// <summary>
        /// Скалярное произведение двух векторов
        /// </summary>
        /// <param name="other">Другой вектор</param>
        /// <returns>Результат скалярного произведения</returns>
        public float Dot(Vector2 other)
        {
            return X * other.X + Y * other.Y;
        }

        /// <summary>
        /// Оператор сложения векторов
        /// </summary>
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Оператор вычитания векторов
        /// </summary>
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Оператор умножения вектора на скаляр
        /// </summary>
        public static Vector2 operator *(Vector2 v, float scalar)
        {
            return new Vector2(v.X * scalar, v.Y * scalar);
        }

        /// <summary>
        /// Оператор умножения скаляра на вектор
        /// </summary>
        public static Vector2 operator *(float scalar, Vector2 v)
        {
            return new Vector2(v.X * scalar, v.Y * scalar);
        }

        /// <summary>
        /// Оператор деления вектора на скаляр
        /// </summary>
        public static Vector2 operator /(Vector2 v, float scalar)
        {
            if (scalar != 0)
            {
                return new Vector2(v.X / scalar, v.Y / scalar);
            }
            return new Vector2(0, 0);
        }

        /// <summary>
        /// Создает копию вектора
        /// </summary>
        public Vector2 Clone()
        {
            return new Vector2(X, Y);
        }

        /// <summary>
        /// Строковое представление вектора
        /// </summary>
        public override string ToString()
        {
            return $"({X:F2}, {Y:F2})";
        }
    }
}

