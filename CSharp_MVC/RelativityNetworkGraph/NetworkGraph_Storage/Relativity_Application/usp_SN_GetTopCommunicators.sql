-- Server NCSSQL84250\NCSSQL84250
-- EXEC usp_SN_GetTopCommunicators 2, 6615
-- EXEC GetNetworkGraph

--DROP PROCEDURE usp_GetNetworkGraph


CREATE PROCEDURE dbo.usp_SN_GetTopCommunicators @Levels int = 2, @EntityID int = 6615
AS


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

SELECT tc.s_EntityID, tc.s_EntityDisplay, p.r_EntityID, p.r_EntityDisplay, p.PairsCount, 'sender' as theRole FROM cteTopCommunicators tc
JOIN SN_Pairs p ON (tc.s_EntityID = p.s_EntityID AND tc.r_EntityID = p.r_EntityID) 
UNION ALL
SELECT tc.s_EntityID, tc.s_EntityDisplay, tc.r_EntityID, tc.r_EntityDisplay, p.PairsCount, 'recipient' FROM cteTopCommunicators tc
JOIN SN_Pairs p ON (tc.r_EntityID = p.s_EntityID AND tc.s_EntityID = p.r_EntityID)

RETURN
GO