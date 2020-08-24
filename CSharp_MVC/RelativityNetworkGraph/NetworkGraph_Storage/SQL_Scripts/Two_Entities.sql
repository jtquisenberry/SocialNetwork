/*
DECLARE @DBID smallint = DB_ID()
SELECT @DBID
DBCC DROPCLEANBUFFERS 
DBCC FLUSHPROCINDB(@DBID)
DBCC FREEPROCCACHE 
DBCC FREESESSIONCACHE
*/


/********************************/
-- Base query
/********************************/

SELECT ArtifactID
FROM
(
	SELECT ArtifactID
	FROM simple_SocialNetwork
	WHERE EntityID = 6621
		AND EntityType = 1.00
	INTERSECT
	SELECT ArtifactID
	FROM simple_SocialNetwork
	WHERE EntityID = 17072
		AND EntityType > 1.00
) a
UNION
SELECT ArtifactID 
FROM
(
	SELECT ArtifactID
	FROM simple_SocialNetwork
	WHERE EntityID = 17072
		AND EntityType = 1.00
	INTERSECT
	SELECT ArtifactID
	FROM simple_SocialNetwork
	WHERE EntityID = 6621
		AND EntityType > 1.00
) b



/**********************************************/
-- Top Senders Two Entities
/**********************************************/

-- Method 1, 0 seconds

SELECT TOP 10 sn.EntityID, el.EntityDisplay, COUNT(*) FROM
(
	SELECT ArtifactID
	FROM
	(
		SELECT ArtifactID
		FROM simple_SocialNetwork
		WHERE EntityID = 6621
			AND EntityType = 1.00
		INTERSECT
		SELECT ArtifactID
		FROM simple_SocialNetwork
		WHERE EntityID = 17072
			AND EntityType > 1.00
		) a
		UNION
		SELECT ArtifactID 
		FROM
		(
		SELECT ArtifactID
		FROM simple_SocialNetwork
		WHERE EntityID = 17072
			AND EntityType = 1.00
		INTERSECT
		SELECT ArtifactID
		FROM simple_SocialNetwork
		WHERE EntityID = 6621
			AND EntityType > 1.00
	) b
) recipient
JOIN simple_SocialNetwork sn ON 
recipient.ArtifactID = sn.artifactid AND sn.EntityType = 1.0
JOIN simple_EntityList el ON sn.EntityID = el.EL_ID
GROUP BY sn.EntityID, el.EntityDisplay
ORDER BY COUNT(*) DESC


-- Method 2, 0 seconds
SELECT sender.EntityID, sender.EntityDisplay, COUNT(*)
FROM
(
	SELECT a.ArtifactID
	FROM
	(
		SELECT ArtifactID
		FROM SN_Senders
		WHERE EntityID = 6621
	) a
	JOIN
	(
		SELECT ArtifactID
		FROM SN_Recipients
		WHERE EntityID = 17072
	) b
	ON a.ArtifactID = b.ArtifactID
	UNION
	SELECT a.ArtifactID
	FROM
	(
		SELECT ArtifactID
		FROM SN_Senders
		WHERE EntityID = 17072
	) a
	JOIN
	(
		SELECT ArtifactID
		FROM SN_Recipients
		WHERE EntityID = 6621
	) b
	ON a.ArtifactID = b.ArtifactID
) recipient
JOIN SN_Senders sender ON recipient.ArtifactID = sender.ArtifactID
GROUP BY sender.EntityID, sender.EntityDisplay
ORDER BY COUNT(*) DESC




/**********************************************/
-- Top Recipients Two Entities
/**********************************************/

-- Method 1, 7 seconds

