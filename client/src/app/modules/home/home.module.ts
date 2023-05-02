import { NgModule } from '@angular/core';
import { HomePage } from './home.page';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
    declarations: [
        HomePage
    ],
    imports: [
        SharedModule
    ]
})
export class HomePageModule { }
