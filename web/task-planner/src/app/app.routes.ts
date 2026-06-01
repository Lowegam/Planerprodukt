import { Routes } from '@angular/router';
export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./components/login/login').then(m => m.LoginComponent) },
  { path: 'register', loadComponent: () => import('./components/register/register').then(m => m.RegisterComponent) },
  { path: 'tasks', loadComponent: () => import('./components/task-list/task-list').then(m => m.TaskListComponent) },
  { path: 'tasks/new', loadComponent: () => import('./components/task-form/task-form').then(m => m.TaskFormComponent) },
  { path: 'tasks/:id', loadComponent: () => import('./components/task-detail/task-detail').then(m => m.TaskDetailComponent) },
  { path: 'tasks/:id/edit', loadComponent: () => import('./components/task-form/task-form').then(m => m.TaskFormComponent) },
  { path: 'dashboard', loadComponent: () => import('./components/dashboard/dashboard').then(m => m.DashboardComponent) },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];