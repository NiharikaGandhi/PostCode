using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using PostCodeAPI.Message;

namespace PostCodeAPI.Data
{
    public interface IPostCodeRepository
    {
       Task<List<PostCodeGetResponse>> GetSuburb(int postCode);
        Task<bool> UpdateDetails(int id, int postCode, string suburb);
    }

    public class PostCodeRepository : IPostCodeRepository
    {
        private readonly string _connectionString;

        public PostCodeRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionString = connectionStringProvider.ConnectionString;
        }

        public async Task<List<PostCodeGetResponse>> GetSuburb(int postCode)
        {
            try
            {
                const string sql = @"Select Id, postcode, suburb from dbo.Auspostcodes where postcode=@postcode";
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var response = await connection.QueryAsync<PostCodeGetResponse>(sql, new {postcode = postCode});
                    return response.ToList();
                }
            }
            catch (Exception ex)
            {
               
            }
            return null;
        }

        public async Task<bool> UpdateDetails(int id, int postCode, string suburb)
        {
            try
            {
                var sql = BuildSql(postCode, suburb);

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    await connection.QueryAsync<bool>(sql.ToString(), new { postcode = postCode, suburb = suburb, id= id });
                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        private static StringBuilder BuildSql(int postCode, string suburb)
        {
            var sql = new StringBuilder(@" update dbo.Auspostcodes SET ");
            if (postCode > 0 && !string.IsNullOrEmpty(suburb))
            {
                sql.Append(@" postcode = @postcode, suburb = @suburb ");
            }
            else if (postCode > 0)
            {
                sql.Append(@" postcode = @postcode ");
            }
            else if (!string.IsNullOrEmpty(suburb))
            {
                sql.Append(@" suburb = @suburb ");
            }

            sql.Append(@"where id =@id");
            return sql;
        }
    }
}