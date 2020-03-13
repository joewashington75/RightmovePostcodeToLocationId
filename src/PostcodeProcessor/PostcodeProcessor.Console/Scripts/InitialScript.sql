CREATE DATABASE RightmoveDemo;
GO

USE RightmoveDemo;
GO

CREATE TABLE PostcodeLocationMapper
(
	Postcode varchar(9) Primary Key,
	LocationId varchar(30),
	ProcessingStatus varchar(30)
);
GO