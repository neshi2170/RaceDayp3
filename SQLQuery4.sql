SELECT
    @@SERVERNAME AS ServerName,
    DB_NAME() AS DatabaseName,
    COUNT(*) AS TotalEvents
FROM dbo.Events;b