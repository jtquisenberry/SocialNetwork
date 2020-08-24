

USE SocialNetwork
GO

--EXECUTE usp_SN_Scope_On_Sender 6621


--USE tempdb
CREATE PROCEDURE [dbo].[usp_SN_Scope_On_Pair] @SenderEntityID bigint = 1, @RecipientEntityID bigint = 1
AS

	DECLARE @DatabaseName nvarchar(300) 

	IF @DatabaseName is NULL
	BEGIN
		SELECT @DatabaseName = (SELECT DB_NAME())
	END

	DECLARE @SQL nvarchar(max) = N''

	if OBJECT_ID('tempdb..#sender') is not null drop table #sender
	if OBJECT_ID('tempdb..#recipient') is not null drop table #recipient
	if OBJECT_ID('tempdb..#tempSN_Pairs') is not null drop table #tempSN_Pairs

	CREATE TABLE #sender
	(
		  ArtifactID int
		, EntityID bigint
		, EntityDisplay nvarchar(max)
	)

	CREATE TABLE #recipient
	(
		  ArtifactID int
		, EntityID bigint
		, EntityDisplay nvarchar(max)
	)

	CREATE TABLE #tempSN_Pairs
	(
		  s_EntityID bigint
		, s_EntityDisplay nvarchar(max)
		, r_EntityID bigint
		, r_EntityDisplay nvarchar(max)
		, PairsCount int
	)


	SELECT @SQL = 
	N'	
	INSERT INTO #sender (ArtifactID, EntityID, EntityDisplay)
	SELECT dds.artifactid, dds.EntityID, el.EntityDisplay
	FROM
	(
		SELECT DISTINCT sn.artifactid, sn.EntityID 
		FROM
		(
			
			SELECT DISTINCT sn.ArtifactID
			FROM '+@DatabaseName+'.dbo.simple_SocialNetwork sn
			WHERE
					sn.EntityID = '+CAST(@SenderEntityID as nvarchar(10))+'
				AND sn.EntityType in (1.0)
			
			INTERSECT

			SELECT DISTINCT sn.ArtifactID
			FROM '+@DatabaseName+'.dbo.simple_SocialNetwork sn
			WHERE
					sn.EntityID = '+CAST(@RecipientEntityID as nvarchar(10))+'
				AND sn.EntityType in (2.1, 2.2, 2.3, 4.0)

		) sn2 JOIN '+@DatabaseName+'.dbo.simple_SocialNetwork sn
		ON sn2.artifactid = sn.artifactid
		WHERE sn.EntityID is NOT NULL
			AND sn.EntityType = 1.0
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
		SELECT DISTINCT sn.artifactid, sn.EntityID 
		FROM
		(
			SELECT DISTINCT sn.ArtifactID
			FROM '+@DatabaseName+'.dbo.simple_SocialNetwork sn
			WHERE
					sn.EntityID = '+CAST(@SenderEntityID as nvarchar(10))+'
				AND sn.EntityType in (1.0)
			
			INTERSECT

			SELECT DISTINCT sn.ArtifactID
			FROM '+@DatabaseName+'.dbo.simple_SocialNetwork sn
			WHERE
					sn.EntityID = '+CAST(@RecipientEntityID as nvarchar(10))+'
				AND sn.EntityType in (2.1, 2.2, 2.3, 4.0)
		) sn2 JOIN '+@DatabaseName+'.dbo.simple_SocialNetwork sn
		ON sn2.artifactid = sn.artifactid
		WHERE sn.EntityID is NOT NULL
			AND sn.EntityType in (2.1, 2.2, 2.3, 4.0)
	) dds -- Distinct Document-Recipients
	JOIN '+@DatabaseName+'.dbo.simple_EntityList el ON dds.EntityID = el.EL_ID
	WHERE el.EntityName <> ''null'' 
		AND el.EntityName > ''''
	'

	print @sql
	EXECUTE sp_executeSQL @SQL



	/***********************/
	-- Insert Into Pairs
	/***********************/
	INSERT INTO #tempSN_Pairs (s_EntityID, s_EntityDisplay, r_EntityID, r_EntityDisplay, PairsCount)
	SELECT  
		  s.EntityID as s_EntityID
		, s.EntityDisplay as s_EntityDisplay
		, r.EntityID as r_EntityID
		, r.EntityDisplay as r_EntityDisplay
		, COUNT(*) as PairsCount
	FROM #sender s
	JOIN #recipient r ON s.ArtifactID = r.ArtifactID
	GROUP BY s.EntityID, s.EntityDisplay, r.EntityID, r.EntityDisplay


	-- Results Set #1
	SELECT TOP 10 s_EntityID = EntityID, s_EntityDisplay = EntityDisplay, theCount = COUNT(*) FROM #sender GROUP BY EntityID, EntityDisplay ORDER BY COUNT(*) DESC

	-- Results Set #2
	SELECT TOP 10 r_EntityID = EntityID, r_EntityDisplay = EntityDisplay, theCount = COUNT(*) FROM #recipient GROUP BY EntityID, EntityDisplay ORDER BY COUNT(*) DESC

	-- Results Set #3
	SELECT TOP 10 * FROM #tempSN_Pairs ORDER BY PairsCount DESC


RETURN





/*

 --SELECT * FROM #tempSN_Pairs
 SELECT SUM(PairsCount) FROM #tempSN_Pairs


 
 


 */


