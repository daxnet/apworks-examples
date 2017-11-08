import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { environment } from '../../environments/environment';
import { User } from 'app/models/user';

import 'rxjs/add/operator/toPromise';
import { SignUpModel } from 'app/models/sign-up-model';

@Injectable()
export class AccountService {

  headers = new Headers({ 'Content-Type': 'application/json', 'Accept': 'application/json' });
  options = new RequestOptions({ headers: this.headers });

  public currentUserName: string;

  constructor(private http: Http) {
    this.currentUserName = localStorage.getItem('currentUserName');

  }

  login(userName: string, password: string): Promise<boolean> {
    return this.http.post(`${environment.baseUrl.accountService}api/users/authenticate`,
      JSON.stringify({ userName: userName, password: password }),
      this.options)
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

  /**
   * Performs logout operation.
   *
   * @memberof AccountService
   */
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

  /**
   * Sign up or register a new user.
   *
   * @param {SignUpModel} model The data model which contains the user information.
   * @returns {Promise<User>} The user instance that has been registered to the system.
   * @memberof AccountService
   */
  signUp(model: SignUpModel): Promise<User> {
    return this.http.post(`${environment.baseUrl.accountService}api/users`,
      JSON.stringify({
        userName: model.userName,
        password: model.password,
        displayName: model.nickName,
        email: model.email,
        avatarBackgroundColor: model.avatarBackgroundColor
      }), this.options)
      .toPromise()
      .then(response => {
        if (response.status !== 201) {
          console.log(response);
          throw new Error('Error creating new user account.');
        }

        return new User(model.userName, model.nickName, model.email);
      });
  }
}
