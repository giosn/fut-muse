import { NgModule } from '@angular/core';
import { PlayerPage } from './player.page';
import { SharedModule } from 'src/app/shared/modules/shared.module';
import { PlayerRoutingModule } from './player-routing.module';
import { NgxSkeletonModule } from 'ngx-skeleton';

@NgModule({
    declarations: [
        PlayerPage
    ],
    imports: [
        SharedModule,
        PlayerRoutingModule,
        NgxSkeletonModule
    ]
})
export class PlayerModule { }
