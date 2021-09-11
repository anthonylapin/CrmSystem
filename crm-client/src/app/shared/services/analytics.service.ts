import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IAnalytics, IAnalyticsOverview } from '../interfaces';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AnalyticsService {
  constructor(private http: HttpClient) {}

  getOverview(): Observable<IAnalyticsOverview> {
    return this.http.get<IAnalyticsOverview>('/api/analytics/overview');
  }

  getAnalytics(): Observable<IAnalytics> {
    return this.http.get<IAnalytics>('/api/analytics');
  }
}
