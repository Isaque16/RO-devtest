import IUser from "./IUser.ts";
import IProduct from "./IProduct.ts";

export default interface ISale {
  id: string;
  createdOn: Date;
  modifiedOn: Date;
  products: IProduct[];
  quantity: number[];
  totalPrice: number;
  customerId: string;
  customer: Omit<IUser, "password">;
}
