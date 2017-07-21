import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs';
import {environment} from '../../environments/environment';
import 'rxjs/add/operator/map';

@Injectable()
export class AccountService {

  public currentUserName: string;

  constructor(private http: Http) {
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this.currentUserName = currentUser && currentUser.userName;
  }

  login(userName: string, password: string): Observable<boolean> {
    const headers = new Headers({ 'Content-Type': 'application/json' });
    const options = new RequestOptions({ headers: headers });

    return this.http.post(`${environment.baseUrl.accountService}api/users/authenticate`,
        JSON.stringify({ username: userName, password: password }),
        options)
      .map((response: Response) => {
        if (response.status === 200) {
          // store username and jwt token in local storage to keep user logged in between page refreshes
          localStorage.setItem('currentUser', JSON.stringify({ userName: userName }));
          // return true to indicate successful login
          return true;
        } else {
          // return false to indicate failed login
          return false;
        }
      });
   }

   logout(): void {
        localStorage.removeItem('currentUser');
    }
}
