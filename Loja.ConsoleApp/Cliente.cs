namespace Loja.ConsoleApp
{
    public class Cliente
    {
        public Cliente()
        {

        }

        public int Id { get; set; }
        public string Nome { get; internal set; }
        public string Cpf { get; internal set; }
        public string Telefone { get; internal set; }
        public Endereco EnderecoDeEntrega { get; internal set; } /*referencia para Endereco*/
    }
}