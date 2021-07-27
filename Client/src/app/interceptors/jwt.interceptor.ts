import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse,
  HttpErrorResponse,
} from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { AccountService } from '../services/account.service';
import { User } from '../models/user.model';
import {
  catchError,
  filter,
  finalize,
  switchMap,
  take,
  tap,
} from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  private isTokenRefreshing: boolean = false;

  tokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>(null);

  constructor(private accountService: AccountService,private toastr: ToastrService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(this.attachTokenToRequest(request)).pipe(
      tap((event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {
          console.log('Success');
        }
      }),
      catchError((err): Observable<any> => {
        if (err instanceof HttpErrorResponse) {
          if (err.url === 'https://localhost:44377/api/Users/auth') {
            this.accountService.logout();
            this.toastr.warning("login session time out.Please login again");
          } else {
            switch ((<HttpErrorResponse>err).status) {
              case 401:
                console.log('Token expired. Attempting refresh ...');
                return this.handleHttpResponseError(request, next);
            }
          }
        } else {
          return throwError(err);
        }
        return throwError(err);
      })
    );
  }

  // Method to handle http error response
  private handleHttpResponseError(
    request: HttpRequest<any>,
    next: HttpHandler
  ) {
    // First thing to check if the token is in process of refreshing
    if (!this.isTokenRefreshing) {
      // If the Token Refresheing is not true
      this.isTokenRefreshing = true;

      // Any existing value is set to null
      // Reset here so that the following requests wait until the token comes back from the refresh token API call
      this.tokenSubject.next(null);

      /// call the API to refresh the token
      return this.accountService.getNewRefreshToken().pipe(
        switchMap((tokenresponse: any) => {
          if (tokenresponse) {
            this.tokenSubject.next(tokenresponse.authToken.token);
            localStorage.setItem('loginStatus', '1');
            localStorage.setItem('jwt', tokenresponse.authToken.token);
            localStorage.setItem('userName', tokenresponse.authToken.userName);
            localStorage.setItem(
              'expiration',
              tokenresponse.authToken.expiration
            );
            localStorage.setItem(
              'refreshToken',
              tokenresponse.authToken.refreshToken
            );
            console.log('Token refreshed...');
            return next.handle(this.attachTokenToRequest(request));
          }
          return <any>this.accountService.logout();
        }),
        catchError((err) => {
          this.accountService.logout();
          return throwError(err);
        }),
        finalize(() => {
          this.isTokenRefreshing = false;
        })
      );
    } else {
      this.isTokenRefreshing = false;
      return this.tokenSubject.pipe(
        filter((token) => token != null),
        take(1),
        switchMap((token) => {
          return next.handle(this.attachTokenToRequest(request));
        })
      );
    }
  }

  private attachTokenToRequest(request: HttpRequest<any>) {
    var token = localStorage.getItem('jwt');

    return request.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
  }
}
