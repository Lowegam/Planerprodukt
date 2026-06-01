import { Component } from '@angular/core';
import { TaskService } from '../../services/task.service';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [FormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatSelectModule, MatDatepickerModule, MatNativeDateModule, MatCardModule, CommonModule, RouterModule],
  templateUrl: './task-form.html',
  styleUrl: './task-form.css'
})
export class TaskFormComponent {
  t: any = { title: '', description: '', status: 'New', priority: 'Medium', deadline: null };
  edit = false; id: number | null = null;
  constructor(private ts: TaskService, private router: Router, private route: ActivatedRoute) {
    const i = this.route.snapshot.paramMap.get('id');
    if (i) { this.edit = true; this.id = +i; this.ts.getTask(this.id).subscribe(d => this.t = d); }
  }
  save() {
    const a = this.edit ? this.ts.updateTask(this.id!, this.t) : this.ts.createTask(this.t);
    a.subscribe(() => this.router.navigate(['/tasks']));
  }
}