import { useDispatch, useSelector } from "react-redux";
import { RootState } from "../store/store.ts";
import { Link } from "react-router-dom";
import CartItem from "../components/CartItem.tsx";
import { clearCart } from "../store/slices/cartSlice.ts";
import axios from "axios";
import ISale from "../interfaces/ISale.ts";
import { useMutation } from "@tanstack/react-query";
import IUser from "../interfaces/IUser.ts";

async function fetchCreateSale(sale: Omit<ISale, "id">) {
  const response = await axios.post("http://localhost:5087/api/sales", {
    sale,
  });
  return response.data;
}

export default function Cart() {
  const dispatch = useDispatch();
  const cart = useSelector((state: RootState) => state.cart);

  const { mutateAsync: buyProducts } = useMutation({
    mutationFn: fetchCreateSale,
    onSuccess: (data) => {
      console.log("Compra realizada com sucesso!", data);
      dispatch(clearCart());
    },
    onError: (error) => {
      console.error("Erro ao realizar a compra:", error);
    },
  });

  async function onBuyProducts() {
    const customerId = localStorage.getItem("id");
    const response = await axios.get(
      "http://localhost:5087/api/users/" + customerId,
    );
    const customer = response.data as IUser;

    const sale = {
      products: cart.items,
      quantity: cart.quantities,
      totalPrice: cart.totalValue,
      customerId: customerId!,
      customer,
    };

    await buyProducts(sale);
  }

  return cart.items.length == 0 ? (
    <div className="flex flex-col justify-center items-center h-screen px-5">
      <p className="text-2xl text-center">
        Tudo limpo por aqui,{" "}
        <Link to="/catalog" className="link-hover text-info">
          adicione
        </Link>{" "}
        um novo produto à cesta.
      </p>
    </div>
  ) : (
    <>
      <main className="flex flex-col items-center justify-center h-full">
        <div className="flex flex-col items-center justify-center h-full">
          <div className="flex flex-col items-center text-center my-10">
            <h1 className="text-3xl text-yellow-300 font-bold text-center pt-10 pb-2">
              Seu carrinho
            </h1>
            <div className="border-2 border-white w-1/2 mb-5"></div>
          </div>
          <div className="flex flex-col justify-between border-base-300 border-2 rounded-box">
            {cart.items.map((item, index) => (
              <CartItem key={item.id} item={item} index={index} />
            ))}
            <div className="py-2 px-5">
              <p className="text-xl font-bold">
                {cart.quantities.reduce((acc, cur) => acc + cur, 0)} itens
              </p>
              <p className="text-xl font-bold">
                Sub-total: R${cart.totalValue}
              </p>
            </div>
            <div className="flex justify-between items-center">
              <button
                onClick={() => dispatch(clearCart())}
                className="btn btn-error w-fit m-5 text-xl text-white"
              >
                Limpar Lista
              </button>
              <Link
                to="/catalog"
                className="btn btn-info w-fit m-5 text-xl text-white"
              >
                Continuar comprando
              </Link>
              <button
                onClick={() => onBuyProducts()}
                className="btn btn-success w-fit m-5 text-xl text-white"
              >
                Comprar
              </button>
            </div>
          </div>
        </div>
      </main>
      )
    </>
  );
}
