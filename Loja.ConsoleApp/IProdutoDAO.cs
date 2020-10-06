using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.ConsoleApp
{
    interface IProdutoDAO
    {
        void Adicionar(Produto p);  /*Add*/
        void Atualizar(Produto p);  /*Update*/
        void Remover(Produto p);    /*Delete*/
        IList<Produto> Produtos();   /*Select*/
    }
}
