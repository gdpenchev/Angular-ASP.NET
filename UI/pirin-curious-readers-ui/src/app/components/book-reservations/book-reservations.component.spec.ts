import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookReservationsComponent } from './book-reservations.component';

describe('BookReservationsComponent', () => {
  let component: BookReservationsComponent;
  let fixture: ComponentFixture<BookReservationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BookReservationsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BookReservationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
