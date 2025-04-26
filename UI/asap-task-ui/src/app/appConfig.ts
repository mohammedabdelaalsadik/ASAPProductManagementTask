import { ApplicationConfig } from '@angular/core';
import { routes } from './app.routes'; // Your routing configuration
import { TokenInterceptor } from './interceptors/token.interceptor'; // Function-based token interceptor
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors, withInterceptorsFromDi } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes), // Provides routing configuration
    provideAnimations(),
    provideHttpClient(
      withInterceptors([TokenInterceptor]), // Add TokenInterceptor to the HTTP client,a
    ),
    //withInterceptorsFromDi, // Add interceptors from DI
  ],
};
