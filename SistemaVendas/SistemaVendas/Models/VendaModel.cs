using Newtonsoft.Json;
using SistemaVendas.Uteis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVendas.Models
{
    public class VendaModel
    {
        public string Id { get; set; }
        public string Data { get; set; }
        public string Cliente_Id { get; set; }
        public string Vendedor_Id { get; set; }
        public string Produto_Id { get; set; }
        public double Total { get; set; }
        public string ListaProdutos { get; set; }

        //Listagem de Vendas por periodo 
        public List<VendaModel> ListagemVendas(string dataInicio, string dataFim) 
        {
            return RetornarListagemVendas(dataInicio, dataFim);
        }
        //Listagem de Vendas Geral 
        public List<VendaModel> ListagemVendas()
        {
            return RetornarListagemVendas("1900/01/01", "2100/01/01");
        }

        private List<VendaModel> RetornarListagemVendas(string dataInicio, string dataFim)
        {
            List<VendaModel> lista = new List<VendaModel>();
            VendaModel item;
            DAL objDAL = new DAL();
            string sql =  " SELECT vdor.id, vnda.data, vnda.total, vdor.nome as vendedor, cli.nome as cliente " +
                          " FROM venda vnda " +
                          " INNER JOIN vendedor vdor ON vdor.id = vnda.vendedor_id " +
                          " INNER JOIN cliente cli ON cli.id = vnda.cliente_id " +
                         $" WHERE vnda.data >= '{dataInicio}' and vnda.data <= '{dataFim}' " +
                          " ORDER BY data, total ";
            DataTable dt = objDAL.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new VendaModel
                {
                    Id = dt.Rows[i]["id"].ToString(),
                    Data = DateTime.Parse(dt.Rows[i]["data"].ToString()).ToString("dd/MM/yyyy"),
                    Total = double.Parse(dt.Rows[i]["total"].ToString()),
                    Vendedor_Id = dt.Rows[i]["vendedor"].ToString(),
                    Cliente_Id = dt.Rows[i]["cliente"].ToString()
                };
                lista.Add(item);
            }
            return lista;
        }

        public List<ClienteModel> RetornarListaCliente() 
        {
            return new ClienteModel().ListarTodosClientes();
        }

        public List<VendedorModel> RetornarListaVendedor()
        {
            return new VendedorModel().ListarTodosVendedores();
        }

        public List<ProdutoModel> RetornarListaProdutos()
        {
            return new ProdutoModel().ListarTodosProdutos();
        }

        public void Inserir() 
        {
            DAL objDAL = new DAL();

            string dataVenda = DateTime.Now.ToString("yyyy/MM/dd");

            string sql = "insert into venda(data, total, vendedor_id, cliente_id)" +
                $"Values('{dataVenda}', {Total.ToString().Replace(",",".")}, {Vendedor_Id}, {Cliente_Id})";
            objDAL.ExecutarComandoSQL(sql);

            //Recuperar o id da venda:
            sql = $"select id from venda where data='{dataVenda}' and vendedor_id={Vendedor_Id} and cliente_id={Cliente_Id} order by id desc limit 1";
            DataTable dt = objDAL.RetDataTable(sql);
            string id_venda = dt.Rows[0]["id"].ToString();

            //Deserializar o JSON da lista de produtos selecionados e grava-los na tabela itens_venda:
            List<ItemVendaModel> lista_produtos = JsonConvert.DeserializeObject<List<ItemVendaModel>>(ListaProdutos);
            for (int i = 0; i < lista_produtos.Count; i++)
            {
                sql = "insert into itens_venda(venda_id, produto_id, qtde_produto, preco_produto)" +
                    $"values({id_venda}, {lista_produtos[i].CodigoProduto.ToString()}," +
                    $"{ lista_produtos[i].QtdeProduto.ToString()}," +
                    $"{lista_produtos[i].PrecoUnitario.ToString().Replace(",",".")})";

                objDAL.ExecutarComandoSQL(sql);

                //Validação para baixa de estoque:
                sql = " update produto " +
                     $" set quantidade_estoque = (quantidade_estoque - " + int.Parse(lista_produtos[i].QtdeProduto.ToString()) + ") " +
                     $" where id = {lista_produtos[i].CodigoProduto.ToString()}";

                objDAL.ExecutarComandoSQL(sql);
            }
           
        }
    }
}
