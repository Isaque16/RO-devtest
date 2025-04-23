import { Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Catalog from "./pages/Catalog";
import Product from "./pages/Product";
import Cart from "./pages/Cart";
import Estoque from "./pages/Estoque";
import Clientes from "./pages/Clientes";
import Sales from "./pages/Sales";

export default function AppRoutes() {
  return (
    <Routes>
      <Route index element={<Home />} />
      <Route path="login" element={<Login />} />
      <Route path="singup" element={<Register />} />
      <Route path="catalog" element={<Catalog />}>
        <Route path="product/:id" element={<Product />} />
      </Route>
      <Route path="cart" element={<Cart />} />
      <Route path="admin">
        <Route path="estoque" element={<Estoque />} />
        <Route path="clientes" element={<Clientes />} />
        <Route path="sales" element={<Sales />} />
      </Route>
    </Routes>
  );
}
