import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { reservation } from '../utils/models/reservation';

@Injectable({
  providedIn: 'root'
})
export class ReservationsService {

  constructor(private httpClient: HttpClient) { }

  requestReservation(reservation: any): Observable<reservation> {
    return this.httpClient.post<reservation>(`${environment.baseUrl}/Reservations`, reservation);
  }

  getAllReservationsForTheUser(page: number, itemsPerPage: number, userEmail: string = "", reservationStatusSelected: string = ""): Observable<reservation[]> {
    const params = new HttpParams()
    .set('page', page)
    .set('itemsPerPage', itemsPerPage)
    .set('userEmail', userEmail)
    .set('reservationStatus', reservationStatusSelected);

    return this.httpClient.get<reservation[]>(`${environment.baseUrl}/Reservations`, { params: params });
  }

  getPendingReturn(page:number,itemsPerPage:number){
    const params = new HttpParams()
    .set('page', page)
    .set('itemsPerPage', itemsPerPage);

    return this.httpClient.get<reservation[]>(`${environment.baseUrl}/Reservations/PendingReturn`, { params: params });
  }
  getPendingReturnCount(){
    return this.httpClient.get(`${environment.baseUrl}/Reservations/PendingCount`);
  }

  getUserReservationsTotalCount(userEmail: string = "", reservationStatusSelected: string = "") {
    const params = new HttpParams()
      .set('userEmail', userEmail)
      .set('reservationStatus', reservationStatusSelected);

    return this.httpClient.get<number>(`${environment.baseUrl}/Reservations/Count`, { params: params });
  }

  approveBookStatusChange(reservationId: number, isRejected : boolean): Observable<reservation> {

    return this.httpClient.patch<reservation>(`${environment.baseUrl}/Reservations/${reservationId}/${isRejected}`, {});
  }

  checkStatus(bookId: number, userId: string){
    const params = new HttpParams()
      .set('bookId', bookId)
      .set('userId', userId)
    return this.httpClient.get<any>(`${environment.baseUrl}/Reservations/CheckStatus`,{params});
  }
  requestProlongation(reservationId: number){
    const params = new HttpParams()
      .set('reservationId', reservationId);
    return this.httpClient.patch(environment.baseUrl + '/Reservations/Prolongation', {body:params},{params:params});
  }
  rejectProlongation(reservationId: number){
    const params = new HttpParams()
      .set('reservationId', reservationId);
    return this.httpClient.patch(environment.baseUrl + '/Reservations/Reject', {body:params},{params:params});
  }
}
