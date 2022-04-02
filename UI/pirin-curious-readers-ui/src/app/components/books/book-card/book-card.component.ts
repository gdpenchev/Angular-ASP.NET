import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { BooksService } from 'src/app/services/books.service';
import { ReservationsService } from 'src/app/services/reservations.service';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmModalComponent } from '../../common/confirm-modal/confirm-modal.component';
import { Router } from '@angular/router';
import { NoopScrollStrategy } from '@angular/cdk/overlay';
import { book } from 'src/app/utils/models/book';
import { InputModalityDetector } from '@angular/cdk/a11y';
import { NotificationsService } from 'src/app/services/notifications.service';

@Component({
	selector: 'app-book-card',
	templateUrl: './book-card.component.html',
	styleUrls: [ './book-card.component.scss' ]
})
export class BookCardComponent implements OnInit {
	@Input() book: any;
	@Input() reservations: any;
	@Input() reservationPage: boolean = false;
	@Input() pendingReturnPage: boolean = false;
	@Input() reservation: any = {};
	@Output() sendPending: EventEmitter<any> = new EventEmitter();
	@Output() updateBooks: EventEmitter<any> = new EventEmitter();
	@Output() updateReservations: EventEmitter<any> = new EventEmitter();
	public statusPendingReservationApproval = 'PendingReservationApproval';
	public statusPendingProlongationApproval = 'PendingProlongationApproval';
	public statusReserved: string = 'Reserved';
	public statusBorrowed: string = 'Borrowed';
	public statusDisabled: string = 'Disabled';
	public statusEnabled: string = 'Enabled';
	public statusRejected: string = 'Rejected';
	public statusReturned: string = 'Returned';
	private statusDeleted: string = 'Deleted';
	public cutEndSymbolTitle: number = 14;
	public cutEndSymbolAuthors: number = 17;

	constructor(
		private authService: AuthService,
		private bookService: BooksService,
		private reservationService: ReservationsService,
		private notifyService: NotificationsService,
		private toastr: ToastrService,
		public dialog: MatDialog,
		private router: Router
	) {}

	ngOnInit(): void {}

	public userIsLibrarian() {
		return this.authService.userIsLibrarian();
	}

	public redirectToDetails() {
		this.router.navigate([ '/' ]);
	}

	public userIsLoggedIn(): boolean {
		return this.authService.userIsLoggedIn();
	}

	private updateBookPartially(book: any, bookStatus: string) {
		book.status = bookStatus.trim() + 'd';
		let requestData = Object.assign({}, book);

		if (book.status === this.statusDeleted) {
			requestData.image = null;
			requestData.imageUrl = book.image;
			requestData.oldImageUrl = book.image;
		}

		this.bookService.updateBookPartially(requestData).subscribe({
			next: (res: any) => {
				this.router.navigate([ '/books' ]);
				this.toastr.success(`Book was ${book.status}!`, 'Success');

				if (book.status === 'Deleted') {
					this.updateBooks.emit('');
				}
			},
			error: (err) => {
				this.toastr.error('Something went wrong!', 'Error');
			},
			complete: () => {}
		});
	}

	private sendBookReservationRequest() {
		let reservationData = {
			userEmail: this.authService.getUserEmail(),
			bookId: this.book.id
		};

		this.reservationService.requestReservation(reservationData).subscribe({
			next: (res: any) => {
				this.toastr.success(`Book reservation request has been sent. Please wait for approval!`, 'Success');
				this.updateBooks.emit('');
			},
			error: (err) => {
				this.toastr.error('Something went wrong!', 'Error');
			},
			complete: () => {}
		});
	}

	public openDialog(
		event: any,
		isReservationRequest?: boolean,
		isReminderSender?: boolean,
		isRejectedReservation?: boolean,
		isProlongingRequest?: boolean
	): void {
		let operationType: string = event.target.textContent.toLowerCase();

		if (operationType === 'borrow') {
			operationType = `confirm ${operationType}`;
		} else if (operationType === 'approve') {
			operationType = `${operationType} reservation`;
		} else if (operationType === 'reject') {
			operationType = `${operationType} reservation`;
		} else if (operationType === 'borrow') {
			operationType = `${operationType} reservation`;
		}

		const modalRef = this.dialog.open(ConfirmModalComponent, {
			width: '350px',
			data: {
				title: `Are you sure that you want to ${operationType} ${isReservationRequest ||
				isReminderSender ||
				isProlongingRequest
					? 'for'
					: ''} this book?`,
				message: `This action will ${operationType} ${isReservationRequest ||
				isReminderSender ||
				isProlongingRequest
					? 'for'
					: ''} the current book!`
			},
			scrollStrategy: new NoopScrollStrategy()
		});

		modalRef.afterClosed().subscribe((actionConfirmed) => {
			if (actionConfirmed) {
				if (!isReservationRequest && !isReminderSender && !isProlongingRequest) {
					this.updateBookPartially(this.book, this.toTitleCase(operationType));
				} else if (isReservationRequest) {
					this.handleReservations(isRejectedReservation);
				} else if (isReminderSender) {
					this.sendReminder(this.book.id, this.reservation.userName);
				} else if (isProlongingRequest) {
					this.requestProlongation();
				}
			}
		});
	}

