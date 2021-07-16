import { Component, OnInit } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CourseCheckBox } from 'src/app/models/courseCheckBox.model';
import { CourseService } from 'src/app/services/course.service';
import { StudentService } from 'src/app/services/student.service';

@Component({
  selector: 'app-student',
  templateUrl: './student.component.html',
  styleUrls: ['./student.component.css'],
})
export class StudentComponent implements OnInit {
  registerForm: FormGroup;
  validationErrors: string[] = [];

  constructor(
    public studentService: StudentService,
    private courseService: CourseService,
    private fb: FormBuilder,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.courseService.getCourse().subscribe((res) => {
      this.studentService.courseCheckBoxList = res as CourseCheckBox[];
      this.initializeForm();
    });
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      regNo: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(10),
        ],
      ],
      gender: ['', Validators.required],
      dateOfBirth: ['', [Validators.required]],
      courseCheckBoxList: this.fb.array([]),
    });
  }
  onCheckboxChange(e) {
    const courseCheckBoxList: FormArray = this.registerForm.get(
      'courseCheckBoxList'
    ) as FormArray;

    if (e.target.checked) {
      courseCheckBoxList.push(new FormControl(e.target.value));
    } else {
      let i: number = 0;
      courseCheckBoxList.controls.forEach((item: FormControl) => {
        if (item.value == e.target.value) {
          courseCheckBoxList.removeAt(i);
          return;
        }
        i++;
      });
    }
  }

  register() {
    if (this.registerForm.value.courseCheckBoxList.length <= 0) {
      this.validationErrors.push('Please Select a course');
    } else {
      console.log(this.registerForm.value);
      this.studentService.onSubmit(this.registerForm.value).subscribe(
        (res) => {
          console.log(res);
          this.validationErrors = [];
          this.router.navigateByUrl('students');
          this.toastr.success('Student registration is successfully done');
        },
        (error) => {
          console.log(error);
          this.validationErrors.push(error.error.title);
        }
      );
    }
  }
  cancel() {
    this.router.navigateByUrl('students');
  }
}
