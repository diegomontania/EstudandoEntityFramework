using System.Collections.Generic;

namespace Loja.ConsoleApp
{
    public class Produto
    {
        public int Id { get; internal set; }
        public string Nome { get; internal set; }
        public string Categoria { get; internal set; }
        public double PrecoUnitario { get; internal set; }
        public string Unidade { get; internal set; }

        /*para fazer o relacionamento muitos pra muitos
         será necessario criar uma lista de 'Produtos'
         na classe 'Produto'

        porem é necessario criar uma classe que vai fazer a tabela que irá
        fazer os 'joins' no relacionamento muitos para muitos 
        
        utilizando a classe 'PromocaoProduto' */
        public List<PromocaoProduto> Promocoes { get; set; }

        //lista responsável por saber quais compras foram feitas e que tiveram aquele produto
        public List<Compra> Compras { get; set; }

        //sobreescrevendo o metodo .TosTring()
        public override string ToString()
        {
            return $"Produto: {this.Id}, {this.Nome}, {this.Categoria}, {this.PrecoUnitario}";
        }
    }
}