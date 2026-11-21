using MySql.Data.MySqlClient;

namespace MVC_Contactos.Models
{
    public class ClienteDatos
    {
        private readonly string conexion;

        public ClienteDatos(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("MiConexionMySQL")
                ?? throw new InvalidOperationException("Falta la cadena de conexión MiConexionMySQL.");
        }

        private const string Columnas =
            "IdCliente, StrNombre, NumDocumento, StrDireccion, StrTelefono, StrEmail, DtmFechaModifica, StrUsuarioModifica";

        private static Cliente Mapear(MySqlDataReader reader)
        {
            return new Cliente
            {
                IdCliente = reader["IdCliente"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdCliente"]),
                StrNombre = reader["StrNombre"] == DBNull.Value ? "" : reader["StrNombre"].ToString()!,
                NumDocumento = reader["NumDocumento"] == DBNull.Value ? null : Convert.ToInt64(reader["NumDocumento"]),
                StrDireccion = reader["StrDireccion"] == DBNull.Value ? "" : reader["StrDireccion"].ToString(),
                StrTelefono = reader["StrTelefono"] == DBNull.Value ? "" : reader["StrTelefono"].ToString(),
                StrEmail = reader["StrEmail"] == DBNull.Value ? "" : reader["StrEmail"].ToString(),
                DtmFechaModifica = reader["DtmFechaModifica"] == DBNull.Value ? null : Convert.ToDateTime(reader["DtmFechaModifica"]),
                StrUsuarioModifica = reader["StrUsuarioModifica"] == DBNull.Value ? "" : reader["StrUsuarioModifica"].ToString()
            };
        }

        public List<Cliente> ListarClientes()
        {
            List<Cliente> lista = new List<Cliente>();
            using (MySqlConnection con = new MySqlConnection(conexion))
            {
                MySqlCommand cmd = new MySqlCommand($"SELECT {Columnas} FROM TBLCLIENTES", con);
                con.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(Mapear(reader));
                    }
                }
            }
            return lista;
        }

        public Cliente? ObtenerCliente(int id)
        {
            Cliente? cliente = null;
            using (MySqlConnection con = new MySqlConnection(conexion))
            {
                MySqlCommand cmd = new MySqlCommand(
                    $"SELECT {Columnas} FROM TBLCLIENTES WHERE IdCliente = @IdCliente", con);
                cmd.Parameters.AddWithValue("@IdCliente", id);
                con.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cliente = Mapear(reader);
                    }
                }
            }
            return cliente;
        }

        public bool ExisteDocumento(long documento, int? excluirId = null)
        {
            using (MySqlConnection con = new MySqlConnection(conexion))
            {
                string sql = "SELECT COUNT(*) FROM TBLCLIENTES WHERE NumDocumento = @Doc";
                if (excluirId.HasValue) sql += " AND IdCliente <> @Excluir";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Doc", documento);
                if (excluirId.HasValue) cmd.Parameters.AddWithValue("@Excluir", excluirId.Value);
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public void InsertarCliente(Cliente cliente)
        {
            using (MySqlConnection con = new MySqlConnection(conexion))
            {
                string sql = @"INSERT INTO TBLCLIENTES
                    (StrNombre, NumDocumento, StrDireccion, StrTelefono, StrEmail, DtmFechaModifica, StrUsuarioModifica)
                    VALUES
                    (@StrNombre, @NumDocumento, @StrDireccion, @StrTelefono, @StrEmail, @DtmFechaModifica, @StrUsuarioModifica)";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@StrNombre", cliente.StrNombre ?? string.Empty);
                cmd.Parameters.AddWithValue("@NumDocumento",
                    cliente.NumDocumento.HasValue ? (object)cliente.NumDocumento.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@StrDireccion", cliente.StrDireccion ?? string.Empty);
                cmd.Parameters.AddWithValue("@StrTelefono", cliente.StrTelefono ?? string.Empty);
                cmd.Parameters.AddWithValue("@StrEmail", cliente.StrEmail ?? string.Empty);
                cmd.Parameters.AddWithValue("@DtmFechaModifica", cliente.DtmFechaModifica ?? DateTime.Now);
                cmd.Parameters.AddWithValue("@StrUsuarioModifica", cliente.StrUsuarioModifica ?? "Sistema");
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ActualizarCliente(Cliente cliente)
        {
            using (MySqlConnection con = new MySqlConnection(conexion))
            {
                string sql = @"UPDATE TBLCLIENTES SET
                    StrNombre = @StrNombre,
                    NumDocumento = @NumDocumento,
                    StrDireccion = @StrDireccion,
                    StrTelefono = @StrTelefono,
                    StrEmail = @StrEmail,
                    DtmFechaModifica = @DtmFechaModifica,
                    StrUsuarioModifica = @StrUsuarioModifica
                    WHERE IdCliente = @IdCliente";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@StrNombre", cliente.StrNombre ?? string.Empty);
                cmd.Parameters.AddWithValue("@NumDocumento",
                    cliente.NumDocumento.HasValue ? (object)cliente.NumDocumento.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@StrDireccion", cliente.StrDireccion ?? string.Empty);
                cmd.Parameters.AddWithValue("@StrTelefono", cliente.StrTelefono ?? string.Empty);
                cmd.Parameters.AddWithValue("@StrEmail", cliente.StrEmail ?? string.Empty);
                cmd.Parameters.AddWithValue("@DtmFechaModifica", cliente.DtmFechaModifica ?? DateTime.Now);
                cmd.Parameters.AddWithValue("@StrUsuarioModifica", cliente.StrUsuarioModifica ?? "Sistema");
                cmd.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarCliente(int id)
        {
            using (MySqlConnection con = new MySqlConnection(conexion))
            {
                MySqlCommand cmd = new MySqlCommand("DELETE FROM TBLCLIENTES WHERE IdCliente = @IdCliente", con);
                cmd.Parameters.AddWithValue("@IdCliente", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}