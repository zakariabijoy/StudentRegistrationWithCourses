import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private currentUserSource = new ReplaySubject<any>(1);
  currentUser$ = this.currentUserSource.asObservable();
  constructor(private http: HttpClient, private router: Router) {}

  private loginStatus = new BehaviorSubject<boolean>(this.checkLoginStatus());

  // login(model: any) {
  //   return this.http.post(environment.apiUrl + 'Users/login', model).pipe(
  //     map((response: User) => {
  //       const user = response;
  //       if (user) {
  //         localStorage.setItem('user', JSON.stringify(user));
  //         this.currentUserSource.next(user);
  //       }
  //     })
  //   );
  // }

  login(model: any) {
    const grantType = 'password';
    // pipe() let you combine multiple functions into a single function.
    // pipe() runs the composed functions in sequence.
    return this.http
      .post<any>(environment.apiUrl + 'Users/auth', {
        ...model,
        grantType,
      })
      .pipe(
        map((result) => {
          // login successful if there's a jwt token in the response
          if (result && result.authToken.token) {
            // store user details and jwt token in local storage to keep user logged in between page refreshes

            this.loginStatus.next(true);
            localStorage.setItem('loginStatus', '1');
            localStorage.setItem('jwt', result.authToken.token);
            localStorage.setItem('userName', result.authToken.userName);
            localStorage.setItem('expiration', result.authToken.expiration);
            localStorage.setItem('refreshToken', result.authToken.refreshToken);
            this.currentUserSource.next(result);
          }

          return result;
        })
      );
  }

  // Method to get new refresh token
  getNewRefreshToken(): Observable<any> {
    let userName = localStorage.getItem('userName');
    let refreshToken = localStorage.getItem('refreshToken');
    const grantType = 'refresh_token';

    return this.http
      .post<any>(environment.apiUrl + 'Users/auth', {
        userName,
        refreshToken,
        grantType,
      })
      .pipe(
        map((result) => {
          
          if (result && result.authToken.token) {
            this.loginStatus.next(true);
            localStorage.setItem('loginStatus', '1');
            localStorage.setItem('jwt', result.authToken.token);
            localStorage.setItem('userName', result.authToken.userName);
            localStorage.setItem('expiration', result.authToken.expiration);
            localStorage.setItem('refreshToken', result.authToken.refreshToken);
            this.currentUserSource.next(result);
          }

          return <any>result;
        })
      );
  }

  setCurrentUser(result: any) {
    this.currentUserSource.next(result);
  }

  logout() {
    this.loginStatus.next(false);
    localStorage.removeItem('jwt');
    localStorage.removeItem('userName');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('expiration');
    localStorage.setItem('loginStatus', '0');
    console.log('Logged Out Successfully');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  checkLoginStatus(): boolean {
    var loginCookie = localStorage.getItem('loginStatus');

    if (loginCookie == '1') {
      if (
        localStorage.getItem('jwt') != null ||
        localStorage.getItem('jwt') != undefined
      ) {
        return true;
      }
      return false;
    }
    return false;
  }
}
