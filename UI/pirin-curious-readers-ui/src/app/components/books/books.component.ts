import { Component, OnInit } from '@angular/core';
import { BooksService } from 'src/app/services/books.service';
import { ReservationsService } from 'src/app/services/reservations.service';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { reservation } from 'src/app/utils/models/reservation';
import { book } from 'src/app/utils/models/book';

@Component({
  selector: 'app-books',
  templateUrl: './books.component.html',
  styleUrls: ['./books.component.scss']
})
export class BooksComponent implements OnInit {
  public searchTerm = '';
  public books: any[] = [];
  public reservations: reservation[] = [];
  public page: number = 1;
  public itemsPerPage: number = 12;
  public totalBooksCount: number = 0;

  constructor(
    private bookService: BooksService,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this.updatePagination();
  }

  public search() {
    this.router.navigate(
      [this.route.snapshot.routeConfig?.path],
      {
        queryParams: {
          'searchText': this.searchTerm
        }
      });

    this.route.queryParams.subscribe(params => {
      let querySearchParam = this.searchTerm ? `searchText=${params['searchText']}` : '';
      this.getBooks(querySearchParam);
      this.getBooksTotalCount(querySearchParam);
    });
  }

  public getBooks(querySearchParam: string) {
    let queryParams = `?userEmail=${this.authService.getUserEmail()}&page=${this.page}&pageSize=${this.itemsPerPage}&${querySearchParam}`;

    this.bookService.getBooks(queryParams).subscribe({
      next: (res: any) => {
        if (res) {
          this.books = res;
        }
      },
      error: (err) => {
      },
      complete: () => { }
    });
  }

  public getBooksTotalCount(querySearchParam?: string) {
    let queryParams = `?userEmail=${this.authService.getUserEmail()}`;

    if (querySearchParam) {
      querySearchParam = `&${querySearchParam}`;
      queryParams += querySearchParam;
    }

    this.bookService.getBooksTotalCount(queryParams).subscribe({
      next: (res: any) => {
        if (res) {
          this.totalBooksCount = res;
        }
      },
      error: (err) => {
      },
      complete: () => { }
    });
  }

  public updatePagination() {
    this.searchTerm = '';
    this.getBooks('');
    this.getBooksTotalCount('');
  }

  public userIsLibrarian() {
    return this.authService.userIsLibrarian();
  }

  public checkVisibility(book: book) {
    return this.authService.userIsLoggedIn() && this.userIsLibrarian() ? true : book.status === 'Enabled'
  }
}
