"use client";
import { createSlice } from "@reduxjs/toolkit";
import IUser from "../../interfaces/IUser.ts";

const initialState: Pick<IUser, "id" | "userName"> = {
  id: "",
  userName: "",
};

const userSlice = createSlice({
  name: "user",
  initialState,
  reducers: {
    setUserData: (state, { payload }) => {
      const { id, userName }: typeof initialState = payload;
      state.id = id;
      state.userName = userName;
    },
  },
});

export default userSlice.reducer;
export const { setUserData } = userSlice.actions;
