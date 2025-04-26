// src/app/auth/login/login.component.ts
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service'; // Your AuthService
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

export class LoginDto {
  username = '';
  password = '';
}

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    RouterModule
    ],
  providers : [
    AuthService

  ]
})
export class LoginComponent {
  loginDto: LoginDto = new LoginDto();
  errorMessage = '';

  constructor(private authService: AuthService, private router: Router) { }

  onLogin() {
    this.authService.login(this.loginDto).subscribe({
      next: (response) => {
        debugger;
        this.authService.setToken(response.token); // Assuming you set the token
        this.router.navigate(['/products']);
        this.authService.isAuthenticatedSubject.next(true);

      },
      error: (err) => {
        this.errorMessage = 'Login failed. Please check your credentials.';
        console.error(err);
      }
    });
  }
}
