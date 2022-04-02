import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CommentsService } from 'src/app/services/comments.service';

@Component({
  selector: 'app-comment-approval',
  templateUrl: './comment-approval.component.html',
  styleUrls: ['./comment-approval.component.scss']
})
export class CommentApprovalComponent implements OnInit {

  comments: any[];

  public page: number = 1;
  public commentsPerPage: number = 7;
  public totalComments: number;

  constructor(private commentService: CommentsService,
    private toastr: ToastrService ) { }

  ngOnInit(): void {
    this.getUnapproved()
    this.countUnapproved()
  }

  getUnapproved(){
    this.commentService.getUnapproved(this.page, this.commentsPerPage).subscribe({
      next: (res:any) => {
        this.comments = res;
      },
      error: (error) =>{
      },
      complete: () => {}
    });
  }

  approve(commentId:any){
    this.commentService.approveComment(commentId).subscribe({
      next: (res: any) => {
        this.toastr.success('Success', 'Comment has been approved!');
        this.getUnapproved();
      },
      error: (err) => {
        this.toastr.error('Approval error', 'Unable to approve!')
      },
      complete: () => { }
    })
  }

  onPageChange(event: any) {
    this.page = event;
    this.getUnapproved();
  }
  
  countUnapproved(){
    this.commentService.countUnapproved().subscribe({
      next: (res:any) => {
        this.totalComments = res;
      },
      error: () =>{

      },
      complete: () => { }
    })
  }
}
