import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CommentsService {

  constructor(private http: HttpClient) { }

  addComment(data: any){
    return this.http.post(environment.baseUrl + '/Comments/Add', data);
  }
  getCommentsById(bookId: number, skip:number, commentsPerPage: number){
    const params = new HttpParams()
    .set('bookId',bookId)
    .set('skip', skip)
    .set('commentsPerPage',commentsPerPage)
    return this.http.get(`${environment.baseUrl}/Comments/All`,{params})
  }
  getUnapproved(page: number, commentsPerPage: number){
    const params = new HttpParams()
    .set('page', page)
    .set('commentsPerPage', commentsPerPage);
    return this.http.get(`${environment.baseUrl}/Comments/Unapproved`,{params})
  }
  approveComment(commentId: number){
    const params = new HttpParams()
      .set('commentId', commentId);
    return this.http.patch(environment.baseUrl + '/Comments/ApproveComment', {body:params},{params:params});
  }
  countUnapproved(){
    return this.http.get(`${environment.baseUrl}/Comments/Count`)
  }
}
