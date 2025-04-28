USE agora;

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId) 
VALUES (
    'Samsung Galaxy S24 Ultra 12/256 Black',
    'Android 14, 12GB RAM, 256GB Storage, 6.8-inch Display, 7680x4320 Resolution, 5G, Black Titanium',
    899.99, 
    50,  
    4.5, 
    'images/Samsung Galaxy S24 Ultra', 
    1,  -- 1 = доступен
    (SELECT Id FROM Subcategories WHERE Name = 'Smartphones'),
    (SELECT Id FROM Categories WHERE Name = 'Electronics'),
    (SELECT Id FROM Brands WHERE Name = 'Samsung'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId) 
VALUES (
    'HP Pavilion (960P6EA) Laptop',
    '14" Full-HD Display, Intel Core i7, 16GB RAM, 512GB SSD, Windows 11 Home, Natural Silver',
    1099.00,
    30,
    4.3,
    'images/HP Pavilion (960P6EA) Laptop',
    1,
    (SELECT Id FROM Subcategories WHERE Name = 'Laptops'),
    (SELECT Id FROM Categories WHERE Name = 'Electronics'),
    (SELECT Id FROM Brands WHERE Name = 'HP'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId)
VALUES (
    'Asus Zenbook 14',
    'AMD Ryzen 7 8840HS 5.1GHz, 16GB DDR5X RAM, 512GB SSD, AMD Radeon Graphics, Windows 11 Home, 14-inch OLED, Black',
    1019.99,
    30,  
    4.2, 
    'images/Asus Zenbook 14', 
    1,  
    (SELECT Id FROM Subcategories WHERE Name = 'Laptops'),
    (SELECT Id FROM Categories WHERE Name = 'Electronics'),
    (SELECT Id FROM Brands WHERE Name = 'Asus'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId)
VALUES (
    'KitchenAid Artisan Multi-Function Kitchen Processor',
    '4.8L Capacity, Stainless Steel, 10 Speeds, 300W Power, Black',
    449.00, 
    15,  
    4.6, 
    'images/KitchenAid Artisan Multi-Function', 
    1,  
    (SELECT Id FROM Subcategories WHERE Name = 'Kitchen Appliances'),
    (SELECT Id FROM Categories WHERE Name = 'Home & Kitchen'),
    (SELECT Id FROM Brands WHERE Name = 'KitchenAid'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId) 
VALUES (
    'KitchenAid Digital Glass Kitchen Scale',
    'KitchenAid - Digital Glass Kitchen Scale, Dry and Liquid Ingredients, 5000 g / 5000 ml, Black. Weigh dry ingredients (5000 g capacity, accuracy 1 g) and liquids (5000 ml capacity, 1 ml accuracy).',
    33.07, 
    100,  
    4.5, 
    'images/KitchenAid Digital Glass Kitchen Scale', 
    1,  
    (SELECT Id FROM Subcategories WHERE Name = 'Kitchen Appliances'),
    (SELECT Id FROM Categories WHERE Name = 'Home & Kitchen'),
    (SELECT Id FROM Brands WHERE Name = 'KitchenAid'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId) 
VALUES (
    'KitchenAid Mini Classic Household Processor, White 0.83 Litre, 240',
    'Mini food processor, 0.83-litre capacity, Safety Lock, Compact, White.',
    89.90, 
    10, 
    4.5, 
    'images/KitchenAid Mini Classic Household Processor', 
    1, 
    (SELECT Id FROM Subcategories WHERE Name = 'Kitchen Appliances'),
    (SELECT Id FROM Categories WHERE Name = 'Home & Kitchen'),
    (SELECT Id FROM Brands WHERE Name = 'KitchenAid'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId) 
VALUES (
    'Dyson Airwrap Multi Styler HS05',
    'Multi Styler with filter cleaning brush, Coanda smoothing drying tip, airwrap rollers, and multiple styling brushes. Model: HS05.',
    659.99,  
    15,  
    4.4, 
    'images/Dyson Airwrap HS05 Multi Styler Complete Long', 
    1,
    (SELECT Id FROM Subcategories WHERE Name = 'Hair Care'),
    (SELECT Id FROM Categories WHERE Name = 'Beauty & Personal Care'),
    (SELECT Id FROM Brands WHERE Name = 'Dyson'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);


INSERT INTO Subcategories (Name, CategoryId)
VALUES ('Construction Toys', (SELECT Id FROM Categories WHERE Name = 'Toys & Games'));

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId) 
VALUES (
    'LEGO Botanicals The Bouquet of Roses',
    'Artificial Flowers for Room Decoration - Home or Office Decoration Accessories - Birthday Gift for Adult Plant-Lovers 10328. Material: Plastic, Multicolored, Scratch Resistant, Weight: 710g, Dimensions: 26.2 x 38.2 x 7.05 cm',
    49.08, 
    100,  
    4.8, 
    'images/LEGO Botanicals The Bouquet of Roses', 
    1,  
    (SELECT Id FROM Subcategories WHERE Name = 'Construction Toys'),
    (SELECT Id FROM Categories WHERE Name = 'Toys & Games'),
    (SELECT Id FROM Brands WHERE Name = 'Lego'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);

INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId) 
VALUES (
    'LEGO Technic 42164 All-Terrain Racing Buggy',
    'LEGO Technic 42164 All-Terrain Racing Buggy, Rally Vehicle, Racing Car Construction Toy, Gift for Boys and Girls from 8 Years Old. Features: Realistic rear suspension, movable 4-cylinder engine, highly flexible steering. Dimensions: 9cm high, 17cm long, 11cm wide.',
    12.79, 
    150,  
    4.6, 
    'images/LEGO Technic 42164 All-Terrain Racing Buggy', 
    1,  
    (SELECT Id FROM Subcategories WHERE Name = 'Construction Toys'),
    (SELECT Id FROM Categories WHERE Name = 'Toys & Games'),
    (SELECT Id FROM Brands WHERE Name = 'Lego'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);


INSERT INTO Products (Name, Description, Price, StockQuantity, Rating, ImagesPath, IsAvailable, SubcategoryId, CategoryId, BrandId, StoreId) 
VALUES (
    'LEGO Icons Lord of the Rings: Barad-dûr Fortress Building Set',
    'Gift Idea for Adults - Minifigures of Sauron, Frodo, Sam, Gollum, Gothmog, and Orcs. Material: Plastic, Multicolored. Weight: 4kg. Dimensions: 37.8 x 52.3 x 25.7 cm. Assembly required.',
    459.99, 
    50,  
    4.7, 
    'images/LEGO Icons Lord of the Rings', 
    1,  
    (SELECT Id FROM Subcategories WHERE Name = 'Construction Toys'),
    (SELECT Id FROM Categories WHERE Name = 'Toys & Games'),
    (SELECT Id FROM Brands WHERE Name = 'Lego'),
    (SELECT Id FROM Stores WHERE Name = 'Agora Store')
);


