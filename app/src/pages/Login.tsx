import { z } from "zod";
import { useNavigate } from "react-router";
import { useDispatch } from "react-redux";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import axios from "axios";
import { useState } from "react";
import { Link } from "react-router-dom";
import { setUserData } from "../store/slices/userSlice.ts";
import { useMutation } from "@tanstack/react-query";
import { useToast } from "../components/Toast.tsx";

async function fetchLogin(credentials: { userName: string; password: string }) {
  const response = await axios.post(
    "http://localhost:5087/api/auth/login",
    credentials,
  );
  return response.data;
}

const formSchema = z.object({
  userName: z
    .string()
    .min(3, "O nome de usuário precisa ter pelo menos 3 caracteres"),
  password: z.string().min(6, "A senha precisa ter pelo menos 6 caracteres"),
});

type LoginType = z.infer<typeof formSchema>;

export default function Login() {
  const router = useNavigate();
  const dispatch = useDispatch();
  const { showToast } = useToast();

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isValid, isSubmitting },
  } = useForm<LoginType>({
    resolver: zodResolver(formSchema),
    mode: "onChange",
  });

  const { mutateAsync: loginUser } = useMutation({
    mutationFn: fetchLogin,
    onSuccess({ customerData, token }) {
      dispatch(setUserData({ nomeUsuario: customerData.nomeUsuario }));
      localStorage.setItem("id", customerData?.id);
      localStorage.setItem("token", token);

      showToast("Logado com sucesso", "success");
      router("/catalogo");
    },
    onError: () => showToast("Erro ao logar usuário", "error"),
  });

  const [isPasswordVisible, setIsPasswordVisible] = useState(false);

  async function onLogin({ userName, password }: LoginType) {
    const user = await loginUser({ userName, password });

    if (!user) {
      reset();
      showToast("Usuário ou senha incorretos", "error");
      return;
    }
  }

  return (
    <main className="flex flex-col items-center justify-center h-screen">
      <div className="grid grid-cols-1 md:grid-cols-2 items-center justify-center rounded-xl shadow-md">
        <div className="flex flex-col items-center justify-center h-full w-full rounded-tr-box rounded-tl-box md:rounded-tl-box md:rounded-bl-box md:rounded-tr-none">
          <h1 className="text-3xl font-bold text-yellow-300 text-center pt-10 pb-2">
            Login
          </h1>
          <div className="border-2 border-white md:w-1/6 w-1/2 mb-5"></div>
        </div>
        <div className="p-10 w-full max-w-80 h-full rounded-bl-box rounded-br-box md:rounded-br-box md:rounded-tr-box md:rounded-bl-none">
          <form
            className="flex flex-col gap-5"
            onSubmit={handleSubmit(onLogin)}
          >
            <div className="form-control">
              <label htmlFor="userName" className="label">
                Usuario
              </label>
              <input
                id="userName"
                type="text"
                placeholder="Digite seu Usuario"
                className="input input-bordered bg-neutral-content focus-within:ring-white focus-within:ring-2 w-full"
                {...register("userName")}
              />
              <p className="text-error text-sm break-words">
                {errors.userName?.message}
              </p>
            </div>

            <div className="form-control">
              <label htmlFor="password" className="label">
                Senha
              </label>
              <div className="flex flex-row bg-neutral-content justify-between input input-bordered">
                <input
                  id="password"
                  type={isPasswordVisible ? "text" : "password"}
                  placeholder="Digite sua Senha"
                  className="input input-bordered focus-within:ring-white focus-within:ring-2 w-full"
                  {...register("password")}
                />
                <button
                  type="button"
                  onClick={() => setIsPasswordVisible((value) => !value)}
                >
                  {isPasswordVisible ? (
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      fill="none"
                      viewBox="0 0 24 24"
                      strokeWidth={1.5}
                      stroke="#000000"
                      className="size-6"
                    >
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        d="M3.98 8.223A10.477 10.477 0 0 0 1.934 12C3.226 16.338 7.244 19.5 12 19.5c.993 0 1.953-.138 2.863-.395M6.228 6.228A10.451 10.451 0 0 1 12 4.5c4.756 0 8.773 3.162 10.065 7.498a10.522 10.522 0 0 1-4.293 5.774M6.228 6.228 3 3m3.228 3.228 3.65 3.65m7.894 7.894L21 21m-3.228-3.228-3.65-3.65m0 0a3 3 0 1 0-4.243-4.243m4.242 4.242L9.88 9.88"
                      />
                    </svg>
                  ) : (
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      fill="none"
                      viewBox="0 0 24 24"
                      strokeWidth={1.5}
                      stroke="#000000"
                      className="size-6"
                    >
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        d="M2.036 12.322a1.012 1.012 0 0 1 0-.639C3.423 7.51 7.36 4.5 12 4.5c4.638 0 8.573 3.007 9.963 7.178.07.207.07.431 0 .639C20.577 16.49 16.64 19.5 12 19.5c-4.638 0-8.573-3.007-9.963-7.178Z"
                      />
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        d="M15 12a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z"
                      />
                    </svg>
                  )}
                </button>
              </div>
              <p className="text-error text-sm break-words">
                {errors.password?.message}
              </p>
            </div>

            {errors.root?.message && (
              <div className="toast toast-top toast-end">
                <div className="alert alert-error">
                  <span className="text-white font-bold">
                    {errors.root?.message}
                  </span>
                </div>
              </div>
            )}

            <button
              type="submit"
              className={`text-xl btn ${
                !isValid || isSubmitting ? "btn-disabled" : ""
              }`}
              disabled={!isValid || isSubmitting}
            >
              {isSubmitting ? (
                <div className="loading loading-dots loading-lg"></div>
              ) : (
                "Entrar"
              )}
            </button>
          </form>
          <Link to="/singup" className="link link-hover mt-2">
            Não tenho uma conta
          </Link>
        </div>
      </div>
    </main>
  );
}
