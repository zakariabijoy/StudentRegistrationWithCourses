import { CourseDto } from './courseDto.model';

export interface StudentDto {
  studentId: number;
  name: string;
  regNo: number;
  gender: string;
  dateOfBirth: Date;
  courseList: CourseDto[];
  coursesName: string;
}
