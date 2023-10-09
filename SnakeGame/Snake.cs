using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Пшш Пшш... Проверка... Проверка...
namespace SnakeGame
{
    public class Snake
    {
        private readonly ConsoleColor headColor;
        private readonly ConsoleColor bodyColor;

        public Snake(int initialX, int initialY, ConsoleColor HeadColor, ConsoleColor BodyColor, int bodyLength = 3) //принимает начальное положение головы и тела, цвета для головы и тела, а также начальный размер тела
        {
            headColor = HeadColor;
            bodyColor = BodyColor;

            Head = new Pixel(initialX, initialY, headColor);  //Инициализируем голову

            for (int i = bodyLength; i >= 0; i--) //Цикл для тела, в котором мы добавляем пиксели начиная с хвоста. Проходим от длины тела до нуля и дикрементируем i
            {
                Body.Enqueue(new Pixel(Head.X - i - 1, initialY, bodyColor)); //Вызываем метод Enqueue для того, чтобы добавить метод в очередь
            }

            Draw();
        }
        public Pixel Head { get; private set;  } //пиксель головы 

        
        public Queue<Pixel> Body { get; } = new Queue<Pixel>(); //очерель для тела

        public void Draw() //метод отрисовки
        {
            Head.Draw();

            foreach (Pixel pixel in Body)
            {
                pixel.Draw();
            }
        }

        public void Clear() //метод отчистки
        {
            Head.Clear();

            foreach (Pixel pixel in Body)
            {
                pixel.Clear();
            }
        }
    }
}
