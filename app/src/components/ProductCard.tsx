export default function ProductCard({
  id,
  imagePath,
  productTitle,
  productDescription,
  productPrice,
}: {
  id: string;
  imagePath: string;
  productTitle: string;
  productDescription: string;
  productPrice: string;
}) {
  return (
    <a href={`/catalogo/${id}`}>
      <div className="card w-80 md:w-96 max-h-screen bg-neutral-content hover:ring-1 hover:ring-white shadow-xl p-2 cursor-pointer">
        <figure className="image-full">
          <img src={imagePath} alt="image" width={400} height={225} />
        </figure>
        <div className="card-body">
          <h2 className="card-title">{productTitle}</h2>
          <p>{productDescription}</p>
          <p className="text-sm font-bold">
            R$<span className="text-2xl">{productPrice}</span>
          </p>
        </div>
        <p className="opacity-30 px-8">ID: {id}</p>
      </div>
    </a>
  );
}
