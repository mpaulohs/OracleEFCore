
using Dapper;
using Dapper.Oracle;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using OracleEFCore.UI.Site.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace OracleEFCore.UI.Site.Repositories
{
    public interface IDepartamentoRepository
    {
        Task<(int,string)> AddAsync(Departamento dept);
        Task UpdateAsync(Departamento dept);
        Task DeleteAsync(Departamento dept);
        Task<IEnumerable<Departamento>> GetAllAsync();        
        Task<Departamento> FindAsync(int id);
    }

    public class DepartamentoRepository : IDepartamentoRepository
    {
        private readonly IConfiguration _config;

        public DepartamentoRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<Departamento>> GetAllAsyncoooo()
        {
            using (OracleConnection conexao = new OracleConnection(
                _config.GetConnectionString("BaseOracle")))
            {
                var result = await conexao.QueryAsync<Departamento>(
                    "select * from departamento");
                
                return result;
            }
        }

        public async Task<(int, string)> AddAsync(Departamento dept)
        {
            var result = 0;
            using (OracleConnection conexao = new OracleConnection(
                _config.GetConnectionString("BaseOracle")))
            {
                try
                {
                    string sql = "insert into departamento (Nome, Local) values(:Nome,:Local)";
                    OracleCommand command = new OracleCommand(sql, conexao);
                    command.Parameters.Add(new OracleParameter("Nome", dept.Nome));
                    command.Parameters.Add(new OracleParameter("Local", dept.Local));
                    await command.Connection.OpenAsync();
                    result = await command.ExecuteNonQueryAsync();
                    command.Connection.Close();

                }
                catch (Exception ex)
                {
                    return (result, ex.Message.ToString());
                }
               
            }

            return (result,"Success");
        }

        public string RunStoredProcedure(string parametervalue1, string parametervalue2)
        {
            var connection = new OracleConnection("mydatabaseconnectionstring");
            var parameters = new OracleDynamicParameters();
            parameters.Add("RETURN_VALUE", string.Empty, OracleMappingType.Varchar2, ParameterDirection.ReturnValue, 4000, true, 0, 0, string.Empty, DataRowVersion.Current);
            parameters.Add("PARAMETER1", parametervalue1, OracleMappingType.Varchar2, ParameterDirection.Input, 4000, true, 0, 0, String.Empty, DataRowVersion.Current);

            connection.Execute("Schema.Package.MyStoredProcedure", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<string>("RETURN_VALUE");
        }

        public void RunStoredProcedureWithArrayAsParameters(IEnumerable<long> idvalues)
        {
            var connection = new OracleConnection("mydatabaseconnectionstring");
            var parameters = new OracleDynamicParameters();
            var idArray = idvalues.ToArray();
           // parameters.ArrayBindCount = idArray.LongCount();

            parameters.Add("ArrayParameter", idArray, OracleMappingType.Int64, ParameterDirection.Input);
            connection.Execute("Schema.Package.MyStoredProcedure", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(Departamento dept)
        {
            using (OracleConnection conexao = new OracleConnection(
               _config.GetConnectionString("BaseOracle")))
            {
                string sql = string.Format(@"update departamento d set d.nome = '{0}', d.local= '{1}' where d.id = {2}",dept.Nome, dept.Local, dept.Id);
               // string sql = "update departamento d set d.nome = 'Campo', d.local= 'Campo' where d.id = 28";
                OracleCommand command = new OracleCommand(sql, conexao);                
                await command.Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                command.Connection.Close();
            }
        }

        public async Task<Departamento>  FindAsync(int id)
        {
            using (OracleConnection conexao = new OracleConnection(
              _config.GetConnectionString("BaseOracle")))
            {
                var dept_ = new Departamento();
                string sql = "select d.id, d.nome, d.local from departamento d where d.id = :id";
                OracleCommand command = new OracleCommand(sql, conexao);
                command.Parameters.Add(new OracleParameter("id", id));                
                await command.Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                command.CommandType = CommandType.Text;
                // Execute command, create OracleDataReader object
                OracleDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {                   
                    dept_.Id = reader.GetInt32(0);
                    dept_.Nome = reader.GetString(1);
                    dept_.Local = reader.GetString(2);
                }
                // Clean up
                reader.Dispose();
                command.Dispose();
                command.Dispose();
                command.Connection.Close();
                return dept_;
            }
        }

        public async Task<IEnumerable<Departamento>> GetAllAsync()
        {
            using (OracleConnection conexao = new OracleConnection(
              _config.GetConnectionString("BaseOracle")))
            {
                
                var list = new List<Departamento>();
                string sql = "select * from departamento order by id";
                OracleCommand command = new OracleCommand(sql, conexao);
               // command.Parameters.Add(new OracleParameter("id", id));
                await command.Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                command.CommandType = CommandType.Text;
                // Execute command, create OracleDataReader object
                OracleDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var dept_ = new Departamento();
                    dept_.Id = reader.GetInt32(0);
                    dept_.Nome =  await reader.IsDBNullAsync(1)? "" : reader.GetString(1);
                    dept_.Local = await reader.IsDBNullAsync(2) ? "" : reader.GetString(2);
                    list.Add(dept_);
                }
                // Clean up
                reader.Dispose();
                command.Dispose();
                command.Dispose();
                command.Connection.Close();
                return list;
            }
        }

        public async Task DeleteAsync(Departamento dept)
        {
            using (OracleConnection conexao = new OracleConnection(
               _config.GetConnectionString("BaseOracle")))
            {
                string sql = string.Format(@"DELETE FROM DEPARTAMENTO D WHERE D.ID = {0}", dept.Id);
                // string sql = "update departamento d set d.nome = 'Campo', d.local= 'Campo' where d.id = 28";
                OracleCommand command = new OracleCommand(sql, conexao);
                await command.Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                command.Connection.Close();
            }
        }

        public IDbConnection Connection
        {
            get
            {
                return new OracleConnection(_config.GetConnectionString("BaseOracle"));
            }
        }
    }
}
