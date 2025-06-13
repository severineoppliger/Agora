import { Component } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-header',
  imports: [
    MatIcon,
    MatButton
  ],
  templateUrl: './header.html',
  styleUrls: ['./header.scss']
})
export class Header {

}
