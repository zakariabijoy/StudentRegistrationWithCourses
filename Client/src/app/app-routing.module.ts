import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { NavComponent } from './nav/nav.component';
import { StudentEditComponent } from './students/student-edit/student-edit.component';
import { StudentComponent } from './students/student/student.component';
import { StudentsComponent } from './students/students.component';

const routes: Routes = [
  { path: '', component: NavComponent, pathMatch: 'full' },
  {
    path: 'students',
    component: StudentsComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'student',
    children: [
      { path: '', component: StudentComponent },
      { path: 'edit/:id', component: StudentEditComponent },
    ],
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
