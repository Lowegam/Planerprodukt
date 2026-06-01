import { Component } from '@angular/core';
import { TaskService } from '../../services/task.service';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [MatTableModule, MatButtonModule, MatIconModule, MatSelectModule, MatFormFieldModule, MatInputModule, FormsModule, CommonModule],
  templateUrl: './task-list.html',
  styleUrl: './task-list.css'
})
export class TaskListComponent {
  tasks: any[] = [];
  columns = ['title', 'status', 'priority', 'deadline', 'actions'];
  sf = ''; pf = ''; q = '';
  constructor(private ts: TaskService, private router: Router) { this.load(); }
  load() {
    const f: any = {};
    if (this.sf) f.status = this.sf;
    if (this.pf) f.priority = this.pf;
    if (this.q) f.search = this.q;
    this.ts.getTasks(f).subscribe(d => this.tasks = d);
  }
  del(id: number) { if (confirm('Usunąć?')) this.ts.deleteTask(id).subscribe(() => this.load()); }
  view(id: number) { this.router.navigate(['/tasks', id]); }
  edit(id: number) { this.router.navigate(['/tasks', id, 'edit']); }
  add() { this.router.navigate(['/tasks/new']); }
}