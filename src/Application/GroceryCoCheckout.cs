using GroceryCo.Application.Processors;
using GroceryCo.Domain;
using GroceryCo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GroceryCo.Application
{
    public class GroceryCoCheckout
    {
        // Main Grocery Co Datatbase that holds all tables
        private GroceryCoDatabase _gcDatabase;
        private DiscountProcessor _discountProcessor;
        public GroceryCoCheckout()
        {
            Database db = new Database();
            _gcDatabase = new GroceryCoDatabase(db);
            _discountProcessor = new DiscountProcessor();
        }

        // Main checkout process for Products and Discounts
        public void BeginCheckoutProcess()
        {
            ProductTypeMenu();

            List<ProductTypeEntity> productTypeEntities = MapTableToEntity.MapToProductTypeEntity(_gcDatabase.ProductTypeTable);
            ProductMenu(productTypeEntities);

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
                        ProductModifyMenu(productTypeEntities);
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
                    Database db = new Database();
                    _gcDatabase = new GroceryCoDatabase(db);
                    _discountProcessor = new DiscountProcessor(); _gcDatabase.ProductTypeTable.Clear();
                    BeginCheckoutProcess();
                }
            }
        }

        private void AdminMenu()
        {
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
                    Console.WriteLine("Please enter path for Product Types text file:");
                    Console.Write("File Path: ");
                    processor.FilePath = Console.ReadLine();
                }

                showProductTypeMenu = !processor.ParseLines();
            }

            Console.WriteLine($"File loaded successfully.{Environment.NewLine}");
            processor.MapToEntities();
            _gcDatabase.ProductTypeTable.Clear();
            processor.LoadEntityToDatabase(_gcDatabase.ProductTypeTable);
        }

        private void ProductTypeModifyMenu()
        {
            bool showProductTypeMenu = true;
            ProductTypeProcessor processor = new ProductTypeProcessor();
            processor.LoadProductListFromTable(_gcDatabase.ProductTypeTable);

            // If any errors, menu will show again
            while (showProductTypeMenu)
            {
                int selection = DisplayMenu(_productTypeModifyMenu, 2);

                // Add new product type
                switch (selection)
                {
                    // Add new product type
                    case 0:
                        bool newProduct = true;
                        while (newProduct)
                        {
                            Console.WriteLine("Please enter Product Name");
                            Console.Write("Name: ");
                            string productName = Console.ReadLine();

                            if (processor.HasProduct(productName))
                            {
                                Console.WriteLine("Product Type already exists.");
                            }
                            else
                            {
                                while (newProduct)
                                {
                                    Console.WriteLine("Please enter Product Price");
                                    Console.Write("Price: ");
                                    string priceString = Console.ReadLine();
                                    decimal price = -1;

                                    if (decimal.TryParse(priceString, out price))
                                    {
                                        processor.AddProductType(productName, price);
                                        newProduct = false;
                                        showProductTypeMenu = false;
                                        Console.WriteLine($"New product {productName} with price ${price} added successfully.{Environment.NewLine}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid price entered.");
                                    }
                                }
                            }
                        }
                        break;
                    
                    // Modify Product type
                    case 1:
                        foreach (ProductTypeEntity entity in processor.ProductTypeList)
                        {
                            Console.WriteLine($"{entity.UPC} {entity.ProductName} {entity.Price}");
                        }
                        bool inputRequired = true;
                        while (inputRequired)
                        {
                            Console.WriteLine($"{Environment.NewLine}Select product number to modify:");
                            Console.Write("Product Number: ");
                            string upcString = Console.ReadLine();
                            int upc = -1;
                            if (GroceryCoValidator.ValidateSelection(upcString, processor.ProductTypeList.Count, out upc) && processor.HasProduct(upc))
                            {
                                inputRequired = false;

                                selection = DisplayMenu(_productTypeUpdateMenu, 3);

                                switch (selection)
                                {
                                    // Update price on product
                                    case 0:
                                        ProductTypeEntity updateEntity = processor.ProductTypeList.FirstOrDefault(p => p.UPC == upc);
                                        bool requiresPrice = true;
                                        
                                        while (requiresPrice)
                                        {
                                            Console.WriteLine($"Enter new price for product [{updateEntity.ProductName}]");
                                            Console.Write("Price: ");
                                            string priceString = Console.ReadLine();
                                            decimal price = -1;

                                            if (decimal.TryParse(priceString, out price))
                                            {
                                                updateEntity.Price = price;
                                                Console.WriteLine($"Price of {updateEntity.ProductName} updated to ${price}.{Environment.NewLine}");
                                                requiresPrice = false;
                                                showProductTypeMenu = false;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid price entered.");
                                            }
                                        }
                                        break;

                                    // Delete Product Type
                                    case 1:
                                        processor.ProductTypeList.RemoveAll(p => p.UPC == upc);

                                        // Clear products with same UPC
                                        List<ProductEntity> productEntities = MapTableToEntity.MapToProductEntity(_gcDatabase.ProductTable);
                                        productEntities.RemoveAll(p => p.UPC == upc);
                                        _gcDatabase.ProductTable.Clear();
                                        MapEntityToDatabase.MapProductEntityToTable(productEntities, _gcDatabase.ProductTable);

                                        // Clear discounts with same UPC
                                        _discountProcessor.RemoveDiscountsByUPC(upc);
                                        _gcDatabase.DiscountDataTable.Clear();
                                        _discountProcessor.MapEntitiesToTable(_gcDatabase.DiscountDataTable);

                                        Console.WriteLine($"Product successfully removed.{Environment.NewLine}");
                                        showProductTypeMenu = false;
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
            }

            _gcDatabase.ProductTypeTable.Clear();
            MapEntityToDatabase.MapProductTypeEntityToTable(processor.ProductTypeList, _gcDatabase.ProductTypeTable);
        }

        private void ProductMenu(List<ProductTypeEntity> productTypes)
        {
            ProductProcessor processor = null;
            bool requireFile = true;
            while (requireFile)
            {
                // If any errors, menu will show again
                processor = new ProductProcessor(productTypes);
                Console.WriteLine("Please enter path for Product text file, see below for example of format:");
                Console.Write("File Path: ");
                processor.FilePath = Console.ReadLine();

                requireFile = !processor.ParseLines();
            }

            Console.WriteLine($"File loaded successfully.{Environment.NewLine}");

            processor.MapToEntities();
            processor.LoadEntityToDatabase(_gcDatabase.ProductTable);
        }

        private void ProductModifyMenu(List<ProductTypeEntity> productTypes)
        {
            bool inputRequired = true;
            while (inputRequired)
            {
                int selection = DisplayMenu(_productModifyMenu, 3);
                List<ProductEntity> products = MapTableToEntity.MapToProductEntity(_gcDatabase.ProductTable);

                switch (selection)
                {
                    // Add Product
                    case 0:
                        foreach (ProductTypeEntity entity in productTypes)
                        {
                            Console.WriteLine($"{entity.UPC}  {entity.ProductName} @ ${entity.Price}ea");
                        }

                        bool pickProductToAdd = true;
                        while (pickProductToAdd)
                        {
                            Console.WriteLine("Select product type to add");
                            Console.Write("Product Number: ");
                            string productTypeString = Console.ReadLine();
                            int productTypeNumber = -1;

                            if (GroceryCoValidator.ValidateSelection(productTypeString, productTypes.Count, out productTypeNumber))
                            {
                                while (pickProductToAdd)
                                {
                                    Console.WriteLine($"{Environment.NewLine}Select quantity to add");
                                    Console.Write("Quantity: ");
                                    string quantityString = Console.ReadLine();
                                    int quantity = -1;

                                    if (int.TryParse(quantityString, out quantity))
                                    {
                                        ProductEntity entity = products.FirstOrDefault(p => p.UPC == productTypeNumber);

                                        if (entity != null)
                                        {
                                            entity.Quantity += quantity;
                                        }
                                        else
                                        {
                                            entity = new ProductEntity() { UPC = productTypeNumber, Quantity = quantity };
                                            products.Add(entity);
                                        }

                                        pickProductToAdd = false;
                                        inputRequired = false;
                                        _gcDatabase.ProductTable.Clear();
                                        MapEntityToDatabase.MapProductEntityToTable(products, _gcDatabase.ProductTable);
                                        Console.WriteLine($"Successfully added product to your cart.{Environment.NewLine}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid quantity entered.");
                                    }
                                }
                            }
                        }
                        break;

                    // Remove Product
                    case 1:
                        foreach (ProductEntity entity in products)
                        {
                            ProductTypeEntity productType = productTypes.FirstOrDefault(p => p.UPC == entity.UPC);
                            Console.WriteLine($"{entity.UPC}  {productType.ProductName} with {entity.Quantity} @ ${productType.Price}ea");
                        }

                        bool pickProductToRemove = true;
                        while(pickProductToRemove)
                        {
                            Console.WriteLine("Select product number to remove");
                            Console.Write("Product Number: ");
                            string productString = Console.ReadLine();
                            int productNumber = -1;

                            if(GroceryCoValidator.ValidateSelection(productString, products.Count, out productNumber))
                            {
                                ProductEntity entity = products.FirstOrDefault(p => p.UPC == productNumber);

                                while(pickProductToRemove)
                                {
                                    Console.WriteLine($"{Environment.NewLine}Select quantity to remove, there are {entity.Quantity} in your cart");
                                    Console.Write("Quantity: ");
                                    string quantityString = Console.ReadLine();
                                    int quantity = -1;

                                    if (GroceryCoValidator.ValidateSelection(quantityString, entity.Quantity, out quantity))
                                    {
                                        if (quantity < entity.Quantity)
                                        {
                                            entity.Quantity -= quantity;
                                        }
                                        else
                                        {
                                            products.Remove(entity);
                                        }

                                        pickProductToRemove = false;
                                        inputRequired = false;
                                        _gcDatabase.ProductTable.Clear();
                                        MapEntityToDatabase.MapProductEntityToTable(products, _gcDatabase.ProductTable);
                                        Console.WriteLine($"Successfully removed.{Environment.NewLine}");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Invalid quantity selected, must be less than {entity.Quantity}.");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Selected product number was not valid.");
                            }
                        }
                        break;

                    // Go Back
                    case 2:
                        inputRequired = false;
                        MainMenu();
                        break;
                }
            }

            Console.WriteLine($"File loaded successfully.{Environment.NewLine}");
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
                    // Add new discount
                    case 0:
                        // Select item for discounting
                        Console.WriteLine($"{Environment.NewLine}Select item number for discount:");
                        foreach (ProductTypeEntity entity in productTypeEntities)
                        {
                            Console.WriteLine($"{entity.UPC} {entity.ProductName}");
                        }

                        Console.Write("Item Number: ");
                        int selectedProductType = -1;
                        string input = Console.ReadLine();
                        int count = productTypeEntities.Count;

                        // Check if number is valid product type
                        if (GroceryCoValidator.ValidateSelection(input, count, out selectedProductType))
                        {
                            // Display the discount types to allow user to choose what discount they would like to apply
                            Console.WriteLine($"{Environment.NewLine}Select Discount Type:");
                            foreach (DiscountTypeIDs value in Enum.GetValues(typeof(DiscountTypeIDs)))
                            {
                                Console.WriteLine($"{(int)value} {value.ToString()}");
                            }
                            Console.Write("Discount Type: ");
                            int discountType = -1;
                            input = Console.ReadLine();

                            count = Enum.GetValues(typeof(DiscountTypeIDs)).Length;
                            if (GroceryCoValidator.ValidateSelection(input, count, out discountType))
                            {
                                DiscountEntity entity = new DiscountEntity();
                                entity.DiscountType = (DiscountTypeIDs)discountType;

                                bool requiresInput = true;

                                switch (discountType)
                                {
                                    // Percent off item, requires percentage off
                                    case (int)DiscountTypeIDs.PercentOff:
                                        requiresInput = true;
                                        while (requiresInput)
                                        {
                                            Console.WriteLine($"{Environment.NewLine}Please enter percentage off (1-100):");
                                            Console.Write("Percent off: %");
                                            input = Console.ReadLine();
                                            int discountPercent = -1;

                                            if (GroceryCoValidator.ValidatePercent(input, out discountPercent))
                                            {
                                                entity.DiscountPercent = discountPercent;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Percent value entered was not a whole number between 1 to 100.");
                                            }
                                        }
                                        break;

                                    // Buy x items to get percent off y
                                    case (int)DiscountTypeIDs.BuyXGetY:
                                        requiresInput = true;
                                        while (requiresInput)
                                        {
                                            // Number of items required for discount (x)
                                            Console.WriteLine($"{Environment.NewLine}How many items required for discount:");
                                            Console.Write("Item count: ");
                                            input = Console.ReadLine();
                                            int itemCount = -1;

                                            if (int.TryParse(input, out itemCount))
                                            {
                                                entity.ItemsRequired = itemCount;

                                                while (requiresInput)
                                                {
                                                    // Percent off for the (y) item, this can be 100% for free item
                                                    Console.WriteLine($"{Environment.NewLine}Please enter percentage off for discounted item (1-100):");
                                                    Console.Write("Percent off: %");
                                                    input = Console.ReadLine();
                                                    int percentOff = -1;

                                                    if (GroceryCoValidator.ValidatePercent(input, out percentOff))
                                                    {
                                                        entity.DiscountPercent = percentOff;
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

                                // Number of days the discount should be valid for
                                bool requiresDateRange = true;
                                while (requiresDateRange)
                                {
                                    Console.WriteLine($"{Environment.NewLine}How many days will this discount be valid for?");
                                    Console.Write("Number of Days: ");
                                    input = Console.ReadLine();
                                    int numDaysValid = -1;
                                    if (GroceryCoValidator.ValidatePercent(input, out numDaysValid))
                                    {
                                        entity.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                                        entity.EndDate = entity.StartDate.AddDays(numDaysValid + 1).AddSeconds(-1);
                                        requiresDateRange = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Number of days entered is not valid.");
                                    }
                                }

                                Console.WriteLine($"Added discount for successfully.{Environment.NewLine}");

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
                        if (_discountProcessor.DiscountEntities.Count == 0)
                        {
                            Console.WriteLine("No discounts to modify.");
                            DiscountMenu(productTypeEntities);
                            break;
                        }

                        int i = 0;
                        foreach (DiscountEntity entity in _discountProcessor.DiscountEntities)
                        {
                            Console.WriteLine($"{i} - Product Name: {productTypeEntities.FirstOrDefault(p => p.UPC == entity.UPC).ProductName} - {entity.StartDate:yyyy-MM-dd} to {entity.EndDate:yyyy-MM-dd}");
                            Console.WriteLine($"    Discount Type: {entity.DiscountTypeID} - Items Required: {entity.ItemsRequired}  Percent: {entity.DiscountPercent}%");
                            i++;
                        }

                        bool requiresDeletionInput = true;
                        while (requiresDeletionInput)
                        {
                            Console.WriteLine($"{Environment.NewLine}Please select what discount you would like to remove:");
                            Console.Write("Number of Discount: ");
                            input = Console.ReadLine();
                            int removeIndex = -1;

                            if (GroceryCoValidator.ValidateSelection(input, _discountProcessor.DiscountEntities.Count, out removeIndex))
                            {
                                _discountProcessor.DiscountEntities.RemoveAt(removeIndex);
                                requiresDeletionInput = false;
                                Console.WriteLine($"Discount successfully removed.{Environment.NewLine}");
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
                            cart.Groceries.Add(new CartEntity() { UPC = entity.UPC, ProductName = ptEntity.ProductName, Price = Math.Round(ptEntity.Price * (dEntity.DiscountPercent / 100), 2) });
                        }
                    }
                    else
                    {
                        int buyQuantity = dEntity.ItemsRequired;
                        int requiredItemsLeft = buyQuantity;

                        for (int i = 0; i < entity.Quantity; i++)
                        {
                            if (requiredItemsLeft > 0)
                            {
                                cart.Groceries.Add(new CartEntity() { UPC = entity.UPC, ProductName = ptEntity.ProductName, Price = ptEntity.Price });
                            }
                            else
                            {
                                cart.Groceries.Add(new CartEntity() { UPC = entity.UPC, ProductName = ptEntity.ProductName, Price = Math.Round(ptEntity.Price * (dEntity.DiscountPercent / 100), 2) });
                                requiredItemsLeft = buyQuantity;
                            }
                            requiredItemsLeft--;
                        }
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

            foreach (CartEntity entity in cart.Groceries)
            {
                Console.WriteLine($"{entity.UPC}  {entity.ProductName} - ${entity.Price}");
            }
            Console.WriteLine($"Total Price: {cart.TotalPrice}");

            bool showCheckoutMenu = true;

            // If any errors, menu will show again
            while (showCheckoutMenu)
            {
                int selection = DisplayMenu(_checkoutMenu, 2);

                if (selection == 0)
                {
                    Console.WriteLine($"Thank you for shopping at Grocery Co!{Environment.NewLine}{Environment.NewLine}");
                    showCheckoutMenu = false;
                    BeginCheckoutProcess();
                    break;
                }
                else
                {
                    MainMenu();
                    break;
                }
            }
        }

        // Displays menu on console
        private int DisplayMenu(string menu, int numOfSelections)
        {
            bool invalidSelection = true;
            int selectedValue = -1;

            while (invalidSelection)
            {
                Console.WriteLine();
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
|  1 - Modify Product Type   |
|  2 - Go Back               |
------------------------------";

        private static string _productTypeUpdateMenu =
@"----------------------
|  0 - Change Price  |
|  1 - Delete        |
|  2 - Go Back       |
----------------------";

        private static string _productModifyMenu =
@"-----------------------
|  0 - Add Product    |
|  1 - Remove Product |
|  2 - Go Back        |
-----------------------";

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
