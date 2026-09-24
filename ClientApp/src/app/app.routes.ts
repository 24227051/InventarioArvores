import { Routes } from '@angular/router';
import { EspecieFormComponent } from './components/especie-form/especie-form.component';
import { EspecieListComponent } from './components/especie-list/especie-list.component';
import { ArvoreFormComponent } from './components/arvore-form/arvore-form.component';
import { ArvoreListComponent } from './components/arvore-list/arvore-list.component';
import { ArvoreViewComponent } from './components/arvore-view/arvore-view.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthGuard } from './guards/auth.guard';
import { MenuVisibilityGuard } from './guards/menu-visibility.guard';

export const routes: Routes = [
  // Public pages
  { path: 'login', component: LoginComponent, canActivate: [MenuVisibilityGuard], data: { showMenu: false } },
  { path: 'register', component: RegisterComponent, canActivate: [MenuVisibilityGuard], data: { showMenu: false } },

  // Protected sections
  { path: 'especies', component: EspecieListComponent, canActivate: [AuthGuard, MenuVisibilityGuard], data: { showMenu: true } },
  { path: 'add-especie', component: EspecieFormComponent, canActivate: [AuthGuard, MenuVisibilityGuard], data: { showMenu: true } },
  { path: 'edit-especie/:id', component: EspecieFormComponent, canActivate: [AuthGuard, MenuVisibilityGuard], data: { showMenu: true } },

  { path: 'arvores', component: ArvoreListComponent, canActivate: [AuthGuard, MenuVisibilityGuard], data: { showMenu: true } },
  { path: 'add-arvore', component: ArvoreFormComponent, canActivate: [AuthGuard, MenuVisibilityGuard], data: { showMenu: true } },
  { path: 'edit-arvore/:id', component: ArvoreFormComponent, canActivate: [AuthGuard, MenuVisibilityGuard], data: { showMenu: true } },

  // Tree detail without menu
  { path: 'arvores/:id', component: ArvoreViewComponent, canActivate: [MenuVisibilityGuard], data: { showMenu: false } },

  { path: '', redirectTo: '/especies', pathMatch: 'full' },
];
