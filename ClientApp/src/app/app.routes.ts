import { Routes } from '@angular/router';
import { EspecieFormComponent } from './components/especie-form/especie-form.component';
import { EspecieListComponent } from './components/especie-list/especie-list.component';
import { ArvoreFormComponent } from './components/arvore-form/arvore-form.component';
import { ArvoreListComponent } from './components/arvore-list/arvore-list.component';

export const routes: Routes = [
  { path: 'especies', component: EspecieListComponent },
  { path: 'add-especie', component: EspecieFormComponent },
  { path: 'edit-especie/:id', component: EspecieFormComponent },
  { path: 'arvores', component: ArvoreListComponent },
  { path: 'add-arvore', component: ArvoreFormComponent },
  { path: 'edit-arvore/:id', component: ArvoreFormComponent },
  { path: '', redirectTo: '/especies', pathMatch: 'full' },
];
