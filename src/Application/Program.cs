using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GroceryCo.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GroceryCoCheckout groceryCoCheckout = new GroceryCoCheckout();
            groceryCoCheckout.BeginCheckoutProcess();
        }
    }
}
