export default function Home() {
  return (
    <main>
      <div className="flex flex-col gap-3 py-10 text-center">
        <h1 className="text-4xl font-bold">
          TRANSFORME SUA <br /> OFICINA EM UMA <br />
          <span className="text-yellow-300">MÁQUINA DE VENDAS</span>
        </h1>
        <p className="text-2xl container mx-auto max-w-4xl">
          A Rota é especialista em Pós-venda para o Setor Automotivo e já ajuda
          mais de 200 oficinas e centros automotivos a aumentarem suas vendas.
        </p>
      </div>

      <article className="container mx-auto max-w-4xl">
        <div>
          <h2 className="text-3xl text-center font-bold py-10">Quem somos</h2>
        </div>
        <div>
          <p className="text-justify text-xl px-20">
            A Rota é uma startup fundada em 2016, referência em tecnologia e
            inovação para o setor automotivo. Atualmente, somos o Departamento
            de Pós-venda de mais de 200 centros automotivos pelo Brasil,
            garantindo a entrega de serviços de qualidade ao cliente final e o
            crescimento constante dos nossos parceiros. Nossa expertise no setor
            automotivo permite traçar estratégias de fidelização de clientes de
            maneira efetiva, possibilitando aos nossos parceiros o crescimento
            constante dos seus negócios. Toda comunicação do proceso de
            Pós-venda é realizada via WhatsApp de forma personalizada e
            estratégica, isto é, cada empresa parceira possui seu WhatsApp
            exclusivo de Pós-venda e sua régua de relacionamento com os seus
            clientes, de acordo com as suas metas.
          </p>
        </div>
      </article>
    </main>
  );
}
