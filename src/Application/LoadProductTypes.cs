using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryCo.Application
{
    public class LoadProductTypes
    {
        private List<string> _productTypeList;
        private List<string> _defaultProductTypeList = new List<string>
        {
            "Apples",
            "Bacon",
            "Bananas",
            "Bread",
            "Cake",
            "Candy",
            "Carrots",
            "Celery",
            "Chocolate",
            "Coke",
            "Cookies",
            "Crackers",
            "Donuts",
            "Eggs",
            "Gatorade",
            "Ginger",
            "Grapes",
            "Honey",
            "Ice_Cream",
            "Ketchup",
            "Onion",
            "Orange",
            "Other",
            "Pizza",
            "Spinach",
            "Wine",
            "Yogurt",
            "Zucchini"
        };

        public LoadProductTypes()
        {
            _productTypeList = _defaultProductTypeList;
        }

        public LoadProductTypes(List<string> productList)
        {
            _productTypeList = productList;
        }

        public void LoadProductTypesToDB()
        {

        }
    }
}
