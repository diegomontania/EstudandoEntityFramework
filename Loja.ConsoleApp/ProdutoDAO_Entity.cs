using System;
using System.Collections.Generic;
using System.Linq;

namespace Loja.ConsoleApp
{
    class ProdutoDAO_Entity : IProdutoDAO, IDisposable
    {
        private LojaContext contexto;

        public ProdutoDAO_Entity()
        {
            //instancia o objeto de contexto
            this.contexto = new LojaContext();
        }

        public void Adicionar(Produto p)
        {
            contexto.Produtos.Add(p);   /*faz o insert into*/
            contexto.SaveChanges();     /*salva as alterações*/
        }
        public void Remover(Produto p)
        {
            contexto.Produtos.Remove(p);    /*faz o delete*/
            contexto.SaveChanges();         /*salva as alterações*/
        }
        public void Atualizar(Produto p)
        {
            contexto.Produtos.Update(p);    /*faz o update*/
            contexto.SaveChanges();         /*salva as alterações*/
        }
        public IList<Produto> Produtos()
        {
            return contexto.Produtos.ToList();  /*faz o select e retorna*/
        }
        public void Dispose()
        {
            contexto.Dispose();
        }
    }
}
