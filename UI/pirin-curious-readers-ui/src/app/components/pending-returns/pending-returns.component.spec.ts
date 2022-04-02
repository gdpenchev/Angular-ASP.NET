import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PendingReturnsComponent } from './pending-returns.component';

describe('PendingReturnsComponent', () => {
  let component: PendingReturnsComponent;
  let fixture: ComponentFixture<PendingReturnsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PendingReturnsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PendingReturnsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
