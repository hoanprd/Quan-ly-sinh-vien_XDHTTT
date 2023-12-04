using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV_3layers
{
    public class Database
    {
        private string connetionString = @"Data Source=localhost\sqlexpress;Initial Catalog = QLSV; User ID = sa; Password=01677607954hoan";
        private SqlConnection conn;             //khai báo các đối tượng cần cho việc truy xuất dữ liệu
        private DataTable dt;
        private SqlCommand cmd;
        public Database()     //hàm khởi tạo-hàm đầu tiên chạy khi class db đc gọi
        {
            try
            {
                conn = new SqlConnection(connetionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show("connected failed: " + ex.Message);
            }
        }

        public DataTable SelectData(string sql, List<CustomParameter> lstPara)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    //kiem tra ket noi toi csdl co bi dong hay ko
                    //neu dang bi dong thi mo lai
                    conn.Open();
                }
                
                cmd = new SqlCommand(sql, conn);//nội dung sql đc truyền vào
                cmd.CommandType = CommandType.StoredProcedure;//set command type cho cmd
                foreach(var para in lstPara)//gán các tham số cho cmd
                {
                    cmd.Parameters.AddWithValue(para.key,para.value);
                }
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
                return null;
            }
            finally
            {
                //kiem tra trang thai ket noi toi csdl
                //neu dang mo thi tien hanh dong ket noi
                if(conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                
            }
        }

        public DataRow Select(string sql)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    //kiem tra ket noi toi csdl co bi dong hay ko
                    //neu dang bi dong thi mo lai
                    conn.Open();
                }
                cmd = new SqlCommand(sql, conn);//truyền giá trị vào cmd
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());//thực thi câu lệnh
                return dt.Rows[0];//trả về kết quả
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load thông tin chi tiết: "+ex.Message);
                return null;
            }
            finally
            {
                //kiem tra trang thai ket noi toi csdl
                //neu dang mo thi tien hanh dong ket noi
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

            }
        }

        public int ExeCute(string sql,List<CustomParameter> lstPara)      //lstPara :danh sách  chứa các tham số tùy chỉnh 
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    //kiem tra ket noi toi csdl co bi dong hay ko
                    //neu dang bi dong thi mo lai
                    conn.Open();
                }
                cmd = new SqlCommand(sql, conn);//thực thi câu lệnh sql
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (var p in lstPara)//gán các tham số cho cmd
                {
                    cmd.Parameters.AddWithValue(p.key,p.value);
                }
                var rs = cmd.ExecuteNonQuery();//lấy kết quả thực thi truy vấn
                return (int)rs;//trả về kết quả
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thực thi câu lệnh: "+ex.Message);
                return -100;
            }
            finally
            {
                //kiem tra trang thai ket noi toi csdl
                //neu dang mo thi tien hanh dong ket noi
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

            }
        }
    }
}
