import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, RouterLink } from '@angular/router';

interface OrderDetail {
  id: number;
  customerId: number;
  status: number;
  total: number;
  createdAt: string;
  items: Array<{
    productName: string;
    quantity: number;
    unitPrice: number;
  }>;
}

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <section style="padding: var(--space-6);">
      <a routerLink="/orders">← Back</a>

      @let o = order();
      @if (loading()) {
        <p>Loading…</p>
      } @else if (error()) {
        <p style="color: var(--color-danger);">{{ error() }}</p>
      } @else if (o) {
        <h1>Order #{{ o.id }}</h1>
        <p>Customer: {{ o.customerId }}</p>
        <p>Status: {{ o.status }}</p>
        <p>Total: {{ o.total | number:'1.2-2' }}</p>

        <h2>Items</h2>
        <ul>
          @for (item of o.items; track $index) {
            <li>
              {{ item.productName }} — {{ item.quantity }} × {{ item.unitPrice | number:'1.2-2' }}
            </li>
          }
        </ul>
      }
    </section>
  `,
})
export class OrderDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly http = inject(HttpClient);

  readonly order = signal<OrderDetail | null>(null);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.error.set('Missing order id.');
      this.loading.set(false);
      return;
    }

    this.http.get<OrderDetail>(`/api/orders/${id}`).subscribe({
      next: data => {
        this.order.set(data);
        this.loading.set(false);
      },
      error: err => {
        this.error.set(err?.message ?? 'Failed to load order.');
        this.loading.set(false);
      },
    });
  }
}
