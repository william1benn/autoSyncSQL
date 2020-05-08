using System;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
using Flurl.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Establish Connection
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "address";
                builder.UserID = "se";             
                builder.Password ="password";      
                builder.InitialCatalog = "database";

                Console.Write("Connecting");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Query", connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            List<responseTypes> dataList = new List<responseTypes>();
                            
                            dataList.Add(new responseTypes { factura = reader["a"], total = reader["b"] });

                            if(dataList.Count > 0)
                            {
                                dataList.ForEach(i => apiRes(i));
                                Thread.Sleep(10000);
                            }
                            else
                            {
                                Console.WriteLine("Nothing to post");
                            }

                            dataList.Clear();

                            Console.WriteLine("Completed");


                        }
                    }

                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public async static void apiRes(object i)
        {
            try
            {
                var responseString = await "API_URL".PostJsonAsync(i);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
        }
    }

    public class responseTypes
    {
        public object total { get; set; }
        public object factura { get; set; }
    }
}