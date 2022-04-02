import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';
import { GenresService } from 'src/app/services/genres.service';
import { BooksService } from 'src/app/services/books.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  public activeUsersCount: number;
  public totalGenresCount: number;
  public totalBooksCount: number;
  public genres: any[] = [];
  
  constructor(
    private authService: AuthService, 
    private userService: UserService,
    private genreService: GenresService,
    private bookService: BooksService
    ) { }

  ngOnInit(): void {
    this.getAllActiveUsersCount(true);
    this.getGenres();
    this.getGenresTotalCount();
    this.getBooksTotalCount();
  }
  
  public getUserFullName() {
    return this.authService.getUserFullName();
  }

  public userIsLoggedIn() {
    return this.authService.userIsLoggedIn();
  }

  public getAllActiveUsersCount(isActive: boolean) {
    this.userService.getAllUsersCount(isActive).subscribe({
      next: (res: any) => {
        this.activeUsersCount = res;
      },
      error: (err) => {
      },
      complete: () => { }
    })
  }

  public getGenres() {

    this.genreService.getGenres().subscribe({
      next: (res: any) => {
        if (res) {
          res.forEach(genre => {
            this.genres.push(genre.name);
          });

        }
      },
      error: (err) => {
      },
      complete: () => { }
    });
  }

  public getGenresTotalCount() {

    this.genreService.getGenresTotalCount().subscribe({
      next: (res: any) => {
        if (res) {
          this.totalGenresCount = res;
        }
      },
      error: (err) => {
      },
      complete: () => { }
    });
  }

  public getBooksTotalCount() {
    var queryParams = '';

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

  public userIsActive() {
    return this.authService.userIsActive();
  }
}
