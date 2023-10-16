using System;
using System.Diagnostics;
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

        private const int FrameMS = 200; //Глобальная переменная, которая отвечает за паузу между кадрами

        private const ConsoleColor BorderColor = ConsoleColor.Gray; //глобальная переменная, которая содержит в себе цвет для бортика

        private const ConsoleColor HeadColor = ConsoleColor.DarkRed; //глобальная переменная, которая содержит в себе цвет головы змеи
        private const ConsoleColor BodyColor = ConsoleColor.White; //глобальная переменная, которая содержит в себе цвет тела змеи

        private const ConsoleColor FoodColor = ConsoleColor.Green; //глобальная переменная, которая содержит в себе цвет еды


        private static readonly Random Random = new Random();

        static void Main()
        {
            SetWindowSize(ScreenWidth, ScreenHeight); //Устанавливаем размер окна
            SetBufferSize(ScreenWidth, ScreenHeight);
            CursorVisible = false; //Выключаем курсор
            
            while(true) //перезапускаем игру с перерывом через 1 секунду
            {
                StartGame();

                Thread.Sleep(1000);
                ReadKey();
            }

        }

        static void StartGame()
        {
            Clear();

            DrowBorder();

            Direction currentMovement = Direction.Right;

            var snake = new Snake(10, 5, HeadColor, BodyColor); //Создаем экземпляр змеи в нашей программе

            Pixel food = GenFood(snake);
            food.Draw();

            int score = 0; //переменная для количества очков

            
            Stopwatch sw = new Stopwatch();

            while (true) //Для теста метода используется бесконечный цикл
                         // в котором змея будет двигаться вправо с перерывом 200 секунд
            {
                sw.Restart();

                Direction oldMovement = currentMovement; //ограничение изменения направления движения несколько раз подряж ха 1 кадр


                while (sw.ElapsedMilliseconds <= FrameMS)
                {
                    if (currentMovement == oldMovement) //считываем направление только если оно не поменялось
                    {
                        currentMovement = ReadMovement(currentMovement);
                    }
                }

                if(snake.Head.X == food.X && snake.Head.Y == food.Y) //проверяе попала ли голова на место еды
                {
                    snake.Move(currentMovement, true);
                    food = GenFood(snake); //если попали на еду, генерируе новую еду
                    food.Draw();

                    score++;    
                }
                else
                {
                    snake.Move(currentMovement);
                };

                snake.Move(currentMovement);

                if (snake.Head.X == MapWidth - 1
                    || snake.Head.X == 0
                    || snake.Head.Y == MapHeight - 1
                    || snake.Head.Y == 0
                    || snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
                    break;//условие для проигрыша. он произойдет если змея упрется головой в борт или в свое тело
            }

            snake.Clear();

            SetCursorPosition(ScreenWidth / 3, ScreenHeight / 2);
            WriteLine($"Game over, score: {score}");
        }

        static Pixel GenFood(Snake snake) //метод еды возвращающий пиксель. Принимает змею, чтобы еда не генерировалась в самой змее
        {
            Pixel food;

            do
            {
                food = new Pixel(Random.Next(1, MapWidth - 2), Random.Next(1, MapHeight - 2), FoodColor);
            } while ( snake.Head.X == food.X && snake.Head.Y == food.Y
            || snake.Body.Any(b => b.X == food.X && b.Y == food.Y)); //Продолжать в том случае, если еда попала на положение головы либо на положение одного из пикселей тела

            return food;
        }

        static Direction ReadMovement(Direction CurrentDirection) //Метод для чтения управления с клавиатуры. Принимаем в него текущее направление
        {
            if (KeyAvailable) //Если ничего не нажато, возвращаем текущее направление
                return CurrentDirection;

            ConsoleKey key = ReadKey(true).Key; //Если нажато, считываем эту кнопку

            CurrentDirection = key switch //ограничение направления. Например если змея идет вправо,то ей нельзя налево
            {
                ConsoleKey.UpArrow when CurrentDirection != Direction.Down => Direction.Up, //Если наше движение не яляется движением вниз, то возвращаем движение в вверх
                ConsoleKey.DownArrow when CurrentDirection != Direction.Up => Direction.Down,
                ConsoleKey.LeftArrow when CurrentDirection != Direction.Right => Direction.Left,
                ConsoleKey.RightArrow when CurrentDirection != Direction.Left => Direction.Right,
                _ => CurrentDirection
            };

            return CurrentDirection;
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