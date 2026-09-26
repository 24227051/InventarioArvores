import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  model = { email: '', password: '', confirmPassword: '' };
  error = '';
  success = '';

  constructor(private auth: AuthService, private router: Router) {}

  submit() {
    this.error = '';
    this.success = '';

    if (this.model.password !== this.model.confirmPassword) {
      this.error = 'As senhas não coincidem.';
      return;
    }

    this.auth.register({ email: this.model.email, password: this.model.password }).subscribe({
      next: () => {
        this.success = 'Conta criada com sucesso. Faça login.';
        setTimeout(() => this.router.navigate(['/login']), 1200);
      },
      error: (err) => {
        const apiErrors = err?.error?.errors;

        if (apiErrors && typeof apiErrors === 'object') {
          this.error = Object.values(apiErrors).flat().join(' ');
          return;
        }

        this.error = err?.error?.title || err?.error?.message || 'Erro ao registrar';
        console.error('Erro ao registrar:', err);
      }
    });
  }
}
