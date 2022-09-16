import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Chat } from '../models/chat'

@Injectable({
  providedIn: 'root'
})
export class InspectionApiService {

  readonly inspectionAPIUrl = "https://sarnwebapi.azurewebsites.net/api";
  // readonly inspectionAPIUrl = "http://localhost:49490/api";

  constructor(private http:HttpClient) { }


  getMessageList():Observable<any[]> {
    return this.http.get<any>(this.inspectionAPIUrl + '/messagelist');
  }
  
  getMessageListPaged(id:number|string):Observable<any[]> {
    return this.http.get<any>(this.inspectionAPIUrl +  `/PaginationMessages/paginationData?pg=${id}`)
  }

  getMessageListMessages(id:number|string):Observable<any[]> {
    return this.http.get<any>(this.inspectionAPIUrl +  `/PaginationMessages/messageData?pg=${id}`)
  }

  postMessageList(data:Chat) {
    return this.http.post(this.inspectionAPIUrl + '/messagelist', data);
  }

  deleteMessageList(data:any) {
    return this.http.put(this.inspectionAPIUrl + `/messagelist/delete`, data);
  }

}
