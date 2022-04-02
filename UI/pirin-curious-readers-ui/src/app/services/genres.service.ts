import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { genre } from '../utils/models/genre';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GenresService {

  constructor(private httpClient: HttpClient) { }

  getGenres(): Observable<genre[]> {
    return this.httpClient.get<genre[]>(`${environment.baseUrl}/Genres`);
  }

  getGenresTotalCount() {
    return this.httpClient.get<number>(`${environment.baseUrl}/Genres/Count`);
  }
}
