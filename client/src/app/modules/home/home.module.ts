import { NgModule } from '@angular/core';
import { HomePage } from './home.page';
import { ProfileComponent } from '../../components/profile/profile.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorInterceptor } from 'src/app/interceptors/error.interceptor';

@NgModule({
    declarations: [
        HomePage,
        ProfileComponent
    ],
    imports: [
        SharedModule
    ],
    providers: [
        {
            provide: HTTP_INTERCEPTORS,
            useClass: ErrorInterceptor,
            multi: true
        }
    ]
})
export class HomePageModule { }
