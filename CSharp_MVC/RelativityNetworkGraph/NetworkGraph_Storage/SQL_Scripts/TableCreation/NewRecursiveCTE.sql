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
			WHERE s_EntityID = 6621
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
			WHERE Level < 2
		)

		, cteB as
		(
			SELECT 
				  s_EntityID
				, s_EntityDisplay
				, r_EntityID
				, r_EntityDisplay
				, PairsCount
				, theRow
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
			--OPTION (MAXRECURSION 10000)
		)

		SELECT b1.s_EntityID, b1.s_EntityDisplay, b1.r_EntityID, b1.r_EntityDisplay, b1.PairsCount, 
			CASE WHEN ((b1.PairsCount > ISNULL(b2.PairsCount, 0)) OR ((b1.PairsCount = ISNULL(b2.PairsCount, 0) AND (b1.s_EntityID <= b1.r_EntityID))))
			THEN 'sender' ELSE 'recipient' END as theRole
		FROM cteB b1
		LEFT JOIN cteB b2 ON (b1.s_EntityID = b2.r_EntityID) AND (b1.r_EntityID = b2.s_EntityID)