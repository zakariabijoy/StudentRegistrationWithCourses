import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { CourseCheckBox } from '../models/courseCheckBox.model';
import { Student } from '../models/student.model';
import { StudentDto } from '../models/studentDto.model';

@Injectable({
  providedIn: 'root',
})
export class StudentService {
  formData: Student;
  courseCheckBoxList: CourseCheckBox[];
  studentList: StudentDto[];

  constructor(private http: HttpClient) {}

  onSubmit(model: {}) {
    return this.http.post(environment.apiUrl + 'Students', model);
  }

  geStudentList() {
    return this.http.get(environment.apiUrl + 'Students');
  }
}
