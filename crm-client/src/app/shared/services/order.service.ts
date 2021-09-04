import { Injectable } from '@angular/core';
import { IOrderItem, IPositionToAdd } from '../interfaces';

const orderCartLocalStorageKey = 'order-cart';

@Injectable()
export class OrderService {
  public orderItems: IOrderItem[] = [];

  constructor() {
    this.fetchFromLocalStorage();
  }

  add(position: IPositionToAdd) {
    const orderItem: IOrderItem = Object.assign(
      {},
      {
        name: position.name,
        cost: position.cost,
        quantity: position.quantity,
        id: position.id ?? 0,
      }
    );

    const candidate = this.orderItems.find((p) => p.id === position.id);

    if (candidate) {
      candidate.quantity += orderItem.quantity;
    } else {
      this.orderItems.push(orderItem);
    }

    this.saveInLocalStorage();
  }

  remove(orderItem: IOrderItem) {
    const index = this.orderItems.findIndex((item) => item.id === orderItem.id);
    this.orderItems.splice(index, 1);

    this.saveInLocalStorage();
  }

  clear() {
    this.orderItems = [];
    this.saveInLocalStorage();
  }

  get price(): number {
    return this.orderItems.reduce((sum, currItem) => sum + currItem.quantity * currItem.cost, 0);
  }

  private saveInLocalStorage() {
    localStorage.setItem(orderCartLocalStorageKey, JSON.stringify(this.orderItems));
  }

  private fetchFromLocalStorage() {
    const jsonData = localStorage.getItem(orderCartLocalStorageKey);
    this.orderItems = jsonData ? JSON.parse(jsonData) : [];
  }
}
