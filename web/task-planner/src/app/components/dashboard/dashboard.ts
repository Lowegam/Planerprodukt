import { Component } from '@angular/core';
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
export class DashboardComponent {
  stats: any = null;
  constructor(private ts: TaskService) {
    this.ts.getStats().subscribe(d => { this.stats = d; setTimeout(() => this.draw(), 300); });
  }
  draw() {
    const s = (document.getElementById('sc') as HTMLCanvasElement)?.getContext('2d');
    const p = (document.getElementById('pc') as HTMLCanvasElement)?.getContext('2d');
    if(!s||!p||!this.stats) return;
    new Chart(s,{type:'doughnut',data:{labels:['Nowe','W trakcie','Wykonane'],datasets:[{data:[this.stats.byStatus.new,this.stats.byStatus.inProgress,this.stats.byStatus.done],backgroundColor:['#FF6384','#FFCE56','#36A2EB']}]}});
    new Chart(p,{type:'bar',data:{labels:['Niski','Średni','Wysoki'],datasets:[{label:'Zadania',data:[this.stats.byPriority.low,this.stats.byPriority.medium,this.stats.byPriority.high],backgroundColor:'#3F51B5'}]}});
  }
}