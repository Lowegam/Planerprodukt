import { Component, AfterViewInit } from '@angular/core';
import { TaskService } from '../../services/task.service';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { Chart, registerables } from 'chart.js';

Chart.register(...registerables);

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [MatCardModule, CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent implements AfterViewInit {
  stats: any = null;

  constructor(private ts: TaskService) {}

  ngAfterViewInit() {
    this.ts.getStatistics().subscribe(d => {
      this.stats = d;
      setTimeout(() => this.drawCharts(), 300);
    });
  }

  drawCharts() {
    const statusEl = document.getElementById('statusChart');
    const priorityEl = document.getElementById('priorityChart');
    if (!this.stats || !statusEl || !priorityEl) return;
    
    new Chart(statusEl as HTMLCanvasElement, {
      type: 'pie',
      data: { 
        labels: ['Nowe', 'W trakcie', 'Wykonane'], 
        datasets: [{ data: [this.stats.byStatus.new, this.stats.byStatus.inProgress, this.stats.byStatus.done] }] 
      }
    });
    
    new Chart(priorityEl as HTMLCanvasElement, {
      type: 'bar',
      data: { 
        labels: ['Niski', 'Średni', 'Wysoki'], 
        datasets: [{ label: 'Priorytety', data: [this.stats.byPriority.low, this.stats.byPriority.medium, this.stats.byPriority.high] }] 
      }
    });
  }
}