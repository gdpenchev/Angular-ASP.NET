import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BooksService } from 'src/app/services/books.service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { author } from 'src/app/utils/models/author';
import { genre } from 'src/app/utils/models/genre';
import { regexes } from 'src/app/utils/Regexes';

@Component({
  selector: 'app-edit-book',
  templateUrl: './edit-book.component.html',
  styleUrls: ['./edit-book.component.scss']
})
export class EditBookComponent implements OnInit {
  public authorDropdownList: Array<author> = [];
  public genreDropdownList: Array<genre> = [];
  public dropdownSettings: IDropdownSettings = {};
  public author = '';
  public genres: any[] = [];
  public genre = '';
  public authors: any[] = [];
  public formIsInvalid: boolean = false;
  public editBookForm: FormGroup;
  private newAuthors: any[] = [];
  private newGenres: any[] = [];
  private bookId: number;
  public selectedGenreItems: any[] = [];
  public selectedAuthorItems: any[] = [];
  private uploadData = new FormData();
  private initialBookCover: string = "";
  public currentBookCover: string = "";
  private bookCoverImageFile: File;

  constructor(private formBuilder: FormBuilder,
    private bookService: BooksService,
    private toastr: ToastrService,
    private router: Router,
    public activatedRoute: ActivatedRoute
  ) {
    this.editBookForm = this.createBookForm();
    this.getAuthors();
    this.getGenres();
  }

  ngOnInit() {
    this.bookId = Number(this.activatedRoute.snapshot.paramMap.get('id'));
    this.getBook(this.bookId);
    this.configureDropdownSettings();
  }

  private createBookForm(): FormGroup {
    return this.formBuilder.group({
      title: ['', [Validators.required, Validators.maxLength(128), Validators.pattern(regexes.noEmptySpace)]],
      author: ['', [Validators.maxLength(128), Validators.pattern(regexes.noEmptySpace)]],
      authors: ['', Validators.required],
      genre: ['', [Validators.maxLength(128), Validators.pattern(regexes.noEmptySpace)]],
      genres: ['', Validators.required],
      quantity: ['', [Validators.required, Validators.min(1)]],
      isbn: ['', [Validators.required, Validators.pattern(regexes.isbn)]],
      description: ['', [Validators.maxLength(1028)]],
      image: [null]
    });
  }

  public onAddBookSubmit(event: Event) {
    event.preventDefault();

    if (this.editBookForm.valid) {
      let requestData = Object.assign({}, this.editBookForm.value);
      let authorsArray: string[] = [];
      requestData.authors.forEach((author: any) => {
        if (author && author.id) {
          authorsArray.push(author.name);
        } else if (author) {
          authorsArray.push(author);
        }
      });
      requestData.authors = authorsArray;

      let genresArray: string[] = [];
      requestData.genres.forEach((genre: any) => {
        if (genre && genre.id) {
          genresArray.push(genre.name);
        } else if (genre) {
          genresArray.push(genre);
        }
      });
      requestData.genres = genresArray;
      requestData.status = "enabled";

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
      this.uploadData.set('status', requestData.status);
      this.uploadData.set('imageUrl', this.currentBookCover);
      this.uploadData.set('oldImageUrl', this.initialBookCover);
      this.uploadData.set('image', this.bookCoverImageFile);

      this.bookService.updateBook(this.bookId, this.uploadData).subscribe({
        next: (res: any) => {
          this.router.navigate(['/books']).then(() => {
            this.toastr.success('Book was updated!', 'Success');
          });
        },
        error: (err) => {
          this.toastr.error('Something went wrong!', 'Error');
        },
        complete: () => { }
      });
    } else {
      this.toastr.error('Please, complete the required fields', 'Error');
    }
  }

  public addAuthorIfNotPresent(authorName: string) {
    let authorExists = this.authors.some(function (author) {
      return author.name === authorName
    });
    if (!authorExists && authorName.trim().length !== 0) {
      let nextAuthorId = this.authorDropdownList[this.authorDropdownList.length - 1].id + 1;
      this.newAuthors.push({ id: nextAuthorId, name: authorName });
      this.editBookForm.get('authors')?.setValue((this.editBookForm.controls['authors'].value || []).concat(this.newAuthors));
      this.authorDropdownList = this.authorDropdownList.concat(this.newAuthors);
      this.newAuthors = [];
      this.editBookForm.get('author')?.reset();
    }
  }

  public addGenreIfNotPresent(genreName: string) {
    let genreExists = this.genres.some(function (genre) {
      return genre.name === genreName
    });
    if (!genreExists && genreName.trim().length !== 0) {
      let nextGenreId = this.genreDropdownList[this.genreDropdownList.length - 1].id + 1;
      this.newGenres.push({ id: nextGenreId, name: genreName });
      this.editBookForm.get('genres')?.setValue((this.editBookForm.controls['genres'].value || []).concat(this.newGenres));
      this.genreDropdownList = this.genreDropdownList.concat(this.newGenres);
      this.newGenres = [];
      this.editBookForm.get('genre')?.reset();
    }
  }

  private getAuthors() {
    this.bookService.getAuthors().subscribe((response: any) => {
      if (response) {
        this.authorDropdownList = response.sort(function (a: any, b: any) {
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

  private getBook(bookId: number) {
    this.bookService.getBookById(bookId).subscribe(
      {
        next: (res: any) => {
          if (res) {
            this.fillBookData(res);
          }
        },
        error: (err) => {
          let errorMessage = err.error === "Book with this ISBN already exists." ? err.error : "Something went wrong";
          this.toastr.error(errorMessage, 'Error');
        },
        complete: () => { }
      }
    )
  }

  private configureDropdownSettings() {
    this.dropdownSettings = {
      idField: 'id',
      textField: 'name',
      allowSearchFilter: true
    };
  }

  private fillBookData(currentBookData: any) {
    setTimeout(() => {
      this.editBookForm.get('title')?.setValue(currentBookData.title);
      this.editBookForm.get('authors')?.setValue(currentBookData.authors);
      this.editBookForm.get('genres')?.setValue(currentBookData.genres);
      this.editBookForm.get('quantity')?.setValue(currentBookData.quantity);
      this.editBookForm.get('isbn')?.setValue(currentBookData.isbn);
      this.editBookForm.get('description')?.setValue(currentBookData.description);
      
      this.currentBookCover = currentBookData.image;
      this.initialBookCover = currentBookData.image;

      this.selectedGenreItems = this.genreDropdownList.filter((elem) => currentBookData.genres.find(genreName => elem.name === genreName));
      this.selectedAuthorItems = this.authorDropdownList.filter((elem) => currentBookData.authors.find(authorName => elem.name === authorName));
    }, 50);
  }

  public uploadImage(event: any) {    
    if (event.target.files.length) {
      this.bookCoverImageFile = event.target.files[0];
      this.editBookForm.get('image')?.clearValidators();
    }
  }

  public clearBookCover() {
    this.editBookForm.get('image')?.setValue(null);
    this.currentBookCover = "";
    this.editBookForm.get('image')?.setValidators(Validators.required);
  }
}
