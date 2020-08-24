

/****************/
-- Top Senders - No Entity Selected
/****************/

-- Top Senders Method 1
SELECT TOP 10 EntityID, EntityDisplay, COUNT(*)
FROM SN_Senders
GROUP BY EntityID, EntityDisplay
ORDER BY COUNT(*) DESC

-- Top Senders Method 2
SELECT TOP 10 EntityID, COUNT(*)
FROM
(
	SELECT DISTINCT EntityID, ArtifactID
	FROM simple_SocialNetwork
	WHERE EntityType = 1.0
) a
GROUP BY EntityID
ORDER BY COUNT(*) DESC


/****************/
-- Top Senders - Entity 6621 Selected
/****************/

-- Top Senders Method 1
SELECT TOP 10 s_EntityID, s_EntityDisplay, PairsCount
FROM SN_Pairs
WHERE r_EntityID = 6621
ORDER BY PairsCount DESC

-- Top Senders Method 2
SELECT TOP 10 EntityID, COUNT(*)
FROM
(
	SELECT DISTINCT sn2.EntityID, sn2.artifactid
	FROM 
	(
		SELECT ArtifactID 
		FROM simple_SocialNetwork
		WHERE EntityID = 6621
			AND EntityType > 1.0
	) sn1
	JOIN simple_SocialNetwork sn2
	ON sn1.artifactid = sn2.artifactid
	WHERE EntityType = 1.0
) a
GROUP BY EntityID
ORDER BY COUNT(*) DESC



/****************/
-- Top Recipients - No Entity Selected
/****************/

-- Top Recipients Method 1
SELECT TOP 10 EntityID, EntityDisplay, COUNT(*)
FROM SN_Recipients
GROUP BY EntityID, EntityDisplay
ORDER BY COUNT(*) DESC

-- Top Recipients Method 2
SELECT TOP 10 EntityID, COUNT(*)
FROM
(
	SELECT DISTINCT EntityID, ArtifactID
	FROM simple_SocialNetwork
	WHERE EntityType > 1.0
) a
GROUP BY EntityID
ORDER BY COUNT(*) DESC

SELECT * FROM simple_EntityList WHERE EL_ID = 14100


/****************/
-- Top Recipients - Entity 6621 Selected
/****************/
 
-- Top Senders Method 1
SELECT TOP 10 r_EntityID, r_EntityDisplay, PairsCount
FROM SN_Pairs
WHERE s_EntityID = 6621
ORDER BY PairsCount DESC

-- Top Senders Method 2
SELECT TOP 10 EntityID, COUNT(*)
FROM
(
	SELECT DISTINCT sn2.EntityID, sn2.artifactid
	FROM 
	(
		SELECT ArtifactID 
		FROM simple_SocialNetwork
		WHERE EntityID = 6621
			AND EntityType = 1.0
	) sn1
	JOIN simple_SocialNetwork sn2
	ON sn1.artifactid = sn2.artifactid
	WHERE EntityType > 1.0
) a
GROUP BY EntityID
ORDER BY COUNT(*) DESC


/****************/
-- Top Sender <-> Recipient Pairs - No Entity Selected
/****************/

SELECT TOP 10
	  p1.s_EntityID
	, p1.s_EntityDisplay
	, p1.r_EntityID
	, p1.r_EntityDisplay
	, ISNULL(p1.PairsCount, 0) + ISNULL(p2.PairsCount, 0) as PairsCount
FROM SN_Pairs p1
LEFT JOIN SN_Pairs p2 ON p1.s_EntityID = p2.r_EntityID AND p1.r_EntityID = p2.s_EntityID
WHERE ((ISNULL(p1.PairsCount, 0) > ISNULL(p2.PairsCount, 0)) OR ((ISNULL(p1.PairsCount, 0) = ISNULL(p2.PairsCount, 0)) AND p1.s_EntityID < p2.s_EntityID))
ORDER BY ISNULL(p1.PairsCount, 0) + ISNULL(p2.PairsCount, 0) DESC


/****************/
-- Top Sender <-> Recipient Pairs - Entity 6621 Selected
/****************/
 
SELECT TOP 10
	  p1.s_EntityID
	, p1.s_EntityDisplay
	, p1.r_EntityID
	, p1.r_EntityDisplay
	, ISNULL(p1.PairsCount, 0) + ISNULL(p2.PairsCount, 0) as PairsCount
