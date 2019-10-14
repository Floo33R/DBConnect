using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;   //SQL
using System.Windows;           //MessageBox

namespace StockControlSystem_V1._0
{
    class DBConnect
    {

        private MySqlConnection connection;
        private string datasource;
        private string username;
        private string password;
        private string database;


        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            datasource = "127.0.0.1";
            username = "root";
            password = "";
            database = "test";

            string connectionString = "datasource=" + datasource + ";" + "username=" + username + ";" + "password=" + password + ";" + "database=" + database + ";";

            connection = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Test Connection to the Server
        /// </summary>
        private void DBConnection()
        {
            string ConnectionString = "datasource = localhost; username = root; password = ; database = test ";

            MySqlConnection DBConnect = new MySqlConnection(ConnectionString);

            try
            {
                DBConnect.Open();
                System.Windows.Forms.MessageBox.Show("Sucessfully connected!");
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Opens a MySQL-Connection
        /// </summary>
        /// <returns></returns>
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //Most common error numbers
                //0: cannot connect to the server
                //1045: invalid username and/or password

                switch (ex.Number)
                {
                    case 0:
                        System.Windows.Forms.MessageBox.Show("Cannot connect to server. Contact the administrator");
                        break;

                    case 1045:
                        System.Windows.Forms.MessageBox.Show("Invalid username/password! Please try again");
                        break;
                }

                if (ex.Number != 0 || ex.Number != 1045)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }

                return false;
            }
        }

        /// <summary>
        /// Close a MySQL-Connection
        /// </summary>
        /// <returns></returns>
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Inserts a Value into a MySQL-Database table
        /// </summary>
        /// <param name="location">in which column the value will be inserted</param>
        /// <param name="value">which value will be inserted</param>
        public void Insert(string tableName, string column, string value)
        {
            string query = "INSERT INTO " + tableName + " (" + column + ") VALUE ('" + value + "')";


            if (this.OpenConnection())
            {
                //create command annd assign the query
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //execute cmd
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        /// <summary>
        /// Updates a statement in a MySQL-Database table
        /// </summary>
        /// <param name="tableName">name of the table which should be updated</param>
        /// <param name="column">column which should be searched</param>
        /// <param name="searchColumn">column where the serachValue should be</param>
        /// <param name="value">new updated value</param>
        /// <param name="searchValue">value which you want to search</param>
        public void Update(string tableName, string searchColumn, string searchValue, string column, string value)
        {
            string query = "UPDATE " + tableName + " SET " + column + " = '" + value + "' WHERE " + searchColumn + " = '" + searchValue + "'";


            if (this.OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand();

                cmd.CommandText = query;

                cmd.Connection = connection;

                cmd.ExecuteNonQuery();

                this.CloseConnection();

            }

        }

        /// <summary>
        /// Deletes a statement in a MySQL-Database table
        /// </summary>
        /// <param name="tableName">name of the table</param>
        /// <param name="searchColumn">in which column should be searched</param>
        /// <param name="searchValue">searching value</param>
        public void Delete(string tableName, string searchColumn, string searchValue)
        {
            string query = "DELETE FROM " + tableName + " WHERE " + searchColumn + " = '" + searchValue + "'";

            if (this.OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand();

                cmd.CommandText = query;

                cmd.Connection = connection;

                cmd.ExecuteNonQuery();

                this.CloseConnection();
            }
        }

        /// <summary>
        /// Used to select the User in a database
        /// </summary>
        /// <param name="tableName">name of the table</param>
        /// <param name="username">username to select</param>
        /// <param name="password">password to select</param>
        public Int16 SelectLogin(string tableName, string username, string password)
        {
            Int16 retVal = 999;

            string query = "SELECT COUNT(*) FROM users WHERE username='" + username + "' AND password='" + password + "'";

            if (this.OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand();

                cmd.CommandText = query;

                cmd.Connection = connection;


                MySqlDataAdapter sda = new MySqlDataAdapter("SELECT COUNT(*) FROM users WHERE username='" + username + "' AND password='" + password + "'", connection);

                /* in above line the program is selecting the whole data from table and the matching it with the user name and password provided by user. */

                System.Data.DataTable dt = new System.Data.DataTable(); //this is creating a virtual table  

                sda.Fill(dt);

                if (dt.Rows[0][0].ToString() == "1")
                {
                    //System.Windows.Forms.MessageBox.Show("Successfull login!");

                    retVal = 1;
                }
                else
                {
                    //System.Windows.Forms.MessageBox.Show("Invalid username or password");

                    retVal = 0;
                }

                this.CloseConnection();

                return retVal;
            }

            return retVal;
        }
    }
}

