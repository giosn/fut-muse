import { NgModule } from '@angular/core';

import { SearchRoutingModule } from './search-routing.module';
import { SharedModule } from 'src/app/shared/modules/shared.module';
import { SearchPage } from './search.page';

@NgModule({
    declarations: [
        SearchPage
    ],
    imports: [
        SearchRoutingModule,
        SharedModule
    ]
})
export class SearchModule { }
