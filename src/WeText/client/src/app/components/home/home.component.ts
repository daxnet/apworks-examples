import { Component, OnInit } from '@angular/core';
import { AccountService } from 'app/services/account.service';
import { User } from 'app/models/user';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  private user: User;
  constructor(private accountService: AccountService) {
    this.user = accountService.currentUser;
  }

  ngOnInit() {
  }

}
