import { TestBed } from '@angular/core/testing';

import { ReaderUserAuthenticationGuard } from './reader-user-authentication.guard';

describe('ReaderUserAuthenticationGuard', () => {
  let guard: ReaderUserAuthenticationGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(ReaderUserAuthenticationGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
