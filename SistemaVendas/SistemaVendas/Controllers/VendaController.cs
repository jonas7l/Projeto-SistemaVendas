using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVendas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVendas.Controllers
{
    public class VendaController : Controller
    {
        private IHttpContextAccessor httpContext;

        //Recebendo o contexto HTTP via injeção de dependencia 
        public VendaController(IHttpContextAccessor HttpContextAccessor)
        {
            httpContext = HttpContextAccessor;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.ListaVendas = new VendaModel().ListagemVendas();
            return View();
        }


        [HttpGet]
        public IActionResult Registrar()
        {
            CarregarDados();
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(VendaModel venda)
        {
            //captura o id do vendedor logado no sistema
            venda.Vendedor_Id = httpContext.HttpContext.Session.GetString("IdUsuarioLogado");
            venda.Inserir();
            CarregarDados();
            return View();
        }

        private void CarregarDados() 
        {
            ViewBag.ListaClietes = new VendaModel().RetornarListaCliente();
            ViewBag.ListaVendedores = new VendaModel().RetornarListaVendedor();
            ViewBag.ListaProdutos = new VendaModel().RetornarListaProdutos();
        }
    }
}
