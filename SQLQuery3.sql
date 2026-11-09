USE RaceDayDB;
GO

SELECT
    EventId,
    OrganiserId,
    Title,
    Location,
    EventDate,
    EventType
FROM dbo.Events
ORDER BY EventId DESC;