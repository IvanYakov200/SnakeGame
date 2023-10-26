using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;
using Microsoft.Data.Sqlite;

namespace SnakeConsole
{
    class Program
    {
        private const int MapWidth = 30;
        private const int MapHeight = 20;

        private const int ScreenWidth = MapWidth * 3;
        private const int ScreenHeight = MapHeight * 3;

        private const int FrameMilliseconds = 200;

        private const ConsoleColor BorderColor = ConsoleColor.Gray;

        private const ConsoleColor FoodColor = ConsoleColor.Green;

        private const ConsoleColor BodyColor = ConsoleColor.Cyan;
        private const ConsoleColor HeadColor = ConsoleColor.DarkBlue;

        private static readonly Random Random = new Random();

        private static SqliteConnection connection = new SqliteConnection("Data Source=\"C:\\Users\\andre\\OneDrive\\Рабочий стол" +
            "\\C#\\SnakeConsole\\SnakeDB\"");


        static int Main()
        {
            SetWindowSize(ScreenWidth, ScreenHeight);
            SetBufferSize(ScreenWidth, ScreenHeight);
            CursorVisible = false;

            while (true)
            {
                switch (Menu())
                {
                    case 1:
                        {
                            StartGame();
                            Thread.Sleep(700);
                            Console.Clear();
                            break;
                        }
                    case 2:
                        {
                            var Leaders = DataBase.LeaderBoard(connection);
                            if (Leaders.Count == 0)
                            {
                                Console.WriteLine("Таблица лидеров пуста!");
                            }
                            else
                            for(int i = 0; i < Leaders.Count;i++)
                            {
                                Console.WriteLine(Leaders[i]);
                            }
                            break;
                        }
                    case 0: 
                        {
                            return 0;
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Введите значение соответсвующее Меню!");
                            break;
                        }
                }
            }
        }

        static void StartGame()
        {
            int score = 0;

            Clear();
            DrawBoard();

            Snake snake = new Snake(10, 5, HeadColor, BodyColor);

            Pixel food = GenFood(snake);
            food.Draw();

            Direction currentMovement = Direction.Right;

            int lagMs = 0;
            var sw = new Stopwatch();

            while (true)
            {
                sw.Restart();
                Direction oldMovement = currentMovement;

                while (sw.ElapsedMilliseconds <= FrameMilliseconds - lagMs)
                {
                    if (currentMovement == oldMovement)
                        currentMovement = ReadMovement(currentMovement);
                }

                sw.Restart();

                if (snake.Head.X == food.X && snake.Head.Y == food.Y)
                {
                    snake.Move(currentMovement, true);
                    food = GenFood(snake);
                    food.Draw();

                    score++;

                    Task.Run(() => Beep(1200, 200));
                }
                else
                {
                    snake.Move(currentMovement);
                }

                if (snake.Head.X == MapWidth - 1
                    || snake.Head.X == 0
                    || snake.Head.Y == MapHeight - 1
                    || snake.Head.Y == 0
                    || snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
                    break;

                lagMs = (int)sw.ElapsedMilliseconds;
            }

            snake.Clear();
            food.Clear();

            SetCursorPosition(ScreenWidth / 3, ScreenHeight / 2);
            WriteLine($"Game over, Score: {score}");

            Task.Run(() => Beep(200, 600));

            Thread.Sleep(1000);

            Clear();

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Введите ваш Nickname:");

            string NickName = Console.ReadLine();
            if(DataBase.AddUser(connection, NickName, score))
            {
                Console.WriteLine("Вы успешно добавлены в таблицу лидеров!");
            }
            else
            {
                Console.WriteLine("Упс! Что - то пошло не так!");
            }
        }

        static void DrawBoard()
        {
            for (int i = 0; i < MapWidth; i++)
            {
                new Pixel(i, 0, BorderColor).Draw();
                new Pixel(i, MapHeight - 1, BorderColor).Draw();
            }

            for (int i = 0; i < MapHeight; i++)
            {
                new Pixel(0, i, BorderColor).Draw();
                new Pixel(MapWidth - 1, i, BorderColor).Draw();
            }
        }

        static Pixel GenFood(Snake snake)
        {
            Pixel food;

            do
            {
                food = new Pixel(Random.Next(1, MapWidth - 2), Random.Next(1, MapHeight - 2), FoodColor);
            } while (snake.Head.X == food.X && snake.Head.Y == food.Y ||
                     snake.Body.Any(b => b.X == food.X && b.Y == food.Y));

            return food;
        }

        static Direction ReadMovement(Direction currentDirection)
        {
            if (!KeyAvailable)
                return currentDirection;

            ConsoleKey key = ReadKey(true).Key;

            currentDirection = key switch
            {
                ConsoleKey.UpArrow when currentDirection != Direction.Down => Direction.Up,
                ConsoleKey.DownArrow when currentDirection != Direction.Up => Direction.Down,
                ConsoleKey.LeftArrow when currentDirection != Direction.Right => Direction.Left,
                ConsoleKey.RightArrow when currentDirection != Direction.Left => Direction.Right,
                _ => currentDirection
            };

            return currentDirection;
        }

        public static int Menu()
        {
            Console.WriteLine("------Меню------");
            Console.WriteLine("1. Играть в змейку");
            Console.WriteLine("2. Таблица рекордов");
            Console.WriteLine("0. Выход");
            Console.WriteLine("------------------------");
            return GetValue();
        }
        public static int GetValue()
        {
            Console.WriteLine("Введите ваше число:");
            string input = Console.ReadLine();
            bool success = Int32.TryParse(input, out int result);
            if (success)
            {
                return result;
            }
            else
            {
                Console.WriteLine("Некорректное значение");
                return GetValue();
            }
        }
    }
}