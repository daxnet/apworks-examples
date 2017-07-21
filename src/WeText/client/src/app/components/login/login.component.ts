import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'app/services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  userName: string;
  password: string;

  constructor(private router: Router,
    private accountService: AccountService) { }

  ngOnInit() {
    this.accountService.logout();
  }

  login(): void {
    this.accountService.login(this.userName, this.password)
      .subscribe(result => {
        if (result === true) {
          this.router.navigate(['/']);
        } else {
          alert('login failed.');
        }
      })
  }
}
