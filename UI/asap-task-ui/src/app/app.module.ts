import { ButtonsModule } from '@progress/kendo-angular-buttons';
import { InputsModule } from '@progress/kendo-angular-inputs';
import { GridModule } from '@progress/kendo-angular-grid';
import { NgModule } from '@angular/core';
import { LoginComponent } from './auth/login/login.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AppComponent } from './app.component';

import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [

  ],
  imports: [
    ButtonsModule,
    InputsModule,
    GridModule,
    BrowserModule,
    HttpClientModule,
  ],
  providers: [],
})
export class AppModule { }
