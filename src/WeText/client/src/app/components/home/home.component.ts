import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from 'app/services/account.service';
import { User } from 'app/models/user';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, OnDestroy {

  private user: User;
  private subscriber: Subscription;
  private userName: string;

  constructor(private accountService: AccountService,
    private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.subscriber = this.route.params.subscribe(params => {
      this.userName = params['uname'];
      this.accountService.getUserByName(this.userName)
      .then(response => this.user = response);
    });
  }

  ngOnDestroy(): void {
    this.subscriber.unsubscribe();
  }

}
