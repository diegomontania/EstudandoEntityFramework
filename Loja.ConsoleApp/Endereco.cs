namespace Loja.ConsoleApp
{
    public class Endereco
    {
        public int Numero { get; internal set; }
        public string Logradouro { get; internal set; }
        public string Complemento { get; internal set; }
        public string Bairro { get; internal set; }
        public string Cidade { get; internal set; }

        //'Endereco' seja dependente de 'Cliente'
        public Cliente Cliente { get; set; }    /*referencia para Cliente*/
    }
}