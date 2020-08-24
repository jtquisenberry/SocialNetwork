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
			   --
			   
			ORDER BY ISNULL(p1.PairsCount, 0) + ISNULL(p2.PairsCount, 0) DESC
		)

, cteGeneration1 as
	(
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
		--ORDER BY PairsCount DESC, s_EntityID
	)
--SELECT * FROM cteGeneration1

, cteEntityCombinations as
(
	SELECT a.s_EntityID, a.s_EntityDisplay, b.s_EntityID as r_EntityID, b.s_EntityDisplay as r_EntityDisplay FROM
	(
		SELECT s_EntityID, s_EntityDisplay
		FROM cteGeneration1
		UNION
		SELECT r_EntityID, r_EntityDisplay
		FROM cteGeneration1
	) a
	CROSS JOIN 
	(
		SELECT s_EntityID, s_EntityDisplay
		FROM cteGeneration1
		UNION
		SELECT r_EntityID, r_EntityDisplay
		FROM cteGeneration1
	) b
)

, cteEdges as
(
	SELECT p.s_EntityID, p.s_EntityDisplay, p.r_EntityID, p.r_EntityDisplay, p.PairsCount
	FROM cteEntityCombinations ec
	JOIN SN_Pairs p 
	ON (ec.s_EntityID = p.s_EntityID AND ec.r_EntityID = p.r_EntityID) OR (ec.s_EntityID = p.r_EntityID AND ec.r_EntityID = p.s_EntityID)
)

SELECT c.s_EntityID, c.s_EntityDisplay, c.r_EntityID, c.r_EntityDisplay,
	CASE WHEN (c.PairsCount > d.PairsCount) OR (c.PairsCount = d.PairsCount AND c.s_EntityID > c.r_EntityID) OR (c.s_EntityID = c.r_EntityID) THEN 'sender' ELSE 'recipient' END as theRole
--SELECT *
FROM 
(
	SELECT DISTINCT s_EntityID, s_EntityDisplay, r_EntityID, r_EntityDisplay, PairsCount 
	FROM cteEdges e1
) c
LEFT JOIN 
(
	SELECT DISTINCT s_EntityID, s_EntityDisplay, r_EntityID, r_EntityDisplay, PairsCount  
	FROM cteEdges e1
) d
ON c.s_EntityID = d.r_EntityID AND c.r_EntityID = d.s_EntityID




--ORDER BY s_EntityDisplay, r_EntityDisplay



--LEFT JOIN cteEdges e2 ON e1.s_EntityID = e2.r_EntityID AND e1.r_EntityID = e2.s_EntityID