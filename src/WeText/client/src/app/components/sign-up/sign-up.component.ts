import { Component, OnInit } from '@angular/core';
import { SignUpModel, DEFAULT_AVATAR_BASE64 } from 'app/models/sign-up-model';
import { DomSanitizer } from '@angular/platform-browser';
import { SignUpValidationModel } from 'app/models/sign-up-validation-model';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {

  model: SignUpModel = new SignUpModel();
  validationModel: SignUpValidationModel = new SignUpValidationModel();
  validate = false;

  constructor(private sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.model.avatarBase64 = DEFAULT_AVATAR_BASE64;
  }

  get avatarSrc() {
    return this.sanitizer.bypassSecurityTrustUrl(`data:image/png;base64,${this.model.avatarBase64}`);
  }

  submit(): void {
    this.validate = true;
    this.validateModel();
  }


  /**
   * Validates the sign up model. This is a very stupid solution to validate
   * the fields one by one.
   *
   * @returns {boolean} True if validate succeeded, otherwise false.
   * @memberof SignUpComponent
   */
  validateModel(): boolean {
    let hasError = false;

    if (!this.model.userName) {
      this.validationModel.validUserName = false;
      this.validationModel.userNameValidationMessage = 'User Name is required.';
      hasError = true;
    } else {
      this.validationModel.validUserName = true;
      this.validationModel.userNameValidationMessage = '';
    }

    if (!this.model.password) {
      this.validationModel.validPassword = false;
      this.validationModel.passwordValidationMessage = 'Password is required.';
      hasError = true;
    } else {
      this.validationModel.validPassword = true;
      this.validationModel.passwordValidationMessage = '';
    }

    if (!this.model.confirmPassword) {
      this.validationModel.validConfirmPassword = false;
      this.validationModel.confirmPasswordValidationMessage = 'Confirm Password is required.';
      hasError = true;
    } else {
      this.validationModel.validConfirmPassword = true;
      this.validationModel.confirmPasswordValidationMessage = '';
    }

    if (this.validationModel.validConfirmPassword) {
      if (this.model.confirmPassword !== this.model.password) {
        this.validationModel.validConfirmPassword = false;
        this.validationModel.confirmPasswordValidationMessage = 'Confirm Password is not the same as Password.';
        hasError = true;
      } else {
        this.validationModel.validConfirmPassword = true;
        this.validationModel.confirmPasswordValidationMessage = '';
      }
    }

    if (!this.model.email) {
      this.validationModel.validEmail = false;
      this.validationModel.emailValidationMessage = 'Email is required.';
      hasError = true;
    } else {
      this.validationModel.validEmail = true;
      this.validationModel.emailValidationMessage = '';
    }

    return hasError;
  }
}
