import { TestBed } from '@angular/core/testing';

import { GuestUserAuthenticationGuard } from './guest-user-authentication.guard';

describe('GuestUserAuthenticationGuard', () => {
  let guard: GuestUserAuthenticationGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(GuestUserAuthenticationGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
