CREATE DATABASE IF NOT EXISTS Northwind;

DROP TABLE IF EXISTS `Order Details`;
DROP TABLE IF EXISTS Products;
DROP TABLE IF EXISTS Categories;
DROP TABLE IF EXISTS Suppliers;
DROP TABLE IF EXISTS Orders;
DROP TABLE IF EXISTS Shippers;
DROP TABLE IF EXISTS Employees;
DROP TABLE IF EXISTS Customers;

CREATE TABLE Customers ( 
	CustomerID VARCHAR(5) NOT NULL , 
	CompanyName VARCHAR(40) NOT NULL , 
	ContactName VARCHAR(30) NULL , 
	ContactTitle VARCHAR(30) NULL , 
	Address VARCHAR(60) NULL , 
	City VARCHAR(15) NULL , 
	Region VARCHAR(15) NULL , 
	PostalCode VARCHAR(10) NULL , 
	Country VARCHAR(15) NULL , 
	Phone VARCHAR(24) NULL , 
	Fax VARCHAR(24) NULL , 
	PRIMARY KEY (CustomerID) );
	
DROP TABLE IF EXISTS Shippers;
CREATE TABLE Shippers ( 
	ShipperID int NOT NULL AUTO_INCREMENT,
	CompanyName varchar(40) NOT NULL ,
	Phone varchar(24) NULL , 
	PRIMARY KEY (ShipperID) );
	
DROP TABLE IF EXISTS Categories;	
CREATE TABLE Categories (
	CategoryID int NOT NULL AUTO_INCREMENT ,
	CategoryName varchar(15) NOT NULL ,
	Description LONGTEXT NULL ,
	Picture LONGBLOB NULL ,
	PRIMARY KEY (CategoryID)
);

DROP TABLE IF EXISTS Suppliers;
CREATE TABLE Suppliers (
	SupplierID int NOT NULL AUTO_INCREMENT ,
	CompanyName varchar(40) NOT NULL ,
	ContactName varchar(30) NULL ,
	ContactTitle varchar(30) NULL ,
	Address varchar(60) NULL ,
	City varchar(15) NULL ,
	Region varchar(15) NULL ,
	PostalCode varchar(10) NULL ,
	Country varchar(15) NULL ,
	Phone varchar(24) NULL ,
	Fax varchar(24) NULL ,
	HomePage LONGTEXT NULL ,
	PRIMARY KEY (SupplierID)
);

DROP TABLE IF EXISTS Products;
CREATE TABLE Products (
	ProductID int NOT NULL AUTO_INCREMENT,
	ProductName varchar(40) NOT NULL ,
	SupplierID int NULL ,
	CategoryID int NULL ,
	QuantityPerUnit varchar(20) NULL ,
	UnitPrice decimal(15,2) NULL DEFAULT (0),
	UnitsInStock smallint NULL DEFAULT (0),
	UnitsOnOrder smallint NULL DEFAULT (0),
	ReorderLevel smallint NULL DEFAULT (0),
	Discontinued bit NOT NULL DEFAULT (0),
	PRIMARY KEY (ProductID),
	CONSTRAINT fk_Products_Categories FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID) ,
	CONSTRAINT fk_Products_Suppliers FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID)
);

DROP TABLE IF EXISTS Employees;
CREATE TABLE Employees (
	EmployeeID int NOT NULL AUTO_INCREMENT,
	LastName varchar(20) NOT NULL ,
	FirstName varchar(10) NOT NULL ,
	Title varchar(30) NULL ,
	TitleOfCourtesy varchar(25) NULL ,
	BirthDate datetime NULL ,
	HireDate datetime NULL ,
	Address varchar(60) NULL ,
	City varchar(15) NULL ,
	Region varchar(15) NULL ,
	PostalCode varchar(10) NULL ,
	Country varchar(15) NULL ,
	HomePhone varchar(24) NULL ,
	Extension varchar(4) NULL ,
	Photo LONGBLOB NULL ,
	Notes LONGTEXT NULL ,
	ReportsTo int NULL ,
	PhotoPath varchar(255) NULL ,
	PRIMARY KEY (EmployeeID),
	CONSTRAINT fk_Employees_Employees FOREIGN KEY (ReportsTo) REFERENCES Employees(EmployeeID) 
);

DROP TABLE IF EXISTS Orders;
CREATE TABLE Orders (
	OrderID int NOT NULL AUTO_INCREMENT ,
	CustomerID VARCHAR(5) NULL ,
	EmployeeID int NULL ,
	OrderDate datetime NULL ,
	RequiredDate datetime NULL ,
	ShippedDate datetime NULL ,
	ShipVia int NULL ,
	Freight decimal(15,2) NULL DEFAULT (0),
	ShipName varchar(40) NULL ,
	ShipAddress varchar(60) NULL ,
	ShipCity varchar(15) NULL ,
	ShipRegion varchar(15) NULL ,
	ShipPostalCode varchar(10) NULL ,
	ShipCountry varchar(15) NULL ,
	PRIMARY KEY (OrderID),
	CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID) ,
	CONSTRAINT FK_Orders_Employees FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID) ,
	CONSTRAINT FK_Orders_Shippers FOREIGN KEY (ShipVia) REFERENCES shippers(ShipperID) 
);

