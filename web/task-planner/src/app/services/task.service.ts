import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class TaskService {
  private apiUrl = 'http://localhost:5056/api/Tasks';

  constructor(private http: HttpClient) {}

  getTasks(filters?: any): Observable<any> {
    return this.http.get(this.apiUrl, { params: filters });
  }
  getTask(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`);
  }
  createTask(task: any): Observable<any> {
    return this.http.post(this.apiUrl, task);
  }
  updateTask(id: number, task: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, task);
  }
  deleteTask(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
  getStatistics(): Observable<any> {
    return this.http.get(`${this.apiUrl}/statistics`);
  }
}