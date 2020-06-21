using System;
using System.Collections.Generic;
using System.Linq;
using MagStore.Models;
using MagStore.Connection;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MagStore.Controllers
{
	public class ProdutoController : Controller
	{

		List<ProdutoViewModel> listaProdutos;
		private readonly IConnection _conexao;

		//Construtor da Classe Controller
		public ProdutoController(IConnection conexao){
			
			_conexao = conexao;
		}

	[Authorize]
	[HttpGet("Produto/Listar")]
			public IActionResult Index()
			{
					using (var conn = _conexao.OpenConnection()){
							
							var querySQL = @"SELECT idproduto, Nome, Valor FROM produto;";
							listaProdutos = conn.Query<ProdutoViewModel>(querySQL).ToList();
							
					}

				return View(listaProdutos);
			}

			[HttpGet("Produto/Editar/{id}")]
        public IActionResult Edit(int id)
        {
            ProdutoViewModel produto;
            using (var conn = _conexao.OpenConnection())
            {
               var querySQL = $"SELECT idproduto, Nome, Valor from produto where idproduto = {id}";
               produto = conn.QueryFirst<ProdutoViewModel>(querySQL);
            }
                return View(produto);

        }

			[HttpPost("Produto/Salvar")]
			public IActionResult Post([FromForm] ProdutoViewModel model){

					string sql = "";
					if(model.IdProduto != 0){
							sql = @"update produto set
											nome = @nome,
											valor = @valor
											where idproduto = @idproduto";
					}else{
							sql = @"Insert into produto(nome, valor) values(@nome, @valor);";
					}

					using (var conn = _conexao.OpenConnection()){

						conn.Execute(sql,model);
					}

					return RedirectToAction("Index");
			}

			public IActionResult Delete(int id){

				using (var conn = _conexao.OpenConnection()){

					var querySQL = $"Delete from produto where idproduto = {id};";
					conn.Execute(querySQL);
				}
				return RedirectToAction("Index");
			}

			public IActionResult New(){

				return View();
			}

	}
}