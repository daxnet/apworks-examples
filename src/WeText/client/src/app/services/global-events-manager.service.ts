import { Injectable } from '@angular/core';

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";

@Injectable()
export class GlobalEventsManagerService {

  private currentLoginUserName: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public updateCurrentLoginUserNameEmitter: Observable<string> = this.currentLoginUserName.asObservable();

  constructor() { }

  updateCurrentLoginUserName(userName: string) {
    this.currentLoginUserName.next(userName);
  }
}
