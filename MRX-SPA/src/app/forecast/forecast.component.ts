import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-forecast',
  templateUrl: './forecast.component.html',
  styleUrls: ['./forecast.component.scss']
})
export class ForecastComponent implements OnInit {


  values:any;
  constructor(private http:HttpClient) { }

  ngOnInit(): void {
    this.getForecasts();
  }
  getForecasts(){

    return this.http.get("http://localhost:5000/WeatherForecast/api/").subscribe(response => {
        console.log(response);
        this.values = response;
    },
    error => {
      console.log(error);
    });
  }

}
