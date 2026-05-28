import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./components/login/login.component').then(m => m.LoginComponent) },
  { path: 'register', loadComponent: () => import('./components/register/register.component').then(m => m.RegisterComponent) },
  { path: 'tasks', loadComponent: () => import('./components/task-list/task-list.component').then(m => m.TaskListComponent), canActivate: [() => import('./guards/auth-guard').then(m => m.AuthGuard)] },
  { path: 'tasks/new', loadComponent: () => import('./components/task-form/task-form.component').then(m => m.TaskFormComponent) },
  { path: 'tasks/:id', loadComponent: () => import('./components/task-detail/task-detail.component').then(m => m.TaskDetailComponent) },
  { path: 'tasks/:id/edit', loadComponent: () => import('./components/task-form/task-form.component').then(m => m.TaskFormComponent) },
  { path: 'dashboard', loadComponent: () => import('./components/dashboard/dashboard.component').then(m => m.DashboardComponent) },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];