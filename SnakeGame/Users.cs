using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeConsole
{
    internal class Users
    {
        public string Name { get; }

        public int Score { get; }

        public Users(string name, int score)
        {
            Name = name;
            Score = score;
        }

        public override string ToString()
        {
            return $"NickName: {Name}, Record: {Score}";
        }
    }
}
