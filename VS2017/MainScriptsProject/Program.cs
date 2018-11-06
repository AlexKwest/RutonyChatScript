using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainScriptsProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 200;
            Script script = new Script();
            script.RunScript("twith.tv", "hren", "500");
            script.RunScript("twith.tv", "alexkwest", "500");
            Console.ReadLine();
        }
    }

}
