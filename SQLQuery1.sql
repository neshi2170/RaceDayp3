USE [RaceDayDB];
GO

SELECT
    DB_NAME() AS CurrentDatabase,
    COL_LENGTH('dbo.Users', 'ProfileImageUrl') AS ProfileImageUrlColumn,
    COL_LENGTH('dbo.Events', 'BannerUrl') AS BannerUrlColumn;
GO