FROM SN_Pairs p1
LEFT JOIN SN_Pairs p2 ON p1.s_EntityID = p2.r_EntityID AND p1.r_EntityID = p2.s_EntityID
WHERE ((ISNULL(p1.PairsCount, 0) > ISNULL(p2.PairsCount, 0)) OR ((ISNULL(p1.PairsCount, 0) = ISNULL(p2.PairsCount, 0)) AND p1.s_EntityID < p2.s_EntityID))
	AND (p1.s_EntityID = 6621 OR p1.r_EntityID = 6621)
ORDER BY ISNULL(p1.PairsCount, 0) + ISNULL(p2.PairsCount, 0) DESC



/****************/
-- Generation 1.0 Graph <-> Recipient Pairs - No Entity Selected
/****************/

; with cteTopCommunicators as
(
	SELECT TOP 20
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

SELECT 
	  p1_s_EntityID as s_EntityID
	, p1_s_EntityDisplay as s_EntityDisplay
	, p1_r_EntityID as r_EntityID
	, p1_r_EntityDisplay as r_EntityDisplay
	, p1_PairsCount as PairsCount
FROM cteTopCommunicators
UNION ALL
SELECT 
	  p2_s_EntityID
	, p2_s_EntityDisplay
	, p2_r_EntityID
	, p2_r_EntityDisplay
	, p2_PairsCount
FROM cteTopCommunicators
WHERE p2_s_EntityID is NOT NULL
ORDER BY PairsCount DESC, s_EntityID




/****************/
-- Generation 1.0 Graph <-> Recipient Pairs - Entity 6621 Selected
/****************/

; with cteTopCommunicators as
(
	SELECT TOP 20
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
		AND (p1.s_EntityID = 6621 OR p1.r_EntityID = 6621)
	ORDER BY ISNULL(p1.PairsCount, 0) + ISNULL(p2.PairsCount, 0) DESC
)

SELECT 
	  p1_s_EntityID as s_EntityID
	, p1_s_EntityDisplay as s_EntityDisplay
	, p1_r_EntityID as r_EntityID
	, p1_r_EntityDisplay as r_EntityDisplay
	, p1_PairsCount as PairsCount
FROM cteTopCommunicators
UNION ALL
SELECT 
	  p2_s_EntityID
	, p2_s_EntityDisplay
	, p2_r_EntityID
	, p2_r_EntityDisplay
	, p2_PairsCount
FROM cteTopCommunicators
WHERE p2_s_EntityID is NOT NULL
ORDER BY PairsCount DESC, s_EntityID





























/****************/
-- Top Sender -> Recipient Pairs - Entity 6621 Selected
/****************/

-- Method 1

SELECT TOP 10 * 
FROM SN_Pairs 
WHERE s_EntityID = 6621 OR r_EntityID = 6621
ORDER BY PairsCount DESC




; with CteC as
(
	SELECT TOP 20
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
		, ISNULL(p1.PairsCount, 0) + ISNULL(p2.PairsCount, 0) as p1_p2_Pairs
	FROM SN_Pairs p1
	LEFT JOIN SN_Pairs p2 ON p1.s_EntityID = p2.r_EntityID AND p1.r_EntityID = p2.s_EntityID
	WHERE p1.s_EntityID = 6621 OR p1.r_EntityID = 6621
	ORDER BY ISNULL(p1.PairsCount, 0) + ISNULL(p2.PairsCount, 0) DESC
)

SELECT TOP 10 p1_s_EntityID, p1_s_EntityDisplay, p1_r_EntityID, p1_r_EntityDisplay, SUM(p1_PairsCount)
FROM
(
	SELECT 
		  p1_s_EntityID
		, p1_s_EntityDisplay
		, p1_r_EntityID
		, p1_r_EntityDisplay
		, p1_PairsCount
	FROM CteC
	UNION ALL
	SELECT 
		  p2_s_EntityID
		, p2_s_EntityDisplay
		, p2_r_EntityID
		, p2_r_EntityDisplay
		, p2_PairsCount
	FROM CteC
	WHERE p2_s_EntityID is NOT NULL
) a
GROUP BY p1_s_EntityID, p1_s_EntityDisplay, p1_r_EntityID, p1_r_EntityDisplay
ORDER BY SUM(p1_PairsCount) DESC
