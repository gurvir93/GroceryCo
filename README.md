# GroceryCo 

GroceryCo is a kiosk checkout system for customers shopping at GroceryCo.

## Usage

Run GroceryCo.Application.exe via command prompt, no arguments are required. User input will be requested by the application.

## Product Type File Format
A default product list is provided, please use option 0 when prompted to use the default product type list. To use your own file, please use the following format -
```bash
item1, price1
item2, price2
```

Example:
```bash
Apples, 0.50
Oranges, 0.75
Grapes, 3.00
```

## Product File Format
Sample file available in directory 'Files\ProductList.txt'. To use your own file, please use the following format -
```bash
item1
item2
```

Example:
```bash
Apples
Oranges
Grapes
Apples
Oranges
```

## Functionality
* File format for product types, with a default product type list available.
* Add/Modify/Delete product types via console.
* Admin / Product / Checkout menus, with sub-menu navigation
* Add/Delete discounts via console.
* Start/End dates functionality for discounts.
* Checkout process with receipt view.

## Design Choices
* Used a database for storing data. The database can be changed because of the use of a database interface.
* Infrastructure backend for database communication.
* Domain layer for entities, validation, and communication from infrastructure to application.
* Application layer for all user logic and processing.

## Assumptions
* Admin console available on the main menu, with no security restrictions.
* Payment processing/methods are not handled by this application.

## Limitations
* The use of Datasets for databases was quite limiting requiring a lot of LINQ to query for information which could easily be done in SQL.
* Not every logical situation is covered.
* Since there is no functionality (specifically the database) required to be mocked, no mock's are used in test cases.
