import { Component, OnInit } from '@angular/core';
import { User } from './models/user.model';
import { AccountService } from './services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'Client';

  constructor(private accountService: AccountService) {}

  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser() {
    if (localStorage.getItem('jwt') === null) {
      this.accountService.setCurrentUser(null);
    } else {
      const jwt = localStorage.getItem('jwt');
      const userName = localStorage.getItem('userName');
      const expiration = localStorage.getItem('expiration');
      const refreshToken = localStorage.getItem('refreshToken');
      let result = { authToken: { jwt, userName, expiration, refreshToken } };
      console.log(result);
      this.accountService.setCurrentUser(result);
    }
  }
}
