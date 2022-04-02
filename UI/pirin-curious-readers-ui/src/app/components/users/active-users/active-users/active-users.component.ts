import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { AuthService } from 'src/app/services/auth.service';
@Component({
  selector: 'app-active-users',
  templateUrl: './active-users.component.html',
  styleUrls: ['./active-users.component.scss']
})
export class ActiveUsersComponent implements OnInit {
  isActive: boolean;
  id: string
  data: Array<any>
  totalRecords: number;
  itemsPerPage: number = 20;
  page: number = 1;

  constructor(private userService: UserService, private authService: AuthService) {
    this.data = new Array<any>();
    this.isActive = true;
  }

  ngOnInit(): void {
    this.getUsers();
  }

  onPageChange(event: any) {
    this.page = event;
    this.getUsers();
  }

  getUsers(): void {
    this.userService.getAllUsers(this.authService.getUserEmail(), this.page, this.itemsPerPage, this.isActive).subscribe({
      next: (res: any) => {
        this.data = res.users;
        this.totalRecords = res.usersCount;
      },
      error: (err) => {
      },
      complete: () => { }
    })
  }
}
