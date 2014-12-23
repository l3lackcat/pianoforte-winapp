﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PianoForte.Model;

namespace PianoForte.Dao
{
    public interface ClassroomDetailDao
    {
        bool insertClassroomDetail(ClassroomDetail classroomDetail);
        bool updateClassroomDetail(ClassroomDetail classroomDetail);
        bool deleteClassroomDetail(int classroomDetailId);

        // bool updateClassroomDetailStatus

        ClassroomDetail findClassroomDetail(int classroomDetailId);

        ClassroomDetail findClassroomDetailByRegularClassroomDetail(int regularClassroomDetailId);

        List<ClassroomDetail> findAllClassroomDetail();

        List<ClassroomDetail> findAllClassroomDetailByClassroomId(int classroomId);
        List<ClassroomDetail> findAllClassroomDetailByClassroomId(int classroomId, List<ClassroomDetail.ClassroomType> typeList, string columnForArrange);
        
        List<ClassroomDetail> findAllActiveClassroomDetailByClassroomId(int classroomId);

        List<ClassroomDetail> findAllActiveClassroomDetailByEndDate(DateTime endDate);

        List<ClassroomDetail> findAllActiveClassroomDetailByClassroomIdAndFromDate(int classroomId, DateTime fromDate);

        List<ClassroomDetail> findAllClassroomDetailByRoomDetailId(int roomDetailId);

        List<ClassroomDetail> findAllClassroomDetailByTeacherIdAndDate(int teacherId, DateTime date);
        List<ClassroomDetail> findAllClassroomDetailByTeacherIdAndDate(int teacherId, DateTime date, List<ClassroomDetail.ClassroomStatus> statusList, string columnForArrange);

        List<ClassroomDetail> findLastDateOfClassroomDetailByClassroomId(int classroomId);
    }
}
