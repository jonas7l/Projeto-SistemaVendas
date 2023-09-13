using Microsoft.AspNetCore.Mvc;
using SistemaVendas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVendas.Controllers
{
    public class RelatorioController : Controller
    {
        public IActionResult Index() 
        {
            return View();
        }
        [HttpGet]
        public IActionResult Vendas()
        {
            ReletorioModel relatorio = new ReletorioModel();
            relatorio.DataInicio = DateTime.Now.AddDays(-7);
            relatorio.DataFim = DateTime.Now;
            string dateInicio = relatorio.DataInicio.ToString("yyyy/MM/dd");
            string dateFim = relatorio.DataFim.ToString("yyyy/MM/dd");
            ViewBag.ListarVendas = new VendaModel().ListagemVendas(dateInicio, dateFim);
            return View(relatorio);
        }
        [HttpPost]
        public IActionResult Vendas(ReletorioModel relatorio)
        {
            if (relatorio.DataInicio == null) 
            {
                ViewBag.ListarVendas = new VendaModel().ListagemVendas();
            }
            else 
            {
                string dateInicio = relatorio.DataInicio.ToString("yyyy/MM/dd");
                string dateFim = relatorio.DataFim.ToString("yyyy/MM/dd");
                ViewBag.ListarVendas = new VendaModel().ListagemVendas(dateInicio, dateFim);
            }
            
            return View(relatorio);
        }

        public IActionResult Grafico()
        {
            List<GraficoProdutos> lista = new GraficoProdutos().RetornarGrafico();
            string valores = "";
            string labels = "";
            string cores = "";

            var random = new Random();

            //Percorrendo a lista de itens para compor o grafico:
            for (int i = 0; i < lista.Count; i++)
            {
                valores += lista[i].QtdeVendido.ToString() + ",";
                labels += "'" + lista[i].DescricaoProduto.ToString() + "',";

                //escolher aleatoriamente as cores para compor as partes do grafico:
                cores += "'" + string.Format("#{0:X6}", random.Next(0x1000000)) + "',";
            }

            ViewBag.Valores = valores;
            ViewBag.Labels = labels;
            ViewBag.Cores = cores;

            return View();
        }

        public IActionResult Comissao()
        {
            return View();
        }
    }
}
