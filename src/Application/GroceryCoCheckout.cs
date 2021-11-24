using GroceryCo.Application.Processors;
using GroceryCo.Domain;
using GroceryCo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Application
{
    public class GroceryCoCheckout
    {
        // Main Grocery Co Datatbase that holds all tables
        private GroceryCoDatabase _gcDatabase;
        private DiscountProcessor _discountProcessor;
        public GroceryCoCheckout()
        {
            _gcDatabase = new GroceryCoDatabase();
            _discountProcessor = new DiscountProcessor();
        }

        // Main checkout process for Products and Discounts
        public void BeginCheckoutProcess()
        {
            ProductTypeMenu();

            Console.WriteLine();
            List<ProductTypeEntity> productTypeEntities = MapTableToEntity.MapToProductTypeEntity(_gcDatabase.ProductTypeTable);
            ProductMenu(productTypeEntities);

            Console.WriteLine();
            MainMenu();
        }

        private void MainMenu()
        {
            bool showMainMenu = true;

            // If any errors, menu will show again
            while (showMainMenu)
            {
                bool resetCheckout = false;
                int selection = DisplayMenu(_mainMenu, 3);

                switch (selection)
                {
                    // Admin Menu
                    case 0:
                        AdminMenu();
                        break;

                    // Product Menu
                    case 1:
                        List<ProductTypeEntity> productTypeEntities = MapTableToEntity.MapToProductTypeEntity(_gcDatabase.ProductTypeTable);
                        ProductMenu(productTypeEntities);
                        break;

                    // Checkout
                    case 2:
                        CheckoutMenu();
                        showMainMenu = false;
                        resetCheckout = true;
                        break;
                }

                if (resetCheckout)
                {
                    // Reset
                    _gcDatabase = new GroceryCoDatabase();
                    _discountProcessor = new DiscountProcessor(); _gcDatabase.ProductTypeTable.Clear();
                    BeginCheckoutProcess();
                }
            }
        }

        private void AdminMenu()
        {
            bool goBack = false;
            bool showAdminMenu = true;

            // If any errors, menu will show again
            while (showAdminMenu)
            {
                int selection = DisplayMenu(_adminMenu, 3);

                switch (selection)
                {
                    // Discount Menu
                    case 0:
                        List<ProductTypeEntity> productTypeEntities = MapTableToEntity.MapToProductTypeEntity(_gcDatabase.ProductTypeTable);
                        DiscountMenu(productTypeEntities);
                        break;

                    // Product Type Menu
                    case 1:
                        ProductTypeModifyMenu();
                        break;

                    // Go Back
                    case 2:
                        showAdminMenu = false;
                        MainMenu();
                        break;
                }
            }
        }

        private void CheckoutMenu()
        {
            CheckoutCart cart = new CheckoutCart();

            List<ProductTypeEntity> productTypes = MapTableToEntity.MapToProductTypeEntity(_gcDatabase.ProductTypeTable);
            List<ProductEntity> products = MapTableToEntity.MapToProductEntity(_gcDatabase.ProductTable);
            List<DiscountEntity> discounts = MapTableToEntity.MapToDiscountEntity(_gcDatabase.DiscountDataTable).Where(d => d.StartDate <= DateTime.Now && DateTime.Now <= d.EndDate).ToList();

            foreach (ProductEntity entity in products)
            {
                ProductTypeEntity ptEntity = productTypes.FirstOrDefault(p => p.UPC == entity.UPC);
                DiscountEntity dEntity = discounts.FirstOrDefault(d => d.UPC == entity.UPC);

                if (dEntity != null)
                {
                    if (dEntity.DiscountType == DiscountTypeIDs.PercentOff)
                    {
                        for (int i = 0; i < entity.Quantity; i++)
                        {
                            cart.Groceries.Add(new CartEntity() { UPC = entity.UPC, ProductName = ptEntity.ProductName, Price = Math.Round(ptEntity.Price * (dEntity.DiscountPercentage / 100), 2) });
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    for (int i = 0; i < entity.Quantity; i++)
                    {
                        cart.Groceries.Add(new CartEntity() { UPC = entity.UPC, ProductName = ptEntity.ProductName, Price = ptEntity.Price });
                    }
                }
            }

            bool showCheckoutMenu = true;

            // If any errors, menu will show again
            while (showCheckoutMenu)
            {
                int selection = DisplayMenu(_checkoutMenu, 2);

                if (selection == 0)
                {
                    
                }
                else
                {
                    MainMenu();
                    break;
                }

            }
        }

        private void ProductTypeMenu()
        {
            bool showProductTypeMenu = true;
            ProductTypeProcessor processor = null;

            // If any errors, menu will show again
            while (showProductTypeMenu)
            {
                processor = new ProductTypeProcessor();
                int selection = DisplayMenu(_productTypeMenu, 2);

                // Use file for product types
                if (selection == 1)
                {
                    Console.WriteLine("Please enter path for Product Types text file, see below for example of format:");
                    Console.WriteLine($"product1, 1.00{Environment.NewLine}product2, 1.50");
                    Console.Write("File Path: ");
                    processor.FilePath = Console.ReadLine();
                }
                // Modify / Manually add product types
                else
                {

                }

                showProductTypeMenu = !processor.ParseLines();
            }

            processor.MapToEntities();
            _gcDatabase.ProductTypeTable.Clear();
            processor.LoadEntityToDatabase(_gcDatabase.ProductTypeTable);
        }

        private void ProductTypeModifyMenu()
        {
            bool showProductTypeMenu = true;
            ProductTypeProcessor processor = null;

            // If any errors, menu will show again
            while (showProductTypeMenu)
            {
                processor = new ProductTypeProcessor();
                int selection = DisplayMenu(_productTypeModifyMenu, 2);

                // Add new product type
                switch (selection)
                {
                    // Add new product type
                    case 0:
                        Console.WriteLine("Please enter Product Name");
                        Console.Write("Name: ");
                        string productName = Console.ReadLine();
                        break;
                    
                    // Modify Product type
                    case 1:
                        List<ProductTypeEntity> productTypes = MapTableToEntity.MapToProductTypeEntity(_gcDatabase.ProductTypeTable);
                        foreach (ProductTypeEntity entity in productTypes)
                        {
                            Console.WriteLine($"{entity.UPC} {entity.ProductName} {entity.Price}");
                        }
                        bool inputRequired = true;
                        while (inputRequired)
                        {
                            Console.WriteLine("Select product number to modify:");
                            Console.Write("Product Number: ");
                            string upcString = Console.ReadLine();
                            int upc = -1;
                            if (int.TryParse(upcString, out upc) && productTypes.Any(p => p.UPC == upc))
                            {
                                inputRequired = false;

                                selection = DisplayMenu(_productTypeUpdateMenu, 3);

                                switch (selection)
                                {
                                    // Update price on product
                                    case 0:
                                        ProductTypeEntity updateEntity = productTypes.FirstOrDefault(p => p.UPC == upc);
                                        bool requiresPrice = true;
                                        
                                        while (requiresPrice)
                                        {
                                            string priceString = Console.ReadLine();
                                            decimal price = -1;

                                            Console.WriteLine($"Enter new price for product [{updateEntity.ProductName}].");
                                            priceString = Console.ReadLine();

                                            if (decimal.TryParse(priceString, out price))
                                            {
                                                updateEntity.Price = price;
                                                _gcDatabase.ProductTypeTable.Clear();
                                                MapEntityToDatabase.MapProductTypeEntityToTable(productTypes, _gcDatabase.ProductTypeTable);

                                                requiresPrice = false;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid price entered.");
                                            }
                                        }
                                        break;

                                    // Delete Product Type
                                    case 1:
                                        ProductTypeEntity deleteEntity = productTypes.FirstOrDefault(p => p.UPC == upc);
                                        productTypes.Remove(deleteEntity);

                                        _gcDatabase.ProductTypeTable.Clear();
                                        MapEntityToDatabase.MapProductTypeEntityToTable(productTypes, _gcDatabase.ProductTypeTable);
                                        break;

                                    // Go Back
                                    case 2:
                                        ProductTypeModifyMenu();
                                        break;
                                }                           
                            }
                            else
                            {
                                Console.WriteLine("Invalid product number entered, please try again.");
                            }
                        }

                        break;

                    // Go Back
                    case 2:
                        AdminMenu();
                        break;
                }

                showProductTypeMenu = !processor.ParseLines();
            }

            processor.MapToEntities();
            _gcDatabase.ProductTypeTable.Clear();
            processor.LoadEntityToDatabase(_gcDatabase.ProductTypeTable);
        }

        private void ProductMenu(List<ProductTypeEntity> productTypes)
        {
            bool showProductMenu = true;
            ProductProcessor processor = null;

            // If any errors, menu will show again
            while (showProductMenu)
            {
                processor = new ProductProcessor(productTypes);
                int selection = DisplayMenu(_productMenu, 2);

                if (selection == 0)
                {
                    Console.WriteLine("Please enter path for Product text file, see below for example of format:");
                    Console.WriteLine($"product1{Environment.NewLine}product2");
                    Console.Write("File Path: ");
                    processor.FilePath = Console.ReadLine();
                }

                showProductMenu = !processor.ParseLines();
            }

            processor.MapToEntities();
            processor.LoadEntityToDatabase(_gcDatabase.ProductTable);
        }

        private void DiscountMenu(List<ProductTypeEntity> productTypeEntities)
        {
            bool showDiscountMenu = true;
     
            // If any errors, menu will show again
            while (showDiscountMenu)
            {
                int selection = DisplayMenu(_discountMenu, 3);
                switch (selection)
                {
                    // Admin Menu
                    case 0:
                        Console.WriteLine($"{Environment.NewLine}Select item number for discount:");
                        foreach (ProductTypeEntity entity in productTypeEntities)
                        {
                            Console.WriteLine($"{entity.UPC} {entity.ProductName}");
                        }

                        Console.Write("Item Number: ");
                        int selectedValue = -1;
                        string input = Console.ReadLine();
                        int count = productTypeEntities.Count;

                        if (GroceryCoValidator.ValidateSelection(input, count - 1, out selectedValue))
                        {
                            Console.WriteLine($"{Environment.NewLine}Select Discount Type:");
                            foreach (DiscountTypeIDs value in Enum.GetValues(typeof(DiscountTypeIDs)))
                            {
                                Console.WriteLine($"{(int)value} {value.ToString()}");
                            }
                            Console.Write("Discount Type: ");
                            selectedValue = -1;
                            input = Console.ReadLine();

                            count = Enum.GetValues(typeof(DiscountTypeIDs)).Length;
                            if (GroceryCoValidator.ValidateSelection(input, count - 1, out selectedValue))
                            {
                                DiscountEntity entity = new DiscountEntity();
                                entity.DiscountType = (DiscountTypeIDs)selectedValue;

                                bool requiresInput = true;

                                switch (selectedValue)
                                {
                                    case (int)DiscountTypeIDs.PercentOff:
                                        requiresInput = true;
                                        while (requiresInput)
                                        {
                                            Console.WriteLine($"{Environment.NewLine}Please enter percentage off (1-100):");
                                            Console.Write("Percent off: %");
                                            input = Console.ReadLine();
                                            if (GroceryCoValidator.ValidatePercent(input, out selectedValue))
                                            {
                                                entity.DiscountPercentage = selectedValue;

                                                while (requiresInput)
                                                {
                                                    Console.WriteLine($"{Environment.NewLine}How many days will this discount be valid for?");
                                                    Console.Write("Number of Days: ");
                                                    input = Console.ReadLine();
                                                    if (GroceryCoValidator.ValidatePercent(input, out selectedValue))
                                                    {
                                                        entity.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                                                        entity.EndDate = entity.StartDate.AddDays(selectedValue + 1).AddSeconds(-1);
                                                        entity.DiscountPercentage = selectedValue;
                                                        requiresInput = false;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Number of days entered is not valid.");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Percent value entered was not a whole number between 1 to 100.");
                                            }
                                        }
                                        break;

                                    case (int)DiscountTypeIDs.BuyXGetY:
                                        requiresInput = true;
                                        while (requiresInput)
                                        {
                                            Console.WriteLine($"{Environment.NewLine}How many items required for discount:");
                                            Console.Write("Item count: ");
                                            input = Console.ReadLine();
                                            if (int.TryParse(input, out selectedValue))
                                            {
                                                entity.ItemsRequired = selectedValue;

                                                while (requiresInput)
                                                {
                                                    Console.WriteLine($"{Environment.NewLine}Please enter percentage off for discounted item (1-100):");
                                                    Console.Write("Percent off: %");
                                                    input = Console.ReadLine();
                                                    if (GroceryCoValidator.ValidatePercent(input, out selectedValue))
                                                    {
                                                        entity.DiscountPercentage = selectedValue;
                                                        requiresInput = false;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Value entered was not a whole number between 1 to 100.");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("A valid number was not entered, please ensure you enter a whole number.");
                                            }
                                        }
                                        break;
                                }

                                _discountProcessor.AddDiscount(entity);
                                showDiscountMenu = false;
                            }
                            else
                            {
                                Console.WriteLine($"{Environment.NewLine}Invalid value, please enter a value between 0 and {count - 1}.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{Environment.NewLine}Invalid value, please enter a value between 0 and {count - 1}.");
                        }
                    break;

                    // Modify Existing Discount
                    case 1:
                        int i = 0;
                        foreach (DiscountEntity entity in _discountProcessor.DiscountEntities)
                        {
                            Console.WriteLine($"{i} {productTypeEntities.FirstOrDefault(p => p.UPC == entity.UPC).ProductName} {entity.DiscountTypeID.ToString()}  Items Required: {entity.ItemsRequired} {entity.DiscountPercentage}% {entity.StartDate:yyyy-MM-dd} to {entity.EndDate:yyyy-MM-dd}");
                            i++;
                        }

                        bool requiresDeletionInput = true;
                        while (requiresDeletionInput)
                        {
                            Console.WriteLine($"{Environment.NewLine}Please select what discount you would like to remove:");
                            Console.Write("Number of Discount: ");
                            input = Console.ReadLine();
                            if (GroceryCoValidator.ValidateSelection(input, _discountProcessor.DiscountEntities.Count, out selectedValue))
                            {
                                _discountProcessor.DiscountEntities.RemoveAt(selectedValue);
                                requiresDeletionInput = false;
                            }
                            else
                            {
                                Console.WriteLine("Discount number not found.");
                            }
                        }

                        break;

                    // Go Back
                    case 2:
                        AdminMenu();
                        break;
                }
            }
            _gcDatabase.DiscountDataTable.Clear();
            _discountProcessor.MapEntitiesToTable(_gcDatabase.DiscountDataTable);
        }

        // Displays menu on console
        private int DisplayMenu(string menu, int numOfSelections)
        {
            bool invalidSelection = true;
            int selectedValue = -1;

            while (invalidSelection)
            {
                Console.WriteLine(menu);
                Console.Write("Selection: ");
                string selection = Console.ReadLine();

                if (GroceryCoValidator.ValidateSelection(selection, 3, out selectedValue))
                {
                    invalidSelection = false;
                }
                else
                {
                    Console.WriteLine($"{Environment.NewLine}Invalid value, please enter a value between 0 and {numOfSelections - 1}.");
                }
            }

            return selectedValue;
        }

        private static string _productTypeMenu = 
@"---------------------------------------
|  0 - Use Default Product Type List  |
|  1 - Use File for Product Type List |
---------------------------------------";

        private static string _productTypeModifyMenu =
@"------------------------------
|  0 - Add new Product Type  |
|  1 - Change Product Price  |
|  2 - Go Back               |
------------------------------";

        private static string _productTypeUpdateMenu =
@"----------------------
|  0 - Change Price  |
|  1 - Delete        |
|  2 - Go Back       |
----------------------";

        private static string _productMenu =
@"----------------------------------------------
|  0 - Use File for Grocery List             |
|  1 - Manually add Products to Grocery List |
----------------------------------------------";

        private static string _mainMenu =
 @"--------------------
|  0 - Admin Menu   |
|  1 - Product Menu |
|  2 - Checkout     |
---------------------";

        private static string _adminMenu =
@"--------------------------
|  0 - Discount Menu     |
|  1 - Product Type Menu |
|  2 - Go Back           |
--------------------------";

        private static string _discountMenu =
@"--------------------------------
|  0 - Add new Discount         |
|  1 - Delete existing Discount |
|  2 - Go Back                  |
---------------------------------";
        private static string _checkoutMenu =
@"------------------
|  0 - Pay Now   |
|  1 - Go Back   |
------------------";
    }
}
