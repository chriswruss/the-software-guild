USE [master]
GO

--Delete the database if it exists
if exists (select * from sys.databases where name = N'HotelReservation')
begin
	EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'HotelReservation';
	ALTER DATABASE HotelReservation SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE HotelReservation;
end

CREATE DATABASE HotelReservation
GO

-- Use the newly created database
USE HotelReservation
GO

-- Table holds necessary information about all the available promotions
CREATE TABLE Promotions (
	PromotionID INT PRIMARY KEY IDENTITY(1,1),
	Code VARCHAR(20) NOT NULL,
	[Description] VARCHAR(200) NOT NULL,
	FlatAmount DECIMAL(6,2) NOT NULL,
	PercentAmount DECIMAL(5, 4) NOT NULL,
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL
)
GO

-- Sample data put into the promotions table for testing.
INSERT INTO Promotions (Code, [Description], FlatAmount, PercentAmount, StartDate, EndDate) VALUES 
--Code					Description															Flat		%		StartDate	  EndDate
('CHRIS',				'Chris viewers get 75% off',										0.00,		0.75,	'2019-03-01', '2019-06-01'),
('SUPERBOWL98',			'Superbowl 98 viewers get $100 off',								100.00,		0.00,	'2019-04-01', '2019-04-14'),
('NEWCUSTOMER',			'New customers get 10% off',										0.00,		0.10,	'2019-01-01', '2029-01-01'),
('RETURNCUSTOMER',		'Customers that make reservation two years in a row get 20% off',	0.00,		0.20,	'2019-01-01', '2029-01-01'),
('SOFTWAREGUILDGRAD',	'Software Guild graduates get 1% off',								0.00,		0.01,	'2019-01-01', '2019-12-31')
GO

-- Table used to hold all the bills, future and past.
CREATE TABLE Bill (
	BillID INT PRIMARY KEY IDENTITY(1,1),
	PromotionID INT FOREIGN KEY REFERENCES Promotions(PromotionID),
	Total DECIMAL(10,2) NOT NULL,
	Paid BIT NOT NULL
)
GO

-- Sample data put into the Bill table for testing.
INSERT INTO Bill (PromotionID, Total, Paid) VALUES
--PID	Total	Paid
(1,		100.23,		0),
(NULL,	200.59,		1),
(NULL,	220.64,		0),
(1,		1023.32,	0),
(5,		102.25,		0),
(3,		1056.95,	1),
(NULL,	62.23,		0),
(NUll,	2095.63,	0),
(3,		222.22,		0),
(3,		333.33,		0)
GO

-- Table used for keeping track of addons currently offered by the hotel.
CREATE TABLE Addon (
	AddonID INT PRIMARY KEY IDENTITY(1,1),
	[Name] VARCHAR(40) NOT NULL,
	Fee DECIMAL(10,2) NOT NULL
)
GO

-- Sample data put into the addon table for testing.
INSERT INTO Addon ([Name], Fee) VALUES
--Name				Fee
('HBO',				2.00),
('Hot Water',		3.00),
('Toilet Paper',	0.50),
('Room Service',	20.00),
('Wake Up Service',	0.25)
GO

-- Bridge table that keeps track of the bill ID and the addon ID and is used
-- to keep track of how long that specific addon was assigned to the specific
-- bill.
CREATE TABLE BillAddons (
	BillID INT FOREIGN KEY REFERENCES Bill(BillID),
	AddonID INT FOREIGN KEY REFERENCES Addon(AddonID),
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL
)
GO

-- Sample data put into the bill addons bridge table for testing.
INSERT INTO BillAddons (BillID, AddonID, StartDate, EndDate) VALUES
(1, 1, '2019-01-01', '2019-01-01'),
(1, 3, '2019-01-01', '2019-01-02'),
(3, 1, '2019-11-01', '2019-11-03'),
(3, 2, '2019-11-01', '2019-11-01'),
(3, 4, '2019-11-01', '2019-11-02'),
(7, 1, '2019-03-11', '2019-03-15'),
(7, 2, '2019-03-11', '2019-03-12'),
(7, 3, '2019-03-11', '2019-03-15'),
(7, 4, '2019-03-11', '2019-03-15'),
(7, 5, '2019-03-11', '2019-03-12'),
(8, 3, '2019-03-11', '2019-03-15'),
(9, 2, '2019-07-01', '2019-09-10'),
(9, 3, '2019-07-01', '2019-09-10')
GO