DROP TABLE IF EXISTS `Order Details`;
CREATE TABLE `Order Details` (
	OrderID int NOT NULL ,
	ProductID int NOT NULL ,
	UnitPrice decimal(15,2) NOT NULL DEFAULT (0),
	Quantity smallint NOT NULL DEFAULT (1),
	Discount float NOT NULL DEFAULT (0),
	PRIMARY KEY (OrderID,ProductID),
	CONSTRAINT FK_Order_Details_Orders FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ,
	CONSTRAINT FK_Order_Details_Products FOREIGN KEY (ProductID) REFERENCES Products(ProductID) 
	
);

ALTER TABLE `customers` ADD INDEX `idxCity` (`City`);
ALTER TABLE `customers` ADD INDEX `idxCompanyName` (`CompanyName`);
ALTER TABLE `customers` ADD INDEX `idxPostalCode` (`PostalCode`);
ALTER TABLE `customers` ADD INDEX `idxRegion` (`Region`);
ALTER TABLE `Categories` ADD INDEX `idxCategoryName` (`CategoryName`);
ALTER TABLE `Suppliers` ADD INDEX `idxCompanyName` (`CompanyName`);
ALTER TABLE `Suppliers` ADD INDEX `idxPostalCode` (`PostalCode`);
ALTER TABLE `Products` ADD INDEX `idxCategoriesProducts` (`CategoryID`);
ALTER TABLE `Products` ADD INDEX `idxCategoryID` (`CategoryID`);
ALTER TABLE `Products` ADD INDEX `idxProductName` (`ProductName`);
ALTER TABLE `Products` ADD INDEX `idxSupplierID` (`SupplierID`);
ALTER TABLE `Products` ADD INDEX `idxSuppliersProducts` (`SupplierID`);
ALTER TABLE `Employees` ADD INDEX `idxLastName` (`LastName`);
ALTER TABLE `Employees` ADD INDEX `idxPostalCode` (`PostalCode`);
ALTER TABLE `Orders` ADD INDEX `idxCustomerID` (`CustomerID`);
ALTER TABLE `Orders` ADD INDEX `idxCustomersOrders` (`CustomerID`);
ALTER TABLE `Orders` ADD INDEX `idxEmployeeID` (`EmployeeID`);
ALTER TABLE `Orders` ADD INDEX `idxEmployeesOrders` (`EmployeeID`);
ALTER TABLE `Orders` ADD INDEX `idxOrderDate` (`OrderDate`);
ALTER TABLE `Orders` ADD INDEX `idxShippedDate` (`ShippedDate`);
ALTER TABLE `Orders` ADD INDEX `idxShippersOrders` (`ShipVia`);
ALTER TABLE `Orders` ADD INDEX `idxShipPostalCode` (`ShipPostalCode`);
ALTER TABLE `Order Details` ADD INDEX `idxOrderID` (`OrderID`);
ALTER TABLE `Order Details` ADD INDEX `idxOrdersOrder_Details` (`OrderID`);
ALTER TABLE `Order Details` ADD INDEX `idxProductID` (`ProductID`);
ALTER TABLE `Order Details` ADD INDEX `idxProductsOrder_Details` (`ProductID`);


DROP VIEW IF EXISTS Invoices;
create view Invoices AS
SELECT Orders.ShipName, Orders.ShipAddress, Orders.ShipCity, Orders.ShipRegion, Orders.ShipPostalCode, 
	Orders.ShipCountry, Orders.CustomerID, Customers.CompanyName AS CustomerName, Customers.Address, Customers.City, 
	Customers.Region, Customers.PostalCode, Customers.Country, 
	(FirstName + ' ' + LastName) AS Salesperson, 
	Orders.OrderID, Orders.OrderDate, Orders.RequiredDate, Orders.ShippedDate, Shippers.CompanyName As ShipperName, 
	OD.ProductID, Products.ProductName, OD.UnitPrice, OD.Quantity, 
	OD.Discount, 
	(CONVERT((OD.UnitPrice*Quantity*(1-Discount)/100), decimal(15,2))*100) AS ExtendedPrice, 
	Orders.Freight
FROM 	Shippers INNER JOIN 
		(Products INNER JOIN 
			(
				(Employees INNER JOIN 
					(Customers INNER JOIN Orders ON Customers.CustomerID = Orders.CustomerID) 
				ON Employees.EmployeeID = Orders.EmployeeID) 
			INNER JOIN `Order Details` OD ON Orders.OrderID = OD.OrderID) 
		ON Products.ProductID = OD.ProductID) 
	ON Shippers.ShipperID = Orders.ShipVia;