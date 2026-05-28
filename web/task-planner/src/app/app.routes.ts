import { Routes } from '@angular/router';
import { AuthGuard } from './guards/auth-guard';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./components/login/login').then(m => m.LoginComponent) },
  { path: 'register', loadComponent: () => import('./components/register/register').then(m => m.RegisterComponent) },
  { path: 'tasks', loadComponent: () => import('./components/task-list/task-list').then(m => m.TaskListComponent), canActivate: [AuthGuard] },
  { path: 'tasks/new', loadComponent: () => import('./components/task-form/task-form').then(m => m.TaskFormComponent), canActivate: [AuthGuard] },
  { path: 'tasks/:id', loadComponent: () => import('./components/task-detail/task-detail').then(m => m.TaskDetailComponent), canActivate: [AuthGuard] },
  { path: 'tasks/:id/edit', loadComponent: () => import('./components/task-form/task-form').then(m => m.TaskFormComponent), canActivate: [AuthGuard] },
  { path: 'dashboard', loadComponent: () => import('./components/dashboard/dashboard').then(m => m.DashboardComponent), canActivate: [AuthGuard] },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];