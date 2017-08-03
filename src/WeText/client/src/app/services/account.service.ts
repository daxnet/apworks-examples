import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { environment } from '../../environments/environment';
import { User } from 'app/models/user';

import 'rxjs/add/operator/toPromise';

@Injectable()
export class AccountService {

  public currentUser: User;

  constructor(private http: Http) {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));

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
          localStorage.setItem('currentUser', response.text());
          return true;
        } else {
          return false;
        }
      });
  }

  logout(): void {
    localStorage.removeItem('currentUser');
  }

  getUserByName(userName: string): Promise<User> {
    const params = new URLSearchParams();
    params.append('query', `UserName eq ${userName}`);
    const options = new RequestOptions({ params: params });

    return this.http.get(`${environment.baseUrl.accountService}api/users`, options)
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
