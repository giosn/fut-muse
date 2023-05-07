import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePage } from './pages/home/home.page';

const routes: Routes = [
    {
        path: '',
        component: HomePage
    },
    {
        path: 'player/:id',
        loadChildren: () => import('./pages/player/player.module').then(m => m.PlayerModule)
    },
    {
        path: 'search/:query',
        loadChildren: () => import('./pages/search/search.module').then(m => m.SearchModule)
    },
    {
        path: '**',
        redirectTo: '/'
    }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
