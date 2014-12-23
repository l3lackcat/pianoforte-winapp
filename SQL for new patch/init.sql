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
/*UPDATE classroom_detail SET currentStatus = 'CHECKED_IN', previousStatus = 'WAITING' WHERE currentStatus = 'WAITING' AND classroomDate < CURDATE()*/

/*06*/
/*SELECT * FROM classroom_detail WHERE currentStatus = 'WAITING' AND classroomDate = CURDATE() AND ADDTIME(STR_TO_DATE(classroomTime, '%H:%i:%s'), classroomDuration * 100) < CURTIME()*/
/*UPDATE classroom_detail SET currentStatus = 'CHECKED_IN', previousStatus = 'WAITING' WHERE currentStatus = 'WAITING' AND classroomDate = CURDATE() AND ADDTIME(STR_TO_DATE(classroomTime, '%H:%i:%s'), classroomDuration * 100) < CURTIME()*/

/*07*/
/*SELECT * FROM student WHERE studentId = 1*/
/*UPDATE student SET lastDateOfClass = NULL, status = 'INACTIVE' WHERE studentId = 1*/

/*08*/
/*SELECT * FROM student WHERE studentId <> 1*/
/*UPDATE student SET lastDateOfClass = NULL, status = 'INACTIVE' WHERE studentId <> 1*/

/*09*/
/*SELECT DISTINCT classroomId FROM classroom_detail WHERE currentStatus = 'WAITING'*/
/*SELECT DISTINCT enrollmentId FROM classroom WHERE classroomId IN (
SELECT DISTINCT classroomId FROM classroom_detail WHERE currentStatus = 'WAITING')*/
/*SELECT DISTINCT studentId FROM enrollment WHERE enrollmentId IN (
SELECT DISTINCT enrollmentId FROM classroom WHERE classroomId IN (
SELECT DISTINCT classroomId FROM classroom_detail WHERE currentStatus = 'WAITING'))*/
/*SELECT * FROM student WHERE studentId <> 1 AND studentId IN (
SELECT DISTINCT studentId FROM enrollment WHERE enrollmentId IN (
SELECT DISTINCT enrollmentId FROM classroom WHERE classroomId IN (
SELECT DISTINCT classroomId FROM classroom_detail WHERE currentStatus = 'WAITING')))*/
/*UPDATE student SET status = 'ACTIVE' WHERE studentId <> 1 AND studentId IN (
SELECT DISTINCT studentId FROM enrollment WHERE enrollmentId IN (
SELECT DISTINCT enrollmentId FROM classroom WHERE classroomId IN (
SELECT DISTINCT classroomId FROM classroom_detail WHERE currentStatus = 'WAITING')))*/