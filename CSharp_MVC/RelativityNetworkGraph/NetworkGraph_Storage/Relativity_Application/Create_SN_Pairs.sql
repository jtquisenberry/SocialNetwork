
-- Server NCSSQL84250\NCSSQL84250
--USE DR_wtemp_SocialNetwork_EDDS1015989


--USE tempdb
SET NOCOUNT ON


DECLARE @DatabaseName nvarchar(300) 

IF @DatabaseName is NULL
BEGIN
	--SELECT @DatabaseName = N'DR_wtemp_SocialNetwork_EDDS1030755'
	SELECT @DatabaseName = (SELECT DB_NAME())
END

DECLARE @SQL nvarchar(max) = N''

if OBJECT_ID('tempdb..#sender') is not null drop table #sender
if OBJECT_ID('tempdb..#recipient') is not null drop table #recipient
if OBJECT_ID('tempdb..#participant') is not null drop table #participant
if OBJECT_ID('SN_Pairs') is not null drop table SN_Pairs

CREATE TABLE #sender
(
	  ArtifactID int
	, EntityID int
	, EntityDisplay nvarchar(max)
)

CREATE TABLE #recipient
(
	  ArtifactID int
	, EntityID int
	, EntityDisplay nvarchar(max)
)

CREATE TABLE #participant
(
	  ArtifactID int
	, EntityID int
	, EntityDisplay nvarchar(max)
)

CREATE TABLE SN_Pairs
(
	  s_EntityID int
	, s_EntityDisplay nvarchar(max)
	, r_EntityID int
	, r_EntityDisplay nvarchar(max)
	, PairsCount int
)

/*
CREATE TABLE SN_Pairs
(
	  s_EntityID int
	, s_EntityDisplay nvarchar(max)
	, r_EntityID int
	, r_EntityDisplay nvarchar(max)
	, PairsCount int
)
*/

SELECT @SQL = 
N'	
INSERT INTO #sender (ArtifactID, EntityID, EntityDisplay)
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
print @sql
EXECUTE sp_executeSQL @SQL


SELECT @SQL = 
N'
INSERT INTO #recipient (ArtifactID, EntityID, EntityDisplay)
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


SELECT @SQL = 
N'
INSERT INTO #participant (ArtifactID, EntityID, EntityDisplay)
SELECT dds.artifactid, dds.EntityID, el.EntityDisplay
FROM
(
	SELECT DISTINCT artifactid, EntityID 
	FROM '+@DatabaseName+'.dbo.simple_SocialNetwork 
	WHERE EntityID is NOT NULL
		AND EntityType in (1.0, 2.1, 2.2, 2.3, 4.0)
) dds -- Distinct Document-Senders
JOIN '+@DatabaseName+'.dbo.simple_EntityList el ON dds.EntityID = el.EL_ID
WHERE el.EntityName <> ''null'' 
	AND el.EntityName > ''''
'

EXECUTE sp_executeSQL @SQL



/***********************/
-- Insert Into Pairs
/***********************/
INSERT INTO SN_Pairs (s_EntityID, s_EntityDisplay, r_EntityID, r_EntityDisplay, PairsCount)
SELECT  
	  s.EntityID as s_EntityID
	, s.EntityDisplay as s_EntityDisplay
	, r.EntityID as r_EntityID
	, r.EntityDisplay as r_EntityDisplay
	, COUNT(*) as PairsCount
FROM #sender s
JOIN #recipient r ON s.ArtifactID = r.ArtifactID
GROUP BY s.EntityID, s.EntityDisplay, r.EntityID, r.EntityDisplay



 --SELECT * FROM SN_Pairs
 



