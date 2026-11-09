USE RaceDayDB;
GO

SELECT
    e.EventId,
    e.Title,
    e.OrganiserId,
    u.Email AS OrganiserEmail
FROM dbo.Events e
LEFT JOIN dbo.Users u
    ON e.OrganiserId = u.UserId
ORDER BY e.EventId DESC;