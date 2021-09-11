import { Component, OnInit } from '@angular/core';
import { AnalyticsService } from '../shared/services/analytics.service';
import { Observable } from 'rxjs';
import { IAnalyticsOverview } from '../shared/interfaces';

@Component({
  selector: 'app-overview-page',
  templateUrl: './overview-page.component.html',
  styleUrls: ['./overview-page.component.scss'],
})
export class OverviewPageComponent implements OnInit {
  yesterday: Date;
  overview$: Observable<IAnalyticsOverview>;

  constructor(private analyticsService: AnalyticsService) {}

  ngOnInit(): void {
    this.yesterday = new Date();
    this.yesterday.setDate(this.yesterday.getDate() - 1);

    this.overview$ = this.analyticsService.getOverview();
  }
}
