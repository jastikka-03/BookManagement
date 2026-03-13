import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Book } from '../Models/book';

@Injectable({
  providedIn: 'root',
})
export class BookService {
  private apiUrl = 'https://localhost:7143/api/Book';
  http = inject(HttpClient);

  getAllBooks() {
    return this.http.get<Book[]>('https://localhost:7143/api/Books');
  }

  addBook(data: any) {
    return this.http.post(this.apiUrl, data);
  }

  updateBook(book: Book) {
    return this.http.put(`${this.apiUrl}/${book.id}`, book);
  }

  deleteBook(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
