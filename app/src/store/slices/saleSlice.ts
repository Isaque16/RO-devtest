import { createSlice } from "@reduxjs/toolkit";
import ISale from "../../interfaces/ISale.ts";

const initialState: ISale = {
  id: "",
  createdOn: new Date(),
  modifiedOn: new Date(),
  products: [],
  quantity: [],
  totalPrice: 0,
  customerId: "",
  customer: {
    id: "",
    name: "",
    userName: "",
    email: "",
    phoneNumber: "",
    role: 0,
  },
};

const saleSlice = createSlice({
  name: "sale",
  initialState,
  reducers: {
    setSale: (state, action) => {
      const { sale }: { sale: ISale } = action.payload;
      state.customer = sale.customer;
      state.products = sale.products;
      state.totalPrice = sale.totalPrice;
    },
  },
});

export default saleSlice.reducer;
export const { setSale } = saleSlice.actions;
