using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ShorterUrls.Controllers
{
    public class ShorterController : ApiController
    {
        // GET: Shorter
       [System.Web.Http.HttpGet]
        public HttpResponseMessage Indexa(string sCode)
        {

            var url= SearchDb(sCode);

            var response = Request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = new Uri(url);
            return response;
        }

        [System.Web.Http.HttpPost]
        public string Index(string url)
        {

            string data = getBetween(url, "http", "com");
            string datas = getBetween(url, "com", "");
            string ShortCode = RandomString(6);
            var a = insertDb(url, ShortCode);
            //return datas;
            //return "http"+data+"com/"+ ShortCode;
            return Request.RequestUri.GetLeftPart(UriPartial.Authority) + "/api/Shorter?sCode=" + ShortCode;
        }
        public int insertDb (string url,string shortCode)
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.ConnectionString = @"Data Source=.\SQLEXPRESS;
                          AttachDbFilename=C:\Users\DELI\source\repos\ShorterUrls\ShorterUrls\App_Data\Database1.mdf;
                          Integrated Security=True;
                          Connect Timeout=30;
                          User Instance=True";

            using (SqlConnection conn = new SqlConnection(csb.ConnectionString))
            {
                conn.Open();


                string query = "INSERT INTO dbo.Links (url, shortCode) " +
               "VALUES (@Uri, @SCode);";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                   
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@Uri", url);
                    cmd.Parameters.AddWithValue("@SCode", shortCode);

                    cmd.ExecuteNonQuery();   
                }
                conn.Close();
            }
            return 0;
        }
        public string SearchDb(string shortCode)
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.ConnectionString = @"Data Source=.\SQLEXPRESS;
                          AttachDbFilename=C:\Users\DELI\source\repos\ShorterUrls\ShorterUrls\App_Data\Database1.mdf;
                          Integrated Security=True;
                          Connect Timeout=30;
                          User Instance=True";
            string url;

            using (var conn = new SqlConnection(csb.ConnectionString))
            {
                //string sqlString = @"select top 1 url from dbo.Links where shortCode='" + shortCode + "';";
                string sqlString = @"select top 1 url from dbo.Links where shortCode=@SCode; ";
                using (var command = new SqlCommand(sqlString, conn))
                {
                    conn.Open();
                    command.Parameters.AddWithValue("@SCode", shortCode);
                    var result = command.ExecuteScalar();
                    url = result.ToString();
                }
            }
            return url;
        }
        private readonly Random _random = new Random();

        public string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);           
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; 

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
        public static string getBetween(string strSource, string strStart, string strEnd)
        {

            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                if (String.IsNullOrEmpty(strEnd))
                {                   
                    End = strSource.Length;
                }
                else
                 End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else if (String.IsNullOrEmpty(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.Length;
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }
    }
}