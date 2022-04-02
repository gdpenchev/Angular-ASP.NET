import { book } from "./book";

export interface reservation {
    id: number,
    userName: string,
    book: book,
    requestDate: Date,
    returnDate: Date,
    status: string
  }
  