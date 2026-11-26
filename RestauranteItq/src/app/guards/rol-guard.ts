import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthApi } from '../services/auth-api';

export const rolGuard = (...roles: string[]): CanActivateFn => {
  return () => {
    const auth = inject(AuthApi);
    const router = inject(Router);
    const rol = auth.rol();

    if (rol && roles.includes(rol)) return true;

    return router.createUrlTree(['/login']);
  };
};
