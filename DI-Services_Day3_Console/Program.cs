using System;
using System.Text;
using DI_Services_Day3_Console.Data;
using DI_Services_Day3_Console.BusinessLogic;

namespace DI_Services_Day3_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //format encoding 
            Console.OutputEncoding = Encoding.UTF8;
            //Process     

            //Constructor Injection - sử dụng nguyên tắc DI (Dependency Injection)
            SVBusinessLogic svBL = new SVBusinessLogic(new DataAccessList());
        }
    }
}
