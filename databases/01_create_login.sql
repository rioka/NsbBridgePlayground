USE [master]
GO

IF NOT EXISTS
(  
  SELECT 1
  FROM master.sys.server_principals
  WHERE name = 'docker'
)
BEGIN
  CREATE LOGIN [docker] 
    WITH PASSWORD = N'docker', 
    DEFAULT_DATABASE = [master], 
    CHECK_EXPIRATION = OFF, 
    CHECK_POLICY = OFF
END
GO
