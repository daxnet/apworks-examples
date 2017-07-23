import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'app/services/account.service';
import { GlobalEventsManagerService } from 'app/services/global-events-manager.service';
import { DialogService } from 'app/services/dialog.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  userName: string;
  password: string;

  constructor(private router: Router,
    private accountService: AccountService,
    private gem: GlobalEventsManagerService,
    private dialogService: DialogService) {
  }

  ngOnInit() {
    this.accountService.logout();
  }

  login(): void {
    this.accountService.login(this.userName, this.password)
      .then(response => {
        if (response === true) {
          this.gem.updateCurrentLoginUserName(this.userName);
          this.router.navigate(['']);
        } else {
          this.dialogService.showError('Error occurs when sign in.', 'Login failed');
        }
      })
      .catch(err => {
        switch (err.status) {
          case 404:
            this.dialogService.showError('Invalid user name. The specified user does not exist.', 'Login failed');
            break;
          case 401:
            this.dialogService.showError('The provided password is incorrect.', 'Login failed');
            break;
        }
      });
  }
}
