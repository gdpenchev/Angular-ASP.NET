import { TestBed } from '@angular/core/testing';

import { LibrarianAndReaderAuthenticationGuard } from './librarian-and-reader-authentication.guard';

describe('LibrarianAndReaderAuthenticationGuard', () => {
  let guard: LibrarianAndReaderAuthenticationGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(LibrarianAndReaderAuthenticationGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
