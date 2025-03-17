import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TravelRouteService {
  private apiUrl = 'https://localhost:44300/api/TravelRoute';

  constructor(private http: HttpClient) {}

  getRoutes(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}`);
  }

  insertRoute(route: { origin: string, destination: string, price: number }): Observable<any> {
    return this.http.post(`${this.apiUrl}`, route, { responseType: 'text' });
  }

  updateRoute(route: { origin: string, destination: string, price: number }): Observable<any> {
    return this.http.put(`${this.apiUrl}`, route, { responseType: 'text' });
  }

  deleteRoute(origin: string, destination: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${origin}/${destination}`, { responseType: 'text' });
  }

  getBestRoute(origin: string, destination: string): Observable<string> {
    return this.http.get(`${this.apiUrl}/BestRoute/${origin}/${destination}`, { responseType: 'text' });
  }
}
