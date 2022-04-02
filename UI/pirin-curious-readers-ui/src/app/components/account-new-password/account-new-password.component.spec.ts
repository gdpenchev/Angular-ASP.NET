import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountNewPasswordComponent } from './account-new-password.component';

describe('AccountNewPasswordComponent', () => {
  let component: AccountNewPasswordComponent;
  let fixture: ComponentFixture<AccountNewPasswordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountNewPasswordComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountNewPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
