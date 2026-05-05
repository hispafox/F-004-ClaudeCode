import { Component, inject, input, output, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
{{INJECTIONS_IMPORTS}}

@Component({
  selector: 'app-{{KEBAB_NAME}}',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './{{KEBAB_NAME}}.component.html',
  styleUrl: './{{KEBAB_NAME}}.component.scss',
})
export class {{PASCAL_NAME}}Component {
  // 3. Inputs
{{INPUTS}}

  // 4. Outputs
{{OUTPUTS}}

  // 5. Inyecciones
{{INJECTIONS}}

  // 6. Estado (signals)
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  // 7. Lifecycle hooks
  // (añadir ngOnInit / ngOnDestroy si aplica)

  // 8. Métodos
  // (públicos primero, privados después)
}
