using System.Data.SqlClient;
using Cw4.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw4.Services;

public class WarehouseService
{
    public int AddProduct(ProductWarehouse productWarehouse, int idOrder)
    {
        double price = GetProductPrice(productWarehouse.IdProduct);
        int idProductWarehouse = -1;
        
        using (var con = new SqlConnection("Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s20373;Integrated Security=True"))
        {
            var com = new SqlCommand(
                $"INSERT INTO Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) VALUES(@param1, @param2, @param3, @param4, @param5, @param6)",
                con
            );
            
            com.Parameters.AddWithValue("param1", productWarehouse.IdWarehouse);
            com.Parameters.AddWithValue("param2", productWarehouse.IdProduct);
            com.Parameters.AddWithValue("param3", idOrder);
            com.Parameters.AddWithValue("param4", productWarehouse.Amount);
            com.Parameters.AddWithValue("param5", price);
            com.Parameters.AddWithValue("param6", productWarehouse.CreatedAt);
            
            con.Open();
            com.ExecuteReader();
            con.Close();
            
            com.Parameters.Clear();
            com = new SqlCommand(
                $"SELECT TOP 1 IdProductWarehouse FROM Product_Warehouse ORDER BY IdProductWarehouse DESC",
                con
            );

            con.Open();
            var dr = com.ExecuteReader();
            while (dr.Read())
            {
                idProductWarehouse = int.Parse(dr["IdProductWarehouse"].ToString());
            }
            con.Close();
        }
        return idProductWarehouse;
    }

    private double GetProductPrice(int idProduct)
    {
        var price = -1.00;
        using (var con = new SqlConnection("Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s20373;Integrated Security=True"))
        {
            var com = new SqlCommand(
                $"SELECT Price FROM product WHERE IdProduct = @IdProduct",
                con
            );

            com.Parameters.AddWithValue("IdProduct", idProduct);
            con.Open();
            var dr = com.ExecuteReader();
            while (dr.Read())
            {
                price = double.Parse(dr["Price"].ToString());
            }
        }
        return price;
    }

    public bool CheckIfExists(int id, string columnName, string tableName)
    {
        var foundId = -1;
        
        using (var con = new SqlConnection("Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s20373;Integrated Security=True"))
        {
            var com = new SqlCommand(
                $"SELECT {columnName} FROM {tableName} WHERE {columnName} = @id",
                con
            );
            com.Parameters.AddWithValue("@id", id);
            
            con.Open();
            var dr = com.ExecuteReader();
            while (dr.Read())
            {
                foundId = int.Parse(dr[$"{columnName}"].ToString());
            }
            con.Close();
        }
        
        return foundId != -1;
    }

    public Order GetOrder(ProductWarehouse productWarehouse)
    {
        using (var con = new SqlConnection("Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s20373;Integrated Security=True"))
        {
            var com = new SqlCommand(
                $"SELECT IdOrder FROM \"order\" WHERE IdProduct = @idProduct AND Amount = @amount AND CreatedAt < @createdAt",
                con
            );

            com.Parameters.AddWithValue("idProduct", productWarehouse.IdProduct);
            com.Parameters.AddWithValue("amount", productWarehouse.Amount);
            com.Parameters.AddWithValue("createdAt", productWarehouse.CreatedAt);
            
            con.Open();
            var dr = com.ExecuteReader();
            while (dr.Read())
            {
                return new Order()
                {
                    IdOrder = int.Parse(dr["IdOrder"].ToString()),
                    FulfilledAt = DateTime.Parse(dr["FulfilledAt"].ToString())
                };
            }
            con.Close();
        }

        return null;
    }

    public Order UpdateOrder(int idOrder)
    {
        using (var con = new SqlConnection("Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s20373;Integrated Security=True"))
        {
            var com = new SqlCommand(
                $"UPDATE \"order\" SET FulfilledAt = @createdAt WHERE IdOrder = @idOrder",
                con
            );

            com.Parameters.AddWithValue("createdAt", DateTime.Now);
            com.Parameters.AddWithValue("idOrder", idOrder);

            con.Open();
            var dr = com.ExecuteReader();
            while (dr.Read())
            {
                return new Order()
                {
                    IdOrder = int.Parse(dr["IdOrder"].ToString()),
                    FulfilledAt = DateTime.Parse(dr["FulfilledAt"].ToString())
                };
            }
            con.Close();
        }

        return null;
    }

    public ProductWarehouse GetByOrderId(ProductWarehouse productWarehouse)
    {
        using (var con = new SqlConnection("Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s20373;Integrated Security=True"))
        {
            var com = new SqlCommand(
                $"SELECT * FROM ProductWarehouse WHERE IdOrder = @idOrder",
                con
            );

            com.Parameters.AddWithValue("idOrder", productWarehouse.IdOrder);
            
            con.Open();
            var dr = com.ExecuteReader();
            while (dr.Read())
            {
                return new ProductWarehouse()
                {
                    // IdOrder = 
                };
            }
            con.Close();
        }

        return null;
    }
}