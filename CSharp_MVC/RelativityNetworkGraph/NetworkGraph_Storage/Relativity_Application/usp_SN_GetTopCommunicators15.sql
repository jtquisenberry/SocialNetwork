-- Server NCSSQL84250\NCSSQL84250
-- EXEC usp_SN_GetTopCommunicators15 2, 6615
-- EXEC GetNetworkGraph

--DROP PROCEDURE usp_GetNetworkGraph

USE DR_wtemp_SocialNetwork_EDDS1030755
GO

CREATE PROCEDURE dbo.usp_SN_GetTopCommunicators15 @Levels int = 2, @EntityID int = 6615
AS


IF OBJECT_ID('tempdb.dbo.#Generation1') is NOT NULL
BEGIN
	DROP TABLE #Generation1
END


CREATE TABLE #Generation1
(
	  r_EntityID int
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
      s_EntityID int
	, s_EntityDisplay nvarchar(300)
	, r_EntityID int
	, r_EntityDisplay nvarchar(300)
	, PairsCount int
	, theRole nvarchar(30)
)







/**********************/
-- Recursive CTE  
/**********************/
; with cteTopCommunicators as 
(
	SELECT TOP 20 s_EntityID, s_EntityDisplay, r_EntityID, r_EntityDisplay, SUM(PairsCount) as PairsCount
	FROM 
	(
		SELECT * FROM 
		(
			SELECT TOP 20 s_EntityID, s_EntityDisplay, r_EntityID, r_EntityDisplay, PairsCount FROM SN_Pairs p1
			WHERE s_EntityID = @EntityID
			ORDER BY PairsCount DESC
		) a
		UNION ALL
		SELECT * FROM 
		(
			SELECT TOP 20 r_EntityID, r_EntityDisplay, s_EntityID, s_EntityDisplay, PairsCount FROM SN_Pairs p2
			WHERE r_EntityID = @EntityID
			ORDER BY PairsCount DESC
		) b
	)  c
	GROUP BY s_EntityID, s_EntityDisplay, r_EntityID, r_EntityDisplay
	ORDER BY SUM(PairsCount) DESC
)

INSERT INTO #Generation1 (r_EntityID, r_EntityDisplay, PairsCount, theRole)
SELECT p.r_EntityID, p.r_EntityDisplay, p.PairsCount, 'sender' as theRole FROM cteTopCommunicators tc
JOIN SN_Pairs p ON (tc.s_EntityID = p.s_EntityID AND tc.r_EntityID = p.r_EntityID) 
UNION ALL
SELECT tc.r_EntityID, tc.r_EntityDisplay, p.PairsCount, 'recipient' FROM cteTopCommunicators tc
JOIN SN_Pairs p ON (tc.r_EntityID = p.s_EntityID AND tc.s_EntityID = p.r_EntityID)


INSERT INTO #Return
SELECT g1.r_EntityID as s_EntityID, g1.r_EntityDisplay as s_EntityDisplay, g2.r_EntityID, g2.r_EntityDisplay, p.PairsCount, 'sender' as theRole
FROM (SELECT DISTINCT r_EntityID, r_EntityDisplay FROM #Generation1) g1
CROSS JOIN (SELECT DISTINCT r_EntityID, r_EntityDisplay FROM #Generation1) g2 --ORDER BY g1.r_EntityDisplay, g2.r_EntityDisplay
JOIN SN_Pairs p ON (g1.r_EntityID = p.s_EntityID AND g2.r_EntityID = p.r_EntityID)
UNION ALL
SELECT g1.r_EntityID, g1.r_EntityDisplay, g2.r_EntityID, g2.r_EntityDisplay, p.PairsCount, 'recipient' as theRole
FROM (SELECT DISTINCT r_EntityID, r_EntityDisplay FROM #Generation1) g1
CROSS JOIN (SELECT DISTINCT r_EntityID, r_EntityDisplay FROM #Generation1) g2 
JOIN SN_Pairs p ON (g1.r_EntityID = p.r_EntityID AND g2.r_EntityID = p.s_EntityID)

INSERT INTO #Return
EXECUTE usp_SN_GetTopCommunicators @Levels, @EntityID

SELECT * FROM #Return








RETURN
GO