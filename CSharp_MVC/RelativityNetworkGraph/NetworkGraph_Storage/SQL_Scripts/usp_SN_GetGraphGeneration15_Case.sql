-- Server localhost
-- EXEC usp_SN_GetTopCommunicators 6621
-- EXEC GetNetworkGraph

--DROP PROCEDURE usp_GetNetworkGraph
USE SocialNetwork
GO

CREATE PROCEDURE dbo.usp_SN_GetGraphGeneration15_Case @SenderEntityID int = -1, @RecipientEntityID int = -1
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

	SELECT s_EntityID, s_EntityDisplay, r_EntityID, r_EntityDisplay, PairsCount, theRole
	FROM SN_Case_Graph15
	ORDER BY PairsCount DESC, s_EntityDisplay ASC, r_EntityDisplay ASC
	


RETURN
GO