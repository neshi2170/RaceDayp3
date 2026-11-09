USE [RaceDayDB];
GO

-- Check that the correct database is selected
SELECT DB_NAME() AS DatabaseName;
GO

-- Check that the Users table exists
SELECT OBJECT_ID('dbo.Users') AS UsersTable;
GO

-- Add missing profile image column
IF COL_LENGTH('dbo.Users', 'ProfileImageUrl') IS NULL
BEGIN
    ALTER TABLE dbo.Users
    ADD ProfileImageUrl NVARCHAR(1000) NULL;
END;
GO

-- Add missing event banner column
IF COL_LENGTH('dbo.Events', 'BannerUrl') IS NULL
BEGIN
    ALTER TABLE dbo.Events
    ADD BannerUrl NVARCHAR(1000) NULL;
END;
GO

-- Verify both columns
SELECT
    COL_LENGTH('dbo.Users', 'ProfileImageUrl') AS ProfileImageUrl,
    COL_LENGTH('dbo.Events', 'BannerUrl') AS BannerUrl;
GO