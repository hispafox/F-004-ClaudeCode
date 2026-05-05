import { ComponentFixture, TestBed } from '@angular/core/testing';
import { {{PASCAL_NAME}}Component } from './{{KEBAB_NAME}}.component';

describe('{{PASCAL_NAME}}Component', () => {
  let component: {{PASCAL_NAME}}Component;
  let fixture: ComponentFixture<{{PASCAL_NAME}}Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [{{PASCAL_NAME}}Component],
    }).compileComponents();

    fixture = TestBed.createComponent({{PASCAL_NAME}}Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('debe crearse', () => {
    expect(component).toBeTruthy();
  });
});
