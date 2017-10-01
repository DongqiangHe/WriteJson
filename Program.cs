using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using static System.Console;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;

namespace WriteJson
{
    class Program
    {
        class Person
        {
            public string Name{get; set;}
            public int Age { get; set; }
            public long Id { get; set; }
            public DateTime Birth { get; set; }
            public Person(string name, int age, long id, DateTime dt)
            {
                this.Name = name;
                this.Age = age;
                this.Id = id;
                this.Birth = dt;
            }
            public Person() { }
        }
        
        static void Main(string[] args)
        {
            string connstr = ConfigurationManager.AppSettings.Get("connstr");
            //string connstr = "Data Source=(local);Initial Catalog=AutoLot;User ID=sa;Password=Lm5201314-=;";
            //string sql = "select * from Inventory where CarID=999";
            string sql2 = "select * from Inventory";
           
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                SqlCommand command = new SqlCommand(sql2, conn);
                conn.Open();
                DbDataReader reader = command.ExecuteReader();
                //while (reader.Read())
                //{

                //}
                
                //string str = JSONHelper.Rows2JsonString(reader);
                string str2 = JsonHelper.Rows2JsonString2(reader, true);
                //WriteLine(str);
                WriteLine(str2);

            }

            using (SqlConnection conn = new SqlConnection(connstr))
            {
                SqlDataAdapter myAdp = new SqlDataAdapter(sql2, conn);
                DataSet myDataSet = new DataSet();

                myAdp.Fill(myDataSet, "Inventory");
                WriteLine(JsonHelper.DataSet2Json2(myDataSet, true));
            }

            Person a = new Person("jack", 23, 343344244242449882, new DateTime(2000, 12, 23));
            Person b = new Person("alice", 21, 34334424449882, new DateTime(1990, 12, 23));
            Person c = new Person("david", 23, 343344244242442, new DateTime(1994, 12, 23));
            List<Person> list = new List<Person>();
            list.Add(a);
            list.Add(b);
            list.Add(c);
            string json = JsonHelper.Object2Json(a);
            string js = JsonHelper.List2Json<Person>(list);
            WriteLine(json);
            WriteLine(js);

            ReadKey();
        }
    }
}
