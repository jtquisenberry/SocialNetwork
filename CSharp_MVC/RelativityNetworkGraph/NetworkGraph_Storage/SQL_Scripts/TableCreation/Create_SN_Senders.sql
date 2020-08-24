

USE SocialNetwork


--USE tempdb
SET NOCOUNT ON


DECLARE @DatabaseName nvarchar(300) 

IF @DatabaseName is NULL
BEGIN
	SELECT @DatabaseName = (SELECT DB_NAME())
END

DECLARE @SQL nvarchar(max) = N''

if OBJECT_ID('SN_Senders') is not null drop table SN_Senders

CREATE TABLE SN_Senders
(
	  ArtifactID int
	, EntityID bigint
	, EntityDisplay nvarchar(max)
)


SELECT @SQL = 
N'	
INSERT INTO SN_Senders (ArtifactID, EntityID, EntityDisplay)
SELECT dds.artifactid, dds.EntityID, el.EntityDisplay
FROM
(
	SELECT DISTINCT artifactid, EntityID 
	FROM '+@DatabaseName+'.dbo.simple_SocialNetwork 
	WHERE EntityID is NOT NULL
		AND EntityType = 1.00
) dds -- Distinct Document-Senders
JOIN '+@DatabaseName+'.dbo.simple_EntityList el ON dds.EntityID = el.EL_ID
WHERE el.EntityName <> ''null'' 
	AND el.EntityName > ''''

'
-- print @sql
EXECUTE sp_executeSQL @SQL

