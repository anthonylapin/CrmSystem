import { Component, OnDestroy, OnInit } from '@angular/core';
import { OrdersService } from '../shared/services/orders.service';
import { Subscription } from 'rxjs';
import { IFilter, IOrder, IOrderViewModel } from '../shared/interfaces';
import { map } from 'rxjs/operators';
import { OrderUtil } from '../shared/utilities/order.util';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

const step = 5;

@Component({
  selector: 'app-history-page',
  templateUrl: './history-page.component.html',
  styleUrls: ['./history-page.component.scss'],
})
export class HistoryPageComponent implements OnInit, OnDestroy {
  aSub: Subscription;
  bSub: Subscription;
  showFilter: boolean = false;
  offset: number = 0;
  limit: number = 5;
  count: number = 0;
  orders: IOrderViewModel[] = [];
  loading: boolean = false;
  filter: IFilter = {};

  constructor(private ordersService: OrdersService) {}

  ngOnDestroy(): void {
    if (this.aSub) {
      this.aSub.unsubscribe();
    }
    if (this.bSub) {
    }
    this.bSub.unsubscribe();
  }

  ngOnInit(): void {
    this.fetch();
  }

  private fetch() {
    this.loading = true;

    const params = Object.assign({}, this.filter, {
      offset: this.offset,
      limit: this.limit,
    });

    this.aSub = this.ordersService
      .fetch(params)
      .pipe(map((orders) => orders.map((o) => ({ ...o, cost: OrderUtil.computeOverallCost(o.orderItems) }))))
      .subscribe((orders) => {
        this.orders = orders;
        this.loading = false;
      });

    this.bSub = this.ordersService.getOrdersCount(this.filter).subscribe((count) => (this.count = count));
  }

  onToggleFilter() {
    this.showFilter = !this.showFilter;
  }

  loadNextPage() {
    this.offset += step;
    this.fetch();
  }

  loadPreviousPage() {
    this.offset -= step;
    this.fetch();
  }

  get hasNextPage(): boolean {
    return this.offset + step < this.count;
  }

  applyFilter(filter: IFilter) {
    this.orders = [];
    this.offset = 0;
    this.filter = filter;
    this.loading = true;

    this.fetch();
  }
}
