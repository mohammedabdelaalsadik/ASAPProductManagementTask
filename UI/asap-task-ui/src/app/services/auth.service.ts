// src/app/services/auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'; // Assuming you're using HttpClient to communicate with backend
import { BehaviorSubject, Observable } from 'rxjs';
import { LoginDto } from '../auth/login/login.component'; // Import LoginDto
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7033/api/Auth'; // Replace with your API URL
  public isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  public isAuthenticated$: Observable<boolean> = this.isAuthenticatedSubject.asObservable();

  constructor(private http: HttpClient,private router:Router) { }

  login(loginDto: LoginDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, loginDto);
  }

  setToken(token: string): void {
    this.isAuthenticatedSubject.next(true);

    localStorage.setItem('authToken', token); // Save token in localStorage
  }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }
  register(username: string, password: string): Observable<any> {
    const payload = { username, password };
    return this.http.post(`${this.apiUrl}/register`, payload);
  }
  logout()
  {
    this.router.navigate(['/login']);
    localStorage.clear();
    this.isAuthenticatedSubject.next(false);

  }


}
