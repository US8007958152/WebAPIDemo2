using System.Data.SqlClient;
using WebAPIDemo2.Entities;
using WebAPIDemo2.Interfaces;

namespace WebAPIDemo2.Services
{
    public class EmployeeService : IEmployee
    {
        private readonly IConfiguration configuration;
        private readonly string ConnectionString;

        public EmployeeService(IConfiguration configuration)
        {
            this.configuration = configuration;
            ConnectionString = this.configuration.GetConnectionString("Default");
        }
        public int Create(Employee employee)
        {
            string sqlQuery = "INSERT INTO Employee(Name,EmailId) VALUES('" + employee.Name + "','" + employee.EmailId + "')";

            using (SqlConnection conn=new SqlConnection(ConnectionString))
            {
                if(conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                using(SqlCommand cmd=new SqlCommand(sqlQuery, conn))
                {
                   int rowCount= cmd.ExecuteNonQuery();
                    return rowCount;
                }
            }
        }

        public int Delete(int empId)
        {
            string sqlQuery = "DELETE From Employee WHERE Id =" + empId;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                   int rowCout= cmd.ExecuteNonQuery();// DML - Data Manipulation Langugae => Create/Delete/Update
                    return rowCout;
                }
            }
        }

        public Employee Detail(int empId)
        {
            string sqlQuery = "SELECT * From Employee WHERE Id ="+ empId;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    using (SqlDataReader sda =cmd.ExecuteReader())
                    {
                        Employee emp = null;
                        while(sda.Read())
                        {
                            emp = new Employee();
                            emp.Id = sda.GetInt32(0);
                            emp.Name = sda.GetString(1);
                            emp.EmailId=sda.GetString(2);
                        }
                        return emp;
                    }   
                }
            }
        }

        public List<Employee> GetAll()
        {
            throw new NotImplementedException();
        }

        public int Update(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
