import { Component, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { TaskService } from '../../services/task.service';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { Chart, registerables } from 'chart.js';

Chart.register(...registerables);

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [MatCardModule, CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements AfterViewInit {
  stats: any = null;

  constructor(private ts: TaskService, private cdr: ChangeDetectorRef) {}

  ngAfterViewInit() {
    this.ts.getStatistics().subscribe(d => {
      this.stats = d;
      this.cdr.detectChanges();
      setTimeout(() => this.drawCharts(), 100);
    });
  }

  drawCharts() {
    const statusEl = document.getElementById('statusChart');
    const priorityEl = document.getElementById('priorityChart');
    if (!this.stats || !statusEl || !priorityEl) return;
    
    // Usuń stare wykresy jeśli istnieją
    Chart.getChart('statusChart')?.destroy();
    Chart.getChart('priorityChart')?.destroy();
    
    new Chart('statusChart', {
      type: 'pie',
      data: { 
        labels: ['Nowe', 'W trakcie', 'Wykonane'], 
        datasets: [{ 
          data: [this.stats.byStatus.new, this.stats.byStatus.inProgress, this.stats.byStatus.done],
          backgroundColor: ['#FF6384', '#FFCE56', '#36A2EB']
        }] 
      }
    });
    
    new Chart('priorityChart', {
      type: 'bar',
      data: { 
        labels: ['Niski', 'Średni', 'Wysoki'], 
        datasets: [{ 
          label: 'Priorytety', 
          data: [this.stats.byPriority.low, this.stats.byPriority.medium, this.stats.byPriority.high],
          backgroundColor: '#3F51B5'
        }] 
      }
    });
  }
}