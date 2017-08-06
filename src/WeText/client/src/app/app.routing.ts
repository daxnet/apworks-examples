import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { AuthGuard } from 'app/guards/auth.guard';
import { DashboardComponent } from 'app/components/dashboard/dashboard.component';
import { PostsComponent } from 'app/components/posts/posts.component';
import { FriendsComponent } from 'app/components/friends/friends.component';


const routes: Routes = [
    { path: '', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent },
    {
        path: 'home/:uname',
        component: HomeComponent,
        canActivate: [AuthGuard],
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: DashboardComponent },
            { path: 'posts', component: PostsComponent },
            { path: 'friends', component: FriendsComponent }
        ]
    },

    { path: '**', redirectTo: 'home' }
];

export const Routing = RouterModule.forRoot(routes);
