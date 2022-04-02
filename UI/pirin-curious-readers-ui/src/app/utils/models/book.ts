import { author } from "./author";
import { genre } from "./genre";
export interface book {
  id: number,
  title: string,
  isbn: string,
  status: string,
  description: string,
  quantity: number,
  image: string,
  genres: genre[],
  authors: author[],
}
