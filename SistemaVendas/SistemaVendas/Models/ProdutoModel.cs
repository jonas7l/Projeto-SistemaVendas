using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaVendas.Uteis;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace SistemaVendas.Models
{
    public class ProdutoModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Informe o Nome do Produto")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe a Descrição do Produto")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Informe o Preço Unitario do Produto")]
        public float? Preco_Unitario { get; set; }

        [Required(ErrorMessage = "Informe a Quantidade de Estoque do Produto")]
        public float? Quantidade_Estoque { get; set; }

        [Required(ErrorMessage = "Informe a Unidade Medida do Produto")]
        public string Unidade_Medida { get; set; }

        [Required(ErrorMessage = "Informe o link da foto do Produto")]
        public string Link_foto { get; set; }

        public string PrecoUnitarioToString() 
        {
            return Preco_Unitario.ToString().Replace(",", ".");
        }

        public List<ProdutoModel> ListarTodosProdutos() 
        {
            List<ProdutoModel> lista = new List<ProdutoModel>();
            ProdutoModel item;
            DAL objDAL = new DAL();
            string sql = "select id, nome, descricao, preco_unitario, quantidade_estoque, unidade_medida, link_foto from produto order by nome asc";
            DataTable dt = objDAL.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++) 
            {
                item = new ProdutoModel
                {
                    Id =  dt.Rows[i]["Id"].ToString(),
                    Nome = dt.Rows[i]["Nome"].ToString(),
                    Descricao = dt.Rows[i]["Descricao"].ToString(),                    
                    Preco_Unitario = Convert.ToSingle(dt.Rows[i]["Preco_Unitario"]),
                    Unidade_Medida = dt.Rows[i]["Unidade_Medida"].ToString(),                    
                    Quantidade_Estoque = Convert.ToSingle(dt.Rows[i]["Quantidade_Estoque"]),
                    Link_foto = dt.Rows[i]["Link_foto"].ToString()
                };
                lista.Add(item);
            }
            return lista;
        }

        public ProdutoModel RetornarProduto(int? id)
        {
            ProdutoModel item;
            DAL objDAL = new DAL();
            string sql = $"select id, nome, descricao, preco_unitario, quantidade_estoque, unidade_medida, link_foto from produto where id = '{id}' order by nome asc";
            DataTable dt = objDAL.RetDataTable(sql);

            
            

          item = new ProdutoModel
          {
              Id = dt.Rows[0]["Id"].ToString(),
              Nome = dt.Rows[0]["Nome"].ToString(),
              Descricao = dt.Rows[0]["Descricao"].ToString(),
              Preco_Unitario = Convert.ToSingle(dt.Rows[0]["Preco_Unitario"]),
              Unidade_Medida = dt.Rows[0]["Unidade_Medida"].ToString(),
              Quantidade_Estoque = Convert.ToSingle(dt.Rows[0]["Quantidade_Estoque"]),
              Link_foto = dt.Rows[0]["Link_foto"].ToString()
          };
               
            
          return item;
        }

        //Metodo de insert ou update 
        public void Gravar() 
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            if (Id != null)
            {
              sql = $"update produto set " +
                    $"nome='{Nome}', " +
                    $"descricao='{Descricao}', " +
                    $"preco_unitario= {PrecoUnitarioToString()}, " +
                    $"quantidade_estoque='{Quantidade_Estoque}', " +
                    $"unidade_medida='{Unidade_Medida}', " +
                    $"link_foto='{Link_foto}' " +
                    $"where id = {Id}";

            }
            else 
            {
              sql = "insert into produto (nome, descricao, preco_unitario, quantidade_estoque, unidade_medida, link_foto)" +
                    $" values ('{Nome}', " +
                    $"'{Descricao}', " +
                    $" {PrecoUnitarioToString()}, " +
                    $"'{Quantidade_Estoque}', " +
                    $"'{Unidade_Medida}', " +
                    $"'{Link_foto}')";
            }
            
            objDAL.ExecutarComandoSQL(sql);
        }
        
        //Metodo Excluir
        public void Excluir(int id) 
        {
            DAL objDAL = new DAL();           

            string sql = $"delete from produto where id= {id}";
            objDAL.ExecutarComandoSQL(sql);
        }
    }
}
