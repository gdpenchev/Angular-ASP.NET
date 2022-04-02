import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { countries } from 'src/app/utils/Countries';
import { regexes } from 'src/app/utils/Regexes';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signUp',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent {
  public countries = countries;
  public formIsInvalid: boolean = false;
  public signUpForm: FormGroup;

  constructor(private formBuilder: FormBuilder, private authService: AuthService, private toastr: ToastrService, private router: Router) {
    this.signUpForm = this.formBuilder.group({
      firstName: ['', [Validators.required, Validators.maxLength(128), Validators.pattern(regexes.noEmptySpace)]],
      lastName: ['', [Validators.required, Validators.maxLength(128), Validators.pattern(regexes.noEmptySpace)]],
      email: ['', [Validators.required, Validators.pattern(regexes.email)]],
      password: ['', [Validators.required, Validators.pattern(regexes.password)]],
      repeatPassword: ['', [Validators.required]],
      phoneNumber: ['', [Validators.required, Validators.pattern(regexes.phone)]],
      country: ['', Validators.required],
      city: ['', [Validators.required, Validators.pattern(regexes.noEmptySpace)]],
      street: ['', [Validators.required, Validators.maxLength(128), Validators.pattern(regexes.noEmptySpace)]],
      streetNumber: ['', [Validators.required, Validators.maxLength(65), Validators.pattern(regexes.noEmptySpace)]],
      buildingNumber: ['', Validators.maxLength(65)],
      apartmentNumber: ['', Validators.maxLength(65)],
      additionalInfo: ['', Validators.maxLength(1028)],
    }, { validators: this.passwordConfirming });
  }

  public onRegisterSubmit(event: Event): void {
    event.preventDefault();

    if (this.signUpForm.valid) {
      let requestData = Object.assign({}, this.signUpForm.value);
      requestData.phoneNumber = requestData.phoneNumber.toString();

      this.authService.register(requestData).subscribe({
        next: (res: any) => {
          this.toastr.success('You have registered successfully, please wait for approval!',
            'Success',
            { timeOut: 10000 });

          this.router.navigateByUrl('/');
        },
        error: (err) => {
          this.toastr.error('Something went wrong!', 'Register failed');
        },
        complete: () => { }
      });
    } else {
      this.toastr.error('Please, complete the required fields!', 'Error');
    }
  }

  private passwordConfirming(control: AbstractControl): { passwordsNotMatching: boolean } | null {
    if (control.get('password')?.value !== control.get('repeatPassword')?.value) {
      return { passwordsNotMatching: true };
    }

    return null;
  }
}
