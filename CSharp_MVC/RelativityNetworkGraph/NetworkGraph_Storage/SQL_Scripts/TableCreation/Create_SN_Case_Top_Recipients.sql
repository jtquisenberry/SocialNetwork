

USE SocialNetwork


SET NOCOUNT ON


DECLARE @DatabaseName nvarchar(300) 

IF @DatabaseName is NULL
BEGIN
	SELECT @DatabaseName = (SELECT DB_NAME())
END

DECLARE @SQL nvarchar(max) = N''

if OBJECT_ID('SN_Case_Top_Recipients') is not null drop table SN_Case_Top_Recipients

CREATE TABLE SN_Case_Top_Recipients
(
	  r_EntityID bigint
	, r_EntityDisplay nvarchar(max)
	, theCount int
)

INSERT INTO SN_Case_Top_Recipients (r_EntityID, r_EntityDisplay, theCount)
SELECT TOP 100 EntityID as r_EntityID, EntityDisplay as r_EntityDisplay, COUNT(*) as theCount
FROM SN_Recipients
GROUP BY EntityID, EntityDisplay
ORDER BY COUNT(*) DESC

--SELECT * FROM SN_Case_Top_Recipients



