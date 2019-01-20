
if exists ( select 1 from sysobjects where id = OBJECT_ID('Airport') and type = 'U')
drop table Airport
Go

CREATE TABLE [Airport] (
    [Id] INT NOT NULL IDENTITY,
	[Name] nvarchar(200) NOT NULL,
	[City] nvarchar(100) NOT NULL,
	[CountryName]  nvarchar(100) NOT NULL,
	[IATA]	varchar(3) NULL,
	[ICAO]	varchar(4) NULL,
	[Latitude]	Decimal(18,15),
	[Longitude]	Decimal(18,15)

    CONSTRAINT [PK_Airport] PRIMARY KEY ([Id]),
    CONSTRAINT [UK_Airport_Name] UNIQUE ([Name]),
	CONSTRAINT [UK_Airport_Iata] UNIQUE ([IATA]),
	CONSTRAINT [UK_Airport_Icao] UNIQUE ([Icao])

);
GO


IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'AirportCountryNameIndex' AND object_id = OBJECT_ID('Airport'))
    BEGIN
        -- Index with this name, on this table does NOT exist
		CREATE INDEX AirportCountryNameIndex ON Airport (CountryName);  
END