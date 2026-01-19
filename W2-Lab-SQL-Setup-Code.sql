/*
    IT410 Week 2 Lab – Add 3 New Tables to the EXISTING Demo Database
    Database: IT410Week2Demo
    New tables:
      1) dbo.Customers
      2) dbo.Orders
      3) dbo.OrderItems

    Relationships:
      Customers (1) -> (M) Orders
      Orders (1) -> (M) OrderItems
      Products (1) -> (M) OrderItems  (reuses dbo.Products from the demo)

    Run in SSMS.
*/

-- Uncomment to drop the database before rebuilding it

--USE master;

--GO

--IF EXISTS (SELECT database_id 
--FROM sys.databases 
--WHERE name = 'IT410Week2Demo')
--BEGIN

--alter database IT410Week2Demo set single_user with rollback immediate;

--DROP DATABASE IT410Week2Demo;


--END

--GO

--CREATE DATABASE IT410Week2Demo;

--GO

USE IT410Week2Demo;
GO

/*------------------------------------------------------------
  OPTIONAL RESET (safe rerun)
  Drops tables in dependency order if they already exist
------------------------------------------------------------*/
IF OBJECT_ID('dbo.OrderItems', 'U') IS NOT NULL DROP TABLE dbo.OrderItems;
IF OBJECT_ID('dbo.Orders', 'U') IS NOT NULL DROP TABLE dbo.Orders;
IF OBJECT_ID('dbo.Customers', 'U') IS NOT NULL DROP TABLE dbo.Customers;
GO

/*------------------------------------------------------------
  1) Customers
------------------------------------------------------------*/
CREATE TABLE dbo.Customers
(
    CustomerId   INT IDENTITY(1,1) NOT NULL,
    FirstName    NVARCHAR(50)      NOT NULL,
    LastName     NVARCHAR(50)      NOT NULL,
    Email        NVARCHAR(255)     NOT NULL,
    IsActive     BIT               NOT NULL CONSTRAINT DF_Customers_IsActive DEFAULT (1),

    CONSTRAINT PK_Customers PRIMARY KEY CLUSTERED (CustomerId),
    CONSTRAINT UQ_Customers_Email UNIQUE (Email)
);
GO

/*------------------------------------------------------------
  2) Orders
------------------------------------------------------------*/
CREATE TABLE dbo.Orders
(
    OrderId      INT IDENTITY(1,1) NOT NULL,
    CustomerId   INT               NOT NULL,
    OrderDate    DATETIME2(0)      NOT NULL CONSTRAINT DF_Orders_OrderDate DEFAULT (SYSUTCDATETIME()),
    OrderStatus  NVARCHAR(20)      NOT NULL CONSTRAINT DF_Orders_Status DEFAULT ('New'),

    CONSTRAINT PK_Orders PRIMARY KEY CLUSTERED (OrderId),
    CONSTRAINT FK_Orders_Customers
        FOREIGN KEY (CustomerId)
        REFERENCES dbo.Customers (CustomerId)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
);
GO

/*------------------------------------------------------------
  3) OrderItems
------------------------------------------------------------*/
CREATE TABLE dbo.OrderItems
(
    OrderItemId  INT IDENTITY(1,1) NOT NULL,
    OrderId      INT               NOT NULL,
    ProductId    INT               NOT NULL,
    Quantity     INT               NOT NULL,
    UnitPrice    DECIMAL(10,2)     NOT NULL,

    CONSTRAINT PK_OrderItems PRIMARY KEY CLUSTERED (OrderItemId),

    CONSTRAINT FK_OrderItems_Orders
        FOREIGN KEY (OrderId)
        REFERENCES dbo.Orders (OrderId)
        ON DELETE CASCADE
        ON UPDATE NO ACTION,

    CONSTRAINT FK_OrderItems_Products
        FOREIGN KEY (ProductId)
        REFERENCES dbo.Products (ProductId)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,

    CONSTRAINT CK_OrderItems_Quantity_Positive CHECK (Quantity > 0),
    CONSTRAINT CK_OrderItems_UnitPrice_NonNegative CHECK (UnitPrice >= 0)
);
GO

