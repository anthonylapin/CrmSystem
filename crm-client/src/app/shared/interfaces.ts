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

export interface IOrderViewModel extends IOrder {
  cost: number;
}

export interface IFilter {
  start?: Date;
  end?: Date;
  order?: number;
}

export interface IAnalyticsOverviewItem {
  percent: number;
  compare: number;
  yesterday: number;
  isHigher: boolean;
}

export interface IAnalyticsOverview {
  gain: IAnalyticsOverviewItem;
  orders: IAnalyticsOverviewItem;
}

export interface IAnalyticsChartItem {
  gain: number;
  order: number;
  label: string;
}

export interface IAnalytics {
  average: number;
  chart: IAnalyticsChartItem[];
}
