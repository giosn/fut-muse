import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material.module';
import { ToolbarComponent } from './components/toolbar/toolbar.component';
import { HttpClientModule } from '@angular/common/http';
import { HomePage } from './modules/home/home.page';
import { FormsModule } from '@angular/forms';
import { SnackbarComponent } from './services/snackbar.service';

@NgModule({
  declarations: [
    AppComponent,
    ToolbarComponent,
    HomePage,
    SnackbarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
