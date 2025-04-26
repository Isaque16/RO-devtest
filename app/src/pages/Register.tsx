import axios from "axios";
import IUser from "../interfaces/IUser.ts";
import { useNavigate } from "react-router";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { useState } from "react";
import { useMutation } from "@tanstack/react-query";
import { Link } from "react-router-dom";
import { useToast } from "../components/Toast.tsx";

async function createUser(data: IUser) {
  const response = await axios.post("http://localhost:5087/api/users", {
    name: data.name,
    userName: data.userName,
    password: data.password,
    email: data.email,
    phoneNumber: data.phoneNumber,
    role: data.role,
  });

  if (response.status !== 201) throw new Error("Erro ao criar usuário");

  return response.data;
}

const formDataSchema = z.object({
  id: z.string().optional(),
  name: z.string().min(3, "O nome precisa ter pelo menos 3 caracteres"),
  userName: z
    .string()
    .min(3, "O nome de usuário precisa ter pelo menos 3 caracteres"),
  password: z.string().min(6, "A senha precisa ter pelo menos 6 caracteres"),
  email: z.string().email("Email inválido"),
  phoneNumber: z.string().min(11, "O telefone deve ter pelo menos 11 dígitos"),
  role: z.number().optional(),
});

export default function Register() {
  const router = useNavigate();
  const { showToast } = useToast();

  const {
    register,
    handleSubmit,
    formState: { errors, isValid, isSubmitting },
  } = useForm<z.infer<typeof formDataSchema>>({
    resolver: zodResolver(formDataSchema),
    mode: "onChange",
    defaultValues: {
      role: 0,
    },
  });

  const { mutateAsync: registerUser } = useMutation({
    mutationFn: createUser,
    onSuccess: () => {
      showToast("Usuário criado com sucesso", "success");
      router("/login");
    },
    onError: () => showToast("Erro ao criar usuário", "error"),
  });

  const [isPasswordVisible, setIsPasswordVisible] = useState(false);

  async function onRegister(data: z.infer<typeof formDataSchema>) {
    await registerUser(data as IUser);
  }

  return (
    <main className="flex flex-col items-center justify-center h-full md:h-screen pt-10">
      <div className="grid grid-cols-1 md:grid-cols-2 items-center justify-center shadow-md">
        <div className="flex flex-col items-center justify-center h-full w-full rounded-tr-box rounded-tl-box md:rounded-tl-box md:rounded-bl-box md:rounded-tr-none">
          <h1 className="text-3xl text-yellow-300 font-bold text-center pt-10 pb-2">
            Cadastre-se
          </h1>
          <div className="border-2 border-white w-1/6 mb-5"></div>
        </div>
        <div className="p-10 w-full h-full rounded-bl-box rounded-br-box md:rounded-br-box md:rounded-tr-box md:rounded-bl-none">
          <form
            className="flex flex-col gap-5"
            onSubmit={handleSubmit(onRegister)}
          >
            <div className="form-control">
              <label htmlFor="name" className="label">
                Nome Completo
              </label>
              <input
                id="name"
                type="text"
                placeholder="Digite seu Nome Completo"
                className="input input-bordered bg-neutral-content focus-within:ring-white focus-within:ring-2 w-full"
                {...register("name")}
              />
              <p className="text-error text-sm break-words">
                {errors.name?.message}
              </p>
            </div>

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
              <div className="flex flex-row justify-between input input-bordered bg-neutral-content">
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

            <div className="form-control">
              <label htmlFor="email" className="label">
                Email
              </label>
              <input
                id="email"
                type="text"
                placeholder="Digite seu Email"
                className="input input-bordered bg-neutral-content focus-within:ring-white focus-within:ring-2 w-full"
                {...register("email")}
              />
              <p className="text-error text-sm break-words">
                {errors.email?.message}
              </p>
            </div>

            <div className="form-control">
              <label htmlFor="phoneNumber" className="label">
                Telefone
              </label>
              <input
                id="phoneNumber"
                type="text"
                placeholder="Digite seu Telefone"
                className="input input-bordered bg-neutral-content focus-within:ring-white focus-within:ring-2 w-full"
                {...register("phoneNumber")}
              />
              <p className="text-error text-sm break-words">
                {errors.phoneNumber?.message}
              </p>
            </div>

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
                "Registrar"
              )}
            </button>
          </form>
          <Link to="/login" className="link link-hover mt-2">
            Já tenho uma conta
          </Link>
        </div>
      </div>
    </main>
  );
}
