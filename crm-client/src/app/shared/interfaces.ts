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
