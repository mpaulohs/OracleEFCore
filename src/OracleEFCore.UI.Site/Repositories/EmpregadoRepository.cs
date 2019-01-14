using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using OracleEFCore.UI.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OracleEFCore.UI.Site.Repositories
{
    public interface IEmpregadoRepository
    {
        void Add(Empregado empregado);
        Task<IEnumerable<Empregado>> GetAllAsync();
    }

    public class EmpregadoRepository : IEmpregadoRepository
    {
        private readonly IConfiguration _config;

        public EmpregadoRepository(IConfiguration config)
        {
            _config = config;
        }

        public void Add(Empregado empregado)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Empregado>> GetAllAsync()
        {
            using (OracleConnection conexao = new OracleConnection(
                _config.GetConnectionString("BaseOracle")))
            {
                var result = await conexao.QueryAsync<Empregado>(
                    "");

                return result;
            }
        }

        public OracleConnection Connection
        {
            get
            {
                return new OracleConnection(_config.GetConnectionString("BaseOracle"));
            }
        }
    }
}
