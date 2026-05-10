import { Component, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-order-search',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './order-search.component.html',
  styleUrl: './order-search.component.scss',
})
export class OrderSearchComponent {
  readonly searchTerm = signal('');
  readonly searched = output<string>();

  onSubmit(): void {
    const term = this.searchTerm().trim();
    if (term.length === 0) {
      return;
    }
    this.searched.emit(term);
  }
}
