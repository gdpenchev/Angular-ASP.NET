import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { changePassword } from '../utils/models/changePassword';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) {
  }

  getAllUsers(userEmail: string, page: number, itemsPerPage: number, isActive: boolean) {
    const params = new HttpParams()
      .set('userEmail', userEmail)
      .set('page', page)
      .set('itemsPerPage', itemsPerPage)
      .set('isActive',isActive);
    return this.http.get(environment.baseUrl + '/Users/All' ,{params:params})
  }

  approve(id: string) {
    const params = new HttpParams()
      .set('id', id);
    return this.http.patch(environment.baseUrl + '/Users/ApproveUser', { body: params }, { params: params } );
  }

  getAllUsersCount(isActive: boolean) {
    const params = new HttpParams()
      .set('isActive', isActive);
    return this.http.get(environment.baseUrl + '/Users/Count', { params: params })
  }

  forgotPassword(userEmail: string) {
    const params = new HttpParams()
      .set('userEmail', userEmail);
    return this.http.post(environment.baseUrl + '/Users/ForgotPassword', { body: params }, { params: params });
  }

  changePassword(changePasswordRequest: changePassword) {
    return this.http.post(environment.baseUrl + '/Users/ChangePassword', changePasswordRequest);
  }
}
