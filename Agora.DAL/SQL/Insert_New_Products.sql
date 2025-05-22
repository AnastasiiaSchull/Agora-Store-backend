
-- new brands for Construction Toys
INSERT INTO Brands (Name) VALUES
('Playmobil'),
('Mega Bloks'),
('K''NEX'),
('Geomag'),
('Engino'),
('Magformers'),
('Meccano'),
('Robotime'),
('Cobi'),
('Brio'),
('Tinkertoy'),
('Snap Circuits'),
('Zometool'),
('Sluban');

-- new smartphone brands
INSERT INTO Brands (Name) VALUES
('Huawei'),
('OnePlus'),
('Google'),
('Motorola'),
('Nokia'),
('Oppo'),
('Vivo'),
('Realme'),
('Honor'),
('ZTE'),
('Infinix'),
('Tecno'),
('Alcatel'),
('Nothing'),
('Lenovo');

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId)

VALUES (
    'Playmobil 70281 Playground Set with 4 Figures',
    'Playground set with slide, climbing wall, tire swing, seesaw and figures. Suitable for children aged 4+.',
    34.99, 
    100, 
    4.6, 
    'images/Playmobil 70281 Playground Set with 4 Figures', 
    1,
    (SELECT Id FROM Subcategories WHERE Name = 'Educational Toys'),
    (SELECT Id FROM Categories WHERE Name = 'Toys & Games'),
    (SELECT Id FROM Brands WHERE Name = 'Playmobil'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId)

VALUES (
    'Playmobil 9463 Fire Truck with Ladder',
    'Fire truck playset with 3 figures, extendable ladder, light and sound module, and over 80 accessories. Perfect for emergency rescue missions. Recommended for ages 4+.',
    58.12, 
    80, 
    4.8, 
    'images/Playmobil 9463 Fire Truck with Ladder', 
    1,
    (SELECT Id FROM Subcategories WHERE Name = 'Educational Toys'),
    (SELECT Id FROM Categories WHERE Name = 'Toys & Games'),
    (SELECT Id FROM Brands WHERE Name = 'Playmobil'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId)

VALUES (
    'Playmobil 71202 Ambulance with Light and Sound',
    'Ambulance playset with 3 figures, stretcher on wheels, medical equipment, and light & sound module. Doors open. Recommended for children aged 4 and up.',
    39.59, 
    120, 
    4.8, 
    'images/Playmobil 71202 Ambulance with Light and Sound', 
    1,
    (SELECT Id FROM Subcategories WHERE Name = 'Educational Toys'),
    (SELECT Id FROM Categories WHERE Name = 'Toys & Games'),
    (SELECT Id FROM Brands WHERE Name = 'Playmobil'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId)

VALUES (
    'Playmobil 71615 Modern Hospital Set with 4 Figures',
    'Modern hospital playset including operation room and medical equipment. Comes with 4 figures and over 160 accessories. Encourages creative play. Recommended for ages 4+.',
    70.08, 
    60, 
    4.7, 
    'images/Playmobil 71615 Modern Hospital Set with 4 Figures', 
    1,
    (SELECT Id FROM Subcategories WHERE Name = 'Educational Toys'),
    (SELECT Id FROM Categories WHERE Name = 'Toys & Games'),
    (SELECT Id FROM Brands WHERE Name = 'Playmobil'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId)

VALUES (
    'Playmobil 5024 Childrens Playground Set',
    'Playground set with carousel, swings, slides, skateboard ramp, bike and food stand. Designed for children ages 4 and up.',
    70.00, 
    70, 
    4.5, 
    'images/Playmobil 5024 Childrens Playground Set', 
    1,
    (SELECT Id FROM Subcategories WHERE Name = 'Educational Toys'),
    (SELECT Id FROM Categories WHERE Name = 'Toys & Games'),
    (SELECT Id FROM Brands WHERE Name = 'Playmobil'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId)

VALUES (
    'Playmobil 71762 Asian Garden with Pandas',
    'Asian-style playset with a pavilion, koi pond, and two pandas. Includes food accessories and chopsticks. Recommended for ages 4+.',
    27.28, 
    90, 
    4.5, 
    'images/Playmobil 71762 Asian Garden with Pandas', 
    1,
    (SELECT Id FROM Subcategories WHERE Name = 'Educational Toys'),
    (SELECT Id FROM Categories WHERE Name = 'Toys & Games'),
    (SELECT Id FROM Brands WHERE Name = 'Playmobil'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId)

VALUES (
    'Playmobil 71793 Pirates Danger with Giant Shark',
    'Exciting pirate adventure playset featuring a giant shark and shark pirate figure. Encourages imaginative underwater play. For children aged 4+.',
    30.79, 
    85, 
    4.4, 
    'images/Playmobil 71793 Pirates Danger with Giant Shark', 
    1,
    (SELECT Id FROM Subcategories WHERE Name = 'Educational Toys'),
    (SELECT Id FROM Categories WHERE Name = 'Toys & Games'),
    (SELECT Id FROM Brands WHERE Name = 'Playmobil'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId)

VALUES (
    'Playmobil 71743 My Life Puppy House',
    'Dog lovers will enjoy this playset with adorable puppies and accessories. Encourages nurturing play and creativity. For children aged 4 and up.',
    39.99, 
    75, 
    4.5, 
    'images/Playmobil 71743 My Life Puppy House', 
    1,
    (SELECT Id FROM Subcategories WHERE Name = 'Educational Toys'),
    (SELECT Id FROM Categories WHERE Name = 'Toys & Games'),
    (SELECT Id FROM Brands WHERE Name = 'Playmobil'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId)

VALUES (
    'LEGO Friends 42626 Water Sports Adventure Camp',
    'Creative building set for kids aged 7+ featuring 3 mini-dolls, a bear figure, kayaks, and various outdoor water sports accessories. Encourages imaginative play and role-playing adventures.',
    49.99,
    60,
    4.8,
    'images/LEGO Friends 42626 Water Sports Adventure Camp',
    1,
    (SELECT Id FROM Subcategories WHERE Name = 'Educational Toys'),
    (SELECT Id FROM Categories WHERE Name = 'Toys & Games'),
    (SELECT Id FROM Brands WHERE Name = 'Lego'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId)

VALUES (
    'LEGO Friends 42624 Cozy Cabins at Adventure Camp',
    'Camping-themed building set with 2 cabins, 3 mini-dolls, a fox figure, and outdoor accessories like campfire and marshmallows. Great for creative role-playing for children aged 7 and up.',
    35.99,
    70,
    4.8,
    'images/LEGO Friends 42624 Cozy Cabins at Adventure Camp',
    1,
    (SELECT Id FROM Subcategories WHERE Name = 'Educational Toys'),
    (SELECT Id FROM Categories WHERE Name = 'Toys & Games'),
    (SELECT Id FROM Brands WHERE Name = 'Lego'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId)

VALUES (
    'LEGO Friends 42670 Heartlake City Shops & Apartments',
    'LEGO set for children aged 12+ featuring colorful buildings including apartments, a bakery, pottery studio, and laundromat with 9 characters and over 2,000 pieces. Great for role-play and display.',
    139.99,
    35,
    4.7,
    'images/LEGO Friends 42670 Heartlake City Shops & Apartments',
    1,
    (SELECT Id FROM Subcategories WHERE Name = 'Educational Toys'),
    (SELECT Id FROM Categories WHERE Name = 'Toys & Games'),
    (SELECT Id FROM Brands WHERE Name = 'Lego'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId)

VALUES (
    'LEGO 75954 Harry Potter Hogwarts Great Hall',
    'Buildable LEGO set featuring the Great Hall and tower of Hogwarts Castle with 10 characters including Harry, Ron, Hermione, Dumbledore, Hagrid, and more. Contains magical accessories, a boat, and moveable staircase.',
    176.00,
    20,
    4.8,
    'images/LEGO 75954 Harry Potter Great Hall',
    1,
    (SELECT Id FROM Subcategories WHERE Name = 'Educational Toys'),
    (SELECT Id FROM Categories WHERE Name = 'Toys & Games'),
    (SELECT Id FROM Brands WHERE Name = 'Lego'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId)
VALUES (
    'LEGO Icons NASA Artemis Rocket Launch System 10341',
    'Highly detailed LEGO Icons model of NASAs Artemis Space Launch System, including multi-stage rocket, Orion capsule, mobile launch tower, and movable parts. Ideal for display and space enthusiasts.',
    259.99,
    10,
    4.8,
    'images/LEGO Icons NASA Artemis Rocket Launch System 10341',
    1,
    (SELECT Id FROM Subcategories WHERE Name = 'Educational Toys'),
    (SELECT Id FROM Categories WHERE Name = 'Toys & Games'),
    (SELECT Id FROM Brands WHERE Name = 'Lego'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId)
VALUES (
    'LEGO Star Wars UCS TIE Interceptor 75382',
    'Highly detailed LEGO Star Wars Ultimate Collector Series model of the iconic TIE Interceptor starfighter, featuring angled wings, detailed cockpit, and display stand with 25th anniversary plaque and minifigures.',
    229.99,
    8,
    4.7,
    'images/LEGO Star Wars UCS TIE Interceptor 75382',
    1,
    (SELECT Id FROM Subcategories WHERE Name = 'Educational Toys'),
    (SELECT Id FROM Categories WHERE Name = 'Toys & Games'),
    (SELECT Id FROM Brands WHERE Name = 'Lego'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);