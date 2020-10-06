using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Loja.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Todos os metodos abaixo são para gravar, recuperar, excluir, atualizar dados de um banco de dados

            //GravarUsandoEntity();    /*insert into*/
            //RecuperarUsandoEntity(); /*select*/
            //ExcluirUsandoEntity();   /*delete*/
            //RecuperarUsandoEntity();
            //AtualizaUsandoEntity();  /*update*/
            //UsandoChangeTracker();

            //FazCompraDeProdutoNaoExistente();
            //InserePromocaoEProdutos_UmParaMuitos();
            //InsereClienteEEnderecoEmTabelasDiferentes_UmParaUm();
            //InsereUmaPromocaoESeusProdutos();
            //ExibeProdutosPromocao_FazendoJoinEntreDuasTabelas();
            //FazendoSelect_UmParaUm();

            using (var contexto = new LojaContext())
            {
                //fazendo select um para um
                var cliente = contexto
                    .Clientes
                    .Include(c => c.EnderecoDeEntrega)      /*instancia classe que por sua vez possui a propriedade Logradouro*/
                    .FirstOrDefault();

                Console.WriteLine($"Endereço de entrega {cliente.EnderecoDeEntrega.Logradouro}");

                //seleciona apenas o produto com ID especifica
                var produto = contexto
                    .Produtos
                    .Where(p => p.Id == 6002)
                    .FirstOrDefault();

                //filtrando apenas compras deste produto que forem maiores que 2.40 reais//

                //Para entry que está na referencia produto
                contexto.Entry(produto)
                    .Collection(p => p.Compras) /*pega a coleção compras*/
                    .Query()                    /*faz uma consulta*/
                    .Where(c => c.Preco > 2.40)   /*todas as compras cujo o preço é maior que 2.40*/
                    .Load();                    /*carrega a consulta para a referencia produto*/

                Console.WriteLine($"Mostrando as compras do produto {produto.Nome}");

                foreach (var item in produto.Compras)
                {
                    Console.WriteLine($"{item.Produto}");
                }
            }
        }

        private static void FazendoSelect_UmParaUm()
        {
            using (var contexto = new LojaContext())
            {
                //fazendo select um para um
                var cliente = contexto
                    .Clientes
                    .Include(c => c.EnderecoDeEntrega)      /*instancia classe que por sua vez possui a propriedade Logradouro*/
                    .FirstOrDefault();

                Console.WriteLine($"Endereço de entrega {cliente.EnderecoDeEntrega.Logradouro}");

                var produto = contexto
                    .Produtos
                    .Include(p => p.Compras)
                    .Where(p => p.Id == 6002)
                    .FirstOrDefault();

                Console.WriteLine($"Mostrando as compras do produto {produto.Nome}");

                foreach (var item in produto.Compras)
                {
                    Console.WriteLine($"{item.Produto}");
                }
            }
        }

        private static void ExibeProdutosPromocao_FazendoJoinEntreDuasTabelas()
        {
            //recupera informações muitos_para_muitos
            using (var contexto2 = new LojaContext())
            {
                //fazendo select na tabela de promocao
                //var promocao = contexto2.Promocoes.FirstOrDefault();

                //faz um select com join
                var promocao = contexto2
                    .Promocoes
                    .Include(produto => produto.Produtos)          /*inclui a tabela 'PromocaoProduto' para fazer a ligação entre tabela 'Produtos' e 'Promocao', conforme a imagem 'Esquema_muitos-para-muitos'*/
                    .ThenInclude(promocaoProduto => promocaoProduto.Produto)    /*'ThenInclude': desce mais niveis no relacionamento*/
                    .FirstOrDefault();

                Console.WriteLine($"Mostrando os produtos da promoção Queima Total Janeiro 2020");

                //retorna a promoção e os itens da promoção
                //ou seja, retorna informações de duas tabelas distintas

                foreach (var item in promocao.Produtos)
                {
                    Console.WriteLine(item.Produto);
                }
            }
        }

        private static void InsereUmaPromocaoESeusProdutos()
        {
            using (var contexto = new LojaContext())
            {
                //instancia a promocao
                var promocao = new Promocao();
                promocao.Descricao = "Queima Total Janeiro 2020";
                promocao.DataInicio = new DateTime(2020, 1, 1);
                promocao.DataTermino = new DateTime(2020, 1, 31);

                //faz um select utilizando 'where' do sql
                var produtos = contexto
                    .Produtos
                    .Where(p => p.Categoria == "Bebidas") /*<- filtra todo mundo que tiver nessa categoria*/
                    .ToList();

                //adiciona esses produtos a promocação
                foreach (var item in produtos)
                {
                    promocao.IncluiProduto(item);
                }

                //adiciona ao contexto da promocao
                contexto.Promocoes.Add(promocao);

                ExibeEntries(contexto.ChangeTracker.Entries()); /*mostra changetracker das entidades*/

                //salva contexto
                contexto.SaveChanges();
            }
        }

        private static void InsereClienteEEnderecoEmTabelasDiferentes_UmParaUm()
        {
            var fulano2 = new Cliente();
            fulano2.Nome = "Fulaninho de tal";
            fulano2.Cpf = "111.111.111-98";
            fulano2.Telefone = "99999999";
            fulano2.EnderecoDeEntrega = new Endereco()
            {
                Numero = 12,
                Logradouro = "Rua dos inválidos",
                Complemento = "Sobrado",
                Bairro = "Centro",
                Cidade = "Cidade"
            };

            //contexto de loja, representando o entity
            using (var contexto = new LojaContext())
            {
                contexto.Clientes.Add(fulano2);
                contexto.SaveChanges();
            }
        }

        private static void InserePromocaoEProdutos_UmParaMuitos()
        {
            //define promocao
            var promocaoDePascoa = new Promocao();
            promocaoDePascoa.Descricao = "Pascoa Feliz";
            promocaoDePascoa.DataInicio = DateTime.Now;
            promocaoDePascoa.DataTermino = DateTime.Now.AddMonths(3);

            //define produtos da promocao
            var p1 = new Produto() { Nome = "Suco de laranja", Categoria = "Bebidas", PrecoUnitario = 8.79, Unidade = "Litros" };
            var p2 = new Produto() { Nome = "Café", Categoria = "Bebidas", PrecoUnitario = 12.45, Unidade = "Gramas" };
            var p3 = new Produto() { Nome = "Macarrão", Categoria = "Alimentos", PrecoUnitario = 4.23, Unidade = "Gramas" };

            //a promocao de pascoa é valida apenas para um determinado conjunto de produtos
            //ou seja, uma coleção (Collection)
            promocaoDePascoa.IncluiProduto(p1);
            promocaoDePascoa.IncluiProduto(p2);
            promocaoDePascoa.IncluiProduto(p3);

            //persiste os produtos no banco
            using (var contexto = new LojaContext())
            {
                contexto.Promocoes.Add(promocaoDePascoa);
                ExibeEntries(contexto.ChangeTracker.Entries()); /*mostra changetracker das entidades*/
                contexto.SaveChanges();

                //deleta os produtos
                //var promocao = contexto.Promocoes.Find(3);
                //contexto.Promocoes.Remove(promocao);
                //contexto.SaveChanges();
            }
        }

        private static void FazCompraDeProdutoNaoExistente()
        {
            //no caso o pão frances, não existia cadastrado na tabela "Produtos". Não se pode fazer
            //uma compra de um produto que não existe cadastrado no sistema.
            //Ao fazer uma compra onde este produto não existia, o entity adiciona automaticamente
            //a tabela de "Produtos" este novo produto e também adiciona a compra a tabela de "Compras"

            var paoFrances = new Produto();
            paoFrances.Nome = "Pão Francês";
            paoFrances.PrecoUnitario = 0.40;
            paoFrances.Unidade = "Unidade";
            paoFrances.Categoria = "Padaria";

            var compra = new Compra();
            compra.Quantidade = 6;
            compra.Produto = paoFrances;
            compra.Preco = paoFrances.PrecoUnitario * compra.Quantidade;

            //adiciona a compra a tabela de compra
            //o objeto de compra faz referencia ao produto paoFrances
            //que sua vez adiciona o produto a tabela de produto
            using (var contexto = new LojaContext())
            {
                //colocando a compra no contexto
                contexto.Compras.Add(compra);
                ExibeEntries(contexto.ChangeTracker.Entries());
                contexto.SaveChanges();
            }
        }

        private static void UsandoChangeTracker()
        {
            //ChangeTracker é utilizado para gerenciar o estado das entidades (states)

            using (var contexto = new LojaContext())
            {
                //Select
                var produtos = contexto.Produtos.ToList();
                //foreach (var item in produtos) /*escreve os itens selecionados*/
                //{
                //    Console.WriteLine(item);
                //}

                //exbie as alterações de estado no console
                ExibeEntries(contexto.ChangeTracker.Entries());

                //Update - de um campo de um produto
                var p1 = produtos.First();
                p1.Nome = "Harry Potter - Editado 2";
                contexto.SaveChanges();

                //Insert Into - cria um novo produto
                var novoProduto = new Produto()
                {
                    Nome = "Velozes e furiosos 3",
                    Categoria = "Livros",
                    PrecoUnitario = 10.99
                };
                contexto.Produtos.Add(novoProduto);
                ExibeEntries(contexto.ChangeTracker.Entries());  /*apenas para mostrar o estado da entidade antes de salvar (persistir a informação no banco)*/
                contexto.SaveChanges();
                ExibeEntries(contexto.ChangeTracker.Entries()); /*apenas para mostrar o estado da entidade após salvar (persistir a informação no banco)*/

                //Delete - deleta um item do banco
                var p2 = produtos.First();
                contexto.Produtos.Remove(p2);
                ExibeEntries(contexto.ChangeTracker.Entries());  /*'state' antes de deletar*/
                contexto.SaveChanges();
                ExibeEntries(contexto.ChangeTracker.Entries()); /*'state' após deletar*/

                //Adiciona e Deleta um objeto sem utilizar SaveChanges() - o que é errado.
                var p3 = new Produto()
                {
                    Nome = "Velozes e furiosos 4555",
                    Categoria = "Livros",
                    PrecoUnitario = 10.99
                };

                //Quando um método .Remove é chamado em um objeto que que estava em um state 'added' 
                //e não foi salvo (utilizando .SaveChanges)
                //o 'ChangeTracker', ignora e não envia as informações para o banco
                contexto.Produtos.Add(p3);
                //contexto.SaveChanges();
                ExibeEntries(contexto.ChangeTracker.Entries());
                contexto.Produtos.Remove(p3);
                ExibeEntries(contexto.ChangeTracker.Entries());

                var myEntry = contexto.Entry(p3);
                Console.WriteLine(myEntry.ToString() + " - " + myEntry.State);
            }
        }

        private static void ExibeEntries(IEnumerable<EntityEntry> entries)
        {
            //ChangeTracker: faz o rastreio das alterações que estão acontecendo neste contexto
            Console.WriteLine("================");
            foreach (var e in entries)
            {
                Console.WriteLine(e.Entity.ToString() + " - " + e.State); /*exibe o que está aparecendo na lista*/
            }
        }

        private static void AtualizaUsandoEntity()
        {
            using (var repo = new ProdutoDAO_Entity())
            {
                //pegando apenas 1 produto, ou seja, um registro
                Produto primeiro = repo.Produtos().First();

                //altera o campo conforme a necessidade
                primeiro.Nome = "Cassino Royale - Editado";
                primeiro.PrecoUnitario = 49.99;

                //faz o update no banco em si e salva
                repo.Atualizar(primeiro);

                //o .SaveChanges() está na classe ProdutoDAO_Entity
            }

            RecuperarUsandoEntity();
        }

        private static void ExcluirUsandoEntity()
        {
            using (var repo = new ProdutoDAO_Entity())
            {
                //excluuindo todos os produtos

                //primeiro você deve selecionar os itens que você deseja apagar
                IList<Produto> produtos = repo.Produtos();
                foreach (var item in produtos)
                {
                    repo.Remover(item);
                }
            }
        }

        private static void RecuperarUsandoEntity()
        {
            //cria instancia 'LojaContext' que representa o contexto para a aplicação
            using (var repo = new ProdutoDAO_Entity())
            {
                //lista de produtos
                IList<Produto> produtos = repo.Produtos();

                Console.WriteLine($"Foram recuperados {produtos.Count} produtos(s).");

                foreach (var item in produtos)
                {
                    Console.WriteLine(item.Nome + ", "+ item.PrecoUnitario);
                }
            }
        }

        private static void GravarUsandoEntity()
        {
            Produto p = new Produto();
            p.Nome = "Harry Potter e a Ordem da Fênix";
            p.Categoria = "Livros";
            p.PrecoUnitario = 19.89;

            using (var contexto = new ProdutoDAO_Entity())
            {
                contexto.Adicionar(p);  /*adiciona o produto*/
            }
        }
    }
}
