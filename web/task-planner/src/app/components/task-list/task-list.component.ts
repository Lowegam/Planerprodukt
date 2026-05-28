import { Component, ChangeDetectorRef } from '@angular/core';
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
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.css'
})
export class TaskListComponent {
  tasks: any[] = [];
  columns = ['title', 'status', 'priority', 'deadline', 'actions'];
  statusFilter = ''; priorityFilter = ''; searchFilter = '';

  constructor(private taskService: TaskService, private router: Router, private cdr: ChangeDetectorRef) { this.load(); }

  load() {
    const f: any = {};
    if (this.statusFilter) f.status = this.statusFilter;
    if (this.priorityFilter) f.priority = this.priorityFilter;
    if (this.searchFilter) f.search = this.searchFilter;
    this.taskService.getTasks(f).subscribe(d => {
      this.tasks = d;
      this.cdr.detectChanges();
    });
  }

  deleteTask(id: number) { 
    if (confirm('Usunąć?')) this.taskService.deleteTask(id).subscribe(() => this.load()); 
  }
  viewTask(id: number) { this.router.navigate(['/tasks', id]); }
  editTask(id: number) { this.router.navigate(['/tasks', id, 'edit']); }
  addTask() { this.router.navigate(['/tasks/new']); }
}