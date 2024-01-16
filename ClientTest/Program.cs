using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();

            // Вызываем синхронные методы напрямую
            var result1 = client.GetTestOK();
            var result2 = client.GetTest();

            Console.WriteLine(result1);
            Console.WriteLine(result2);
            Console.ReadKey();
        }
    }
}
