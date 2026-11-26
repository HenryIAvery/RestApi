import { Injectable, inject, signal, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';

export interface LoginRespuesta {
  token: string;
  expira: string;
  nombre: string;
  rol: string;
}

const TOKEN_ADMIN_DEMO = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJSZXN0YXVyYW50ZUFwcCIsImlzcyI6IlJlc3RhdXJhbnRlQXBpIiwiZXhwIjoxNzk1NzI5ODYyLCJzdWIiOiIxIiwibmFtZSI6IkFkbWluIElUUSIsImVtYWlsIjoiYWRtaW5AaXRxLmVjIiwicm9sZSI6IkFkbWluaXN0cmFkb3IiLCJpYXQiOjE3OTU3MDEwNjIsIm5iZiI6MTc5NTcwMTA2Mn0.GOZJI7YmnTkM0MrErJuO2ZUATZs7ZckTfHjF7RGYfM8';

@Injectable({ providedIn: 'root' })
export class AuthApi {
  private http = inject(HttpClient);
  private platformId = inject(PLATFORM_ID);
  private url = 'http://localhost:5260/auth';

  private esNavegador(): boolean {
    return isPlatformBrowser(this.platformId);
  }

  token = signal<string | null>(this.leerStorage('token', TOKEN_ADMIN_DEMO));
  rol = signal<string | null>(this.leerStorage('rol', 'Administrador'));
  nombre = signal<string | null>(this.leerStorage('nombre', 'Admin ITQ'));

  private leerStorage(clave: string, valorPorDefecto: string): string | null {
    if (!this.esNavegador()) return valorPorDefecto;
    const guardado = localStorage.getItem(clave);
    if (guardado) return guardado;
    localStorage.setItem(clave, valorPorDefecto);
    return valorPorDefecto;
  }

  login(correo: string, password: string) {
    return this.http
      .post<LoginRespuesta>(`${this.url}/login`, { correo, password })
      .pipe(
        tap((r) => {
          if (this.esNavegador()) {
            localStorage.setItem('token', r.token);
            localStorage.setItem('rol', r.rol);
            localStorage.setItem('nombre', r.nombre);
          }
          this.token.set(r.token);
          this.rol.set(r.rol);
          this.nombre.set(r.nombre);
        })
      );
  }

  salir() {
    if (this.esNavegador()) {
      localStorage.removeItem('token');
      localStorage.removeItem('rol');
      localStorage.removeItem('nombre');
    }
    this.token.set(null);
    this.rol.set(null);
    this.nombre.set(null);
  }
}
