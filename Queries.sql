/* 
KEY WORDS:

UNION
UNION ALL
INTERSECT
EXCEPT
LEFT JOIN
INNER JOIN
FULL OUTER JOIN / FULL JOIN
RIGHT JOIN (unlikely)

*/


/* Find shoulder exercises that are in both Anterior and Lateral Deltoid Tables*/
SELECT Exercise, MuscleGroup, IsCompound
FROM AnteriorDeltoid
INTERSECT
SELECT Exercise, MuscleGroup, IsCompound
FROM LateralDeltoid


/* All Leg exercises */
SELECT * FROM Quadriceps
UNION
SELECT * FROM Hamstrings
UNION
SELECT * FROM Calves
ORDER BY SpecificTarget


/* All Arm exercises */
SELECT * FROM Biceps
UNION
SELECT * FROM Triceps
UNION
SELECT * FROM Forearms
ORDER BY SpecificTarget


/* All Shoulder exercises (obviously still needs work) */
SELECT * FROM AnteriorDeltoid
UNION
SELECT * FROM LateralDeltoid
UNION
SELECT * FROM PosteriorDeltoid
ORDER BY SpecificTarget


/* All Pull exercises (Traps maybe a hypertrophy extra, or may even just chuck them into accessories category) */
SELECT * FROM Biceps
UNION
SELECT * FROM Back
UNION
SELECT * FROM PosteriorDeltoid
UNION
SELECT * FROM Trapezius
ORDER BY SpecificTarget


/* All Push exercises (shoulders needs extra attention [duplicates])*/
SELECT * FROM Triceps
UNION
SELECT * FROM Chest
UNION
SELECT * FROM AnteriorDeltoid
UNION
SELECT * FROM LateralDeltoid
ORDER BY SpecificTarget


/* Disgusting but this selects a random number in the range of non-compound chest exercises */
SELECT FLOOR(RAND()*((SELECT COUNT(*) FROM CHEST WHERE IsCompound = 0)-1+1))+1;




/* Utterly ridiculous looking but this is what we have so far for push strength */
/* HORRIBLE - but this returns pullovers in a 1/(number of non-compounds) chance */
WITH
	ChestNonCompounds AS
	(
		SELECT *, ROW_NUMBER() OVER (ORDER BY ID) as RowNumber FROM Chest WHERE IsCompound = 0
	),
	ChestNonCompIntersect as 
	(
		Select RowNumber from ChestNonCompounds
		Intersect
		Select * from (SELECT FLOOR(RAND()*((SELECT COUNT(*) FROM CHEST WHERE IsCompound = 0)-1+1))+1 as RandNum)A
	),
	CS as
	(
		SELECT * FROM ChestNonCompounds WHERE Exercise LIKE '%Pullover%'
	)
/* Working on a proper push workout - STRENGTH */
SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Chest
	WHERE IsCompound = 1 AND Exercise LIKE '%Flat%'
	ORDER BY NEWID()
)A
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Chest
	WHERE IsCompound = 1 AND Exercise LIKE '%Incline%'
	ORDER BY NEWID()
)B
UNION ALL 
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM AnteriorDeltoid
	WHERE IsCompound = 1 AND Exercise NOT LIKE '%Arnold%' AND Exercise NOT LIKE '%Machine%'		/* Drop Arnold and Machine presses from Strength workouts */
	ORDER BY NEWID()
)C
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Chest	/* Add fly/crossover variation */
	WHERE Exercise LIKE '%Crossover%' OR Exercise LIKE '%Fly%'
	ORDER BY NEWID()
)D
UNION ALL 
SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM CS		/* Add optional Chest exercises */
	WHERE RowNumber IN (SELECT * FROM ChestNonCompIntersect)
)E
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM LateralDeltoid	/* Pick a Lateral Deltoid non-compound movement */
	WHERE IsCompound = 0
	ORDER BY NEWID()
)F
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound /* Pick a lateral deltoid non-compound exercise */
	FROM AnteriorDeltoid		
	WHERE IsCompound = 0
	ORDER BY NEWID()
)G
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound /* Target the tricep's long head */
	FROM Triceps		
	WHERE SpecificTarget LIKE '%Long Head%'
	ORDER BY NEWID()
)H
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound /* Target the tricep's lateral head */
	FROM Triceps		
	WHERE SpecificTarget LIKE '%Lateral Head%'
	ORDER BY NEWID()
)I
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound /* Target the tricep's medial head */
	FROM Triceps		
	WHERE SpecificTarget LIKE '%Medial%'
	ORDER BY NEWID()
)J
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound 
	FROM Triceps		
	WHERE SpecificTarget LIKE '%Triceps Brachii' /* Not head-specific tricep exercises to finish */
	ORDER BY NEWID()
)H


