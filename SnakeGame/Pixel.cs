using System;
using static System.Console;


namespace SnakeGame
{
    public readonly struct Pixel //используем структуру так как это примитивный тип и все его свойства являются значимыми типами
    {
        private const char PixelChar = '█'; //ASCII символ
        public Pixel(int x, int y, ConsoleColor color, int pixelSize = 3) // инициализируем свойства через конструктор
        {
            X = x;
            Y = y;
            Color = color;
            PixelSize = pixelSize;
        }
        public int X { get; }
                                //Эта структура будет содержать координты X и Y 
        public int Y { get; } 
        public ConsoleColor Color { get; } //А также цвет пикселя

        public int PixelSize { get; }  //Размер пикселя

        public void Draw() //метод отрисовки пикселя 
        {
            ForegroundColor = Color; //Применяем цвет пикселя к курсору консоли 
            for (int x = 0; x < PixelSize; x++) //Двойной цикл для отрисовки большого пикселя 3на3
            {
                for (int y = 0; y < PixelSize; y++)
                {
                    Console.SetCursorPosition(left: X * PixelSize + x, top: Y * PixelSize + y); //При установке курсора делаем отступ равный произведению координа пикселя на его размер
                    Console.Write(PixelChar); //ASCII символ, который заполнит ячейку
                }
            }
        }

        public void Clear() //метод для отчистки от пикселя
        {

            for (int x = 0; x < PixelSize; x++) 
            {
                for (int y = 0; y < PixelSize; y++)
                {
                    Console.SetCursorPosition(left: X * PixelSize + x, top: Y * PixelSize + y); 
                    Console.Write(' '); 
                }
            }
        }
    }
}
