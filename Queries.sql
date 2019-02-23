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