/*------------------------------------------------------------
  Seed Data: Customers
------------------------------------------------------------*/
INSERT INTO dbo.Customers (FirstName, LastName, Email, IsActive)
VALUES
 ('Ava',   'Nguyen',   'ava.nguyen@example.com',   1),
 ('Noah',  'Martinez', 'noah.martinez@example.com',1),
 ('Mia',   'Johnson',  'mia.johnson@example.com',  1),
 ('Ethan', 'Patel',    'ethan.patel@example.com',  1),
 ('Sofia', 'Garcia',   'sofia.garcia@example.com', 1);
GO

/*------------------------------------------------------------
  Seed Data: Orders
  Note: CustomerIds assume IDENTITY starting at 1 in insertion order above.
------------------------------------------------------------*/
INSERT INTO dbo.Orders (CustomerId, OrderDate, OrderStatus)
VALUES
 (1, '2025-01-12 10:15:00', 'New'),
 (2, '2025-01-12 11:05:00', 'New'),
 (3, '2025-01-13 09:20:00', 'Processing'),
 (1, '2025-01-13 14:45:00', 'New'),
 (4, '2025-01-14 08:10:00', 'Shipped'),
 (5, '2025-01-14 16:30:00', 'New');
GO

/*------------------------------------------------------------
  Seed Data: OrderItems
  ProductIds must exist in dbo.Products from the demo seed (typically 1–10).
  UnitPrice is stored here intentionally (historical price at time of order).
------------------------------------------------------------*/
INSERT INTO dbo.OrderItems (OrderId, ProductId, Quantity, UnitPrice)
VALUES
 -- Order 1 (Customer 1)
 (1,  1, 2, 1.29),   -- Sparkling Water 1L
 (1,  3, 1, 2.99),   -- Sea Salt Potato Chips

 -- Order 2 (Customer 2)
 (2,  2, 1, 3.49),   -- Cold Brew Coffee Bottle
 (2,  4, 1, 4.49),   -- Trail Mix
 (2,  6, 2, 1.49),   -- Baby Carrots

 -- Order 3 (Customer 3)
 (3,  5, 3, 1.99),   -- Gala Apples
 (3,  9, 1, 8.99),   -- Vitamin C

 -- Order 4 (Customer 1)
 (4,  8, 1, 7.99),   -- Paper Towels
 (4,  7, 1, 5.99),   -- Cleaner

 -- Order 5 (Customer 4)
 (5, 10, 2, 4.29),   -- Herbal Tea
 (5,  1, 1, 1.29),   -- Sparkling Water

 -- Order 6 (Customer 5)
 (6,  3, 2, 2.99),   -- Chips
 (6,  2, 1, 3.49);   -- Cold Brew
GO

/*------------------------------------------------------------
  Quick Sanity Checks (optional)
------------------------------------------------------------*/
SELECT COUNT(*) AS CustomerCount FROM dbo.Customers;
SELECT COUNT(*) AS OrderCount FROM dbo.Orders;
SELECT COUNT(*) AS OrderItemCount FROM dbo.OrderItems;

-- Example join to confirm relationships
SELECT
    o.OrderId,
    o.OrderDate,
    o.OrderStatus,
    c.CustomerId,
    c.FirstName,
    c.LastName,
    p.ProductId,
    p.ProductName,
    oi.Quantity,
    oi.UnitPrice,
    (oi.Quantity * oi.UnitPrice) AS LineTotal
FROM dbo.Orders o
JOIN dbo.Customers c ON o.CustomerId = c.CustomerId
JOIN dbo.OrderItems oi ON o.OrderId = oi.OrderId
JOIN dbo.Products p ON oi.ProductId = p.ProductId
ORDER BY o.OrderId, oi.OrderItemId;
GO
