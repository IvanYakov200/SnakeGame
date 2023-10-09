using System;
using static System.Console;
//14:32
namespace SnakeGame
{
    class Program
    {
        private const int MapWidth = 30; //Настраиваем окно приложения. Создаем пару глобальных переменных. Ширина карты
        private const int MapHeight = 20; //Высота карты
        private const int ScreenWidth = MapWidth * 4; //Переменные, котоорые отвечают за размер экрана консоли
        private const int ScreenHeight = MapHeight * 4; //Размер карты умноженный на 3

        private const ConsoleColor BorderColor = ConsoleColor.Gray; //глобальная переменная, которая содержит в себе цвет для бортика

        private const ConsoleColor HeadColor = ConsoleColor.DarkRed; //глобальная переменная, которая содержит в себе цвет головы змеи
        private const ConsoleColor BodyrColor = ConsoleColor.White; //глобальная переменная, которая содержит в себе цвет тела змеи

        static void Main()
        {
            SetWindowSize(ScreenWidth, ScreenHeight); //Устанавливаем размер окна
            SetBufferSize(ScreenWidth, ScreenHeight);
            CursorVisible = false; //Выключаем курсор

            DrowBorder();

            var snake = new Snake(10, 5, HeadColor, BodyrColor); //Создаем экземпляр змеи в нашей программе

            while (true) //Для теста метода используется бесконечный цикл
                         // в котором змея будет двигаться вправо с перерывом 200 секунд
            {
                snake.Move(Direction.Right);

                Thread.Sleep(200);
            }

            ReadKey();

        }

        static void DrowBorder()                    //Применим структуру Pixel для отображения бортиков в нашей игре

        {
            for (int i = 0; i < MapWidth; i++)
            {
                new Pixel(i, 0, BorderColor).Draw(); //Создаем объекты пикселя, передаем координа и цвет
                new Pixel(i, MapHeight-1, BorderColor).Draw();
            }

            for (int i = 0; i < MapHeight; i++)
            {
                new Pixel(0, i, BorderColor).Draw(); 
                new Pixel(MapWidth-1, i, BorderColor).Draw();
            }
        }

        

    }
}