using System.Data;
using System.Data.SqlClient;
using Cw4.Models;

namespace Cw4.Services;

public class WarehouseService2
{
    public async Task<int> AddProduct(ProductWarehouse productWarehouse)
    {
        using (var con = new SqlConnection("Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s20373;Integrated Security=True"))
        {
            var com = new SqlCommand("AddProductToWarehouse", con);
            var transaction = await con.BeginTransactionAsync();
            
            com.Transaction = (SqlTransaction) transaction;
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("IdProduct", productWarehouse.IdProduct);
            com.Parameters.AddWithValue("IdWarehouse", productWarehouse.IdWarehouse);
            com.Parameters.AddWithValue("Amount", productWarehouse.Amount);
            com.Parameters.AddWithValue("CreatedAt", productWarehouse.CreatedAt);

            await con.OpenAsync();
            await com.ExecuteNonQueryAsync();
            await transaction.CommitAsync();
            
            com.Parameters.Clear();
            com = new SqlCommand(
                $"SELECT TOP 1 IdProductWarehouse FROM Product_Warehouse ORDER BY IdProductWarehouse DESC",
                con
            );

            var dr = await com.ExecuteReaderAsync();
            await dr.ReadAsync();
            int id = int.Parse(dr["IdProductWarehouse"].ToString());
            await dr.CloseAsync();
            await con.CloseAsync();

            return id;
        }
    }
}