import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { OrderService } from '../shared/services/order.service';
import { IOrder, IOrderItem } from '../shared/interfaces';
import { OrdersService } from '../shared/services/orders.service';
import { ToastService } from '../shared/services/toast.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-order-page',
  templateUrl: './order-page.component.html',
  styleUrls: ['./order-page.component.scss'],
  providers: [OrderService],
})
export class OrderPageComponent implements OnInit, OnDestroy {
  isRoot: boolean;
  loading: boolean = false;
  aSub: Subscription;

  constructor(
    private router: Router,
    private modalService: NgbModal,
    public orderService: OrderService,
    private ordersService: OrdersService,
    private toastService: ToastService
  ) {}

  ngOnDestroy(): void {
    this.modalService.dismissAll();

    if (this.aSub) {
      this.aSub.unsubscribe();
    }
  }

  private checkIsRoot(): void {
    this.isRoot = this.router.url === '/orders';
  }

  ngOnInit(): void {
    this.checkIsRoot();
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.checkIsRoot();
      }
    });
  }

  onOpenModal(content: any) {
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then(
      () => {},
      () => {}
    );
  }

  onCompleteOrder() {
    this.loading = true;

    const orderToCreate: IOrder = {
      orderItems: this.orderService.orderItems,
    };

    this.aSub = this.ordersService.create(orderToCreate).subscribe(
      (order: IOrder) => {
        this.orderService.clear();
        this.toastService.show(`Order #${order.orderNumber} was added.`);
      },
      (error) => {
        this.toastService.showDanger(error.error.message);
      },
      () => {
        this.modalService.dismissAll();
        this.loading = false;
      }
    );
  }

  onRemoveOrderItem(orderItem: IOrderItem) {
    this.orderService.remove(orderItem);
  }
}
