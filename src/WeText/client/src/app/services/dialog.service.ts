import { Injectable } from '@angular/core';

import { Modal } from 'angular2-modal/plugins/bootstrap';

@Injectable()
export class DialogService {

  constructor(private modal: Modal) { }

  showError(message: string, title: string): void {
    this.modal.alert()
      .size('sm')
      .showClose(true)
      .title(title)
      .body(`<span class="msg-err"><span class="fa fa-times-circle"></span>&nbsp;${message}</span>`)
      .open();
  }
}
