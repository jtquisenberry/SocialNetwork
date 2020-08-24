

USE SocialNetwork


SET NOCOUNT ON


DECLARE @DatabaseName nvarchar(300) 

IF @DatabaseName is NULL
BEGIN
	SELECT @DatabaseName = (SELECT DB_NAME())
END

DECLARE @SQL nvarchar(max) = N''

if OBJECT_ID('SN_Case_Graph10') is not null drop table SN_Case_Graph10

CREATE TABLE SN_Case_Graph10
(
	  s_EntityID bigint
	, s_EntityDisplay nvarchar(max)
	, r_EntityID bigint
	, r_EntityDisplay nvarchar(max)
	, PairsCount int
	, theRole nvarchar(50)
)


; with cteTopCommunicators as
(
	SELECT TOP 100
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
	ORDER BY ISNULL(p1.PairsCount, 0) + ISNULL(p2.PairsCount, 0) DESC
)

INSERT INTO SN_Case_Graph10 (s_EntityID, s_EntityDisplay, r_EntityID, r_EntityDisplay, PairsCount, theRole)
SELECT 
	  p1_s_EntityID as s_EntityID
	, p1_s_EntityDisplay as s_EntityDisplay
	, p1_r_EntityID as r_EntityID
	, p1_r_EntityDisplay as r_EntityDisplay
	, p1_PairsCount as PairsCount
	, 'sender' as theRole
FROM cteTopCommunicators
UNION ALL
SELECT 
	  p2_r_EntityID
	, p2_r_EntityDisplay
	, p2_s_EntityID
	, p2_s_EntityDisplay
	, p2_PairsCount
	, 'recipient' as theRole
FROM cteTopCommunicators
WHERE p2_s_EntityID is NOT NULL
ORDER BY PairsCount DESC, s_EntityID

--SELECT * FROM SN_Case_Top_Pairs



