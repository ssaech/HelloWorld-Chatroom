import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { InspectionApiService } from '../../services/inspection-api.service';
import { Chat } from '../../models/Chat'

@Component({
  selector: 'app-chatroom',
  templateUrl: './chatroom.component.html',
  styleUrls: ['./chatroom.component.css']
})
export class ChatroomComponent implements OnInit {
  inspectionList$!:Observable<any[]>;
  myPostsPagination$!:Observable<any[]>;
  myPostsMessages$!:Observable<any[]>;
  userID?: string; 
  displayName?: string;
  pictureLink?: string;
  displayNameInput?: string;
  lastID?: string;
  showAddTask: boolean;
  openClose: string;
  inputMessage?: string;
  pageSize: string;
  allpages: any[];

  constructor(private service:InspectionApiService) { }

  ngOnInit(): void {
   this.openClose = 'Add Message';
    this.showAddTask = false;

    this.service.getMessageListPaged(1)
    .subscribe
    (
      data => 
      {
        let indexP = JSON.stringify(data[0])
        let paco = indexP
        let pacoj = paco.indexOf(`"totalPages":`)+13
        let newString =  (paco.substring(pacoj, paco.length))
        let finalSlice = newString
        let finalSliced = finalSlice.substring(0, finalSlice.indexOf(','))
        this.pageSize =  finalSliced
        let numSlice = Number(this.pageSize)
        this.allpages = Array.from({length: numSlice}, (_, i) => i + 1)
        var spinner =   document.getElementById('spinner') 
        spinner.style.display = 'none';
      }
    );
 
    this.myPostsMessages$ = this.service.getMessageListMessages(1);
    this.pictureLink = localStorage.getItem('pictureLink')
    this.displayName = localStorage.getItem('displayName')
    this.userID = localStorage.getItem('userID')
    if (!this.userID || !this.displayName) {
      var closeModalBtn = document.getElementById('openModal');
      closeModalBtn.click();
    }
 
   }
   toggleChange(e){
    var spinner =   document.getElementById('spinner') 
    spinner.style.display = 'block';
    this.myPostsPagination$  = this.service.getMessageListPaged(e);
    this.myPostsMessages$ = this.service.getMessageListMessages(e);
    this.service.getMessageListPaged(e)
    .subscribe
    (
      data=> 
      {
      spinner.style.display = 'none';
      }
      
    )
    
    
  }
   toggleMessageBox() {
    this.showAddTask = !this.showAddTask
    this.inputMessage = ''
    if (this.showAddTask === true) {
      this.openClose = 'Close'
    } else {
      this.openClose = 'Add Message'
    }
   }


  submitMessage() {
    var messageData: Chat = {
      DisplayName:this.displayName,
      UserID:this.userID,
      PictureLink:this.pictureLink,
      Message: this.inputMessage, 
      MessageID: new Date().toLocaleString()+Math.floor(Math.random() * 101)
    }
    this.service.postMessageList(messageData).subscribe(res => {
      this.toggleMessageBox() 
      location.reload()
    })
  };

   setLastClicked(value: string) {
    this.lastID = value
  };


   delete() {
    var data = {
      UserID:this.userID,
      MessageID: this.lastID
    }
    this.service.deleteMessageList(data).subscribe(res => {
      var closeModalBtn = document.getElementById('closeDeleteModal');
      if(closeModalBtn) {
          closeModalBtn.click();
        } else {
        }
      location.reload()
    })
   };

  saveName() {
    if (this.displayNameInput){
      this.userID = (new Date().toLocaleString() + Math.floor(Math.random() * 101 )).replace(/\s/g, "")
      this.displayName = this.displayNameInput

      let pictureCode = Math.floor(Math.random() * 7)
      switch(pictureCode) {
        case 0:
          this.pictureLink = "https://www.sarnsaechao.dev/assets/azure/0.png"
          break;
        case 1:
          this.pictureLink = "https://www.sarnsaechao.dev/assets/azure/1.png"
          break;
        case 2:
          this.pictureLink = "https://www.sarnsaechao.dev/assets/azure/2.gif"
          break;
        case 3:
          this.pictureLink = "https://www.sarnsaechao.dev/assets/azure/3.gif"
          break;
        case 4:
          this.pictureLink = "https://www.sarnsaechao.dev/assets/azure/4.png"
          break;
        case 5:
          this.pictureLink = "https://www.sarnsaechao.dev/assets/azure/5.png"
          break;
        case 6:
          this.pictureLink = "https://www.sarnsaechao.dev/assets/azure/6.png"
          break;
        case 7:
          this.pictureLink = "https://www.sarnsaechao.dev/assets/azure/7.png"
          break;
        default:
          this.pictureLink="https://www.sarnsaechao.dev/assets/azure/0.png"
      }
      localStorage.setItem('userID', this.userID)
      localStorage.setItem('displayName', this.displayName)
      localStorage.setItem('pictureLink', this.pictureLink)
      this.displayNameInput = ''
      var closeModalBtn = document.getElementById('closeAddModal');
      closeModalBtn.click();
    }
  }
}
