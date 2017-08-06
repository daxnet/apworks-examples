import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'app/services/account.service';
import { GlobalEventsManagerService } from 'app/services/global-events-manager.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  currentUserName = '';

  constructor(private accountService: AccountService,
    private router: Router,
    private gem: GlobalEventsManagerService) {
      this.gem.updateCurrentLoginUserNameEmitter.subscribe(updatedUserName => {
        console.log(`updated: ${updatedUserName}`);
        if (updatedUserName !== null) {
          this.currentUserName = updatedUserName;
        }
      });
  }

  ngOnInit(): void {
    if (this.accountService.currentUserName) {
      this.currentUserName = this.accountService.currentUserName;
    }
  }

  logout(): void {
    // Send the global notification which indicates that the current login
    // user name has been cleared, this will trigger the event subscriber
    // defined in the ctor of this class, within which the currentUserName
    // variable will be updated. As a consequence, the view is updated.
    this.gem.updateCurrentLoginUserName('');

    // Calls the logout() method on the account service to clear the userName
    // stored in the local storage.
    this.accountService.logout();

    // Navigates to the login page.
    this.router.navigate(['login']);
  }
}
