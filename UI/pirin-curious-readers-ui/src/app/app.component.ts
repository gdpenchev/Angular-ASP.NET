import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  public title = 'pirin-curious-readers-ui';
  public collapse = false;
  public innerWidth = window.innerWidth;

  ngOnInit(): void {
    this.collapse = this.innerWidth <= 980 ? true : false;
  }

  toggleCollapse(): void {
    this.collapse = !this.collapse;
  }

  onResize(event) {
    this.collapse = event.target.innerWidth <= 980 ? true : false;
  }
}
