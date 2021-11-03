using System;

namespace Topologie
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Gib mir MathML Input");
            DatenHandler daten = new DatenHandler(Console.ReadLine());
            daten.GenerateTopologie();
            Console.ReadKey();
        }
        static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }
    }
}
