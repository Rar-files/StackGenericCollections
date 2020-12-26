using System;

namespace Stos
{
    class Program
    {
        static void Main(string[] args)
        {
            StosWTablicy<string> s = new StosWTablicy<string>(2);
            s.Push("km");
            s.Push("aa");
            s.Push("xx");
            foreach (var x in s)
                Console.WriteLine(x);
            Console.WriteLine();
            var sl = new StosWLiscie<string>();
            sl.Push("km");
            sl.Push("aa");
            sl.Push("xx");
            foreach (var x in sl)
                Console.WriteLine(x);
            foreach (var x in sl.BottomToTop)
                Console.WriteLine(x);            
            Console.WriteLine();
        }
    }
}
