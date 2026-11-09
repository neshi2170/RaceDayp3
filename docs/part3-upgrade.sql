/* Run this once against the existing RaceDayDB database from Part 2.
   It adds the image URL columns required by Part 3 without deleting existing data. */
IF COL_LENGTH('dbo.Users', 'ProfileImageUrl') IS NULL
    ALTER TABLE dbo.Users ADD ProfileImageUrl VARCHAR(1000) NULL;
GO
IF COL_LENGTH('dbo.Events', 'BannerUrl') IS NULL
    ALTER TABLE dbo.Events ADD BannerUrl VARCHAR(1000) NULL;
GO
