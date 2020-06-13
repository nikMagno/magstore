using System;
using System.Collections.Generic;
using System.Linq;
using AppWeb.Models;
using AppWeb.Repositorio;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppWeb.Controllers
{
    public class FornecedorController : Controller
    {
			List<FornecedorViewModel> listaFornecedor;

      private readonly IConexao _conexao;

			//Construtor da Classe Controller
      public FornecedorController(IConexao conexao){
        _conexao = conexao;
      }

			[Authorize]
			[HttpGet("Fornecedor/Listar")]
        public IActionResult Index()
        {
            using (var conn = _conexao.AbrirConexao()){
              
              var querySQL = @"SELECT idFornecedor, Nome, Endereco FROM fornecedor;";
              listaFornecedor = conn.Query<FornecedorViewModel>(querySQL).ToList();
            }

            return View(listaFornecedor);
        }


			[HttpGet("Fornecedor/Editar/{id}")]
        public IActionResult Edit(int id)
        {
            FornecedorViewModel fornecedor;
            using (var conn = _conexao.AbrirConexao())
            {
               var querySQL = $"SELECT idfornecedor, nome, endereco from fornecedor where idfornecedor = {id}";
               fornecedor = conn.QueryFirst<FornecedorViewModel>(querySQL);
            }
                return View(fornecedor);

        }

      [HttpPost("Fornecedor/Salvar")]
			public IActionResult Post([FromForm] FornecedorViewModel model){

					string sql = "";
					if(model.IdFornecedor != 0){
							sql = @"update fornecedor set
											nome = @nome,
											endereco = @endereco
											where idFornecedor = @idFornecedor";
					}else{
							sql = @"Insert into fornecedor(nome, endereco) values(@nome, @endereco);";
					}

					using (var conn = _conexao.AbrirConexao()){

						conn.Execute(sql,model);
					}

					return RedirectToAction("Index");
			}

			public IActionResult Delete(int id){

				using (var conn = _conexao.AbrirConexao()){

					var querySQL = $"Delete from fornecedor where idFornecedor = {id};";
					conn.Execute(querySQL);
				}
				return RedirectToAction("Index");
			}

			public IActionResult New(){

				return View();
			}

    }
}