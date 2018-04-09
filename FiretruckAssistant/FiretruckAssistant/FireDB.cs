using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiretruckAssistant
{
    class FireDB
    {
        const string CONNECT_STRING = @"Server=.\SQLEXPRESS;Database=FireDB;Trusted_Connection=True;";
        SqlConnection conn;

        static FireDB fireDB;

        private FireDB()
        {
            conn = new SqlConnection(CONNECT_STRING);
            conn.Open();
        }

        public static FireDB GetInstance()
        {
            if (fireDB == null)
                fireDB = new FireDB();
            return fireDB;
        }


        public void Add(Fire fire)
        {
            string cmdString = "INSERT INTO Fire " +
                                    "(Location, StatusID, FirefighterName)" +
                                    "VALUES" +
                                    "(@LOCATION, @STATUSID, @FIREFIGHTERNAME)";

            SqlCommand cmd = new SqlCommand(cmdString, conn);
            cmd.Parameters.AddWithValue("@LOCATION", fire.Location);
            cmd.Parameters.AddWithValue("@STATUSID", fire.StatusID);
            cmd.Parameters.AddWithValue("@FIREFIGHTERNAME", fire.Firefighter);

            cmd.ExecuteNonQuery();

            int id = getLastFireID();

            cmdString = "INSERT INTO FireRoutes " +
                                    "(FireID, Route)" +
                                    "VALUES" +
                                    "(@FIREID, @ROUTE)";

            cmd = new SqlCommand(cmdString, conn);
            cmd.Parameters.AddWithValue("@FIREID", id);

            foreach (string route in fire.RouteList)
            {
                cmd.Parameters.AddWithValue("@ROUTE", route);
                cmd.ExecuteNonQuery();
                cmd.Parameters.RemoveAt("@ROUTE");
            }
        }

        //public void Delete(Employee employee)
        //{
        //    string cmdString = "DELETE Employee " +
        //                       "WHERE ID = @ID";

        //    SqlCommand cmd = new SqlCommand(cmdString, conn);
        //    cmd.Parameters.AddWithValue("@ID", employee.ID);

        //    cmd.ExecuteNonQuery();
        //}

        //public void Update(Employee employee)
        //{
        //    string cmdString = "UPDATE Employee SET FirstName = @FIRSTNAME, " +
        //                                            "LastName = @LASTNAME, " +
        //                                            "HourlyPayRate = @HOURLYPAYRATE, " +
        //                                            "Age = @AGE, " +
        //                                            "ImagePath = @IMAGEPATH " +
        //                        "WHERE ID = @ID";

        //    SqlCommand cmd = new SqlCommand(cmdString, conn);
        //    cmd.Parameters.AddWithValue("@ID", employee.ID);
        //    cmd.Parameters.AddWithValue("@FIRSTNAME", employee.FirstName);
        //    cmd.Parameters.AddWithValue("@LASTNAME", employee.LastName);
        //    cmd.Parameters.AddWithValue("@POSITION", employee.Position);
        //    cmd.Parameters.AddWithValue("@HOURLYPAYRATE", employee.HourlyPayRate);
        //    cmd.Parameters.AddWithValue("@AGE", employee.Age);
        //    cmd.Parameters.AddWithValue("@IMAGEPATH", employee.ImagePath);

        //    cmd.ExecuteNonQuery();
        //}

        public List<Fire> Get()
        {
            List<Fire> fireList = new List<Fire>();
            string cmdString = "SELECT ID, Location, StatusID, FirefighterName " +
                               "FROM Fire " +
                               "ORDER BY ID DESC";
            SqlCommand cmd = new SqlCommand(cmdString, conn);
            SqlDataReader rd = cmd.ExecuteReader();

            while (rd.Read())
                fireList.Add(new Fire()
                {
                    ID = rd.GetInt32(rd.GetOrdinal("ID")),
                    Location = rd.GetInt32(rd.GetOrdinal("Location")),
                    StatusID = rd.GetInt32(rd.GetOrdinal("StatusID")),
                    Firefighter = rd.GetString(rd.GetOrdinal("FirefighterName"))
                });
            rd.Close();

            cmdString = "SELECT Route " +
                        "FROM FireRoutes " +
                        "WHERE FireID = @ID";

            cmd = new SqlCommand(cmdString, conn);

            foreach (Fire fire in fireList)
            {
                cmd.Parameters.AddWithValue("@ID", fire.ID);
                rd = cmd.ExecuteReader();

                while (rd.Read())
                    fire.RouteList.Add(rd.GetString(rd.GetOrdinal("Route")));
                rd.Close();

                fire.NumberOfRoutes = fire.RouteList.Count;
                cmd.Parameters.RemoveAt("@ID");
            }

            return fireList;
        }

        private int getLastFireID()
        {
            string cmdString = "SELECT Max(ID) " +
                               "FROM Fire";

            SqlCommand cmd = new SqlCommand(cmdString, conn);
            SqlDataReader rd = cmd.ExecuteReader();

            rd.Read();
            int ret = rd.GetInt32(0);
            rd.Close();

            return ret;
        }
    }
}
