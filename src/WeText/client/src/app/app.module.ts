import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { RouterModule } from '@angular/router';

import { ModalModule } from 'angular2-modal';
import { BootstrapModalModule } from 'angular2-modal/plugins/bootstrap';

import { DxTooltipModule, DxTemplateModule } from 'devextreme-angular';

import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { AccountService } from 'app/services/account.service';
import { AuthGuard } from 'app/guards/auth.guard';
import { HomeComponent } from './components/home/home.component';
import { Routing } from 'app/app.routing';
import { GlobalEventsManagerService } from 'app/services/global-events-manager.service';
import { DialogService } from 'app/services/dialog.service';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { PostsComponent } from './components/posts/posts.component';
import { FriendsComponent } from './components/friends/friends.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    DashboardComponent,
    PostsComponent,
    FriendsComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    Routing,
    ModalModule.forRoot(),
    BootstrapModalModule,
    DxTooltipModule,
    DxTemplateModule
  ],
  providers: [
    AuthGuard,
    AccountService,
    GlobalEventsManagerService,
    DialogService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
