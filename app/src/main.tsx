import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { BrowserRouter } from "react-router-dom";
import Header from "./layout/Header.tsx";
import Footer from "./layout/Footer.tsx";
import StoreProvider from "./store/StoreProvider.tsx";
import AppRoutes from "./AppRoutes.tsx";

const queryClient = new QueryClient();

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <StoreProvider>
        <BrowserRouter>
          <Header />
          <AppRoutes />
          <Footer />
        </BrowserRouter>
      </StoreProvider>
    </QueryClientProvider>
  </StrictMode>,
);
