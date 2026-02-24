using System;

namespace PracticalWork4
{
    // Основной класс Triangle
    public class Triangle
    {
        // Закрытые поля
        private double _a;
        private double _b;
        private double _c;

        // Свойства с проверкой
        public double A
        {
            get { return _a; }
            set
            {
                if (value > 0)
                    _a = value;
                else
                    throw new ArgumentException("Сторона должна быть положительной.");
            }
        }

        public double B
        {
            get { return _b; }
            set
            {
                if (value > 0)
                    _b = value;
                else
                    throw new ArgumentException("Сторона должна быть положительной.");
            }
        }

        public double C
        {
            get { return _c; }
            set
            {
                if (value > 0)
                    _c = value;
                else
                    throw new ArgumentException("Сторона должна быть положительной.");
            }
        }

        // Конструктор по умолчанию (египетский треугольник)
        public Triangle()
        {
            _a = 3;
            _b = 4;
            _c = 5;
        }

        // Конструктор с параметрами
        public Triangle(double a, double b, double c)
        {
            // Проверка на существование треугольника
            if (a + b > c && a + c > b && b + c > a)
            {
                _a = a;
                _b = b;
                _c = c;
            }
            else
            {
                throw new ArgumentException("Треугольник с такими сторонами не существует.");
            }
        }

        // Конструктор копирования
        public Triangle(Triangle other)
        {
            _a = other._a;
            _b = other._b;
            _c = other._c;
        }

        // Метод проверки существования треугольника
        public bool IsValid()
        {
            return _a + _b > _c && _a + _c > _b && _b + _c > _a;
        }

        // Вычисление периметра
        public double GetPerimeter()
        {
            return _a + _b + _c;
        }

        // Вычисление площади по формуле Герона
        public double GetArea()
        {
            if (!IsValid()) return 0;
            double p = GetPerimeter() / 2;
            return Math.Sqrt(p * (p - _a) * (p - _b) * (p - _c));
        }

        // Перегрузка метода Equals для сравнения треугольников по площади
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Triangle other = (Triangle)obj;
            return Math.Abs(GetArea() - other.GetArea()) < 1e-9; // сравнение с точностью
        }

        // Переопределение GetHashCode (требуется при переопределении Equals)
        public override int GetHashCode()
        {
            return GetArea().GetHashCode();
        }

        // Перегрузка метода ToString для удобного вывода
        public override string ToString()
        {
            return $"Треугольник со сторонами a={_a:F2}, b={_b:F2}, c={_c:F2}, периметр={GetPerimeter():F2}, площадь={GetArea():F2}";
        }

        // Статический метод сравнения двух треугольников по площади
        public static int CompareByArea(Triangle t1, Triangle t2)
        {
            double area1 = t1.GetArea();
            double area2 = t2.GetArea();
            if (area1 < area2) return -1;
            if (area1 > area2) return 1;
            return 0;
        }

        // Рекурсивный метод вычисления факториала (демонстрация рекурсии)
        public static long Factorial(int n)
        {
            if (n < 0) throw new ArgumentException("Факториал отрицательного числа не определен.");
            if (n == 0 || n == 1) return 1;
            return n * Factorial(n - 1);
        }

        // Вложенный класс для сортировки по убыванию площади
        public class TriangleComparer : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                Triangle t1 = x as Triangle;
                Triangle t2 = y as Triangle;
                if (t1 == null || t2 == null)
                    throw new ArgumentException("Объекты не являются треугольниками.");
                // Сортировка по убыванию площади
                return -CompareByArea(t1, t2);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Практическая работа №4. Вариант 2");
            Console.WriteLine("Демонстрация работы с классом Triangle\n");

            // 1. Создание объектов разными конструкторами
            Console.WriteLine("1. Создание треугольников:");
            Triangle t1 = new Triangle(); // по умолчанию
            Triangle t2 = new Triangle(5, 6, 7); // с параметрами
            Triangle t3 = new Triangle(t2); // копирование

            Console.WriteLine(t1);
            Console.WriteLine(t2);
            Console.WriteLine(t3);

            // 2. Проверка на существование
            Console.WriteLine("\n2. Проверка существования треугольника со сторонами 1,2,3:");
            try
            {
                Triangle tInvalid = new Triangle(1, 2, 3);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            // 3. Сравнение треугольников по площади через Equals
            Console.WriteLine("\n3. Сравнение треугольников t2 и t3 по площади (Equals):");
            Console.WriteLine($"t2 == t3 ? {t2.Equals(t3)}"); // должны быть равны, т.к. t3 - копия t2

            Triangle t4 = new Triangle(3, 4, 5); // площадь 6
            Triangle t5 = new Triangle(5, 5, 6); // площадь 12
            Console.WriteLine($"t4 площадь = {t4.GetArea():F2}, t5 площадь = {t5.GetArea():F2}");
            Console.WriteLine($"t4.Equals(t5) ? {t4.Equals(t5)}");

            // 4. Статический метод сравнения
            Console.WriteLine("\n4. Сравнение площадей через статический метод CompareByArea:");
            int comp = Triangle.CompareByArea(t4, t5);
            if (comp < 0) Console.WriteLine("Площадь t4 меньше площади t5");
            else if (comp > 0) Console.WriteLine("Площадь t4 больше площади t5");
            else Console.WriteLine("Площади равны");

            // 5. Сортировка массива треугольников с помощью вложенного компаратора
            Console.WriteLine("\n5. Сортировка массива треугольников по убыванию площади:");
            Triangle[] triangles = { t1, t2, t4, t5 };
            Console.WriteLine("Исходный массив:");
            foreach (var t in triangles)
                Console.WriteLine($"   {t}");

            Array.Sort(triangles, new Triangle.TriangleComparer());
            Console.WriteLine("После сортировки по убыванию площади:");
            foreach (var t in triangles)
                Console.WriteLine($"   {t}");

            // 6. Демонстрация рекурсивного метода (факториал)
            Console.WriteLine("\n6. Рекурсивное вычисление факториала:");
            Console.Write("Введите целое неотрицательное число для вычисления факториала: ");
            if (int.TryParse(Console.ReadLine(), out int n))
            {
                try
                {
                    long fact = Triangle.Factorial(n);
                    Console.WriteLine($"Факториал {n}! = {fact}");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Некорректный ввод.");
            }

            Console.WriteLine("\nПрограмма завершена.");
        }
    }
}