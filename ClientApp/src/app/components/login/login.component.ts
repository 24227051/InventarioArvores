import { Component } from '@angular/core';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  model = { email: '', password: '' };
  error = '';

  constructor(
    private auth: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  submit() {
    this.error = '';
    const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') || '/especies';

    this.auth.login(this.model).subscribe({
      next: () => this.router.navigateByUrl(returnUrl),
      error: (err) => this.error = err?.error?.message || 'Erro ao efetuar login'
    });
  }
}
