export default function Footer() {
  return (
    <footer className="footer md:footer-horizontal gap-10 bg-base-200 text-base-content py-10 px-10 md:px-50 mt-10">
      <aside className="flex justify-self-center">
        <img src="/ro_logo.png" alt="Rota das Oficinas Logo" />
      </aside>
      <div className="w-2/4 text-justify justify-self-center">
        <h6 className="footer-title">Sobre nós</h6>
        <p>
          A Rota das Oficinas nasceu em 2016, na cidade de São José do Rio
          Preto. Hoje, somos referência nacional em tecnologia para o Setor
          Automotivo, oferecendo um Departamento de Pós-Venda para mais de 200
          oficinas pelo Brasil.
        </p>
      </div>
      <nav>
        <h6 className="footer-title">Soluções</h6>
        <a
          className="link link-hover"
          href="https://www.rotadasoficinas.com.br/pos-venda-para-oficinas"
        >
          Pós venda 360
        </a>
      </nav>
      <nav>
        <h6 className="footer-title">Acesso Rápido</h6>
        <a
          className="link link-hover"
          href="https://parceiro.rotadasoficinas.com.br/"
        >
          Área do Parceiro
        </a>
        <a
          className="link link-hover"
          href="https://blog.rotadasoficinas.com.br/"
        >
          Blog
        </a>
        <a
          className="link link-hover"
          href="https://www.rotadasoficinas.com.br/melhores-oficinas-do-brasil"
        >
          Top 50
        </a>
        <a
          className="link link-hover"
          href="https://www.rotadasoficinas.com.br/conteudos-em-parceria"
        >
          Conteúdos
        </a>
      </nav>
    </footer>
  );
}
