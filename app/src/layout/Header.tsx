import CartIcon from "../components/CartIcon.tsx";
import UserIcon from "../components/UserIcon.tsx";

export default function Header() {
  return (
    <header className="menu menu-horizontal sticky w-full flex flex-row justify-between px-4 items-center shadow-md">
      <div className="flex flex-row md:flex-row navbar justify-between">
        <div className="flex flex-row">
          <div className="flex-1">
            <a href="/">
              <img
                className="w-17"
                src="/ro_logo.png"
                alt="Rota das Oficinas logo"
              />
            </a>
          </div>
        </div>
        <ul
          tabIndex={0}
          className="hidden md:flex menu menu-vertical md:menu-horizontal rounded-box gap-2 text-xl"
        >
          <li>
            <a className="btn btn-ghost text-2xl" href="/">
              Sobre nós
            </a>
          </li>
          <li>
            <a className="btn btn-ghost text-2xl" href="/catalog">
              Produtos
            </a>
          </li>
        </ul>
        <div className="hidden md:flex flex-row justify-center items-center gap-5">
          <CartIcon />
          <UserIcon />
        </div>
      </div>
    </header>
  );
}
