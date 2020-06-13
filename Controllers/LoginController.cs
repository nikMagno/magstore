using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AppWeb.Models;
using AppWeb.Repositorio;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AppWeb.Controllers
{
    public class LoginController : Controller
    {
      private readonly IConexao _conexao; //underline indica uma variável global.
      public LoginController(IConexao conexao)
      {
          _conexao = conexao;
      }

      [HttpGet]
      public IActionResult UsuarioLogin(){

        return View();
      }

      [HttpPost]
      public async Task<IActionResult> UsuarioLogin([FromForm]UsuarioViewModel model){

        UsuarioViewModel usuario = null;

        //
        using(var conn = _conexao.AbrirConexao()){
          string queryQuery = $"select * from usuario where login = '{model.Login}' and senha = '{model.Senha}'; ";
          usuario = conn.QueryFirst<UsuarioViewModel>(queryQuery);
        }

        if(usuario != null){

          var userClaims = new List<Claim>(){

            //Definir o cookie
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email)
          };

          var minhaIdentidade = new ClaimsIdentity(userClaims, "Usuario");
          var userPrincipal = new ClaimsPrincipal(new[]{minhaIdentidade});

          await HttpContext.SignInAsync(userPrincipal);
          return RedirectToAction("Index", "Home");
        }

        //Variável que acessa da Action para View, mandando uma mensagem caso não acessar nada.
        ViewBag.Mensagem = "Credenciais inválidas!";

        return View(model);

      }

      [HttpGet]
      public async Task<IActionResult> Logout(){
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
      }

    }
}