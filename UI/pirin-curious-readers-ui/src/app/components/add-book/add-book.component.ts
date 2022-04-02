import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators, } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BooksService } from 'src/app/services/books.service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { author } from 'src/app/utils/models/author';
import { genre } from 'src/app/utils/models/genre';
import { regexes } from 'src/app/utils/Regexes';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmModalComponent } from '../common/confirm-modal/confirm-modal.component';
import { NoopScrollStrategy } from '@angular/cdk/overlay';

@Component({
  selector: 'app-add-book',
  templateUrl: './add-book.component.html',
  styleUrls: ['./add-book.component.scss']
})
export class AddBookComponent {
  dropdownList: Array<author> = [];
  genreDropdownList: Array<genre> = [];
  dropdownSettings: IDropdownSettings = {};
  public author = '';
  public genres: any[] = [];
  public genre = '';
  public authors: any[] = [];
  public formIsInvalid: boolean = false;
  public addBookForm: FormGroup;
  private newAuthors: any[] = [];
  private newGenres: any[] = [];
  private uploadData = new FormData();
  private bookCoverImageFile: File;

  constructor(
    private formBuilder: FormBuilder, 
    private bookService: BooksService, 
    private toastr: ToastrService,
    private router: Router,
    private dialog: MatDialog,) {
    this.addBookForm = this.createBookForm();
  }

  ngOnInit() {
    this.getAuthors();
    this.getGenres();
    this.configureDropdownSettings();
  }

  createBookForm(): FormGroup {
    return this.formBuilder.group({
      title: ['', [Validators.required, Validators.maxLength(128), Validators.pattern(regexes.noEmptySpace)]],
      author: ['', [Validators.maxLength(128), Validators.pattern(regexes.noEmptySpace)]],
      authors: ['', Validators.required],
      genre: ['', [Validators.maxLength(128), Validators.pattern(regexes.noEmptySpace)]],
      genres: ['', Validators.required],
      quantity: ['', [Validators.required, Validators.min(1)]],
      isbn: ['', [Validators.required, Validators.pattern(regexes.isbn)]],
      description: ['', [Validators.maxLength(1028)]],
      image: [null, [Validators.required]]
    })
  }

  onAddBookSubmit(event: Event) {
    event.preventDefault();
    if (this.addBookForm.valid) {
      let requestData = Object.assign({}, this.addBookForm.value);
      let authorsArray: string[] = [];
      requestData.authors.forEach((author: any) => {
        authorsArray.push(author.name);
      });
      requestData.authors = authorsArray;

      let genresArray: string[] = [];
      requestData.genres.forEach((genre: any) => {
        genresArray.push(genre.name);
      });
      requestData.genres = genresArray;

      requestData.genres.forEach(genre => {
        this.uploadData.append('genres', genre);
      });

      requestData.authors.forEach(author => {
        this.uploadData.append('authors', author);
      });

      this.uploadData.set('title', requestData.title)
      this.uploadData.set('description', requestData.description);
      this.uploadData.set('isbn', requestData.isbn);
      this.uploadData.set('quantity', requestData.quantity);
      this.uploadData.set('image', this.bookCoverImageFile);

      this.bookService.addBook(this.uploadData).subscribe({
        next: (res: any) => {
          if (res.restoreAvailable) {
            this.openDialog(res.bookId);
          } else {
            this.router.navigate(['/books']).then(() => {
              this.toastr.success('Book was added!', 'Success');
            });
          }
        },
        error: (err) => {
          let errorMessage = err.error === "Book with this ISBN already exists." ? err.error : "Something went wrong";
          this.toastr.error(errorMessage, 'Error');
        },
        complete: () => { }
      });
    } else {
      this.toastr.error('Please, complete the required fields', 'Error');
    }
  }

  addAuthorIfNotPresent(authorName: string) {
    let authorExists = this.authors.some(function (author) {
      return author.name === authorName
    });
    if (!authorExists && authorName.trim().length !== 0) {
      let nextAuthorId = this.dropdownList[this.dropdownList.length - 1].id + 1;
      this.newAuthors.push({ id: nextAuthorId, name: authorName });
      this.addBookForm.get('authors')?.setValue((this.addBookForm.controls['authors'].value || []).concat(this.newAuthors));
      this.dropdownList = this.dropdownList.concat(this.newAuthors);
      this.newAuthors = [];
      this.addBookForm.get('author')?.reset();
    }
  }

  addGenreIfNotPresent(genreName: string) {
    let genreExists = this.genres.some(function (genre) {
      return genre.name === genreName
    });
    if (!genreExists && genreName.trim().length !== 0) {
      let nextGenreId = this.genreDropdownList[this.genreDropdownList.length - 1].id + 1;
      this.newGenres.push({ id: nextGenreId, name: genreName });
      this.addBookForm.get('genres')?.setValue((this.addBookForm.controls['genres'].value || []).concat(this.newGenres));
      this.genreDropdownList = this.genreDropdownList.concat(this.newGenres);
      this.newGenres = [];
      this.addBookForm.get('genre')?.reset();
    }
  }

  private getAuthors() {
    this.bookService.getAuthors().subscribe((response: any) => {
      if (response) {
        this.dropdownList = response.sort(function (a: any, b: any) {
          return a.id - b.id;
        });
      }
    });
  }

  private getGenres() {
    this.bookService.getGenres().subscribe((response: any) => {
      if (response) {
        this.genreDropdownList = response.sort(function (a: any, b: any) {
          return a.id - b.id;
        });
      }
    });
  }

  private configureDropdownSettings() {
    this.dropdownSettings = {
      idField: 'id',
      textField: 'name',
      allowSearchFilter: true
    };
  }

  public uploadImage(event: any) {
    if (event.target.files.length) {
      this.bookCoverImageFile = event.target.files[0];
    }
  }

  public openDialog(bookId: number): void {
    const modalRef = this.dialog.open(ConfirmModalComponent, {
      width: '450px',
      data: {
        title: `Deleted book restoring!`,
        message: `We spotted that book with the same ISBN was deleted previously. If you want to restore this book you need to edit the previous data first! Do you want to continue?`
      },
      scrollStrategy: new NoopScrollStrategy()
    });

    modalRef.afterClosed().subscribe(actionConfirmed => {
      if (actionConfirmed) {
        this.router.navigate([`/edit/${bookId}`]);
      }
    });
  }
}
