import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { ToastrModule } from 'ngx-toastr';
import { AppComponent } from './app.component';
import { SideMenuComponent } from './components/side-menu/side-menu.component';
import { LoginComponent } from './components/login/login.component';
import { AuthService } from './services/auth.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { AddBookComponent } from './components/add-book/add-book.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { AuthenticationInterceptor } from './services/authentication.interceptor';
import { HomeComponent } from './components/home/home.component';
import { LibrarianHubComponent } from './components/librarian-hub/librarian-hub.component';
import { JwtModule } from '@auth0/angular-jwt';
import { InactiveUsersComponent } from './components/users/inactiveUsers/inactive-users.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { BooksComponent } from './components/books/books.component';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
import { BookCardComponent } from './components/books/book-card/book-card.component';
import { FormsModule } from '@angular/forms';
import { DetailBookComponent } from './components/detail-book/detail-book.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { NgxSpinnerModule } from 'ngx-spinner';
import { EditBookComponent } from './components/edit-book/edit-book.component';
import { ConfirmModalComponent } from './components/common/confirm-modal/confirm-modal.component';
import { HamburgerToggleDirective } from './directives/hamburger-toggle.directive';
import { ActiveUsersComponent } from './components/users/active-users/active-users/active-users.component';
import { NotificationComponent } from './components/notification/notification.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { BookReservationsComponent } from './components/book-reservations/book-reservations.component';
import { PendingReturnsComponent } from './components/pending-returns/pending-returns.component';
import { FooterComponent } from './components/footer/footer.component';
import { CommentApprovalComponent } from './components/comment-approval/comment-approval.component';
import { NgxStarRatingModule } from 'ngx-star-rating';
import { BooksReturnComponent } from './components/books-return/books-return.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { AccountNewPasswordComponent } from './components/account-new-password/account-new-password.component';
import { GuestUserAuthenticationGuard } from './guards/guest-user-authentication.guard';
import { ReaderUserAuthenticationGuard } from './guards/reader-user-authentication.guard';
import { LibrarianUserAuthenticationGuard } from './guards/librarian-user-authentication.guard';

@NgModule({
	declarations: [
		AppComponent,
		SideMenuComponent,
		LoginComponent,
		SignUpComponent,
		AddBookComponent,
		HomeComponent,
		LibrarianHubComponent,
		InactiveUsersComponent,
		BooksComponent,
		BookCardComponent,
		DetailBookComponent,
		EditBookComponent,
		ConfirmModalComponent,
		HamburgerToggleDirective,
		ActiveUsersComponent,
		BookReservationsComponent,
		NotificationComponent,
		PageNotFoundComponent,
		PendingReturnsComponent,
		FooterComponent,
		CommentApprovalComponent,
		BooksReturnComponent,
		ForgotPasswordComponent,
		AccountNewPasswordComponent
	],
	imports: [
		AppRoutingModule,
		ReactiveFormsModule,
		HttpClientModule,
		NgxPaginationModule,
		BrowserAnimationsModule,
		ToastrModule.forRoot({
			progressBar: true,
			closeButton: true,
			preventDuplicates: true,
			timeOut: 8000
		}),
		NgSelectModule,
		NgMultiSelectDropDownModule.forRoot(),
		HttpClientModule,
		JwtModule.forRoot({
			config: {
				tokenGetter: () => localStorage.getItem('token')
			}
		}),
		MatCardModule,
		MatInputModule,
		MatButtonModule,
		MatIconModule,
		FormsModule,
		NgxPaginationModule,
		MatDialogModule,
		MatSelectModule,
		InfiniteScrollModule,
		NgxSpinnerModule,
		NgxStarRatingModule
	],
	providers: [
		AuthService,
		GuestUserAuthenticationGuard,
		ReaderUserAuthenticationGuard,
		LibrarianUserAuthenticationGuard,
		{ provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true }
	],
	bootstrap: [ AppComponent ]
})
export class AppModule {}
