import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { environment } from '../../environments/environment';
import { User } from 'app/models/user';

import 'rxjs/add/operator/toPromise';

@Injectable()
export class AccountService {

  public currentUserName: string;

  constructor(private http: Http) {
    this.currentUserName = localStorage.getItem('currentUserName');

  }

  login(userName: string, password: string): Promise<boolean> {
    const headers = new Headers({ 'Content-Type': 'application/json', 'Accept': 'application/json' });
    const options = new RequestOptions({ headers: headers });

    return this.http.post(`${environment.baseUrl.accountService}api/users/authenticate`,
      JSON.stringify({ userName: userName, password: password }),
      options)
      .toPromise()
      .then(response => {
        if (response.status === 200) {
          localStorage.setItem('currentUserName', userName);
          return true;
        } else {
          return false;
        }
      });
  }

  logout(): void {
    localStorage.removeItem('currentUserName');
  }

  getUserByName(userName: string): Promise<User> {
    return this.http.get(`${environment.baseUrl.accountService}api/users?query=userName eq "${userName}"`)
      .toPromise()
      .then(response => {
        const result = response.json();
        if (result.totalRecords > 0) {
          return result._embedded.users[0];
        } else {
          throw new Error(`The user "${userName}" does not exist.`);
        }
      });
  }
}
