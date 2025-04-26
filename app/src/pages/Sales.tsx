import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import SaleCard from "../components/SaleCard";
import ISale from "../interfaces/ISale";
import { useState } from "react";
import IPagedResult from "../interfaces/IPagedResult";

const API_URL = "http://localhost:5087/api/sales";

async function fetchSales(startDate: string = "", endDate: string = "") {
  const url =
    startDate && endDate
      ? `${API_URL}?startDate=${startDate}&endDate=${endDate}`
      : API_URL;

  const response = await axios.get(url);
  return response.data;
}

export default function Sales() {
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [filterApplied, setFilterApplied] = useState(false);

  const {
    data: sales,
    isLoading,
    isError,
    error,
    refetch,
  } = useQuery<IPagedResult<ISale>>({
    queryKey: [
      "sales",
      filterApplied ? startDate : "",
      filterApplied ? endDate : "",
    ],
    queryFn: () =>
      fetchSales(filterApplied ? startDate : "", filterApplied ? endDate : ""),
  });

  const handleFilter = () => {
    if (!startDate || !endDate) return;

    setFilterApplied(true);
    refetch();
  };

  const clearFilters = () => {
    setStartDate("");
    setEndDate("");
    setFilterApplied(false);
    refetch();
  };

  return (
    <main className="min-h-screen pb-10">
      <div className="flex flex-col items-center justify-center">
        <h1 className="text-3xl font-bold text-center pt-10 pb-2">Pedidos</h1>
        <div className="border-b-2 border-white md:w-1/12 w-1/2 mb-5"></div>

        <div className="flex flex-col md:flex-row gap-4 mb-5 p-4 bg-base-200 rounded-lg w-fit max-w-3xl">
          <div>
            <label htmlFor="startDate" className="label text-white font-medium">
              Data Inicial
            </label>
            <input
              id="startDate"
              type="date"
              value={startDate}
              onChange={(e) => setStartDate(e.target.value)}
              className="input input-bordered bg-neutral-content focus-within:ring-white focus-within:ring-2"
            />
          </div>

          <div>
            <label htmlFor="endDate" className="label text-white font-medium">
              Data Final
            </label>
            <input
              id="endDate"
              type="date"
              value={endDate}
              onChange={(e) => setEndDate(e.target.value)}
              className="input input-bordered bg-neutral-content focus-within:ring-white focus-within:ring-2"
            />
          </div>

          <div className="flex gap-2 self-end">
            <button
              className="btn btn-primary"
              onClick={handleFilter}
              disabled={!startDate || !endDate || isLoading}
            >
              {isLoading ? (
                <span className="loading loading-spinner"></span>
              ) : (
                "Filtrar"
              )}
            </button>
            {filterApplied && (
              <button className="btn btn-outline" onClick={clearFilters}>
                Limpar
              </button>
            )}
          </div>
        </div>

        {isLoading && (
          <div className="flex justify-center my-10">
            <span className="loading loading-spinner loading-lg"></span>
          </div>
        )}

        {isError && (
          <div className="alert alert-error max-w-md mx-auto my-5">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              className="stroke-current shrink-0 h-6 w-6"
              fill="none"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z"
              />
            </svg>
            <span>Erro ao carregar pedidos: {(error as Error).message}</span>
          </div>
        )}

        {!isLoading && !isError && sales?.content?.length === 0 && (
          <div className="alert alert-info max-w-md mx-auto my-5">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              className="stroke-current shrink-0 w-6 h-6"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
              ></path>
            </svg>
            <span>
              Nenhum pedido encontrado
              {filterApplied ? " para o período selecionado" : ""}.
            </span>
          </div>
        )}

        {!isLoading &&
          !isError &&
          sales?.content &&
          sales.content.length > 0 && (
            <div
              className="grid grid-cols-1 gap-5 justify-center overflow-y-auto max-h-[70vh]
                      border border-base-300 rounded-lg p-6 bg-base-100 shadow-lg
                      w-full max-w-3xl"
            >
              {sales.content.map((sale: ISale) => (
                <div
                  key={sale.id}
                  className="bg-base-200 py-3 px-4 rounded-box shadow-sm hover:shadow-md transition-shadow"
                >
                  <SaleCard sale={sale} />
                </div>
              ))}
            </div>
          )}
      </div>
    </main>
  );
}
