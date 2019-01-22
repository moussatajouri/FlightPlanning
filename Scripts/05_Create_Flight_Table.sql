
if exists ( select 1 from sysobjects where id = OBJECT_ID('Flight') and type = 'U')
drop table Flight
Go

CREATE TABLE [Flight] (
    [Id] INT NOT NULL IDENTITY,
	[AirportDepartureId] INT NOT NULL,
	[AirportDestinationId] INT NOT NULL,
	[AircraftId] INT NOT NULL,
	[UpdateDate] datetime NULL,
    CONSTRAINT [PK_Flight] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Flight_AirportDeparture_AirportId] FOREIGN KEY ([AirportDepartureId]) REFERENCES [Airport] ([Id]),
	CONSTRAINT [FK_Flight_AirportDestination_AirportId] FOREIGN KEY ([AirportDestinationId]) REFERENCES [Airport] ([Id]),
	CONSTRAINT [FK_Flight_Aircraft_AircraftId] FOREIGN KEY ([AircraftId]) REFERENCES [Aircraft] ([Id])
);
GO
