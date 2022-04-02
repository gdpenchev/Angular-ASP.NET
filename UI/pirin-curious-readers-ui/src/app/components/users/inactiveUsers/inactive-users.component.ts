import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/services/user.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-user',
  templateUrl: './inactive-users.component.html',
  styleUrls: ['./inactive-users.component.scss']
})
export class InactiveUsersComponent implements OnInit {
  isActive: boolean;
  id: string
  data: Array<any>
  totalRecords: number;
  itemsPerPage: number = 20;
  page: number = 1;

  constructor(private userService: UserService, private toastr: ToastrService, private authService: AuthService) {
    this.data = new Array<any>();
    this.isActive = false;
  }

  ngOnInit(): void {
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

  onPageChange(event: any) {
    this.page = event;
    this.getUsers();
  }

  approveUser(event: any) {
    event.preventDefault();
    this.id = event.target.attributes.id.nodeValue;
    this.userService.approve(this.id).subscribe({
      next: (res: any) => {
        this.toastr.success('Approval confirmation', 'User has been approved!');
        this.getUsers();

      },
      error: (err) => {
        this.toastr.error('Approval error', 'Unable to approve!')
      },
      complete: () => { }
    })
  }
}
