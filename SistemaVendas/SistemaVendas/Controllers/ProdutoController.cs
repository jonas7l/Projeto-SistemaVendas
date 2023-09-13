using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaVendas.Models;

namespace SistemaVendas.Controllers
{
    public class ProdutoController : Controller
    {        

        public IActionResult Index()
        {
            ViewBag.ListaProdutos = new ProdutoModel().ListarTodosProdutos();
            return View();
        }

        [HttpGet]
        public IActionResult Cadastro(int? id)
        {
            if (id != null) 
            {
                //Carregar o registro do cliente em uma viewBag
                ViewBag.Produto = new ProdutoModel().RetornarProduto(id);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(ProdutoModel produto)
        {
            if (ModelState.IsValid) 
            {
                produto.Gravar();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Editar(int? id)
        {
            if (id != null)
            {
                //Carregar o registro do cliente em uma viewBag
                ViewBag.Produto = new ProdutoModel().RetornarProduto(id);
            }
            return View();
        }

        public IActionResult Excluir(int id)
        {
            ViewData["IdExcluir"] = id;           
            var produto = new ProdutoModel().RetornarProduto(id);

            return View(produto);
        }

        public IActionResult ExcluirProduto(int id)
        {
            new ProdutoModel().Excluir(id);
            return View();
        }
    }
}
