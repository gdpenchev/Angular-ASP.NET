import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { ReservationsService } from 'src/app/services/reservations.service';
import { reservation } from 'src/app/utils/models/reservation';

@Component({
	selector: 'app-books-return',
	templateUrl: './books-return.component.html',
	styleUrls: [ './books-return.component.scss' ]
})
export class BooksReturnComponent implements OnInit {
	public reservations: reservation[] = [];
	public page: number = 1;
	public itemsPerPage: number = 12;
	public totalReservationsCount: number;
	private userEmail: string;
	public reservationStatusSelected: string = '';

	constructor(private authService: AuthService, private reservationService: ReservationsService) {}

	ngOnInit(): void {
		this.userEmail =
			this.authService.userIsLoggedIn() && this.authService.userIsLibrarian()
				? ''
				: this.authService.getUserEmail();
		this.updateReservationsData();
	}

	getUserEmail() {
		return this.authService.getUserEmail();
	}

	userIsLibrarian() {
		return this.authService.userIsLibrarian();
	}

	public updateReservationsData() {
		this.getAllReservations();
		this.getAllReservationsCount();
	}

	public getAllReservations() {
		this.reservationService
			.getAllReservationsForTheUser(this.page, this.itemsPerPage, this.userEmail, this.reservationStatusSelected)
			.subscribe({
				next: (res: any) => {
					if (res) {
						res = res.filter((r) => {
							return r.status.toLowerCase() === 'borrowed' || r.status.toLowerCase() === 'returned';
						});

						this.reservations = res;
					}
				},
				error: (err) => {},
				complete: () => {}
			});
	}

	public getAllReservationsCount() {
		this.reservationService
			.getUserReservationsTotalCount(this.userEmail, this.reservationStatusSelected)
			.subscribe({
				next: (res: any) => {
					if (res) {
						this.totalReservationsCount = res;
					}
				},
				error: (err) => {},
				complete: () => {}
			});
	}

	public updateReservationsOnFilterChange(newStatus: string) {
		this.reservationStatusSelected = newStatus;
		this.updateReservationsData();
	}
}
