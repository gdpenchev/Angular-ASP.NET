import { Component, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NotificationsService } from 'src/app/services/notifications.service';

@Component({
  selector: 'app-side-menu',
  templateUrl: './side-menu.component.html',
  styleUrls: ['./side-menu.component.scss']
})
export class SideMenuComponent implements OnInit, OnDestroy, OnChanges {

  name: string;
  public isAuthenticated: boolean;
  countUnread: number;
  userId: string;

  constructor(
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService,
    private notifyService: NotificationsService
  ) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.authService.userIsLoggedActivatedReader()) {
      this.getCountUnread()
    }
  }

  ngOnInit(): void {
    if (this.authService.userIsLoggedActivatedReader()) {
      this.getCountUnread();
    }
  }

  ngOnDestroy(): void {
  }

  public userIsLoggedIn(): boolean {
    return this.authService.userIsLoggedIn();
  }

  public getCountUnread(){
    this.userId = this.authService.getUserId();
    this.notifyService.unread(this.userId).subscribe({
      next: (res: any) => {
        if (res) {
          this.countUnread = res;
        }
      },
      error: (err) => {
        this.toastr.error("Something went wrong", 'Error');
      },
      complete: () => { } 
    })
  }
  
  public returnCount(){
    return this.countUnread;
  }

  public userIsLibrarian() {
    return this.authService.userIsLibrarian();
  }
  
  public logoutUser() {
    this.authService.clearToken().subscribe(tokenCleared => {
      if (tokenCleared) {
        this.toastr.success('User logged out successfully!', 'Success');
        this.router.navigateByUrl('/');
      }
    })
  }

  public notifications(){
    this.countUnread = 0;
    this.router.navigateByUrl('/notifications');
  }

  public userIsLoggedActivatedReader(): boolean {
    return this.authService.userIsLoggedActivatedReader();
  }

  public userIsLoggedActivatedLibrarian(): boolean {
    return this.authService.userIsLoggedActivatedLibrarian();
  }
}
