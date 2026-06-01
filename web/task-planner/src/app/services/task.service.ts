import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class TaskService {
  private apiUrl = 'http://localhost:5056/api/Tasks';
  constructor(private http: HttpClient) {}
  getTasks(f?: any): Observable<any> { return this.http.get(this.apiUrl, { params: f }); }
  getTask(id: number): Observable<any> { return this.http.get(`${this.apiUrl}/${id}`); }
  createTask(t: any): Observable<any> { return this.http.post(this.apiUrl, t); }
  updateTask(id: number, t: any): Observable<any> { return this.http.put(`${this.apiUrl}/${id}`, t); }
  deleteTask(id: number): Observable<any> { return this.http.delete(`${this.apiUrl}/${id}`); }
  getStats(): Observable<any> { return this.http.get(`${this.apiUrl}/statistics`); }
}