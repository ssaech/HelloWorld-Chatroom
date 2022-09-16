import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule , ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { InspectionApiService } from './services/inspection-api.service';

import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { TodolistComponent } from './components/todolist/todolist.component';
import { ChatroomComponent } from './components/chatroom/chatroom.component';


const appRoutes: Routes = [
  { path: '', component: TodolistComponent },
  { path: 'chatroom', component: ChatroomComponent }

];
@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    TodolistComponent,
    ChatroomComponent,
  ],
  imports: [
    BrowserModule,
    FontAwesomeModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(appRoutes, { enableTracing: true }),
  ],
  providers: [InspectionApiService],
  bootstrap: [AppComponent]
})
export class AppModule { }
