import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddBookComponent } from './components/add-book/add-book.component';
import { BooksComponent } from './components/books/books.component';
import { EditBookComponent } from './components/edit-book/edit-book.component';
import { HomeComponent } from './components/home/home.component';
import { LibrarianHubComponent } from './components/librarian-hub/librarian-hub.component';
import { LoginComponent } from './components/login/login.component';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { DetailBookComponent } from './components/detail-book/detail-book.component';
import { InactiveUsersComponent } from './components/users/inactiveUsers/inactive-users.component';
import { ActiveUsersComponent } from './components/users/active-users/active-users/active-users.component';
import { NotificationComponent } from './components/notification/notification.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { BookReservationsComponent } from './components/book-reservations/book-reservations.component';
import { PendingReturnsComponent } from './components/pending-returns/pending-returns.component';
import { OurContactsComponent } from './components/our-contacts/our-contacts.component';
import { CommentApprovalComponent } from './components/comment-approval/comment-approval.component';
import { BooksReturnComponent } from './components/books-return/books-return.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { AccountNewPasswordComponent } from './components/account-new-password/account-new-password.component';
import { GuestUserAuthenticationGuard } from './guards/guest-user-authentication.guard';
import { ReaderUserAuthenticationGuard } from './guards/reader-user-authentication.guard';
import { LibrarianUserAuthenticationGuard } from './guards/librarian-user-authentication.guard';
import { LibrarianAndReaderAuthenticationGuard } from './guards/librarian-and-reader-authentication.guard';

const routes: Routes = [
	{
		path: '',
		component: HomeComponent
	},
	{
		path: 'librarian-hub',
		component: LibrarianHubComponent,
		canActivate: [ LibrarianUserAuthenticationGuard ]
	},
	{
		path: 'signup',
		component: SignUpComponent,
		canActivate: [ GuestUserAuthenticationGuard ]
	},
	{
		path: 'login',
		component: LoginComponent,
		canActivate: [ GuestUserAuthenticationGuard ]
	},
	{
		path: 'books',
		component: BooksComponent,
		canActivate: [ LibrarianUserAuthenticationGuard ]
	},
	{
		path: 'book/add',
		component: AddBookComponent,
		canActivate: [ LibrarianUserAuthenticationGuard ]
	},
	{
		path: 'edit/:id',
		component: EditBookComponent,
		canActivate: [ LibrarianUserAuthenticationGuard ]
	},
	{
		path: 'books/edit/:id',
		component: EditBookComponent,
		canActivate: [ LibrarianUserAuthenticationGuard ]
	},
	{
		path: 'details/:id',
		component: DetailBookComponent
	},
	{
		path: 'books/details/:id',
		component: DetailBookComponent,
		canActivate: [ LibrarianUserAuthenticationGuard ]
	},
	{
		path: 'my-books/details/:id',
		component: DetailBookComponent,
		canActivate: [ ReaderUserAuthenticationGuard ]
	},
	{
		path: 'books-reservations/details/:id',
		component: DetailBookComponent,
		canActivate: [ LibrarianUserAuthenticationGuard ]
	},
	{
		path: 'users/inactive',
		component: InactiveUsersComponent,
		canActivate: [ LibrarianUserAuthenticationGuard ]
	},
	{
		path: 'users/active',
		component: ActiveUsersComponent,
		canActivate: [ LibrarianUserAuthenticationGuard ]
	},
	{
		path: 'notifications',
		component: NotificationComponent,
		canActivate: [ LibrarianAndReaderAuthenticationGuard ]
	},
	{
		path: 'my-books',
		component: BookReservationsComponent,
		canActivate: [ ReaderUserAuthenticationGuard ]
	},
	{
		path: 'books-reservations',
		component: BookReservationsComponent,
		canActivate: [ LibrarianUserAuthenticationGuard ]
	},
	{
		path: 'our-contacts',
		component: OurContactsComponent
	},
	{
		path: 'pending-returns',
		component: PendingReturnsComponent,
		canActivate: [ LibrarianUserAuthenticationGuard ]
	},
	{
		path: 'comments-approval',
		component: CommentApprovalComponent,
		canActivate: [ LibrarianUserAuthenticationGuard ]
	},
	{
		path: 'books-return',
		component: BooksReturnComponent,
		canActivate: [ LibrarianUserAuthenticationGuard ]
	},
	{
		path: 'forgot-password',
		component: ForgotPasswordComponent,
		canActivate: [ GuestUserAuthenticationGuard ]
	},
	{
		path: 'account-new-password',
		component: AccountNewPasswordComponent,
		canActivate: [ GuestUserAuthenticationGuard ]
	},
	{
		path: '**',
		pathMatch: 'full',
		component: PageNotFoundComponent
	}
];

@NgModule({
	imports: [ RouterModule.forRoot(routes) ],
	exports: [ RouterModule ]
})
export class AppRoutingModule {}
