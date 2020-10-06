namespace Loja.ConsoleApp
{
    public class Compra
    {
        public int Id { get; set; }
        public int ProdutoID { get; set; } /*propriedade utilizada para ser CHAVEID do produto e não pode ser null*/
        public int Quantidade { get; internal set; }
        public Produto Produto { get; internal set; }
        public double Preco { get; internal set; }
    }
}