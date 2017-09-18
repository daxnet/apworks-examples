import { Component, OnInit } from '@angular/core';
import { SignUpModel, DEFAULT_AVATAR_BASE64 } from 'app/models/sign-up-model';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {

  model: SignUpModel = new SignUpModel();

  constructor(private sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.model.avatarBase64 = DEFAULT_AVATAR_BASE64;
  }

  get avatarSrc() {
    return this.sanitizer.bypassSecurityTrustUrl(`data:image/png;base64,${this.model.avatarBase64}`);
  }

  submit(): void {
    alert(this.model.userName);
  }
}
