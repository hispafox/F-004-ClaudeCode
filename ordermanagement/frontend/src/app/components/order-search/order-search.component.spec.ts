import { ComponentFixture, TestBed } from '@angular/core/testing';
import { OrderSearchComponent } from './order-search.component';

describe('OrderSearchComponent', () => {
  let component: OrderSearchComponent;
  let fixture: ComponentFixture<OrderSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderSearchComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(OrderSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('emits searched event with trimmed term', () => {
    let emitted: string | undefined;
    component.searched.subscribe(value => (emitted = value));

    component.searchTerm.set('  Pending  ');
    component.onSubmit();

    expect(emitted).toBe('Pending');
  });

  it('does not emit when term is empty', () => {
    let emitted: string | undefined;
    component.searched.subscribe(value => (emitted = value));

    component.searchTerm.set('   ');
    component.onSubmit();

    expect(emitted).toBeUndefined();
  });
});
