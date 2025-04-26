import { z } from "zod";
import { useForm } from "react-hook-form";
import IProduct from "../interfaces/IProduct.ts";
import { zodResolver } from "@hookform/resolvers/zod";
import axios from "axios";
import { useMutation, useQuery } from "@tanstack/react-query";
import ProductCard from "../components/ProductCard.tsx";
import IPagedResult from "../interfaces/IPagedResult.ts";
import { useToast } from "../components/Toast.tsx";

const formSchema = z.object({
  id: z.string().optional().readonly(),
  name: z.string().min(3, "O nome precisa ter pelo menos 3 caracteres"),
  description: z.string(),
  price: z
    .string()
    .transform((val) => Number(val))
    .or(z.number().min(1, "O preço deve ser maior que 0")),
  quantity: z
    .string()
    .transform((val) => Number(val))
    .or(z.number().min(1, "Deve haver ao menos 1 produto")),
  imageUrl: z.string().url("URL da imagem inválida"),
});

const apiUrl = "http://localhost:5087/api/products";

async function fetchProducts(): Promise<IPagedResult<IProduct>> {
  const response = await axios.get(apiUrl);
  return response.data as IPagedResult<IProduct>;
}

async function fetchSaveProduct(product: IProduct): Promise<IProduct> {
  const method = product.id ? "put" : "post";
  const response = await axios[method](apiUrl, product);
  return response.data as IProduct;
}

async function fetchDeleteProduct(id: string): Promise<boolean> {
  const response = await axios.delete(`${apiUrl}/${id}`);
  return response.data as boolean;
}

export default function Stock() {
  const { showToast } = useToast();

  const {
    register,
    handleSubmit,
    reset,
    trigger,
    formState: { errors, isValid },
  } = useForm({
    resolver: zodResolver(formSchema),
    mode: "onChange",
  });

  const { data: products, refetch } = useQuery<IPagedResult<IProduct>>({
    queryKey: ["products"],
    queryFn: fetchProducts,
  });

  const { mutateAsync: saveProduct } = useMutation({
    mutationFn: fetchSaveProduct,
    onSuccess() {
      reset();
      refetch();
      showToast("Estoque atualizado com sucesso!", "success");
    },
    onError: () => showToast("Erro ao atualizar estoque", "error"),
  });

  const { mutateAsync: deleteProduct } = useMutation({
    mutationFn: fetchDeleteProduct,
    onSuccess() {
      refetch();
      showToast("Produto removido com sucesso!", "success");
    },
    onError: () => showToast("Erro ao remover produto", "error"),
  });

  const editProduct = (id: string) => {
    const product = products?.content.find((prod: IProduct) => prod.id === id);
    if (product) {
      reset(product);
      trigger();
    }
  };

  const fields = [
    { name: "name", label: "Nome", type: "text" },
    { name: "price", label: "Preço", type: "number" },
    { name: "quantity", label: "Quantidade", type: "number" },
    { name: "description", label: "Descrição", type: "text" },
    { name: "imageUrl", label: "Imagem", type: "text" },
  ];

  return (
    <main className="flex flex-col pt-16">
      <div className="flex flex-col items-center justify-center">
        <h1 className="text-3xl font-bold text-center pb-2">
          Gerenciador de Estoque
        </h1>
        <div className="border-2 border-white md:w-1/12 w-1/2 mb-5"></div>
      </div>
      <div className="flex min-h-screen flex-col md:flex-row items-center justify-around">
        <form onSubmit={handleSubmit((data) => saveProduct(data as IProduct))}>
          <div className="flex flex-col items-start p-5 gap-5">
            <input type="hidden" {...register("id")} />
            {fields.map(({ name, label, type }) => (
              <div key={name} className="w-full">
                <label htmlFor={name} className="label">
                  {label}
                </label>
                <input
                  id={name}
                  type={type}
                  placeholder={`Digite ${label.toLowerCase()}`}
                  className="input input-bordered bg-neutral-content focus-within:ring-white focus-within:ring-2 w-full"
                  {...register(
                    name as keyof Omit<IProduct, "createdOn" | "modifiedOn">,
                  )}
                />
                {errors[name as keyof typeof errors] && (
                  <p className="text-error text-sm break-words">
                    {errors[name as keyof typeof errors]?.message?.toString()}
                  </p>
                )}
              </div>
            ))}
            <button
              type="submit"
              className={`text-xl btn ${!isValid && "btn-disabled"}`}
              disabled={!isValid}
            >
              Registrar
            </button>
          </div>
        </form>

        <div
          id="products_container"
          className="grid grid-col-1 gap-5 justify-center md:justify-normal md:w-96 w-full overflow-y-scroll overflow-x-hidden min-w-80 md:min-w-fit max-h-screen border-2 border-white rounded-lg p-10"
        >
          {products?.content.map((product: IProduct) => (
            <div
              key={product.id}
              className="bg-yellow-300 py-2 rounded-box flex flex-col items-center justify-center gap-2"
            >
              <ProductCard
                imagePath={product.imageUrl}
                productTitle={product.name}
                productDescription={product.description}
                productPrice={product.price.toString()}
                id={product.id!}
              />
              <div className="flex flex-row gap-5 mb-10 md:mb-0">
                <button
                  onClick={() => editProduct(product.id!)}
                  className="btn btn-accent"
                >
                  Editar
                </button>
                <button
                  onClick={() => deleteProduct(product.id!)}
                  className="btn btn-error"
                >
                  Deletar
                </button>
              </div>
            </div>
          ))}
        </div>
      </div>
    </main>
  );
}
