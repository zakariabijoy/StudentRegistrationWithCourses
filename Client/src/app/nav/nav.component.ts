import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    public accountService: AccountService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.loginForm = this.fb.group({
      userName: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  login() {
    this.accountService.login(this.loginForm.value).subscribe(
      (response) => {
        console.log(response);
        this.router.navigateByUrl('/students');
        this.toastr.success('loggin Successfully');
      },
      (error) => {
        console.log(error);
        this.toastr.error(error.error);
      }
    );
  }

  logout() {
    this.accountService.logout();
  }
}
