import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToolbarComponent } from './components/toolbar/toolbar.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { SnackbarComponent } from './services/snackbar.service';
import { SharedModule } from './shared/shared.module';
import { HomePageModule } from './modules/home/home.module';
import { ErrorInterceptor } from './interceptors/error.interceptor';

@NgModule({
    declarations: [
        AppComponent,
        ToolbarComponent,
        SnackbarComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        HttpClientModule,
        SharedModule,
        HomePageModule
    ],
    providers: [
        {
            provide: HTTP_INTERCEPTORS,
            useClass: ErrorInterceptor,
            multi: true
        }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
