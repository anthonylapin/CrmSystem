import { IOrderItem } from '../interfaces';

export class OrderUtil {
  public static computeOverallCost(orderItems: IOrderItem[]): number {
    return orderItems.reduce((acc, curr) => acc + curr.cost * curr.quantity, 0);
  }
}
