namespace Loja.ConsoleApp
{
    //classe utilizada para fazer o relacionamento muitos para muitos 
    //entre produtos e promocoes
    public class PromocaoProduto
    {
        /*chave primaria desta classe será uma chave composta. 
         * E está sendo criada na Classe 'LojaContext'*/
        public int ProdutoId { get; set; }      /*campo de chave estrangeira*/
        public Produto Produto { get; set; }
        public int PromocaoId { get; set; }     /*campo de chave estrangeira*/
        public Promocao Promocao { get; set; }
    }
}
