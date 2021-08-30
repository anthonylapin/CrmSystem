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
