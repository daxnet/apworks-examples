import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import {environment} from '../../environments/environment';

import 'rxjs/add/operator/toPromise';

@Injectable()
export class AccountService {

  public currentUserName: string;

  constructor(private http: Http) {
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this.currentUserName = currentUser && currentUser.userName;
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
            localStorage.setItem('currentUser', JSON.stringify({ userName: userName }));
            return true;
          } else {
            return false;
          }
        });
   }

   logout(): void {
        localStorage.removeItem('currentUser');
    }
}
