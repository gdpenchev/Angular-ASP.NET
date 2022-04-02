import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';
import { BooksService } from 'src/app/services/books.service';
import { CommentsService } from 'src/app/services/comments.service';
import { book } from 'src/app/utils/models/book';
import { NgxSpinnerService } from 'ngx-spinner';
import { ReservationsService } from 'src/app/services/reservations.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-detail-book',
  templateUrl: './detail-book.component.html',
  styleUrls: ['./detail-book.component.scss']
})
export class DetailBookComponent implements OnInit {
  public ratingForm: FormGroup;
  private bookId: number;
  public book: book;
  public userName: string;
  public content: string;
  public rating: number;
  public comments: any[];
  public notEmptyPost: boolean = true;
  public notScrolly: boolean = true;
  public commentsPerPage: number = 3;
  public skip: number = 0;
  public userId: string;
  public isBorrowed: boolean;

  constructor(
     public activatedRoute: ActivatedRoute,
     private bookService: BooksService,
     private commentService: CommentsService,
     private toastr: ToastrService,
     private authService: AuthService,
     private spinner: NgxSpinnerService,
     private reservationService: ReservationsService,
     private formBuilder: FormBuilder,
     private router: Router) { 

      this.ratingForm = this.formBuilder.group({
        rating: [0, Validators.required],
      });
  }

  ngOnInit(): void {
    this.bookId = Number(this.activatedRoute.snapshot.paramMap.get('id'));
    this.getBook(this.bookId);
    this.getComments();
    this.userName = this.authService.getUserEmail();
    this.userId = this.authService.getUserId();
    this.checkStatus();
  }
  
  private getBook(bookId: number) {
    this.bookService.getBookById(bookId).subscribe(
      {
        next: (res: any) => {
          if (res) {
            this.book = res;
          }

          if ((!this.authService.userIsLoggedIn() || !this.authService.userIsLibrarian()) && this.book.status.toLowerCase() !== "enabled") {
            this.router.navigateByUrl('/');
            return;
          }
        },
        error: (err) => {
          this.toastr.error('Something went wrong!', 'Error');
        },
        complete: () => { }
      }
    )
  }

  public addComment(bookId: number, content: string) {
    if (!this.content){
      this.toastr.error('Please write a comment!', 'No content found!');
      return;
    } else if (!this.ratingForm.get('rating')?.value) {
      this.toastr.error('In order to submit this comment you need to select rating!', 'Please add rating!');
      return;
    }

    let requestData = { 
      bookId: bookId, 
      userName: this.userName, 
      content: content, 
      rating: this.ratingForm.get('rating')?.value
    };
    
    this.commentService.addComment(requestData).subscribe({
      next: (res: any) => {
        this.toastr.success('Your comment has been submitted. It will be visible after approval from librarian!','Success');
        this.content = '';
        this.skip = 0;
        this.ratingForm.get('rating')?.reset();
        this.ratingForm.get('rating')?.setValue(0);
        this.getComments();
      },
      error: (err) => {
      },
      complete: () => { }
    });
  }

  private getComments(){
    this.commentService.getCommentsById(this.bookId, this.skip, this.commentsPerPage).subscribe({
      next: (res:any) => {
        this.comments = res;
      },
      error: (error) =>{
      },
      complete: () => {}
    });
  }

  public onScroll(){
    if (this.notScrolly && this.notEmptyPost){
      this.notScrolly = false;
      this.loadNextPost();
      this.spin();
    }
  }

  private loadNextPost(){
    this.skip++;
    this.commentService.getCommentsById(this.bookId,this.skip,this.commentsPerPage).subscribe({
      next: (res:any) => {
        var newData = res;
        if(newData.length == 0) {
          this.notEmptyPost = false;
        }
        this.comments = this.comments.concat(newData);
        this.notScrolly = true;
      },
      error: (error) =>{
      },
      complete: () => {}
    })
  }

  private spin(){
    this.spinner.show();

    setTimeout(() => {
      this.spinner.hide();
    }, 1000);
  }

  public userIsLoggedIn(): boolean {
    return this.authService.userIsLoggedIn();
  }

  public checkStatus(){
    this.reservationService.checkStatus(this.bookId,this.userId).subscribe({
      next: (res:any) => {
        this.isBorrowed = res;
      },
      error: (error) =>{
      },
      complete: () => {}
    })
  }
}
