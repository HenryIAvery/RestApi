import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthApi } from '../services/auth-api';

@Component({
  selector: 'app-login',
  imports: [FormsModule, RouterLink],
  template: `
    <div class="login-wrap">
      <h1>Iniciar Sesión</h1>
      <form (ngSubmit)="entrar()" class="card login-card">
        <label>
          Correo
          <input type="email" [(ngModel)]="correo" name="correo" required />
        </label>
        <label>
          Contraseña
          <input type="password" [(ngModel)]="password" name="password" required />
        </label>
        @if (error()) {
          <p class="error">{{ error() }}</p>
        }
        <button type="submit">Entrar</button>
      </form>
      <button type="button" routerLink="/">Volver al inicio</button>
    </div>
  `,
  styles: [`
    @import '../carta-p/carta-p.css';
    .login-card {
      max-width: 420px;
      display: flex;
      flex-direction: column;
      gap: 16px;
    }
    label {
      display: flex;
      flex-direction: column;
      gap: 6px;
      color: #b9f5ff;
      font-weight: 600;
    }
    input {
      padding: 10px 12px;
      background: #080d16;
      border: 1px solid #263b4e;
      color: #e8f7ff;
      border-radius: 4px;
      font-family: inherit;
      font-size: 1rem;
    }
    .error {
      color: #ff6b6b !important;
      margin: 0;
    }
  `]
})
export class Login {
  private auth = inject(AuthApi);
  private router = inject(Router);

  correo = 'admin@itq.ec';
  password = 'Admin12345';
  error = signal('');

  entrar() {
    this.error.set('');
    this.auth.login(this.correo, this.password).subscribe({
      next: () => this.router.navigateByUrl('/reporte'),
      error: () => this.error.set('Credenciales incorrectas o servidor no disponible.'),
    });
  }
}
