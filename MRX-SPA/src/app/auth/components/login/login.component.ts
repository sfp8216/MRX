import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private authService:AuthService) { }

  ngOnInit(): void {
  }

  onSubmit(f:NgForm){
    this.authService.login(f.value).subscribe(  x => console.log('Logged in'),
    err => console.error(err),
    () => console.log('Observer got a complete notification'))
  }
}
