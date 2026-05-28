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
  templateUrl: './task-detail.component.html',
  styleUrl: './task-detail.component.css'
})
export class TaskDetailComponent {
  task: any = null;
  constructor(private ts: TaskService, private route: ActivatedRoute) {
    this.ts.getTask(+this.route.snapshot.paramMap.get('id')!).subscribe(d => this.task = d);
  }
}