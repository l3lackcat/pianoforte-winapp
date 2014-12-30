CREATE TABLE `saved_payment` (                                                 
`savedPaymentId` int(10) NOT NULL AUTO_INCREMENT,                            
`studentId` int(10) NOT NULL,                                                
`receiverId` int(10) NOT NULL,                                               
`totalPrice` double NOT NULL,                                                
`savedDate` date NOT NULL,                                                   
`paymentId` int(10) NOT NULL DEFAULT '0',                                    
`status` varchar(31) COLLATE utf8_unicode_ci NOT NULL,                       
PRIMARY KEY (`savedPaymentId`)) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

UPDATE classroom_detail SET classroomTime = REPLACE(classroomTime, '.', ':');

UPDATE enrollment SET status = 'CENCELED' WHERE status = 'NOT_PAID';

UPDATE classroom_detail SET previousStatus = 'WAITING' WHERE currentStatus = 'CHECKED_IN' OR 'CANCELED';

UPDATE classroom_detail
SET previousStatus = currentStatus, currentStatus = 'CHECKED_IN'
WHERE currentStatus = 'WAITING'
AND classroomDate < CURDATE();

UPDATE classroom_detail
SET previousStatus = currentStatus, currentStatus = 'CHECKED_IN'
WHERE currentStatus = 'WAITING'
AND classroomDate = CURDATE()
AND ADDTIME(STR_TO_DATE(classroomTime, '%H:%i:%s'), classroomDuration * 100) < CURTIME();

UPDATE student SET lastDateOfClass = NULL, status = 'INACTIVE';

UPDATE student AS s1, (
SELECT sub_e.studentId AS studentId
FROM classroom_detail AS sub_cd, classroom AS sub_c, enrollment AS sub_e
WHERE sub_cd.currentStatus = 'WAITING'
AND sub_cd.classroomId = sub_c.classroomId
AND sub_c.enrollmentId = sub_e.enrollmentId
GROUP BY sub_e.studentId) AS result
SET s1.status = 'ACTIVE'
WHERE s1.studentId = result.studentId
AND s1.status <> 'ACTIVE';

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
WHERE s1.studentId = result.studentId;