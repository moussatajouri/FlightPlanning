
if exists ( select 1 from sysobjects where id = OBJECT_ID('Aircraft') and type = 'U')
drop table Aircraft
Go


CREATE TABLE [Aircraft] (
    [Id] INT NOT NULL IDENTITY,
	[Name] nvarchar(200) NOT NULL,
	[Speed] Decimal(8,4) NOT NULL,
	[FuelCapacity] Decimal(10,4) NOT NULL,
	[FuelConsumption] Decimal(10,4) NOT NULL,
	[TakeOffEffort] Decimal(10,4) NOT NULL
    CONSTRAINT [PK_Aircraft] PRIMARY KEY ([Id])
);
GO
