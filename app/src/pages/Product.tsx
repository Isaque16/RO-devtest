import { useParams } from "react-router-dom";
import axios from "axios";
import { useQuery } from "@tanstack/react-query";
import { useNavigate } from "react-router";
import { addToCart } from "../store/slices/cartSlice.ts";
import { useDispatch } from "react-redux";
import { useState } from "react";
import IProduct from "../interfaces/IProduct.ts";

async function fetchProductById(id: string) {
  const response = await axios.get(`http://localhost:5087/api/products/${id}`);

  if (response.status !== 200) {
    throw new Error("Error fetching product");
  }

  return response.data;
}

export default function Produto() {
  const { id: productId } = useParams<{ id: string }>();

  const { data: product } = useQuery<IProduct>({
    queryKey: ["product", productId],
    queryFn: () => {
      if (!productId) throw new Error("ID do produto não encontrado");
      return fetchProductById(productId);
    },
    enabled: !!productId,
  });

  const router = useNavigate();
  const dispatch = useDispatch();

  const isOutOfStock = product?.quantity === 0;
  const [quantity, setQuantity] = useState(isOutOfStock ? 0 : 1);

  function sendAddToCart() {
    dispatch(addToCart({ product, quantity }));
    router("/cart");
  }

  return (
    <main className="flex flex-col md:flex-row justify-around items-center gap-10 h-full md:h-screen">
      <figure className="bg-base-100 w-1/2 h-96 p-4 rounded-box m-5 image-full">
        <img className="w-full h-full" src={product?.imageUrl} alt="image" />
      </figure>
      <div className="flex flex-col gap-4 w-full md:w-1/2 px-10">
        <h1 className="text-4xl font-bold">{product?.name}</h1>
        <p>{product?.description}</p>
        <p className="text-xl font-bold">
          R$<span className="text-4xl">{product?.price}</span>
        </p>
        <p className={product?.quantity !== 0 ? "text-success" : "text-error"}>
          {isOutOfStock ? "Esgotado" : "Em estoque"}
        </p>
        <div className="bg-slate-300 text-xl rounded-box text-black w-fit h-fit px-2 py-2 flex flex-row gap-2">
          <button
            className="px-2 text-xl"
            onClick={() => setQuantity((q) => Math.max(1, (q -= 1)))}
          >
            -
          </button>
          {quantity}
          <button
            className="px-2 text-xl"
            onClick={() =>
              setQuantity((q) => Math.min(product!.quantity, (q += 1)))
            }
          >
            +
          </button>
        </div>
        <div className="flex flex-col gap-5 mt-5 mb-10">
          <button
            onClick={sendAddToCart}
            className={`bg-base-100 text-xl text-white btn btn-primary rounded-btn ${
              isOutOfStock && "btn-disabled"
            }`}
          >
            Adicionar à cesta
          </button>
        </div>
      </div>
    </main>
  );
}
