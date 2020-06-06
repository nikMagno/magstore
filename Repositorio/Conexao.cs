using System.Data;
using MySql.Data.MySqlClient;

namespace AppWeb.Repositorio
{
  public class Conexao : IConexao
  {
    public IDbConnection AbrirConexao()
    {
      using(MySqlConnection conn = new MySqlConnection("Server=magstore.mysql.database.azure.com; Port=3306; Database=magstore; Uid=nikolasmagno@magstore; Pwd=Magno-10231993; SslMode=Preferred;")){
          
          conn.Open();
          return conn;
      }
    }
  }
}