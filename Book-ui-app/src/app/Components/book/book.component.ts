import {
  Component,
  ElementRef,
  inject,
  OnInit,
  ViewChild,
  viewChild,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Book } from '../../Models/book';
import { BookService } from '../../Services/book.service';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-book',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './book.component.html',
  styleUrl: './book.component.css',
})
export class BookComponent implements OnInit {
  @ViewChild('myModal') model: ElementRef | undefined;
  bookList: Book[] = [];
  bookService = inject(BookService);

  bookForm: FormGroup = new FormGroup({});

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.setFormState();
    this.getBooks();
  }

  openModal() {
    const bookModal = document.getElementById('myModel');
    if (bookModal != null) {
      bookModal.style.display = 'block';
    }
  }

  closeModal() {
    this.setFormState();
    if (this.model != null) {
      this.model.nativeElement.style.display = 'none';
    }
  }

  getBooks() {
    this.bookService.getAllBooks().subscribe((res) => {
      console.log('All books list:', res);
      this.bookList = res;
    });
  }

  setFormState() {
    this.bookForm = this.fb.group({
      id: [0],
      title: ['', [Validators.required]],
      author: ['', [Validators.required]],
      isbn: ['', [Validators.required]],
      publicationDate: ['', [Validators.required]],
    });
  }

  formValues: any;

  onSubmit() {
    if (this.bookForm.invalid) {
      alert('Please Fill All Fields');
      return;
    }

    const formData = this.bookForm.value;

    if (formData.id === 0) {
      this.bookService.addBook(formData).subscribe({
        next: (res: any) => {
          console.log('Saved Book:', res);

          alert('Book Added Successfully');

          this.bookList.push(res);

          this.bookForm.reset({
            id: 0,
            title: '',
            author: '',
            isbn: '',
            publicationDate: '',
          });

          this.closeModal();
        },

        error: (err) => {
          console.error('Error saving book', err);
        },
      });
    } else {
      this.bookService.updateBook(formData).subscribe({
        next: () => {
          alert('Book Updated Successfully');

          this.getBooks();

          this.bookForm.reset({
            id: 0,
            title: '',
            author: '',
            isbn: '',
            publicationDate: '',
          });

          this.closeModal();
        },

        error: (err) => {
          console.error('Error updating book', err);
        },
      });
    }
  }
  onEdit(book: Book) {
    this.openModal();
    this.bookForm.patchValue(book);
  }

  onDelete(book: Book) {
    const isConfirm = confirm(
      'Are you sure you want to delete this Book ' + book.title,
    );
    if (isConfirm) {
      this.bookService.deleteBook(book.id).subscribe((res) => {
        alert('Book Deleted Successfully');
        this.getBooks();
      });
    }
  }
}
