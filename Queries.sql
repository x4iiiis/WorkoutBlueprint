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

