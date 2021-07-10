import { Component, OnInit } from '@angular/core';
import { CourseDto } from '../models/courseDto.model';
import { StudentDto } from '../models/studentDto.model';
import { StudentService } from '../services/student.service';

@Component({
  selector: 'app-students',
  templateUrl: './students.component.html',
  styleUrls: ['./students.component.css'],
})
export class StudentsComponent implements OnInit {
  studentList: StudentDto[];

  constructor(public studentService: StudentService) {}

  ngOnInit(): void {
    this.refreshList();
  }

  refreshList() {
    this.studentService.geStudentList().subscribe((res) => {
      this.studentService.studentList = res as StudentDto[];
      console.log(this.studentService.studentList);
    });
  }
}
