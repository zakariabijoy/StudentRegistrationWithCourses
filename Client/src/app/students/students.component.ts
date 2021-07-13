import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Pagination } from '../models/pagination.model';
import { StudentDto } from '../models/studentDto.model';
import { StudentService } from '../services/student.service';

@Component({
  selector: 'app-students',
  templateUrl: './students.component.html',
  styleUrls: ['./students.component.css'],
})
export class StudentsComponent implements OnInit {
  Students: StudentDto[];
  pagination: Pagination;
  pageNumber = 1;
  pageSize = 5;
  count = 0;
  searchBy = '';

  constructor(
    public studentService: StudentService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.refreshList();
  }

  onSearch() {
    console.log(this.searchBy);
    this.refreshList();
  }
  pageChanged(event: any) {
    this.pageNumber = event;
    this.refreshList();
  }
  refreshList() {
    this.studentService
      .geStudentList(this.pageNumber, this.pageSize, this.searchBy)
      .subscribe((res) => {
        console.log(res);
        this.Students = res.result;
        this.pagination = res.pagination;
        this.count = res.pagination.totalItems;
      });
  }

  openForEdit(studentId: number) {
    this.router.navigateByUrl('/student/edit/' + studentId);
  }

  DeleteStudent(studentId: number) {
    if (confirm('Are you sure to delete this record?')) {
      this.studentService.deleteStudent(studentId).subscribe((res) => {
        this.refreshList();
        this.toastr.warning(
          'Student deleted Successfully with registered course'
        );
      });
    }
  }
}
