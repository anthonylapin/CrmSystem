import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { IFilter } from '../../shared/interfaces';

@Component({
  selector: 'app-history-filter',
  templateUrl: './history-filter.component.html',
  styleUrls: ['./history-filter.component.scss'],
})
export class HistoryFilterComponent implements OnInit {
  @Output() onFilter: EventEmitter<IFilter> = new EventEmitter<IFilter>();

  order: number;
  start: Date;
  end: Date;

  isValid: boolean = true;

  constructor() {}

  ngOnInit(): void {}

  validate(): void {
    if (!this.start || !this.end) {
      this.isValid = true;
      return;
    }

    this.isValid = this.start < this.end;
  }

  submitFilter() {
    const filter: IFilter = {};

    if (this.order) {
      filter.order = this.order;
    }

    if (this.start) {
      filter.start = this.start;
    }

    if (this.end) {
      filter.end = this.end;
    }

    this.onFilter.emit(filter);
  }
}
