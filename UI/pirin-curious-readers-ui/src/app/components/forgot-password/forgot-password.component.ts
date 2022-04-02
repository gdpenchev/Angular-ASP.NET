import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/services/user.service';
import { regexes } from 'src/app/utils/Regexes';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
  public forgotPasswordForm: FormGroup;

  constructor(
      private formBuilder: FormBuilder, 
      private toastr: ToastrService,
      private userService: UserService,
      private router: Router
    ) { 
    this.forgotPasswordForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.pattern(regexes.email)]]
    });
  }

  ngOnInit(): void {
  }

  public onForgotPasswordSubmit(event: any) {
    event.preventDefault();

    if (!this.forgotPasswordForm.valid) {
      this.toastr.error('Please, complete the required field!', 'Error');
      return;
    }

    this.userService.forgotPassword(this.forgotPasswordForm.get('email')?.value).subscribe({
      next: (res: any) => {
        this.toastr.success('Password recovery link sent successfully to the given email!', 'Success');
        this.router.navigateByUrl('/');
      },
      error: (err) => {
        let errorMessage = err.status === 404 && err.error === "User with this email was not found." ? err.error : "Something went wrong!";
        this.toastr.error(errorMessage, 'Error')
      },
      complete: () => {}
    });
  }
}
