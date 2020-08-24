-- Server localhost
-- EXEC usp_SN_GetTopCommunicators 6621
-- EXEC GetNetworkGraph

--DROP PROCEDURE usp_GetNetworkGraph
USE SocialNetwork
GO

CREATE PROCEDURE dbo.usp_SN_GetGraphGeneration10_Case @SenderEntityID int = -1, @RecipientEntityID int = -1
AS

	/*
	DECLARE @SenderEntityID int = 6621
	DECLARE @RecipientEntityID int = -1
	*/
	
	SELECT TOP 10 s_EntityID, s_EntityDisplay, theCount 
	FROM SN_Case_Top_Senders
	ORDER BY theCount DESC

	SELECT TOP 10 r_EntityID, r_EntityDisplay, theCount 
	FROM SN_Case_Top_Recipients
	ORDER BY theCount DESC

	SELECT TOP 10 s_EntityID, s_EntityDisplay, r_EntityID, r_EntityDisplay, PairsCount 
	FROM SN_Case_Top_Pairs
	ORDER BY PairsCount DESC

	SELECT *
	FROM SN_Case_Graph10
	ORDER BY PairsCount DESC, s_EntityDisplay ASC, r_EntityDisplay ASC
	


RETURN
GO