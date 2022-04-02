import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { regexes } from 'src/app/utils/Regexes';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  @Input() userIsLogged: boolean;
  public loginForm: FormGroup;
  public showPassword: boolean = false;

  constructor(private fb: FormBuilder,
    private service: AuthService,
    private router: Router,
    private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      'email': ['', [Validators.required, Validators.pattern(regexes.email)]],
      'password': ['', Validators.required]
    })
  }

  public login(event: Event) {
    event.preventDefault();
    var valueForm = this.loginForm.value;
    this.service.login(valueForm).subscribe({
      next: (res: any) => {
        this.service.saveToken(res.token.result);
        this.toastr.success('User logged in successfully!', 'Authentication succeeded.');
        this.router.navigateByUrl('/');
      },
      error: (err) => {
        this.toastr.error('Incorrect credentials.', 'Authentication failed.');
      },
      complete: () => { }
    });
  }

  public togglePasswordVisibillity() {
    this.showPassword = !this.showPassword;
  }
}
