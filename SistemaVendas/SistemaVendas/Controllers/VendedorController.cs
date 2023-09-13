using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaVendas.Models;
using Microsoft.AspNetCore.Http;

namespace SistemaVendas.Controllers
{
    public class VendedorController : Controller
    {
       
        public IActionResult Index()
        {
            ViewBag.ListaVendedor = new VendedorModel().ListarTodosVendedores();
            return View();
        }

        [HttpGet]
        public IActionResult Cadastro(int? id)
        {
            if (id != null) 
            {
                //Carregar o registro do vendedor em uma viewBag
                ViewBag.Vendedor = new VendedorModel().RetornarVendedor(id);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(VendedorModel vendedor)
        {
            if (ModelState.IsValid) 
            {
                vendedor.Gravar();
                return RedirectToAction("Index");
            }
            return View();
        }        

        public IActionResult Excluir(int id)
        {
            ViewData["IdExcluir"] = id;           
            var vendedor = new VendedorModel().RetornarVendedor(id);

            return View(vendedor);
        }

        public IActionResult ExcluirVendedor(int id)
        {
            new VendedorModel().Excluir(id);
            return View();
        }
    }
}
