

USE SocialNetwork


--USE tempdb
SET NOCOUNT ON


DECLARE @DatabaseName nvarchar(300) 

IF @DatabaseName is NULL
BEGIN
	SELECT @DatabaseName = (SELECT DB_NAME())
END

DECLARE @SQL nvarchar(max) = N''

if OBJECT_ID('SN_Recipients') is not null drop table SN_Pairs

CREATE TABLE SN_Recipients
(
	  ArtifactID int
	, EntityID bigint
	, EntityDisplay nvarchar(max)
)




SELECT @SQL = 
N'
INSERT INTO SN_Recipients (ArtifactID, EntityID, EntityDisplay)
SELECT dds.artifactid, dds.EntityID, el.EntityDisplay
FROM
(
	SELECT DISTINCT artifactid, EntityID 
	FROM '+@DatabaseName+'.dbo.simple_SocialNetwork 
	WHERE EntityID is NOT NULL
		AND EntityType in (2.1, 2.2, 2.3, 4.0)
) dds -- Distinct Document-Senders
JOIN '+@DatabaseName+'.dbo.simple_EntityList el ON dds.EntityID = el.EL_ID
WHERE el.EntityName <> ''null'' 
	AND el.EntityName > ''''
'

EXECUTE sp_executeSQL @SQL


