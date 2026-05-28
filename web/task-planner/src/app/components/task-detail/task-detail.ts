import { Component } from '@angular/core';
import { TaskService } from '../../services/task.service';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-task-detail',
  standalone: true,
  imports: [MatCardModule, MatButtonModule, CommonModule, RouterModule],
  templateUrl: './task-detail.html',
  styleUrl: './task-detail.css'
})
export class TaskDetailComponent {
  task: any = null;

  constructor(private taskService: TaskService, private route: ActivatedRoute) {
    const id = +this.route.snapshot.paramMap.get('id')!;
    this.taskService.getTask(id).subscribe(d => this.task = d);
  }
}