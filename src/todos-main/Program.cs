using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace todos_main
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine(DateTimeOffset.Now.ToUnixTimeMilliseconds());
        }
    }
}