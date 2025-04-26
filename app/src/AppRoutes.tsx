import { Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Catalog from "./pages/Catalog";
import Product from "./pages/Product";
import Cart from "./pages/Cart";
import Stock from "./pages/Stock.tsx";
import Sales from "./pages/Sales";

export default function AppRoutes() {
  return (
    <Routes>
      <Route index element={<Home />} />
      <Route path="login" element={<Login />} />
      <Route path="singup" element={<Register />} />
      <Route path="catalog">
        <Route index element={<Catalog />} />
        <Route path="product/:id" element={<Product />} />
      </Route>
      <Route path="cart" element={<Cart />} />
      <Route path="admin">
        <Route path="stock" element={<Stock />} />
        <Route path="sales" element={<Sales />} />
      </Route>
    </Routes>
  );
}
