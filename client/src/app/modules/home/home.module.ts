import { NgModule } from '@angular/core';
import { HomePage } from './home.page';
import { ProfileComponent } from '../../components/profile/profile.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
    declarations: [
        HomePage,
        ProfileComponent
    ],
    imports: [
        SharedModule
    ]
})
export class HomePageModule { }
