import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, OnDestroy {

  private subscriber: any;
  private userName: string;

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.subscriber = this.route.parent.params.subscribe(params => {
      this.userName = params['uname'];
    });
  }

  ngOnDestroy(): void {
    this.subscriber.unsubscribe();
  }
}
