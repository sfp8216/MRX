import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-forecast',
  templateUrl: './forecast.component.html',
  styleUrls: ['./forecast.component.css']
})
export class ForecastComponent implements OnInit {

  constructor(private http:HttpClient) { }

  ngOnInit(): void {
  }
  getForecasts(){
    return this.http.get("http://localhost:5000/WeatherForecast/api/").subscribe(response = > {
      console.log(response);
    })
  }

}
