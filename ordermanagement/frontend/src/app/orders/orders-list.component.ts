import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { RouterLink } from '@angular/router';

interface OrderListItem {
  id: number;
  customerId: number;
  status: number;
  total: number;
  createdAt: string;
}

@Component({
  selector: 'app-orders-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <section style="padding: var(--space-6);">
      <h1>Orders</h1>

      @if (loading()) {
        <p>Loading…</p>
      } @else if (error()) {
        <p style="color: var(--color-danger);">{{ error() }}</p>
      } @else {
        <ul>
          @for (order of orders(); track order.id) {
            <li>
              <a [routerLink]="['/orders', order.id]">
                Order #{{ order.id }} — customer {{ order.customerId }} — {{ order.total | number:'1.2-2' }}
              </a>
            </li>
          } @empty {
            <li>No orders yet.</li>
          }
        </ul>
      }
    </section>
  `,
})
export class OrdersListComponent implements OnInit {
  private readonly http = inject(HttpClient);

  readonly orders = signal<OrderListItem[]>([]);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  ngOnInit(): void {
    this.http.get<OrderListItem[]>('/api/orders').subscribe({
      next: data => {
        this.orders.set(data);
        this.loading.set(false);
      },
      error: err => {
        this.error.set(err?.message ?? 'Failed to load orders.');
        this.loading.set(false);
      },
    });
  }
}
