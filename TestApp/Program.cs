using System;
using System.Linq;
using DataModel;

namespace TestApp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (var db = new EfContext())
            {
                Console.Write("New product Name: ");
                var name = Console.ReadLine();
                Console.Write("New product Desc: ");
                var desc = Console.ReadLine();
                /*Console.Write("New product Type Id: ");
                var type = Console.ReadLine();*/

                Product prod = new Product
                {
                    ProductName = name,
                    ProductDesc = desc
                };
                db.Products.Add(prod);
                db.SaveChanges();

                var query = from p in db.Products
                    orderby p.ProductName
                    select p;

                Console.WriteLine("All Products:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.ProductName);
                }
            }

            Console.WriteLine("Press the Any Key to exit...");
            Console.ReadKey();
        }
    }
}