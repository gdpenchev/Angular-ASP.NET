import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {

  constructor(private http: HttpClient) { }

  send(data:any){
    return this.http.post(environment.baseUrl + '/Notifications/Send', data);
  }

  unread(userId: string){
    const params = new HttpParams()
    .set("userId", userId);
    return this.http.get(`${environment.baseUrl}/Notifications/Unread`,{params})
  }

  all(userId: string, skip:number, notificationsPerPage: number){
    const params = new HttpParams()
    .set("userId", userId)
    .set("skip", skip)
    .set("notificationsPerPage", notificationsPerPage);
    return this.http.get(`${environment.baseUrl}/Notifications/All`,{params})
  }

  librarianNotification(skip:number, notificationsPerPage: number){
    const params = new HttpParams()
    .set("skip", skip)
    .set("notificationsPerPage", notificationsPerPage);
    return this.http.get(`${environment.baseUrl}/Notifications/BooksNotOnTime`,{params})
  }
}
