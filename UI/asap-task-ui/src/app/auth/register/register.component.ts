import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  imports: [CommonModule, FormsModule, RouterModule
  ]
})
export class RegisterComponent {
  username: string = '';
  password: string = '';

  constructor(private router: Router) {}

  onRegister() {
    if (this.username && this.password) {
      // Here you would typically call an API to register the user
      console.log('Registering user with:', this.username, this.password);

      // On successful registration, navigate to the login page or dashboard
      this.router.navigate(['/']);
    } else {
      console.log('Username and password are required');
    }
  }

  routeToLogin()
  {
    this.router.navigate(['/login']);
  }
}
