import axios from "axios";
import IUser from "../interfaces/IUser.ts";
import { useQuery } from "@tanstack/react-query";
import { useNavigate } from "react-router";

async function fetchUserDataById(id: string): Promise<IUser> {
  const response = await axios.get(`http://localhost:5087/user/${id}`);
  return response.data as IUser;
}

export default function UserIcon() {
  const { data: userData } = useQuery({
    queryKey: ["userData"],
    queryFn: () => {
      const userId = localStorage.getItem("userId");
      if (userId) {
        return fetchUserDataById(userId);
      }
      return null;
    },
  });

  const navigate = useNavigate();

  const logOut = () => {
    localStorage.removeItem("userId");
    navigate("/login");
  };

  return (
    <div className="flex-none">
      {!userData?.id ? (
        <div>
          <a href="/login" className="btn btn-ghost text-xl">
            Login
          </a>
        </div>
      ) : (
        <div className="dropdown dropdown-end cursor-pointer">
          <div role="button" tabIndex={0} className="avatar placeholder">
            <div className="bg-ink-900 text-neutral-content w-12 rounded-full py-3">
              <span className="flex items-center justify-center">
                {userData?.userName.toUpperCase().charAt(0) ?? "C"}
              </span>
            </div>
          </div>
          <div
            tabIndex={0}
            className="card card-compact dropdown-content bg-base-100 z-[1] mt-3 w-52 shadow"
          >
            <div className="card-body">
              <span className="text-lg">{userData?.userName}</span>
              <ul className="list text-base">
                <li>
                  <a
                    href="/config"
                    className="link-primary hover:cursor-pointer"
                  >
                    Configurações
                  </a>
                </li>
                <li>
                  <a
                    href="/historico"
                    className="link-primary hover:cursor-pointer"
                  >
                    Histórico de Compras
                  </a>
                </li>
                <li className="border-b-2 border-black/10 min-w-[95%] flex justify-self-center my-2"></li>
                <li>
                  <button
                    onClick={logOut}
                    className="link-error hover:cursor-pointer"
                  >
                    Log out
                  </button>
                </li>
              </ul>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
