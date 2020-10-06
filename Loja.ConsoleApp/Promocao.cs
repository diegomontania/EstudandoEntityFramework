//representa classe responsável por promoções na loja para baixar os estoques
using System;
using System.Collections.Generic;

namespace Loja.ConsoleApp
{
    public class Promocao
    {
        public int Id { get; set; }
        public string Descricao { get; internal set; }
        public DateTime DataInicio { get; internal set; }
        public DateTime DataTermino { get; internal set; }

        /*para fazer o relacionamento muitos pra muitos
         será necessario criar uma lista de 'Promocoes'
         na classe 'Produto'

        porem é necessario criar uma classe que vai fazer a tabela que irá
        fazer os 'joins' no relacionamento muitos para muitos 
        
        utilizando a classe 'PromocaoProduto' */

        public IList<PromocaoProduto> Produtos { get; internal set; }

        public Promocao()
        {
            this.Produtos = new List<PromocaoProduto>();
        }

        public void IncluiProduto(Produto produto)
        {
            this.Produtos.Add(new PromocaoProduto() { Produto = produto });
        }
    }
}