SELECT TOP 10 EntityID, EntityDisplay, COUNT(*)
FROM
(
SELECT DISTINCT sn.EntityID, el.EntityDisplay, sn.ArtifactID FROM
(
	SELECT ArtifactID
	FROM
	(
	SELECT ArtifactID
	FROM simple_SocialNetwork
	WHERE EntityID = 6621
		AND EntityType = 1.00
	INTERSECT
	SELECT ArtifactID
	FROM simple_SocialNetwork
	WHERE EntityID = 17072
		AND EntityType > 1.00
	) a
	UNION
	SELECT ArtifactID 
	FROM
	(
	SELECT ArtifactID
	FROM simple_SocialNetwork
	WHERE EntityID = 17072
		AND EntityType = 1.00
	INTERSECT
	SELECT ArtifactID
	FROM simple_SocialNetwork
	WHERE EntityID = 6621
		AND EntityType > 1.00
	) b
) sender
JOIN simple_SocialNetwork sn ON 
sender.artifactid = sn.artifactid AND sn.EntityType > 1.0
JOIN simple_EntityList el ON sn.EntityID = el.EL_ID
WHERE el.EntityDisplay > ''
) z
GROUP BY EntityID, EntityDisplay
ORDER BY COUNT(*) DESC
-- seven seconds


-- Method 2, 0 seconds
SELECT c.EntityID, d.EntityDisplay, c.theCount
FROM
(
	SELECT TOP 10 recipient.EntityID,  COUNT(*) as theCount
	FROM
	(
		SELECT a.ArtifactID
		FROM
		(
			SELECT ArtifactID
			FROM SN_Senders
			WHERE EntityID = 6621
		) a
		JOIN
		(
			SELECT ArtifactID
			FROM SN_Recipients
			WHERE EntityID = 17072
		) b
		ON a.ArtifactID = b.ArtifactID
		UNION
		SELECT a.ArtifactID
		FROM
		(
			SELECT ArtifactID
			FROM SN_Senders
			WHERE EntityID = 17072
		) a
		JOIN
		(
			SELECT ArtifactID
			FROM SN_Recipients
			WHERE EntityID = 6621
		) b
		ON a.ArtifactID = b.ArtifactID
	) sender
	JOIN SN_Recipients recipient ON sender.ArtifactID = recipient.ArtifactID
	GROUP BY recipient.EntityID
	ORDER BY theCount DESC
) c JOIN 
simple_EntityList d ON c.EntityID = d.EL_ID
-- 0 seconds


	
/**********************************************/
-- Top Pairs Two Entities
/**********************************************/
; with ctePairs1 as
(
	SELECT s.EntityID as s_EntityID, r.EntityID as r_EntityID, COUNT(*) as theCount
	FROM
	(
		SELECT a.ArtifactID
		FROM
		(
			SELECT ArtifactID
			FROM SN_Senders
			WHERE EntityID = 6621
		) a
		JOIN
		(
			SELECT ArtifactID
			FROM SN_Recipients
			WHERE EntityID = 17072
		) b
		ON a.ArtifactID = b.ArtifactID
		UNION
		SELECT a.ArtifactID
		FROM
		(
			SELECT ArtifactID
			FROM SN_Senders
			WHERE EntityID = 17072
		) a
		JOIN
		(
			SELECT ArtifactID
			FROM SN_Recipients
			WHERE EntityID = 6621
		) b
		ON a.ArtifactID = b.ArtifactID
	) document
	JOIN SN_Senders s ON document.ArtifactID = s.ArtifactID
	JOIN SN_Recipients r ON document.ArtifactID = r.ArtifactID
	GROUP BY s.EntityID, r.EntityID
)

SELECT 
	  pairs1.s_EntityID
	, pairs1.r_EntityID
	, pairs1.theCount
	, pairs2.s_EntityID
	, pairs2.r_EntityID
	, pairs2.theCount
	, pairs1.theCount + ISNULL(pairs2.theCount, 0) as Count2 FROM ctePairs1 pairs1 
	
LEFT JOIN ctePairs1 pairs2 ON pairs1.s_EntityID = pairs2.r_EntityID AND pairs1.r_EntityID = pairs2.s_EntityID
WHERE (pairs1.theCount > ISNULL(pairs2.theCount, 0)) OR ((pairs1.theCount = ISNULL(pairs2.theCount,0))  AND (pairs1.s_EntityID < pairs2.r_EntityID)) 
OR ((pairs1.theCount = ISNULL(pairs2.theCount,0))  AND (pairs1.s_EntityID = pairs2.r_EntityID))
ORDER BY Count2 DESC

	
	
	
	
	
	




	





