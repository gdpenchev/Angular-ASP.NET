import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from "@angular/router";

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {

  constructor(private router: Router) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if(localStorage.getItem('token') != null){
      const clonedReq = request.clone({
        headers: request.headers.set('Authorization','Bearer ' + localStorage.getItem('token'))
      });
      return next.handle(clonedReq)
    }
    else{
      return next.handle(request.clone());
    }
  }
}
