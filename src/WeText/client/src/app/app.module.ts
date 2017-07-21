import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { AccountService } from 'app/services/account.service';
import { AuthGuard } from 'app/guards/auth.guard';
import { HomeComponent } from './components/home/home.component';
import { Routing } from 'app/app.routing';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    Routing
  ],
  providers: [
    AuthGuard,
    AccountService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
