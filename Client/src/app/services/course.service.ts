import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { CourseCheckBox } from '../models/courseCheckBox.model';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  constructor(private http: HttpClient) {}

  getCourse():any {
   return this.http.get(environment.apiUrl + 'Courses');
  }
}
