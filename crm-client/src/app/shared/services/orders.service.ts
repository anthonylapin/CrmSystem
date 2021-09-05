import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IFilter, IOrder } from '../interfaces';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class OrdersService {
  constructor(private http: HttpClient) {}

  create(order: IOrder): Observable<IOrder> {
    return this.http.post<IOrder>('/api/orders', order);
  }

  fetch(params: any = {}): Observable<IOrder[]> {
    return this.http.get<IOrder[]>('/api/orders', {
      params: new HttpParams({
        fromObject: params,
      }),
    });
  }

  getOrdersCount(params: any = {}): Observable<number> {
    return this.http.get<number>('/api/orders/count', {
      params: new HttpParams({
        fromObject: params,
      }),
    });
  }
}
