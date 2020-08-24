

USE SocialNetwork


SET NOCOUNT ON


DECLARE @DatabaseName nvarchar(300) 

IF @DatabaseName is NULL
BEGIN
	SELECT @DatabaseName = (SELECT DB_NAME())
END

DECLARE @SQL nvarchar(max) = N''

if OBJECT_ID('SN_Case_Top_Senders') is not null drop table SN_Case_Top_Senders

CREATE TABLE SN_Case_Top_Senders
(
	  s_EntityID bigint
	, s_EntityDisplay nvarchar(max)
	, theCount int
)

INSERT INTO SN_Case_Top_Senders (s_EntityID, s_EntityDisplay, theCount)
SELECT TOP 100 EntityID as s_EntityID, EntityDisplay as s_EntityDisplay, COUNT(*) as theCount
FROM SN_Senders
GROUP BY EntityID, EntityDisplay
ORDER BY COUNT(*) DESC

--SELECT * FROM SN_Case_Top_Senders



