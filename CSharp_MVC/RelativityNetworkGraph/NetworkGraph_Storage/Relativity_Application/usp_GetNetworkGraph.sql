-- Server NCSSQL84250\NCSSQL84250
-- EXEC GetNetworkGraph 2, 6615
-- EXEC GetNetworkGraph

--DROP PROCEDURE usp_GetNetworkGraph

USE DR_wtemp_SocialNetwork_EDDS1030755
GO

CREATE PROCEDURE dbo.usp_GetNetworkGraph @Levels int = 2, @EntityID int = 6615
AS


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

RETURN
GO