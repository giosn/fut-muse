import { NgModule } from '@angular/core';
import { PlayerPage } from './player.page';
import { SharedModule } from 'src/app/shared/shared.module';
import { ProfileComponent } from 'src/app/components/profile/profile.component';
import { PlayerRoutingModule } from './player-routing.module';

@NgModule({
    declarations: [
        PlayerPage,
        ProfileComponent
    ],
    imports: [
        SharedModule,
        PlayerRoutingModule
    ]
})
export class PlayerModule { }
