

USE SocialNetwork


SET NOCOUNT ON


DECLARE @DatabaseName nvarchar(300) 

IF @DatabaseName is NULL
BEGIN
	SELECT @DatabaseName = (SELECT DB_NAME())
END

DECLARE @SQL nvarchar(max) = N''

if OBJECT_ID('SN_Case_Top_Pairs') is not null drop table SN_Case_Top_Pairs

CREATE TABLE SN_Case_Top_Pairs
(
	  s_EntityID bigint
	, s_EntityDisplay nvarchar(max)
	, r_EntityID bigint
	, r_EntityDisplay nvarchar(max)
	, PairsCount int
)

INSERT INTO SN_Case_Top_Pairs (s_EntityID, s_EntityDisplay, r_EntityID, r_EntityDisplay, PairsCount)
SELECT TOP 100
	  p1.s_EntityID
	, p1.s_EntityDisplay
	, p1.r_EntityID
	, p1.r_EntityDisplay
	, ISNULL(p1.PairsCount, 0) + ISNULL(p2.PairsCount, 0) as PairsCount
FROM SN_Pairs p1
LEFT JOIN SN_Pairs p2 ON p1.s_EntityID = p2.r_EntityID AND p1.r_EntityID = p2.s_EntityID
WHERE ((ISNULL(p1.PairsCount, 0) > ISNULL(p2.PairsCount, 0)) OR ((ISNULL(p1.PairsCount, 0) = ISNULL(p2.PairsCount, 0)) AND p1.s_EntityID < p2.s_EntityID))
ORDER BY ISNULL(p1.PairsCount, 0) + ISNULL(p2.PairsCount, 0) DESC

--SELECT * FROM SN_Case_Top_Pairs



