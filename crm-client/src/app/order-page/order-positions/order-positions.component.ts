import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PositionsService } from '../../shared/services/positions.service';
import { Observable } from 'rxjs';
import { IPosition, IPositionToAdd } from '../../shared/interfaces';
import { map, switchMap } from 'rxjs/operators';
import { OrderService } from '../../shared/services/order.service';
import { ToastService } from '../../shared/services/toast.service';

@Component({
  selector: 'app-order-positions',
  templateUrl: './order-positions.component.html',
  styleUrls: ['./order-positions.component.scss'],
})
export class OrderPositionsComponent implements OnInit {
  positions$: Observable<IPositionToAdd[]>;

  constructor(
    private activatedRoute: ActivatedRoute,
    private positionsService: PositionsService,
    private orderService: OrderService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.positions$ = this.activatedRoute.params.pipe(
      switchMap((params) => this.positionsService.getAll(params['id'])),
      map((positions) => positions.map((p) => ({ ...p, quantity: 1 })))
    );
  }

  onAddToOrder(position: IPositionToAdd) {
    this.orderService.add(position);
    this.toastService.show(`Added ${position.name} x${position.quantity}`);
  }
}
