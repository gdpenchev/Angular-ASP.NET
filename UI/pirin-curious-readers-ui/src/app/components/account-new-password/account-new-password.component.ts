import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/services/user.service';
import { regexes } from 'src/app/utils/Regexes';

@Component({
  selector: 'app-account-new-password',
  templateUrl: './account-new-password.component.html',
  styleUrls: ['./account-new-password.component.scss']
})
export class AccountNewPasswordComponent implements OnInit {
  public newPasswordForm: FormGroup;
  private recoverPasswordToken: string;
  private userEmail: string;

  constructor(
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute,
  ) {
    this.newPasswordForm = this.formBuilder.group({
      password: ['', [Validators.required, Validators.pattern(regexes.password)]],
      repeatPassword: ['', [Validators.required]]
    }, { validators: this.passwordConfirming });


    this.route.queryParams.subscribe(params => {
      this.recoverPasswordToken = params['token'];
      this.userEmail = params['email'];
    });

    if (!this.recoverPasswordToken && !this.userEmail) {
      this.router.navigateByUrl('/login');
    }
  }

  ngOnInit(): void {
  }

  public onAccountNewPasswordSubmit(event: any) {
    event.preventDefault();

    if (!this.newPasswordForm.valid) {
      return;
    }

    let requestData = {
      email: this.userEmail,
      resetToken: this.recoverPasswordToken,
      newPassword: this.newPasswordForm.get('password')?.value
    };

    this.userService.changePassword(requestData).subscribe({
      next: (res: any) => {
        this.toastr.success('Your password has been changed successfully!', 'Success');
        this.router.navigateByUrl('/login');
      },
      error: (err) => {
        let errorMessage = err.error === "Your password recovery link is already used or expired! Click on login page forgot password to send new one and try again." ? err.error : "Something went wrong";
        this.toastr.error(errorMessage, 'Error');
      },
      complete: () => {}
    });
  }

  private passwordConfirming(control: AbstractControl): { passwordsNotMatching: boolean } | null {
    if (control.get('password')?.value !== control.get('repeatPassword')?.value) {
      return { passwordsNotMatching: true };
    }
    return null;
  }
}
