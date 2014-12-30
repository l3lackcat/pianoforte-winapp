/*01*/
/*UPDATE classroom_detail SET classroomTime = REPLACE(classroomTime, ".", ":");*/

/*02*/
/*SELECT * FROM enrollment WHERE status = 'NOT_PAID'*/
/*UPDATE enrollment SET status = 'CENCELED' WHERE status = 'NOT_PAID'*/

/*03*/
/*SELECT * FROM classroom_detail WHERE currentStatus = 'CHECKED_IN'*/
/*UPDATE classroom_detail SET previousStatus = 'WAITING' WHERE currentStatus = 'CHECKED_IN'*/

/*04*/
/*SELECT * FROM classroom_detail WHERE currentStatus = 'CANCELED'*/
/*UPDATE classroom_detail SET previousStatus = 'WAITING' WHERE currentStatus = 'CANCELED'*/

/*05*/
/*SELECT * FROM classroom_detail WHERE currentStatus = 'WAITING' AND classroomDate < CURDATE()*/
/*
UPDATE classroom_detail
SET previousStatus = currentStatus, currentStatus = 'CHECKED_IN'
WHERE currentStatus = 'WAITING'
AND classroomDate < CURDATE()
*/

/*06*/
/*
SELECT *
FROM classroom_detail
WHERE currentStatus = 'WAITING'
AND classroomDate = CURDATE()
AND ADDTIME(STR_TO_DATE(classroomTime, '%H:%i:%s'), classroomDuration * 100) < CURTIME()
*/
/*
UPDATE classroom_detail
SET previousStatus = currentStatus, currentStatus = 'CHECKED_IN'
WHERE currentStatus = 'WAITING'
AND classroomDate = CURDATE()
AND ADDTIME(STR_TO_DATE(classroomTime, '%H:%i:%s'), classroomDuration * 100) < CURTIME()
*/

/*07*/
/*SELECT * FROM student WHERE studentId = 1*/
/*UPDATE student SET lastDateOfClass = NULL, status = 'INACTIVE' WHERE studentId = 1*/

/*08*/
/*SELECT * FROM student WHERE studentId <> 1*/
/*UPDATE student SET lastDateOfClass = NULL, status = 'INACTIVE' WHERE studentId <> 1*/

/*09*/
/*
UPDATE student AS s1, (
SELECT sub_e.studentId AS studentId
FROM classroom_detail AS sub_cd, classroom AS sub_c, enrollment AS sub_e
WHERE sub_cd.currentStatus = 'WAITING'
AND sub_cd.classroomId = sub_c.classroomId
AND sub_c.enrollmentId = sub_e.enrollmentId
GROUP BY sub_e.studentId) AS result
SET s1.status = 'ACTIVE'
WHERE s1.studentId = result.studentId
AND s1.status <> 'ACTIVE'
*/

/*10*/
/*
UPDATE student AS s1, (
SELECT sub_s.studentId AS studentId, MAX(sub_cd.classroomDate) AS maxDate
FROM classroom AS sub_c, classroom_detail AS sub_cd, enrollment AS sub_e, student AS sub_s
WHERE sub_s.studentId <> 1
AND sub_s.studentId = sub_e.studentId
AND sub_e.enrollmentId = sub_c.enrollmentId
AND sub_c.classroomId = sub_cd.classroomId
GROUP BY sub_s.studentId
ORDER BY sub_s.studentId) AS result
SET s1.lastDateOfClass = result.maxDate
WHERE s1.studentId = result.studentId
*/

/*SELECT * FROM payment WHERE studentId IN (SELECT studentId FROM student WHERE ISNULL(lastDateOfClass) AND studentId <> 1)*/