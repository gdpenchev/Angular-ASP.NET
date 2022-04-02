import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { ReservationsService } from 'src/app/services/reservations.service';
import { reservation } from 'src/app/utils/models/reservation';

@Component({
  selector: 'app-pending-returns',
  templateUrl: './pending-returns.component.html',
  styleUrls: ['./pending-returns.component.scss']
})
export class PendingReturnsComponent implements OnInit {

  public page: number = 1;
  public itemsPerPage: number = 6;
  public reservationsPendingReturn: reservation[] = [];
  public totalPendingCount: number

  constructor(private authService: AuthService,
    private reservationService: ReservationsService) { }

  ngOnInit(): void {
    this.getPendingReturn();
    this.getPendingReturnCount();
  }

  userIsLibrarian() {
    return this.authService.userIsLibrarian();
  }

  public getPendingReturn() {
    this.reservationService.getPendingReturn(this.page, this.itemsPerPage).subscribe({
      next: (res: any) => {
        if (res) {
          this.reservationsPendingReturn = res;
        }
      },
      error: (err) => {
      },
      complete: () => { }
    });
  }

  public getPendingReturnCount(){
    this.reservationService.getPendingReturnCount().subscribe({
      next: (res: any) => {
        if (res) {
          this.totalPendingCount = res;
        }
      },
      error: (err) => {
      },
      complete: () => { }
    });
  }

  pageChange(event: any) {
    this.page = event;
    this.getPendingReturn();
    this.getPendingReturnCount();
  }
}
