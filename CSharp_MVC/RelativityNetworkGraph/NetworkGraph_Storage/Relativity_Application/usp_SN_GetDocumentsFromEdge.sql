USE [DR_wtemp_SocialNetwork_EDDS1015989]
GO

--EXECUTE usp_SN_GetDocumentsFromEdge 106646, 81251

CREATE PROCEDURE [dbo].[usp_SN_GetDocumentsFromEdge] @SenderEntityID int = 1, @RecipientEntityID int = 1
AS


; with cteSenderDocuments
AS (
	SELECT DISTINCT ArtifactID 
	FROM simple_SocialNetwork
	WHERE EntityType in (1.0)
		AND EntityID = @SenderEntityID
)

, cteRecipientDocuments
AS (
	SELECT DISTINCT ArtifactID 
	FROM simple_SocialNetwork
	WHERE EntityType in (2.1, 2.2, 2.3, 4.0)
		AND EntityID = @RecipientEntityID
)

SELECT s.artifactid
FROM cteSenderDocuments s
JOIN cteRecipientDocuments r ON s.artifactid = r.artifactid


RETURN

GO


