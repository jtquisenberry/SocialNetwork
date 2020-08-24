

USE SocialNetwork


SET NOCOUNT ON

DECLARE @Levels int = 2
DECLARE @DatabaseName nvarchar(300) 

IF @DatabaseName is NULL
BEGIN
	SELECT @DatabaseName = (SELECT DB_NAME())
END

DECLARE @SQL nvarchar(max) = N''

if OBJECT_ID('SN_Case_Graph20') is not null drop table SN_Case_Graph20

CREATE TABLE SN_Case_Graph20
(
	  s_EntityID bigint
	, s_EntityDisplay nvarchar(max)
	, r_EntityID bigint
	, r_EntityDisplay nvarchar(max)
	, PairsCount int
	, theRole nvarchar(50)
)


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
	WHERE Level < @Levels
)

INSERT INTO SN_Case_Graph20 (s_EntityID, s_EntityDisplay, r_EntityID, r_EntityDisplay, PairsCount, theRole)
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

--SELECT * FROM SN_Case_Graph20



