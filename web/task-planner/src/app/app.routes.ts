import { Routes } from '@angular/router';
import { Login } from './components/login/login';
import { Register } from './components/register/register';
import { TaskList } from './components/task-list/task-list';
import { TaskForm } from './components/task-form/task-form';
import { TaskDetail } from './components/task-detail/task-detail';
import { Dashboard } from './components/dashboard/dashboard';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  { path: 'tasks', component: TaskList },
  { path: 'tasks/new', component: TaskForm },
  { path: 'tasks/:id', component: TaskDetail },
  { path: 'tasks/:id/edit', component: TaskForm },
  { path: 'dashboard', component: Dashboard },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];