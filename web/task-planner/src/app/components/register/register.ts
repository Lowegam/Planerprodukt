import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatCardModule, CommonModule, RouterModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class RegisterComponent {
  username = ''; email = ''; password = ''; error = ''; success = '';
  constructor(private auth: AuthService, private router: Router) {}
  register() {
    this.auth.register(this.username, this.email, this.password).subscribe({
      next: () => { this.success = 'OK!'; setTimeout(() => this.router.navigate(['/login']), 1500); },
      error: () => this.error = 'Błąd'
    });
  }
}