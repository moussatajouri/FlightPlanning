
if exists ( select 1 from sysobjects where id = OBJECT_ID('Aircraft') and type = 'U')
drop table Aircraft
Go


CREATE TABLE [Aircraft] (
    [Id] INT NOT NULL IDENTITY,
	[Name] nvarchar(200) NOT NULL,
	[Speed] float NOT NULL,
	[FuelCapacity] float NOT NULL,
	[FuelConsumption] float NOT NULL,
	[TakeOffEffort] float NOT NULL
    CONSTRAINT [PK_Aircraft] PRIMARY KEY ([Id]),	
    CONSTRAINT [UK_Aircraft_Name] UNIQUE ([Name])
);
GO
