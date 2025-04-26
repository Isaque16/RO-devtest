import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import ProductCard from "../components/ProductCard.tsx";
import IProduct from "../interfaces/IProduct.ts";
import IPagedResult from "../interfaces/IPagedResult.ts";

async function fetchAllProducts(): Promise<IPagedResult<IProduct>> {
  const response = await axios.get("http://localhost:5087/api/products");
  return response.data as IPagedResult<IProduct>;
}

export default function Catalog() {
  const { data: foundProducts } = useQuery({
    queryKey: ["products"],
    queryFn: fetchAllProducts,
  });

  return (
    <div className="flex flex-col items-center justify-center">
      <div className="flex flex-col items-center">
        <h1 className="text-4xl font-bold text-center pt-10 pb-2">CATÁLOGO</h1>
        <div className="border-2 border-white md:w-1/6 w-1/2 mb-5"></div>
      </div>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-10">
        {foundProducts?.content?.map((product: IProduct) => (
          <ProductCard
            key={product.id}
            imagePath={product.imageUrl}
            productTitle={product.name}
            productDescription={product.description}
            productPrice={product.price.toString()}
            id={product.id!}
          />
        ))}
      </div>
    </div>
  );
}
