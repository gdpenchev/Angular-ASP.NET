import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { book } from '../utils/models/book';
import { author } from '../utils/models/author';
import { genre } from '../utils/models/genre';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { JwtHelperService } from '@auth0/angular-jwt';
@Injectable({
  providedIn: 'root'
})
export class BooksService {

  constructor(private httpClient: HttpClient, private jwtHelper: JwtHelperService) { }
  
  getBookById(bookId: number): Observable<book> {
    return this.httpClient.get<book>(`${environment.baseUrl}/Books/${bookId}`);
  }

  getBooks(queryParams?: string): Observable<book[]> {
    return this.httpClient.get<book[]>(`${environment.baseUrl}/Books${queryParams}`);
  }

  getBooksTotalCount(queryParams?: string) {
    return this.httpClient.get<number>(`${environment.baseUrl}/Books/Count${queryParams}`);
  }
  
  addBook(book: FormData): Observable<book> {  
    return this.httpClient.post<book>(`${environment.baseUrl}/Books`, book);
  }

  updateBook(bookId: number, book: FormData): Observable<book> {
    return this.httpClient.put<book>(`${environment.baseUrl}/Books/${bookId}`, book);
  }

  updateBookPartially(book: book): Observable<book> {
    return this.httpClient.patch<book>(`${environment.baseUrl}/Books/${book.id}`, book);
  }

  getAuthors(): Observable<author[]> {
    return this.httpClient.get<author[]>(`${environment.baseUrl}/Authors`,);
  }

  getGenres(): Observable<genre[]> {
    return this.httpClient.get<genre[]>(`${environment.baseUrl}/Genres`,);
  }
}