	public openProlongationDialog(event: any, isPrologned: boolean): void {
		let operationType: string = event.target.textContent.toLowerCase();

		const modalRef = this.dialog.open(ConfirmModalComponent, {
			width: '350px',
			data: {
				title: `Are you sure that you want to ${operationType === 'prolong'
					? 'approve'
					: `${operationType}`} prolongation for this book?`,
				message: `This action will ${operationType === 'prolong'
					? 'confirm'
					: `${operationType} `} prolongation for the current book!`
			},
			scrollStrategy: new NoopScrollStrategy()
		});

		modalRef.afterClosed().subscribe((actionConfirmed) => {
			if (actionConfirmed) {
				if (isPrologned) {
					this.approveProlongation();
				} else {
					this.rejectProlongation();
				}
			}
		});
	}

	public rejectProlongation() {
		this.reservationService.rejectProlongation(this.reservation.id).subscribe({
			next: (res: any) => {
				this.updateReservations.emit();
				this.toastr.success(`Book prolonging was rejected successfully!`, 'Success');
			},
			error: (err) => {
				this.toastr.error('Something went wrong!', 'Error');
			},
			complete: () => {}
		});
	}

	public approveProlongation() {
		this.reservationService.approveBookStatusChange(this.reservation.id, false).subscribe({
			next: (res: any) => {
				this.updateReservations.emit();
				this.toastr.success(`Book was prolonged successfully!`, 'Success');
			},
			error: (err) => {
				this.toastr.error('Something went wrong!', 'Error');
			},
			complete: () => {}
		});
	}

	public bookReservationRequestSent(): boolean {
		if (!this.authService.userIsLibrarian() && this.book.reserveeEmails.includes(this.authService.getUserEmail())) {
			return true;
		}
		return false;
	}

	private toTitleCase(str: string) {
		return str.replace(/\w\S*/g, function(txt: string) {
			return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();
		});
	}

	private handleReservations(isRejectedReservation?: boolean) {
		if (this.userIsLibrarian()) {
			this.approveBookStatusChange(isRejectedReservation);
		} else {
			this.sendBookReservationRequest();
		}
	}

	private approveBookStatusChange(isRejectedReservation?: boolean) {
		this.reservationService.approveBookStatusChange(this.reservation.id, isRejectedReservation || false).subscribe({
			next: (res: any) => {
				this.updateReservations.emit();
				if (isRejectedReservation) {
					this.toastr.success(`Book was ${this.statusRejected.toLowerCase()} successfully!`, 'Success');
				} else {
					if (this.reservation.status === this.statusBorrowed) {
						this.toastr.success(`Book was ${this.statusReturned.toLowerCase()} successfully!`, 'Success');
					} else {
						this.toastr.success(
							`Book was ${this.reservation.status === this.statusReserved
								? this.statusBorrowed.toLowerCase()
								: this.statusReserved.toLowerCase()} successfully!`,
							'Success'
						);
					}
				}
			},
			error: (err) => {
				this.toastr.error('Something went wrong!', 'Error');
			},
			complete: () => {}
		});
	}

	public sendReminder(bookId: number, email: string) {
		let data = { email: email, bookId: bookId };

		this.notifyService.send(data).subscribe({
			next: (res: any) => {
				this.toastr.success('Reminder has been sent to the user successfully', 'Success');
			},
			error: (err) => {},
			complete: () => {}
		});
	}

	public showDisabledBookToTheUserIfReserved(book: book): boolean {
		if (book.status === 'Enabled' || (book.status === 'Disabled' && this.reservationPage)) {
			return true;
		}
		return false;
	}

	public requestProlongation() {
		this.reservationService.requestProlongation(this.reservation.id).subscribe({
			next: (res: any) => {
				this.updateReservations.emit();
				this.toastr.success('Prolonging request has been sent successfully', 'Success');
			},
			error: (err) => {},
			complete: () => {}
		});
	}
}
