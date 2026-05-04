import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'orders',
  },
  {
    path: 'orders',
    loadComponent: () =>
      import('./orders/orders-list.component').then(m => m.OrdersListComponent),
  },
  {
    path: 'orders/:id',
    loadComponent: () =>
      import('./orders/order-detail.component').then(m => m.OrderDetailComponent),
  },
];
