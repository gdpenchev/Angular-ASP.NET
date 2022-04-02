import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibrarianHubComponent } from './librarian-hub.component';

describe('LibrarianHubComponent', () => {
  let component: LibrarianHubComponent;
  let fixture: ComponentFixture<LibrarianHubComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LibrarianHubComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LibrarianHubComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
