import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { CourseCheckBox } from '../models/courseCheckBox.model';
import { PaginatedResult } from '../models/pagination.model';
import { Student } from '../models/student.model';
import { StudentDto } from '../models/studentDto.model';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class StudentService {
  formData: Student;
  courseCheckBoxList: CourseCheckBox[];
  studentList: StudentDto[];
  paginatedResult: PaginatedResult<StudentDto[]> = new PaginatedResult<
    StudentDto[]
  >();

  constructor(private http: HttpClient) {}

  onSubmit(model: {}) {
    return this.http.post(environment.apiUrl + 'Students', model);
  }

  geStudentList(page?: number, itemsPerPage?: number) {
    let params = new HttpParams();

    if (page !== null && itemsPerPage !== null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    return this.http
      .get<StudentDto[]>(environment.apiUrl + 'Students', {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          this.paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            this.paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }

          return this.paginatedResult;
        })
      );
  }

  getStudent(id: number) {
    return this.http.get(environment.apiUrl + 'Students/' + id);
  }

  deleteStudent(id: number) {
    return this.http.delete(environment.apiUrl + 'Students/' + id);
  }
}
