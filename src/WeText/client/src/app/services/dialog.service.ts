import { Injectable } from '@angular/core';

import { Modal } from 'angular2-modal/plugins/bootstrap';

@Injectable()
export class DialogService {

  constructor(private modal: Modal) { }

  showError(message: string, title: string, size: any = 'sm'): Promise<any> {
    return this.show(message, title, 'msg-err', 'fa-times-circle', size);
  }

  showSuccess(message: string, title: string, size: any = 'sm'): Promise<any> {
    return this.show(message, title, 'msg-success', 'fa-check-circle', size);
  }

  private show(message: string, title: string, cls: string, icon: string, size: any = 'sm'): Promise<any> {
    return this.modal.alert()
    .size(size)
    .showClose(true)
    .title(title)
    .body(`<span class="${cls}"><span class="fa ${icon}"></span>&nbsp;${message}</span>`)
    .open();
  }
}
