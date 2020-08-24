-- Server localhost
-- EXEC usp_SN_GetTopCommunicators 6621
-- EXEC GetNetworkGraph

--DROP PROCEDURE usp_GetNetworkGraph
USE SocialNetwork
GO

CREATE PROCEDURE dbo.usp_SN_GetGraphGeneration20_Case @Levels int =2, @SenderEntityID int = -1, @RecipientEntityID int = -1
AS

	/*
	DECLARE @SenderEntityID int = 6621
	DECLARE @RecipientEntityID int = -1
	*/
	
	SELECT TOP 10 s_EntityID, s_EntityDisplay, theCount 
	FROM SN_Case_Top_Senders
	ORDER BY theCount DESC, s_EntityDisplay ASC

	SELECT TOP 10 r_EntityID, r_EntityDisplay, theCount 
	FROM SN_Case_Top_Recipients
	ORDER BY theCount DESC, r_EntityDisplay ASC

	SELECT TOP 10 s_EntityID, s_EntityDisplay, r_EntityID, r_EntityDisplay, PairsCount 
	FROM SN_Case_Top_Pairs
	ORDER BY PairsCount DESC, s_EntityDisplay ASC, r_EntityDisplay ASC

	SELECT 
		  c1.s_EntityID
		, c1.s_EntityDisplay 
		, c1.r_EntityID 
		, c1.r_EntityDisplay 
		, c1.PairsCount
		, CASE WHEN ((c1.PairsCount > ISNULL(c2.PairsCount, 0)) OR ((c1.PairsCount = ISNULL(c2.PairsCount, 0) AND (c1.s_EntityID <= c1.r_EntityID))))
			THEN 'sender' ELSE 'recipient' END as theRole
	FROM SN_Case_Graph20 c1
	LEFT JOIN SN_Case_Graph20 c2
	ON (c1.s_EntityID = c2.r_EntityID) AND (c1.r_EntityID = c2.s_EntityID)
	ORDER BY PairsCount DESC, s_EntityDisplay ASC, r_EntityDisplay ASC
	


RETURN
GO