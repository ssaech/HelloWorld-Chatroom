import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-todolist',
  templateUrl: './todolist.component.html',
  styleUrls: ['./todolist.component.css']
})
export class TodolistComponent implements OnInit {
 
  title?: string;
  note?: string;
  old_data?: any;
  imageId:number;
  myPosts: any[];
  url: string;
  lastID: string;

  constructor() { 
    this.myPosts = [];
  }


  ngOnInit(): void {
    this.myPosts = JSON.parse(localStorage.getItem('note'));

    if (!this.myPosts) {
      this.myPosts = [];
    }
  }

  
  saveNote() {
    var note1 = {
      "noteid": new Date().toLocaleString()+Math.floor(Math.random() * 101),
      "title": this.title,
      "note": this.note,
      "imageData": this.url,
      "datePosted": new Date().toLocaleDateString(undefined, {  year: 'numeric', month: 'long', day: 'numeric' })
    }
    this.myPosts.push(note1);
    localStorage.setItem('note', JSON.stringify(this.myPosts));

    var closeModalBtn = document.getElementById('closeAddModal');
    closeModalBtn.click();
  }

  onselectFile(e) {
    if (e !=='a' && e.target.files) {
      var reader = new FileReader();
      reader.readAsDataURL(e.target.files[0]);
      reader.onload=(event:any) => {
        this.url = event.target.result;
      }
      
    } else {
      e=''
    }
  }
  resetState() {
    this.title=''
    this.note=''
    this.url=''
    this.lastID = ''
  }

  setLastClicked(value: string) {
    this.lastID = value
  }

  delete() {
   var _id = this.lastID;
   this.myPosts = this.myPosts.filter(item => item.noteid != _id)
   localStorage.setItem('note', JSON.stringify(this.myPosts));
   var closeModalBtn = document.getElementById('closeDeleteModal');
   closeModalBtn.click();
  };



}