-- Table used to keep track of customer information.
CREATE TABLE Customer (
	CustomerID INT PRIMARY KEY IDENTITY(1,1),
	[Name] VARCHAR(60) NOT NULL,
	Phone VARCHAR(40) NOT NULL,
	EmailAddress VARCHAR(320) NOT NULL
)
GO

-- Sample data put into the customer table for testing.
INSERT INTO Customer ([Name], Phone, EmailAddress) VALUES
--Name					Phone				Email Adress
('Ambition Softworks',	'555-543-2109',		'ambition@softworks.com'),
('The Software Guild',	'444-333-2222',		'thesoftwareguild@tsg.com'),
('Kroger',				'1-888-KROGER',		'kroger@kroger.com'),
('Walmart',				'1-800-WALMART',	'wal@mart.com'),
('Jimmy Jones',			'423-456-7125',		'jimmy@jjones.com')
GO

-- Table used to hold all reservations made, includes past, present, future and cancelled.
CREATE TABLE Reservation (
	ReservationID INT PRIMARY KEY IDENTITY(1,1),
	CustomerID INT FOREIGN KEY REFERENCES Customer(CustomerID),
	BillID INT FOREIGN KEY REFERENCES Bill(BillID),
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL,
	City VARCHAR(40) NOT NULL,
	[State] VARCHAR(40) NOT NULL
)
GO

-- Sample data put into the reservation table for testing.
INSERT INTO Reservation (CustomerID, BillID, StartDate, EndDate, City, [State]) VALUES
(1, 1, '2019-01-01', '2019-01-10', 'Knoxville', 'TN'),
(2, 2, '2019-11-01', '2019-11-03', 'Knoxville', 'TN'),
(3, 3, '2019-03-11', '2019-03-15', 'Knoxville', 'TN'),
(4, 4, '2019-07-01', '2019-07-10', 'Knoxville', 'TN'),
(5, 5, '2019-12-22', '2019-12-29', 'Knoxville', 'TN')
GO

-- Table used to hold important guest information.
CREATE TABLE Guest (
	GuestID INT PRIMARY KEY IDENTITY(1,1),
	ReservationID INT FOREIGN KEY REFERENCES Reservation(ReservationID),
	FirstName VARCHAR(40) NOT NULL,
	LastName VARCHAR(40) NOT NULL,
	IsAdult BIT NOT NULL,
)
GO

-- Sample data put into the guest table for testing.
INSERT INTO Guest (ReservationID, FirstName, LastName, IsAdult) VALUES
--ResID	FirstName		LastName	IsAdult
(1,		'John',			'Jacob',	1),
(2,		'Ebenheimer',	'Schmidt',	1),
(2,		'Janet',		'Schmidt',	1),
(3,		'Bon',			'Jovi',		1),
(4,		'Sam',			'Walton',	1),
(5,		'Jimmy',		'Jones',	1),
(5,		'Jane',			'Jones',	1),
(5,		'Timmy',		'Jones',	0),
(5,		'Lulu',			'Jones',	0)
GO

-- Table used to keep track of the base rates for each room type.
CREATE TABLE Rate (
	RateID INT PRIMARY KEY IDENTITY(1,1),
	[Name] VARCHAR(40) NOT NULL,
	AdultFee DECIMAL(10,2) NOT NULL,
	ChildFee DECIMAL(10,2) NOT NULL,
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL
)
GO

