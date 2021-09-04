export interface IUser {
  email: string;
  password: string;
}

export interface ICategory {
  name: string;
  imageSource?: string;
  user?: string;
  id: number;
}

export interface IPosition {
  id?: number;
  categoryId: number;
  name: string;
  cost: number;
}

export interface IPositionToAdd extends IPosition {
  quantity: number;
}

export interface IOrder {
  date?: Date;
  orderNumber?: number;
  user?: string;
  orderItems: IOrderItem[];
  id?: number;
}

export interface IOrderItem {
  name: string;
  quantity: number;
  cost: number;
  id: number;
}
