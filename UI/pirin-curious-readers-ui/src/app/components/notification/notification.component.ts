import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthService } from 'src/app/services/auth.service';
import { NotificationsService } from 'src/app/services/notifications.service';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.scss']
})
export class NotificationComponent implements OnInit {

  userId: string;
  notifications: any[];
  librarianNotification: any[];

  public notEmptyPost: boolean = true;
  public notScrolly: boolean = true;
  public notificationsPerPage: number = 7;
  public skip: number = 0;


  constructor(
    private notifyService: NotificationsService,
    private authService: AuthService,
    private spinner: NgxSpinnerService) { }

  ngOnInit(): void {
    if (this.authService.userIsLoggedIn() && !this.authService.userIsLibrarian())
    this.getNotifications();
    else
    this.getlibrarianNotification();
  }

  public getlibrarianNotification(){
    this.notifyService.librarianNotification(this.skip,this.notificationsPerPage).subscribe({
      next: (res:any) => {
        this.librarianNotification = res;
      },
      error: (err) => {
      },
      complete: () => { }
    })
  }

  public getNotifications(){
    this.userId = this.authService.getUserId();

    this.notifyService.all(this.userId,this.skip,this.notificationsPerPage).subscribe({
      next: (res: any) => {
        this.notifications = res;
      },
      error: (err) => {
      },
      complete: () => { } 
    })
  }

  public onScroll(){
    if (this.notScrolly && this.notEmptyPost){
      this.notScrolly = false;
      this.loadNextNotifications();
      this.spin();
    }
  }
  private spin(){
    this.spinner.show();

    setTimeout(() => {
      this.spinner.hide();
    }, 1000);
  }

  private loadNextNotifications(){
    this.skip++;
    this.notifyService.all(this.userId,this.skip,this.notificationsPerPage).subscribe({
      next: (res:any) => {
        var newData = res;
        if(newData.length == 0) {
          this.notEmptyPost = false;
        }
        this.notifications = this.notifications.concat(newData);
        this.notScrolly=true;
      },
      error: (error) =>{
      },
      complete: () => {}
    });
  }
}
