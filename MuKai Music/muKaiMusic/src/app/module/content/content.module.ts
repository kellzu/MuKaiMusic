import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ContentComponent } from '../../components/content/content.component';
import { Routes, RouterModule } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { UserComponent } from 'src/app/components/user/user.component';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { LoginComponent } from 'src/app/components/login/login.component';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AccountInterceptor } from 'src/app/services/network/accountInterceptor';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
const routes: Routes = [
  {
    path: '',
    component: ContentComponent,
    children: [
      {
        path: 'explore',
        loadChildren: () =>
          import('../explore/explore.module').then(m => m.ExploreModule)
      }
    ]
  },

];

@NgModule({
  declarations: [
    ContentComponent,
    LoginComponent,
    UserComponent
  ],
  entryComponents: [
    LoginComponent
  ],
  imports: [
    FormsModule,
    CommonModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatInputModule,
    MatIconModule,
    MatExpansionModule,
    MatSidenavModule,
    RouterModule.forChild(routes)
  ]

})
export class ContentModule { }
