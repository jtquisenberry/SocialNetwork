-- Server localhost
-- EXEC usp_SN_GetGraphGeneration20 2, 6621
-- EXEC GetNetworkGraph

--DROP PROCEDURE usp_GetNetworkGraph

USE SocialNetwork
GO

CREATE PROCEDURE dbo.usp_SN_GetGraphGeneration20 @Levels int = 2, @SenderEntityID int = -1, @RecipientEntityID int = -1
AS

	/*
	DECLARE @SenderEntityID int = 6621
	DECLARE @RecipientEntityID int = -1
	DECLARE @Levels int = 2
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
			'(p1.s_EntityID = ' + CAST(@SenderEntityID as nvarchar(20)) + ' AND ' + 'p1.r_EntityID = ' + CAST(@RecipientEntityID as nvarchar(20)) + ')' + ' OR ' +
			'(p1.s_EntityID = ' + CAST(@RecipientEntityID as nvarchar(20)) + ' AND ' + 'p1.r_EntityID = ' + CAST(@SenderEntityID as nvarchar(20)) + ')' +
			')'
	END
	
	
	/*	
	-- Results Set #1
	SELECT TOP 10 s_EntityID, s_EntityDisplay, theCount = PairsCount
	FROM SN_Pairs
	WHERE r_EntityID = @EntityID
	ORDER BY PairsCount DESC
	*/

	-- Top Senders
	SELECT @Sql = N'
		SELECT TOP 10 s_EntityID, s_EntityDisplay, theCount = PairsCount
		FROM SN_Pairs
		'+@SelectorClauseRecipient+'
		ORDER BY PairsCount DESC, s_EntityDisplay ASC
	'

	EXECUTE sp_executesql @Sql

	/*	
	-- Results Set #2
	SELECT TOP 10 r_EntityID, r_EntityDisplay, theCount = PairsCount 
	FROM SN_Pairs
	WHERE s_EntityID = @EntityID
	ORDER BY PairsCount DESC
	*/

	-- Top Recipients
	SELECT @Sql = N'
		SELECT TOP 10 r_EntityID, r_EntityDisplay, theCount = PairsCount 
		FROM SN_Pairs
		'+@SelectorClauseSender+'
		ORDER BY PairsCount DESC, r_EntityDisplay ASC
	'
	EXECUTE sp_executesql @Sql

	/*	
	-- Results Set #3
	SELECT TOP 10 s_EntityID, s_EntityDisplay, r_EntityID, r_EntityDisplay, PairsCount
	FROM SN_Pairs
	WHERE s_EntityID = @EntityID OR r_EntityID = @EntityID
	ORDER BY PairsCount DESC
	*/

	--Top Pairs
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
	
	
	/*
	-- Results Set #4
	/**********************/
	-- Recursive CTE  
	/**********************/
	; with cteA as 
	(
		SELECT TOP 20 
			  p1.s_EntityID
			, p1.s_EntityDisplay
			, p1.r_EntityID
			, p1.r_EntityDisplay
			, p1.PairsCount 
			, 1 as Level
		FROM SN_Pairs p1
		WHERE p1.s_EntityID = /* Parameter --> */ @EntityID 
		ORDER BY p1.PairsCount DESC
		UNION ALL
		SELECT 
			  c1.r_EntityID
			, c1.r_EntityDisplay
			, p3.r_EntityID
			, p3.r_EntityDisplay
			, p3.PairsCount 
			, Level + 1
		FROM 
		cteA c1 JOIN SN_Pairs p3 ON c1.r_EntityID = p3.s_EntityID
		WHERE Level < /* Parameter --> */ @Levels
	)
	SELECT *
	FROM 
	(
		SELECT 
			  s_EntityID
			, s_EntityDisplay
			, r_EntityID
			, r_EntityDisplay
			, PairsCount
			, ROW_NUMBER() OVER (Partition By s_EntityID ORDER BY PairsCount DESC) as theRow
		FROM 
		(	
			SELECT DISTINCT
				  s_EntityID
				, s_EntityDisplay
				, r_EntityID
				, r_EntityDisplay
				, PairsCount
			FROM cteA
			--WHERE s_EntityID = 6615
		) a 
	) b
	WHERE theRow <= 20
	OPTION (MAXRECURSION 10000)
	*/

	-- Results Set #4
	-- Graph
	SELECT @Sql = N'
		; with cteA as 
		(
			SELECT TOP 20 
				  p1.s_EntityID
				, p1.s_EntityDisplay
				, p1.r_EntityID
				, p1.r_EntityDisplay
				, p1.PairsCount 
				, 1 as Level
			FROM SN_Pairs p1
			'+@SelectorClauseSender+'
			ORDER BY p1.PairsCount DESC
			UNION ALL
			SELECT 
				  c1.r_EntityID
				, c1.r_EntityDisplay
				, p3.r_EntityID
				, p3.r_EntityDisplay
				, p3.PairsCount 
				, Level + 1
			FROM 
			cteA c1 JOIN SN_Pairs p3 ON c1.r_EntityID = p3.s_EntityID
			WHERE Level < '+CAST(@Levels as nvarchar(2))+'
		)
		
		, cteB as
		(
			SELECT 
				  s_EntityID
				, s_EntityDisplay
				, r_EntityID
				, r_EntityDisplay
				, PairsCount
				, theRow
			FROM 
			(
				SELECT 
					  s_EntityID
					, s_EntityDisplay
					, r_EntityID
					, r_EntityDisplay
					, PairsCount
					, ROW_NUMBER() OVER (Partition By s_EntityID ORDER BY PairsCount DESC) as theRow
				FROM 
				(	
					SELECT DISTINCT
						  s_EntityID
						, s_EntityDisplay
						, r_EntityID
						, r_EntityDisplay
						, PairsCount
					FROM cteA
				) a 
			) b
			WHERE theRow <= 20
		)

		SELECT b1.s_EntityID, b1.s_EntityDisplay, b1.r_EntityID, b1.r_EntityDisplay, b1.PairsCount, 
			CASE WHEN ((b1.PairsCount > ISNULL(b2.PairsCount, 0)) OR ((b1.PairsCount = ISNULL(b2.PairsCount, 0) AND (b1.s_EntityID <= b1.r_EntityID))))
			THEN ''sender'' ELSE ''recipient'' END as theRole
		FROM cteB b1
		LEFT JOIN cteB b2 ON (b1.s_EntityID = b2.r_EntityID) AND (b1.r_EntityID = b2.s_EntityID)
		ORDER BY PairsCount DESC, s_EntityDisplay ASC, r_EntityDisplay ASC
	'

	--Print(@Sql)
	EXECUTE sp_executesql @Sql



RETURN
GO