-- Sample data put into the rate table for testing.
INSERT INTO Rate ([Name], AdultFee, ChildFee, StartDate, EndDate) VALUES
--Name				AFee	CFee	Start		  End
('Spring Single',	100,	20,		'2019-03-20', '2019-06-21'),
('Spring Double',	150,	20,		'2019-03-20', '2019-06-21'),
('Spring King',		200,	20,		'2019-03-20', '2019-06-21'),
('Summer Single',	120,	25,		'2019-03-20', '2019-06-21'),
('Summer Double',	175,	25,		'2019-03-20', '2019-06-21'),
('Summer King',		230,	25,		'2019-03-20', '2019-06-21'),
('Fall Single',		110,	22,		'2019-03-20', '2019-06-21'),
('Fall Double',		165,	22,		'2019-03-20', '2019-06-21'),
('Fall King',		220,	22,		'2019-03-20', '2019-06-21'),
('Winter Single',	80,		16,		'2019-03-20', '2019-06-21'),
('Winter Double',	120,	16,		'2019-03-20', '2019-06-21'),
('Winter King',		160,	216,	'2019-03-20', '2019-06-21')
GO

-- Table used to keep track of the different room types (single, double, king, etc)
CREATE TABLE RoomType (
	RoomTypeID INT PRIMARY KEY IDENTITY(1,1),
	RateID INT FOREIGN KEY REFERENCES Rate(RateID),
	[Name] VARCHAR(15) NOT NULL,
	OccupancyLimits TINYINT NOT NULL,
	BedCount TINYINT NOT NULL
)
GO

-- Sample data put into the roomtype table for testing.
INSERT INTO RoomType ([Name], OccupancyLimits, BedCount) VALUES
--Name		Limit	Beds
('Single',	2,		1),
('Double',	4,		2),
('King',	3,		1)
GO

-- Table used to keep track of the different views the hotel has to offer
CREATE TABLE ViewType (
	ViewTypeID INT PRIMARY KEY IDENTITY(1,1),
	[Name] VARCHAR(40) NOT NULL,
	Fee DECIMAL(10,2) NOT NULL
)
GO

-- Sample data put into the viewtype table for testing.
INSERT INTO ViewType ([Name], Fee) VALUES
--Name			Fee
('Court Yard',	2.00),
('City',		5.00),
('None',		0.00)
GO

-- Table used to keep track of specific rooms.
CREATE TABLE Room (
	Number INT PRIMARY KEY NOT NULL,
	RoomTypeID INT FOREIGN KEY REFERENCES RoomType(RoomTypeID),
	ViewTypeID INT FOREIGN KEY REFERENCES ViewType(ViewTypeID),
	[Floor] TINYINT NOT NULL,
	IsSmoking BIT NOT NULL
)
GO

-- Sample data put into the room table for testing.
INSERT INTO Room (RoomTypeID, ViewTypeID, Number, [Floor], IsSmoking) VALUES
(1, 1, 101, 1, 0),
(1, 1, 102, 1, 0),
(1, 1, 103, 1, 0),
(1, 1, 104, 1, 0),
(2, 3, 105, 1, 0),
(2, 3, 106, 1, 0),
(3, 2, 107, 1, 0),
(3, 2, 108, 1, 0),
(3, 2, 109, 1, 0),
(3, 2, 110, 1, 0),
(1, 1, 201, 2, 1),
(1, 1, 202, 2, 1),
(1, 1, 203, 2, 1),
(1, 1, 204, 2, 1),
(2, 3, 205, 2, 1),
(2, 3, 206, 2, 1),
(3, 2, 207, 2, 1),
(3, 2, 208, 2, 1),
(3, 2, 209, 2, 1),
(3, 2, 210, 2, 1)
GO

-- Bridge table used to keep track of what rooms have reservations.
CREATE TABLE ReservationRoom (
	Number INT FOREIGN KEY REFERENCES Room(Number),
	ReservationID INT FOREIGN KEY REFERENCES Reservation(ReservationID),
	[Date] DATE NOT NULL
)
GO

