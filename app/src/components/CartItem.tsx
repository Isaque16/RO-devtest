import IProduct from "../interfaces/IProduct.ts";
import { useDispatch, useSelector } from "react-redux";
import { RootState } from "../store/store.ts";
import { removeFromCart, updateQuantity } from "../store/slices/cartSlice.ts";

export default function CartItem({
  item,
  index,
}: {
  item: IProduct;
  index: number;
}) {
  const dispatch = useDispatch();
  const basket = useSelector((state: RootState) => state.cart);

  return (
    <div
      key={item.id}
      className="bg-neutral-content text-xl px-10 py-5 gap-10 flex flex-col md:flex-row justify-between items-center w-full ring-base-300 ring-1"
    >
      <div
        key={item.id}
        className="flex flex-col md:flex-row items-center gap-5"
      >
        <figure className="image-full">
          <img src={item.imageUrl} alt="image" />
        </figure>
        <div>
          <p className="text-xl">{item.name}</p>
          <p className="text-xl font-bold">R${item.price}</p>
        </div>
      </div>
      <div className="flex flex-row md:flex-col gap-5">
        <div className="bg-slate-300 text-xl rounded-box text-black w-fit h-fit px-3 py-2 card-actions">
          <button
            className="px-2 text-xl"
            onClick={() =>
              dispatch(
                updateQuantity({
                  index,
                  quantity: Math.max(1, basket.quantities[index] - 1),
                }),
              )
            }
          >
            -
          </button>
          {basket.quantities[index]}
          <button
            className="px-2 text-xl"
            onClick={() =>
              dispatch(
                updateQuantity({
                  index,
                  quantity: Math.min(
                    item.quantity,
                    basket.quantities[index] + 1,
                  ),
                }),
              )
            }
          >
            +
          </button>
        </div>
        <button
          className="btn btn-error text-white"
          onClick={() => dispatch(removeFromCart(item.id))}
        >
          Remover
        </button>
      </div>
    </div>
  );
}
