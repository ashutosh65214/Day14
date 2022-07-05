using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Day14
{
    public partial class FormStudentInformation : Form
    {
        public FormStudentInformation()
        {
            InitializeComponent();
        }
        private SqlConnection obj = null;
        private SqlCommand cmd= null;   
        private SqlDataReader reader = null;    
        //When the application start it show the 1st value of a row
        private void FormStudentInformation_Load(object sender, EventArgs e)
        {
            obj = new SqlConnection(ConfigurationManager.ConnectionStrings["STdConn"].ConnectionString);
            cmd = new SqlCommand("select Stud_Code, Stud_Name, Dept_Code, Stud_Dob, Address from Student_master",obj);

            obj.Open();

            reader=cmd.ExecuteReader();
            reader.Read();

            TxtStudentCode.Text = reader["Stud_Code"].ToString();
            TxtStudentName.Text = reader["Stud_Name"].ToString();
            TxtDepartmentCode.Text = reader["Dept_Code"].ToString();
            TxtDateOfBirth.Text = reader["Stud_Dob"].ToString();
            TxtAddress.Text = reader["Address"].ToString();


            reader.Close();
            cmd.Dispose();
            obj.Close();


        }
        public void ClearTxt()
        {
            TxtStudentCode.Clear();
            TxtStudentName.Clear();
            TxtDepartmentCode.Clear();
            TxtDateOfBirth.Clear();
            TxtAddress.Clear();
         

        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            this.ClearTxt();

        }
        //Searching by Student code
        private void Btnsearch_Click(object sender, EventArgs e)
        {
            using(obj=new SqlConnection(ConfigurationManager.ConnectionStrings["STdConn"].ConnectionString))
            {
                using (cmd = new SqlCommand("select Stud_Code, Stud_Name, Dept_Code, Stud_Dob, Address from Student_master  where Stud_Code=@StdCode", obj))
                {
                    cmd.Parameters.AddWithValue("@StdCode", TxtStudentCode.Text);
                    if (obj.State == ConnectionState.Closed)
                    {
                        obj.Open();
                    }
                    using (reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            TxtStudentName.Text = reader["Stud_Name"].ToString();
                            TxtDepartmentCode.Text = reader["Dept_Code"].ToString();
                            TxtDateOfBirth.Text = reader["Stud_Dob"].ToString();
                            TxtAddress.Text = reader["Address"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No Record Found");
                            this.ClearTxt();
                        }
                    }
                }
            }
        }
        //Add a new value in that table
        private void BtnAddNew_Click(object sender, EventArgs e)
        {
            using (obj = new SqlConnection(ConfigurationManager.ConnectionStrings["STdConn"].ConnectionString))
            {
                using (cmd = new SqlCommand("usp_AddNew",obj))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StdCode",TxtStudentCode.Text);
                    cmd.Parameters.AddWithValue("@StdName", TxtStudentName.Text);
                    cmd.Parameters.AddWithValue("@DptCode", TxtDepartmentCode.Text);
                    cmd.Parameters.AddWithValue("@StdDob", TxtDateOfBirth.Text);
                    cmd.Parameters.AddWithValue("@Address", TxtAddress.Text);
                    if (obj.State == ConnectionState.Closed)
                    {
                        obj.Open();
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("New Student Added");


        }
        //To Update student with student code
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            using (obj = new SqlConnection(ConfigurationManager.ConnectionStrings["STdConn"].ConnectionString))
            {
                using(cmd = new SqlCommand("usp_UpdateDeptCodeAndAddress", obj))
                {
                    cmd.CommandType=CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DptCode", TxtDepartmentCode.Text);
                    cmd.Parameters.AddWithValue("@Address", TxtAddress.Text);
                    cmd.Parameters.AddWithValue("@StdCode", TxtStudentCode.Text);
                    if (obj.State == ConnectionState.Closed)
                    {
                        obj.Open();
                    }
                    cmd.ExecuteNonQuery();
                }

                
            }
            MessageBox.Show("Record Update SucessFully");

        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            using (obj = new SqlConnection(ConfigurationManager.ConnectionStrings["STdConn"].ConnectionString))
            {
                using (cmd = new SqlCommand("usp_DeleteARow", obj))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StdCode", TxtStudentCode.Text);
                    if (obj.State == ConnectionState.Closed)
                    {
                        obj.Open();
                    }
                    cmd.ExecuteNonQuery();


                }

            }
            MessageBox.Show("Record Deleted Sucessfully...");

        }
    }
}
