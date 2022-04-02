import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable, of } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient, private jwtHelper: JwtHelperService) { }


  login(formData) {
    return this.http.post(environment.baseUrl + '/Users/Login', formData)
  }

  saveToken(token) {
    localStorage.setItem('token', token);
  }

  userIsLoggedIn() : boolean {
    let userIsLogged: Observable<boolean> = of(localStorage.getItem('token') === null ? false : true);
    
    let userLogged = false;
    userIsLogged.subscribe(res => {
      userLogged = res.valueOf();
    });

    return userLogged;
  }

  clearToken() : Observable<Boolean> {
    localStorage.removeItem('token');
    let tokenCleared: Observable<boolean> = of(localStorage.getItem('token') === null ? true : false);
    return tokenCleared;
  }
 
  register(formData: Object) {
    return this.http.post(environment.baseUrl + '/Users/Register', formData)
  }

  userIsLibrarian() {
    const token = this.jwtHelper.decodeToken(this.jwtHelper.tokenGetter());
    
    if (token) {
      const roleName = 'Librarian';

      return token && token.role && Array.isArray(token.role) && token.role.includes(roleName) ||
        token.role && typeof token.role === 'string' && token.role === roleName
        ? true : false;
    }
    
    return false;
  }

  getUserEmail(): string {
    const token = this.jwtHelper.decodeToken(this.jwtHelper.tokenGetter());

    return token && token.Email ? token.Email : '';
  }

  getUserFullName(): string {
    const token = this.jwtHelper.decodeToken(this.jwtHelper.tokenGetter());

    return token && token.FullName ? token.FullName : '';
  }

  getUserId(): string {
    const token = this.jwtHelper.decodeToken(this.jwtHelper.tokenGetter());
    return token && token.Id ? token.Id : '';
  }

  userIsActive(): boolean {
    const token = this.jwtHelper.decodeToken(this.jwtHelper.tokenGetter());
    return token && token.UserIsActive.toLowerCase() === 'true' ? true : false;
  }

  userIsLoggedActivatedReader(): boolean {
    return this.userIsLoggedIn() && !this.userIsLibrarian() && this.userIsActive();
  }

  userIsLoggedActivatedLibrarian(): boolean {
    return this.userIsLoggedIn() && this.userIsLibrarian() && this.userIsActive();
  }

  userIsLoggedActivatedLibrarianOrReader(): boolean {
    return this.userIsLoggedIn() && (this.userIsLibrarian() || !this.userIsLibrarian()) && this.userIsActive();
  }
}
