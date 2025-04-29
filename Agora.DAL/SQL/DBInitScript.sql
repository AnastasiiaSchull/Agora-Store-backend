USE agora;

INSERT INTO FAQCategories (Name) VALUES
('Payments'),
('Orders & Returns'),
('Shipping & Delivery'),
('General Questions');


INSERT INTO FAQs (Question, Answer, FAQCategoryId) VALUES
('What payment methods does Agora accept?', 
   'You can pay using Visa, MasterCard, and other supported payment methods and gift cards.', 1),

('What to do if my payment fails?', 
    'Ensure your card is enabled for online payments, check your balance, and verify international transactions are allowed by your bank.', 1),

('How do I return an item?', 
    'Items can be returned within 14 days of purchase. Visit the "Returns & Refunds" section for details.', 2),

('What is Agora''s refund policy?', 
    'Refunds are processed within a few business days after the return is received. You can check your refund status in the "Your Orders" section by selecting "Return or Replace Items" for the specific order.', 2),
 

('How long does delivery take?', 
    'Delivery times depend on  delivery method. Standard delivery takes 3-7 business days.', 3),


('How can I track my order?', 
    'You can track your order in the "Your Orders" section. Simply go to "Your Account" > "Your Orders" and click the "Track package" button next to the item.', 3),


('How can I contact Agora support?', 
    'You can get help through the "Customer Service" section. Simply go to "Customer Service" > "Help with a recent item" or choose another support option.', 4),

('How do I contact customer service?', 
    'Visit the "Customer Service" page where you can select an order for assistance or find other help options.', 4);



-- Categories
INSERT INTO Categories (Name) VALUES
('Electronics'),
('Clothing & Shoes'),
('Home & Kitchen'),
('Books'),
('Sports & Outdoors'),
('Beauty & Personal Care'),
('Toys & Games'),
('Automotive'),
('Garden & Outdoor'),
('Music & Instruments'),
('Grocery'),
('Pet Supplies'),
('Health & Personal Care'),
('Office & Stationery'),
('Video Games & Consoles'),
('Gift Cards');

-- Subcategories
INSERT INTO Subcategories (CategoryId, Name) VALUES
-- Electronics
(1, 'Smartphones'),
(1, 'Laptops'),
(1, 'Tablets'),
(1, 'Cameras'),
(1, 'Headphones & Audio'),
(1, 'Smartwatches'),

-- Clothing & Shoes
(2, 'Men''s Clothing'),
(2, 'Women''s Clothing'),
(2, 'Children''s Clothing'),
(2, 'Men''s Shoes'),
(2, 'Women''s Shoes'),
(2, 'Children''s Shoes'),

-- Home & Kitchen
(3, 'Furniture'),
(3, 'Home Decor'),
(3, 'Kitchen Appliances'),
(3, 'Cookware'),

-- Books
(4, 'Fiction'),
(4, 'Non-Fiction'),
(4, 'Children''s Books'),

-- Sports & Outdoors
(5, 'Sports Equipment'),
(5, 'Outdoor Gear'),
(5, 'Fitness Accessories'),

-- Beauty & Personal Care
(6, 'Makeup'),
(6, 'Skincare'),
(6, 'Hair Care'),
(6, 'Fragrances'),

-- Toys & Games
(7, 'Board Games'),
(7, 'Educational Toys'),
(7, 'Dolls & Figures'),

-- Automotive
(8, 'Car Accessories'),
(8, 'Auto Tools'),
(8, 'Car Electronics'),

-- Garden & Outdoor
(9, 'Gardening Tools'),
(9, 'Plants & Seeds'),
(9, 'Outdoor Furniture'),

-- Music & Instruments
(10, 'Musical Instruments'),
(10, 'Music Accessories'),
(10, 'Recorded Music'),

-- Grocery
(11, 'Beverages'),
(11, 'Snacks'),
(11, 'Cooking Ingredients'),

-- Pet Supplies
(12, 'Dog Supplies'),
(12, 'Cat Supplies'),
(12, 'Aquatic Supplies'),

-- Health & Personal Care
(13, 'Medical Supplies'),
(13, 'Vitamins & Supplements'),

-- Office & Stationery
(14, 'Office Supplies'),
(14, 'Printers & Ink'),

-- Video Games 
(15, 'PC Games'),
(15, 'Console Games'),
(15, 'Gaming Accessories'),

-- Gift Cards
(16, 'Digital Gift Cards'),
(16, 'Physical Gift Cards');


-- Insert Brands
INSERT INTO Brands (Name) VALUES
-- Electronics
('Apple'),
('Samsung'),
('Sony'),
('Dell'),
('Asus'),
('Canon'),
('Bose'),
('Xiaomi'),
('HP'),

-- Clothing & Shoes
('Nike'),
('Adidas'),
('Puma'),
('Zara'),
('H&M'),
('Levi''s'),
('Gucci'),
('Louis Vuitton'),

-- Home & Kitchen
('IKEA'),
('Philips'),
('Tefal'),
('Dyson'),
('KitchenAid'),

-- Books
('Random House'),
('HarperCollins'),
('Scholastic'),

-- Sports & Outdoors
('Wilson'),
('Spalding'),
('Decathlon'),

-- Beauty & Personal Care
('L''Oréal'),
('Maybelline'),
('Estée Lauder'),
('Dior'),
('Chanel'),

-- Toys & Games
('Lego'),
('Hasbro'),
('Mattel'),
('Nintendo'),

-- Automotive
('Michelin'),
('Carall'),
('Goodyear'),

-- Garden & Outdoor
('Husqvarna'),
('Fiskars'),

-- Music & Instruments
('Yamaha'),
('Gibson'),
('Divarte'),

-- Grocery
('Nestlé'),
('Lindt'),
('Twinings'),

-- Pet Supplies
('Royal Canin'),
('Purina'),
('Whiskas'),

-- Health & Personal Care
('La Mer'),
('Colgate'),

-- Office & Stationery
('Moleskine'),
('Pilot'),

-- Video Games 
('Sony PlayStation'),
('Microsoft Xbox'),
('Razer'),

-- Gift Cards
('Agora'),
('Google Play'),
('Steam');


INSERT INTO Users (Name, Surname, Email, PhoneNumber, Password) 
VALUES (
    'Agora',  
     NULL,  
    'support@agora.com', 
    '+0123456789', 
    '1234'
);

INSERT INTO Sellers (Rating, UserId) 
VALUES (
    5.0, 
    (SELECT Id FROM Users WHERE Email = 'support@agora.com') 
);

INSERT INTO Stores (Name, Description, CreatedAt, UpdatedAt, SellerId) 
VALUES (
    'Agora Store', 
    'Official Agora store', 
    CURDATE(),
    CURDATE(),
    (SELECT Id FROM Sellers WHERE UserId = (SELECT Id FROM Users WHERE Email = 'support@agora.com'))
);


