extern alias Lib1;
using System;
using Lib1::SharedSrc;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Constants.Key);
        }
    }
}
