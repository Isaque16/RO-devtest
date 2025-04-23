export default interface IProduct {
  id: string;
  createdOn: Date;
  modifiedOn: Date;
  name: string;
  description: string;
  price: number;
  quantity: number;
  imageUrl: string;
}
