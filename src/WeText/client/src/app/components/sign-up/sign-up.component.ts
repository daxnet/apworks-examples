import { Component, OnInit } from '@angular/core';
import { SignUpModel } from 'app/models/sign-up-model';
import { SignUpValidationModel } from 'app/models/sign-up-validation-model';
import { DialogService } from 'app/services/dialog.service';
import { AccountService } from 'app/services/account.service';
import { Router } from '@angular/router';
import { GlobalEventsManagerService } from 'app/services/global-events-manager.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {

  model: SignUpModel = new SignUpModel();
  validationModel: SignUpValidationModel = new SignUpValidationModel();
  validate = false;
  avatarColor: string;

  constructor(private dialogService: DialogService,
    private accountService: AccountService,
    private router: Router,
    private gem: GlobalEventsManagerService) { }

  ngOnInit() {
    this.avatarColor = this.getRandomColor();
  }

  /**
   * Submits the new user registration request.
   *
   * @memberof SignUpComponent
   */
  submit(): void {
    this.validate = true;
    if (this.validateModel()) {
      this.model.avatarBackgroundColor = this.avatarColor;
      this.accountService.signUp(this.model)
        .then(user => {
          this.dialogService.showSuccess(`User "${user.userName}" has been created successfully.`, 'SUCCESS', 'lg')
            .then(dialogRef => dialogRef.result.then(result => {
              this.accountService.login(this.model.userName, this.model.password)
                .then(response => {
                  if (response) {
                    this.gem.updateCurrentLoginUserName(this.model.userName);
                    this.router.navigate(['home', this.model.userName]);
                  }
                });
            }));
        })
        .catch(err => {
          if (err.status === 409) {
            this.dialogService.showError('User name or email already exists.', 'FAILED', 'lg');
          } else {
            console.log(err);
            this.dialogService.showError('Failed to create user.', 'FAILED', 'lg');
          }
        });
    }
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

    return !hasError;
  }

  private getRandomColor(): string {
    const letters = '0123456789ABCDEF'.split('');
    let color = '#';
    for ( let i = 0; i < 6; i++ ) {
      color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
  }
}
