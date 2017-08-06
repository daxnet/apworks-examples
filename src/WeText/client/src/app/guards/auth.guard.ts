import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';


/**
 * Provides the guard service on the given router.
 *
 * @export
 * @class AuthGuard
 * @implements {CanActivate}
 */
@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private router: Router) {
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    // If the storage contains the current user name, which means
    // the user has logged on, then redirect to the home page.
    if (localStorage.getItem('currentUserName')) {
      const currentUserName = localStorage.getItem('currentUserName');
      // Checks if currently it is on the root path, if yes, then redirect to the
      // home page. As HomeComponent also used this AuthGuard, a recursive redirect
      // will occur without the check here, as a result the web browser will stuck.
      if (state.url === '/') {
        this.router.navigate(['/home', currentUserName]);
      }
      // Returns true, means the redirect is accepted.
      return true;
    }

    // Else (the local storage doesn't contain the current user name), redirect
    // to the login page, and returns false, means the redirect has been rejected.
    this.router.navigate(['/login']);
    return false;
  }
}
