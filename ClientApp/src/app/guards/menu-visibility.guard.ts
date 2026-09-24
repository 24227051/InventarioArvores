import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { MenuService } from '../services/menu.service';

@Injectable({ providedIn: 'root' })
export class MenuVisibilityGuard implements CanActivate {
  constructor(private menu: MenuService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const show = route.data['showMenu'];
    // If undefined, default to true
    this.menu.setShowMenu(show === undefined ? true : !!show);
    return true;
  }
}
