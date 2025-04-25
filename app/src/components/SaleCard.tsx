import ISale from "../interfaces/ISale.ts";

export default function SaleCard({ sale }: { sale: ISale }) {
  return (
    <div
      key={sale.id}
      className="bg-base-300 py-2 rounded-box flex flex-col items-center justify-center gap-2"
    >
      <div className="card card-body card-bsaleed shadow-md">
        <p>
          Pedido feito por{" "}
          <span className="font-bold">{sale.customer.name}</span>
        </p>
        <p>
          Produtos da cesta: <br />
          <span className="font-bold">
            {sale.products.map((produto) => produto.name).join(", ")}
          </span>
        </p>
        <p>
          Valor total da compra:{" "}
          <span className="font-bold">R${sale.totalPrice}</span>
        </p>
      </div>
    </div>
  );
}