/* Working on a proper push workout - HYPERTROPHY */
WITH
ChestNonCompounds AS
(
	SELECT *, ROW_NUMBER() OVER (ORDER BY ID) as RowNumber FROM Chest WHERE IsCompound = 0
),
ChestNonCompIntersect as 
(
	Select RowNumber from ChestNonCompounds
	Intersect
	Select * from (SELECT FLOOR(RAND()*((SELECT COUNT(*) FROM CHEST WHERE IsCompound = 0)-1+1))+1 as RandNum)A
),
CS as
(
	SELECT * FROM ChestNonCompounds WHERE Exercise LIKE '%Pullover%'
)

SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Chest
	WHERE IsCompound = 1 AND Exercise LIKE '%Flat%'
	ORDER BY NEWID()
)A
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Chest
	WHERE IsCompound = 1 AND Exercise LIKE '%Incline%'
	ORDER BY NEWID()
)B
UNION ALL 
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM AnteriorDeltoid
	WHERE IsCompound = 1 
	ORDER BY NEWID()
)C
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Chest	/* Add fly/crossover variation */
	WHERE Exercise LIKE '%Crossover%' OR Exercise LIKE '%Fly%'
	ORDER BY NEWID()
)D
UNION ALL 
SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM CS		/* Add optional Chest exercises */
	WHERE RowNumber IN (SELECT * FROM ChestNonCompIntersect)
)E
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM LateralDeltoid	/* Pick a Lateral Deltoid non-compound movement */
	WHERE IsCompound = 0
	ORDER BY NEWID()
)F
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound /* Pick a lateral deltoid non-compound exercise */
	FROM AnteriorDeltoid		
	WHERE IsCompound = 0
	ORDER BY NEWID()
)G
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound /* Target the tricep's long head */
	FROM Triceps		
	WHERE SpecificTarget LIKE '%Long Head%'
	ORDER BY NEWID()
)H
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound /* Target the tricep's lateral head */
	FROM Triceps		
	WHERE SpecificTarget LIKE '%Lateral Head%'
	ORDER BY NEWID()
)I
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound /* Target the tricep's medial head */
	FROM Triceps		
	WHERE SpecificTarget LIKE '%Medial%'
	ORDER BY NEWID()
)J
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound 
	FROM Triceps		
	WHERE SpecificTarget LIKE '%Triceps Brachii' /* Not head-specific tricep exercises to finish */
	ORDER BY NEWID()
)H



/* Working on a proper pull workout - STRENGTH */
WITH
	TrapsCounter AS
	(
		SELECT *, ROW_NUMBER() OVER (ORDER BY ID) as RowNumber FROM Trapezius
	),
	TrapsFarmersWalkIntersect as 
	(
		Select RowNumber from TrapsCounter
		Intersect
		Select * from (SELECT FLOOR(RAND()*((SELECT COUNT(*) FROM CHEST WHERE IsCompound = 0)-1+1))+1 as RandNum)A
	),
	FarmersWalkSelector as
	(
		SELECT * FROM TrapsCounter WHERE Exercise LIKE '%Farmers Walk%'
	)

SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Back
	WHERE Exercise LIKE '%Ups'
	ORDER BY NEWID()
)A
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Back
	WHERE Exercise LIKE '%Row%' AND Exercise NOT LIKE '%Cable%'					/* Drop cable from strength days*/ 
	ORDER BY NEWID()
)B
UNION ALL
SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Back
	WHERE Exercise LIKE '%Pulldown%' 
)C
UNION ALL
SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM PosteriorDeltoid
	WHERE Exercise LIKE '%Face Pull%'
)D
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM PosteriorDeltoid
	WHERE Exercise NOT LIKE '%Face Pull%'					
	ORDER BY NEWID()
)E
UNION ALL
SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Trapezius
	WHERE Exercise LIKE '%Upright Row%'
)F
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Trapezius
	WHERE Exercise NOT LIKE '%Upright Row%' AND Exercise NOT LIKE '%Farmers Walk%' 					
	ORDER BY NEWID()
)G
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Biceps
	WHERE Exercise LIKE '%Curl%' AND Exercise NOT LIKE '%Cable%' AND Exercise NOT LIKE '%Hammer%' AND Exercise NOT LIKE '%Reverse%'
	ORDER BY NEWID()
)H
UNION ALL
SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Biceps
	WHERE Exercise LIKE '%Hammer Curl%'
)I
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Biceps
	WHERE Exercise LIKE '%Reverse%' 					
	ORDER BY NEWID()
)J
UNION ALL 
SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM FarmersWalkSelector		/* Add optional Farmers Walk exercises */
	WHERE RowNumber IN (SELECT * FROM TrapsFarmersWalkIntersect)
)K



/* Working on a proper pull workout - HYPERTROPHY */
WITH
	TrapsCounter AS
	(
		SELECT *, ROW_NUMBER() OVER (ORDER BY ID) as RowNumber FROM Trapezius
	),
	TrapsFarmersWalkIntersect as 
	(
		Select RowNumber from TrapsCounter
		Intersect
		Select * from (SELECT FLOOR(RAND()*((SELECT COUNT(*) FROM CHEST WHERE IsCompound = 0)-1+1))+1 as RandNum)A
	),
	FarmersWalkSelector as
	(
		SELECT * FROM TrapsCounter WHERE Exercise LIKE '%Farmers Walk%'
	)

SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Back
	WHERE Exercise LIKE '%Ups'
	ORDER BY NEWID()
)A
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Back
	WHERE Exercise LIKE '%Row%' 
	ORDER BY NEWID()
)B
UNION ALL
SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Back
	WHERE Exercise LIKE '%Pulldown%' 
)C
UNION ALL
SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM PosteriorDeltoid
	WHERE Exercise LIKE '%Face Pull%'
)D
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM PosteriorDeltoid
	WHERE Exercise NOT LIKE '%Face Pull%'					
	ORDER BY NEWID()
)E
UNION ALL
SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Back
	WHERE Exercise LIKE '%extension%' 
)F
SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Trapezius
	WHERE Exercise LIKE '%Upright Row%'
)G
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Trapezius
	WHERE Exercise NOT LIKE '%Upright Row%' AND Exercise NOT LIKE '%Farmers Walk%' 					
	ORDER BY NEWID()
)H
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Biceps
	WHERE Exercise LIKE '%Curl%' AND Exercise NOT LIKE '%Hammer%' AND Exercise NOT LIKE '%Reverse%'
	ORDER BY NEWID()
)I
UNION ALL
SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Biceps
	WHERE Exercise LIKE '%Hammer Curl%'
)J
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Biceps
	WHERE Exercise LIKE '%Reverse%' 					
	ORDER BY NEWID()
)K
UNION ALL 
SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM FarmersWalkSelector		/* Add optional Farmers Walk exercises */
	WHERE RowNumber IN (SELECT * FROM TrapsFarmersWalkIntersect)
)L




/* Working on a proper leg workout - STRENGTH */
WITH 
	HammyCounter AS
	(
		SELECT *, ROW_NUMBER() OVER (ORDER BY ID) as RowNumber FROM Hamstrings
	),
	ReverseHackSquatIntersect as 
	(
		Select RowNumber from HammyCounter
		Intersect
		Select * from (SELECT FLOOR(RAND()*((SELECT COUNT(*) FROM Hamstrings)-1+1))+1 as RandNum)A
	),
	ReverseHackSquatSelector as
	(
		SELECT * FROM HammyCounter WHERE Exercise LIKE '%Reverse Hack%'
	)
SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Quadriceps
	WHERE Exercise = 'Back Squat'
)A
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Hamstrings
	WHERE Exercise LIKE '%Deadlift%'
	ORDER BY NEWID()
)B
UNION ALL
SELECT * FROM 
( 
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound
	FROM Hamstrings
	WHERE Exercise LIKE '%Deadlift%'
	ORDER BY NEWID()
)C
UNION ALL
SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM ReverseHackSquatSelector		/* Add optional Reverse Hack Squat */
	WHERE RowNumber IN (SELECT * FROM ReverseHackSquatIntersect)
)D
UNION ALL
SELECT * FROM 
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Glutes
	ORDER BY NEWID()
)E
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Quadriceps
	WHERE Exercise NOT LIKE 'Back%' AND Exercise NOT LIKE 'Sissy%' AND Exercise NOT LIKE '%Split%' AND (Exercise LIKE '%Squat' OR Exercise LIKE '%Press')
	ORDER BY NEWID()
)F
UNION ALL
SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Hamstrings
	WHERE Exercise LIKE '%Curls%'
)G
UNION ALL
SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Quadriceps
	WHERE Exercise LIKE '%Extension%'
)H
UNION ALL
SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Calves
	WHERE Exercise LIKE 'Seated%'
)I
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Calves
	WHERE Exercise NOT LIKE 'Seated%' AND Exercise NOT LIKE 'Reverse%'
)J
UNION ALL
SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Calves
	WHERE Exercise LIKE 'Reverse%'
)K


/* Working on a proper leg workout - HYPERTROPHY */
WITH 
HammyCounter AS
(
	SELECT *, ROW_NUMBER() OVER (ORDER BY ID) as RowNumber FROM Hamstrings
),
ReverseHackSquatIntersect as 
(
	SELECT RowNumber FROM HammyCounter
		INTERSECT
	SELECT * FROM (SELECT FLOOR(RAND()*((SELECT COUNT(*) FROM Hamstrings)-1+1))+1 as RandNum)A
),
ReverseHackSquatSelector as
(
	SELECT * FROM HammyCounter WHERE Exercise LIKE '%Reverse Hack%'
)

SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound  FROM Quadriceps
	WHERE Exercise = 'Back Squat'
)A

	UNION ALL

SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound 
	FROM Hamstrings
	WHERE Exercise LIKE '%Deadlift%'
	ORDER BY NEWID()
)B

	UNION ALL

SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM ReverseHackSquatSelector		/* Add optional Reverse Hack Squat */
	WHERE RowNumber IN (SELECT * FROM ReverseHackSquatIntersect)
)C

	UNION ALL

SELECT * FROM 
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Glutes
	ORDER BY NEWID()
)D

	UNION ALL

SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Quadriceps
	WHERE Exercise NOT LIKE 'Back%' AND Exercise NOT LIKE 'Sissy%' AND Exercise NOT LIKE '%Split%' AND (Exercise LIKE '%Squat' OR Exercise LIKE '%Press')
	ORDER BY NEWID()
)E

	UNION ALL

SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Hamstrings
	WHERE Exercise LIKE '%Curls%'
)F

	UNION ALL

SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Quadriceps
	WHERE Exercise LIKE '%Extension%'
)G

	UNION ALL

SELECT * FROM
(
	SELECT Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Calves
	WHERE Exercise LIKE 'Seated%'
)H

	UNION ALL

SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Calves
	WHERE Exercise NOT LIKE 'Seated%' AND Exercise NOT LIKE 'Reverse%'
)I

	UNION ALL

SELECT * FROM
(
	SELECT TOP 1 Exercise, MuscleGroup, SpecificTarget, IsCompound FROM Calves
	WHERE Exercise LIKE 'Reverse%'
)J