-- Sample data put into the reservationroom table for testing.
INSERT INTO ReservationRoom (Number, ReservationID, [Date]) VALUES
(101, 1, '2019-01-01'),
(101, 1, '2019-01-02'),
(101, 1, '2019-01-03'),
(101, 1, '2019-01-04'),
(101, 1, '2019-01-05'),
(101, 1, '2019-01-06'),
(101, 1, '2019-01-07'),
(101, 1, '2019-01-08'),
(101, 1, '2019-01-09'),
(101, 1, '2019-01-10'),
(207, 2, '2019-11-01'),
(207, 2, '2019-11-02'),
(207, 2, '2019-11-03'),
(101, 3, '2019-03-11'),
(101, 3, '2019-03-12'),
(101, 3, '2019-03-13'),
(101, 3, '2019-03-14'),
(101, 3, '2019-03-15'),
(110, 4, '2019-07-01'),
(110, 4, '2019-07-02'),
(110, 4, '2019-07-03'),
(110, 4, '2019-07-04'),
(110, 4, '2019-07-05'),
(110, 4, '2019-07-06'),
(110, 4, '2019-07-07'),
(110, 4, '2019-07-08'),
(110, 4, '2019-07-09'),
(110, 4, '2019-07-10'),
(105, 5, '2019-12-22'),
(105, 5, '2019-12-23'),
(105, 5, '2019-12-24'),
(105, 5, '2019-12-25'),
(105, 5, '2019-12-26'),
(105, 5, '2019-12-27'),
(105, 5, '2019-12-28'),
(105, 5, '2019-12-29')
GO

-- Table used to keep track of the current amenities the hotel has to offer.
CREATE TABLE Amenities (
	AmenityID INT PRIMARY KEY IDENTITY(1,1),
	[Name] VARCHAR(20) NOT NULL,
	Fee DECIMAL(10,2) NOT NULL
)
GO

-- Sample data put into the amenities table for testing.
INSERT INTO Amenities ([Name], Fee) VALUES
--Name				Fee
('Fridge',			2.00),
('Microwave',		1.00),
('Bidet',			45.00),
('Spa Bath',		5.00)
GO

-- Bridge table used to keep track of what specific rooms have a specific amenity(ies)
CREATE TABLE RoomAmenities (
	Number INT FOREIGN KEY REFERENCES Room(Number),
	AmenityID INT FOREIGN KEY REFERENCES Amenities(AmenityID)
)
GO

-- Sample data put into the roomamenities table for testing.
INSERT INTO RoomAmenities (Number, AmenityID) VALUES
(101, 1),
(101, 2),
(102, 1),
(102, 2),
(103, 1),
(103, 2),
(104, 1),
(105, 1),
(106, 1),
(106, 2),
(107, 1),
(107, 2),
(107, 4),
(108, 1),
(108, 2),
(109, 1),
(109, 2), 
(109, 3),
(109, 4),
(110, 1),
(110, 2), 
(110, 3),
(110, 4),
(201, 1),
(201, 2),
(202, 1),
(202, 2),
(203, 1),
(203, 2),
(204, 1),
(205, 1),
(206, 1),
(206, 2),
(207, 1),
(207, 2),
(207, 4),
(208, 1),
(208, 2),
(209, 1),
(209, 2), 
(209, 3),
(209, 4),
(210, 1),
(210, 2), 
(210, 3),
(210, 4)
GO

-- Test that gets all reservations that end tomorrow.
--SELECT * 
--FROM Reservation r
--WHERE r.EndDate = (SELECT DATEADD(day, 1, GETDATE()))
--GO

-- Test that will return a list of rooms by room number that do not 
-- contain a certain amenity
--SELECT ra.Number
--FROM RoomAmenities ra
--WHERE ra.Number NOT IN (
--	SELECT Number 
--	FROM RoomAmenities
--	WHERE RoomAmenities.AmenityID = 3
--)
--GROUP BY ra.Number
--GO

-- Test that will return a joined table by the promotionID and will
-- return the number of times a certain promotion code has been used
--SELECT p.Code, COUNT(*)
--FROM Bill b
--LEFT JOIN Promotions p ON p.PromotionID = b.PromotionID
--GROUP BY p.Code
--GO