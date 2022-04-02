import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BooksReturnComponent } from './books-return.component';

describe('BooksReturnComponent', () => {
  let component: BooksReturnComponent;
  let fixture: ComponentFixture<BooksReturnComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BooksReturnComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BooksReturnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
