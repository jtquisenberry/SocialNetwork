-- Server localhost
-- EXEC usp_SN_GetTopCommunicators15 2, 6615
-- EXEC GetNetworkGraph

--DROP PROCEDURE usp_GetNetworkGraph

USE SocialNetwork
GO

CREATE PROCEDURE dbo.usp_SN_GetGraphGeneration15 @SenderEntityID int = -1, @RecipientEntityID int = -1
AS

	/*
	DECLARE @SenderEntityID int = 6621
	DECLARE @RecipientEntityID int = -1
	*/
	
	
	DECLARE @Sql nvarchar(max) = N''
	DECLARE @SelectorClauseSender nvarchar(max) = N''
	DECLARE @SelectorClauseRecipient nvarchar(max) = N''
	DECLARE @SelectorClausePair nvarchar(max) = N''


	IF ((@SenderEntityID <= 0) AND (@RecipientEntityID <= 0))
	BEGIN
		SELECT @SelectorClauseSender = N''
		SELECT @SelectorClauseRecipient = N''
		SELECT @SelectorClausePair = N''
	END
	ELSE IF ((@SenderEntityID > 0) AND (@RecipientEntityID <= 0))
	BEGIN
		SELECT @SelectorClauseSender = N'WHERE s_EntityID = ' + CAST(@SenderEntityID as nvarchar(20))
		SELECT @SelectorClauseRecipient = N'WHERE r_EntityID = ' + CAST(@SenderEntityID as nvarchar(20))
		SELECT @SelectorClausePair = N' AND (p1.s_EntityID = ' + CAST(@SenderEntityID as nvarchar(20)) + ' OR p1.r_EntityID = ' + CAST(@SenderEntityID as nvarchar(20)) + ')'
	END
	ELSE IF ((@SenderEntityID <= 0) AND (@RecipientEntityID > 0))
	BEGIN
		SELECT @SelectorClauseSender = N'WHERE s_EntityID = ' + CAST(@RecipientEntityID as nvarchar(20))
		SELECT @SelectorClauseRecipient = N'WHERE r_EntityID = ' + CAST(@RecipientEntityID as nvarchar(20))
		SELECT @SelectorClausePair = N' AND (p1.s_EntityID = ' + CAST(@RecipientEntityID as nvarchar(20)) + ' OR p1.r_EntityID = ' + CAST(@RecipientEntityID as nvarchar(20)) + ')'
	END
	ELSE IF ((@SenderEntityID > 0) AND (@RecipientEntityID > 0))
	BEGIN
		SELECT @SelectorClauseSender = N'WHERE '+ 
			'(s_EntityID = ' + CAST(@SenderEntityID as nvarchar(20)) + ' AND ' + 'r_EntityID = ' + CAST(@RecipientEntityID as nvarchar(20)) + ')' + ' OR ' +
			'(s_EntityID = ' + CAST(@RecipientEntityID as nvarchar(20)) + ' AND ' + 'r_EntityID = ' + CAST(@SenderEntityID as nvarchar(20)) + ')'
		SELECT @SelectorClauseRecipient = N'WHERE '+ 
			'(s_EntityID = ' + CAST(@SenderEntityID as nvarchar(20)) + ' AND ' + 'r_EntityID = ' + CAST(@RecipientEntityID as nvarchar(20)) + ')' + ' OR ' +
			'(s_EntityID = ' + CAST(@RecipientEntityID as nvarchar(20)) + ' AND ' + 'r_EntityID = ' + CAST(@SenderEntityID as nvarchar(20)) + ')'
		SELECT @SelectorClausePair = N' AND (' + 
			'(s_EntityID = ' + CAST(@SenderEntityID as nvarchar(20)) + ' AND ' + 'r_EntityID = ' + CAST(@RecipientEntityID as nvarchar(20)) + ')' + ' OR ' +
			'(s_EntityID = ' + CAST(@RecipientEntityID as nvarchar(20)) + ' AND ' + 'r_EntityID = ' + CAST(@SenderEntityID as nvarchar(20)) + ')' +
			')'

	END


		IF OBJECT_ID('tempdb.dbo.#Generation1') is NOT NULL
	BEGIN
		DROP TABLE #Generation1
	END


	CREATE TABLE #Generation1
	(
		  r_EntityID bigint
		, r_EntityDisplay nvarchar(300)
		, PairsCount int
		, theRole nvarchar(20)
	)

	IF OBJECT_ID('tempdb.dbo.#Return') is NOT NULL
	BEGIN
		DROP TABLE #Return
	END


	CREATE TABLE #Return
	(
		  s_EntityID bigint
		, s_EntityDisplay nvarchar(300)
		, r_EntityID bigint
		, r_EntityDisplay nvarchar(300)
		, PairsCount int
		, theRole nvarchar(30)
	)






	/*
	-- Results Set #1
	SELECT TOP 10 s_EntityID, s_EntityDisplay, theCount = PairsCount
	FROM SN_Pairs
	WHERE r_EntityID = @EntityID
	ORDER BY PairsCount DESC
	*/

	-- Top Senders
	-- RecipientSelected
	
	IF ((@SenderEntityID <= 0) AND (@RecipientEntityID <= 0))
	BEGIN
		SELECT @Sql = N'
			SELECT TOP 10 EntityID as s_EntityID, EntityDisplay as s_EntityDisplay, COUNT(*) as theCount
			FROM SN_Senders
			GROUP BY EntityID, EntityDisplay
			ORDER BY COUNT(*) DESC, s_EntityDisplay ASC
		'
	END
	ELSE
	BEGIN
		SELECT @Sql = N'
			SELECT TOP 10 s_EntityID, s_EntityDisplay, theCount = PairsCount
			FROM SN_Pairs 
			'+@SelectorClauseRecipient+' 
			ORDER BY PairsCount DESC, s_EntityDisplay ASC
		'
	END
		
	EXECUTE sp_executesql @Sql


	/*
	-- Results Set #2
	SELECT TOP 10 r_EntityID, r_EntityDisplay, theCount = PairsCount 
	FROM SN_Pairs
	WHERE s_EntityID = @EntityID
	ORDER BY PairsCount DESC
	*/
	
	-- Top Recipients
	
	IF ((@SenderEntityID <= 0) AND (@RecipientEntityID <= 0))
	BEGIN
		SELECT @Sql = N'
			SELECT TOP 10 EntityID as r_EntityID, EntityDisplay as r_EntityDisplay, COUNT(*) as theCount
			FROM SN_Recipients
			GROUP BY EntityID, EntityDisplay
			ORDER BY COUNT(*) DESC, r_EntityDisplay ASC
		'
	END
	ELSE
	BEGIN
		SELECT @Sql = N'
			SELECT TOP 10 r_EntityID, r_EntityDisplay, theCount = PairsCount 
			FROM SN_Pairs
			'+@SelectorClauseSender+'
			ORDER BY PairsCount DESC, r_EntityDisplay ASC
		'
	END
		

	EXECUTE sp_executesql @Sql

	/*
	-- Results Set #3
	SELECT TOP 10 s_EntityID, s_EntityDisplay, r_EntityID, r_EntityDisplay, PairsCount
	FROM SN_Pairs
	WHERE s_EntityID = @EntityID OR r_EntityID = @EntityID
	ORDER BY PairsCount DESC
	*/

	/*
	--Top Pairs
	SELECT @Sql = N'
		SELECT TOP 10 s_EntityID, s_EntityDisplay, r_EntityID, r_EntityDisplay, PairsCount
		FROM SN_Pairs
		'+@SelectorClauseSender+'
		ORDER BY PairsCount DESC
	'
	*/

	SELECT @Sql = N'
		SELECT TOP 10
			  p1.s_EntityID
			, p1.s_EntityDisplay
			, p1.r_EntityID
			, p1.r_EntityDisplay
			, ISNULL(p1.PairsCount, 0) + ISNULL(p2.PairsCount, 0) as PairsCount
		FROM SN_Pairs p1
		LEFT JOIN SN_Pairs p2 ON p1.s_EntityID = p2.r_EntityID AND p1.r_EntityID = p2.s_EntityID
		WHERE ((ISNULL(p1.PairsCount, 0) > ISNULL(p2.PairsCount, 0)) OR ((ISNULL(p1.PairsCount, 0) = ISNULL(p2.PairsCount, 0)) AND p1.s_EntityID < p2.s_EntityID))
		    --'+REPLACE(REPLACE(@SelectorClauseSender, 'WHERE ', 'AND '), 's_EntityID', 'p1.s_EntityID')+'
			'+@SelectorClausePair+'
		ORDER BY ISNULL(p1.PairsCount, 0) + ISNULL(p2.PairsCount, 0) DESC, p1.s_EntityDisplay ASC, p1.r_EntityDisplay ASC
	'

	print @sql
	EXECUTE sp_executesql @Sql




	-- Graph
	-- Results Set #4



	SELECT @Sql = N'
		; with cteTopCommunicators as
		(
			SELECT TOP 20
				  p1.s_EntityID as p1_s_EntityID
				, p1.s_EntityDisplay as p1_s_EntityDisplay
				, p1.r_EntityID as p1_r_EntityID
				, p1.r_EntityDisplay as p1_r_EntityDisplay
				, p1.PairsCount as p1_PairsCount
				, p2.s_EntityID as p2_s_EntityID
				, p2.s_EntityDisplay as p2_s_EntityDisplay
				, p2.r_EntityID as p2_r_EntityID
				, p2.r_EntityDisplay as p2_r_EntityDisplay
				, p2.PairsCount as p2_PairsCount
				, ISNULL(p1.PairsCount, 0) + ISNULL(p2.PairsCount, 0) as  p2_PairsCount2
			FROM SN_Pairs p1
			LEFT JOIN SN_Pairs p2 ON p1.s_EntityID = p2.r_EntityID AND p1.r_EntityID = p2.s_EntityID
			WHERE ((ISNULL(p1.PairsCount, 0) > ISNULL(p2.PairsCount, 0)) OR ((ISNULL(p1.PairsCount, 0) = ISNULL(p2.PairsCount, 0)) AND p1.s_EntityID < p2.s_EntityID))
			   '+@SelectorClausePair+'
			ORDER BY ISNULL(p1.PairsCount, 0) + ISNULL(p2.PairsCount, 0) DESC
		)

		, cteGeneration1 as
		(
			SELECT 
				  p1_s_EntityID as s_EntityID
				, p1_s_EntityDisplay as s_EntityDisplay
				, p1_r_EntityID as r_EntityID
				, p1_r_EntityDisplay as r_EntityDisplay
				, p1_PairsCount as PairsCount
				, ''sender'' as theRole
			FROM cteTopCommunicators
			UNION ALL
			SELECT 
				  p2_r_EntityID
				, p2_r_EntityDisplay
				, p2_s_EntityID
				, p2_s_EntityDisplay
				, p2_PairsCount
				, ''recipient'' as theRole
			FROM cteTopCommunicators
			WHERE p2_s_EntityID is NOT NULL
			--ORDER BY PairsCount DESC, s_EntityID
		)

		, cteEntityCombinations as
		(
			SELECT a.s_EntityID, a.s_EntityDisplay, b.s_EntityID as r_EntityID, b.s_EntityDisplay as r_EntityDisplay FROM
			(
				SELECT s_EntityID, s_EntityDisplay
				FROM cteGeneration1
				UNION
				SELECT r_EntityID, r_EntityDisplay
				FROM cteGeneration1
			) a
			CROSS JOIN 
			(
				SELECT s_EntityID, s_EntityDisplay
				FROM cteGeneration1
				UNION
				SELECT r_EntityID, r_EntityDisplay
				FROM cteGeneration1
			) b
		)

		, cteEdges as
		(
			SELECT p.s_EntityID, p.s_EntityDisplay, p.r_EntityID, p.r_EntityDisplay, p.PairsCount
			FROM cteEntityCombinations ec
			JOIN SN_Pairs p 
			ON (ec.s_EntityID = p.s_EntityID AND ec.r_EntityID = p.r_EntityID) OR (ec.s_EntityID = p.r_EntityID AND ec.r_EntityID = p.s_EntityID)
		)

		SELECT c.s_EntityID, c.s_EntityDisplay, c.r_EntityID, c.r_EntityDisplay, c.PairsCount,
			CASE WHEN (c.PairsCount > d.PairsCount) OR (c.PairsCount = d.PairsCount AND c.s_EntityID > c.r_EntityID) OR (c.s_EntityID = c.r_EntityID) THEN ''sender'' ELSE ''recipient'' END as theRole
		--SELECT *
		FROM 
		(
			SELECT DISTINCT s_EntityID, s_EntityDisplay, r_EntityID, r_EntityDisplay, PairsCount 
			FROM cteEdges e1
		) c
		LEFT JOIN 
		(
			SELECT DISTINCT s_EntityID, s_EntityDisplay, r_EntityID, r_EntityDisplay, PairsCount  
			FROM cteEdges e1
		) d
		ON c.s_EntityID = d.r_EntityID AND c.r_EntityID = d.s_EntityID	
		ORDER BY c.PairsCount DESC, c.s_EntityID ASC, c.r_EntityID ASC
	'

	--Print(@Sql)
	EXECUTE sp_executesql @Sql
	

RETURN
GO