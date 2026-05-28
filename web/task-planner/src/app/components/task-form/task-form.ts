import { Component } from '@angular/core';
import { TaskService } from '../../services/task.service';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [FormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, 
            MatSelectModule, MatDatepickerModule, MatNativeDateModule, MatCardModule, CommonModule, RouterModule],
  templateUrl: './task-form.html',
  styleUrl: './task-form.css'
})
export class TaskFormComponent {
  task: any = { title: '', description: '', status: 'New', priority: 'Medium', deadline: null };
  isEdit = false;
  taskId: number | null = null;

  constructor(private ts: TaskService, private router: Router, private route: ActivatedRoute) {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.taskId = +id;
      this.ts.getTask(this.taskId).subscribe({
        next: (d: any) => {
          this.task = d;
        },
        error: () => {
          this.router.navigate(['/tasks']);
        }
      });
    }
  }

  save() {
    const action = this.isEdit && this.taskId
      ? this.ts.updateTask(this.taskId, this.task) 
      : this.ts.createTask(this.task);
    action.subscribe(() => this.router.navigate(['/tasks']));
  }
}