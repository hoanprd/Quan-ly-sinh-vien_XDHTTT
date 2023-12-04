using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV_3layers
{
    public partial class frmDSSV : Form
    {
        public frmDSSV()
        {
            InitializeComponent();
        }

        private string tukhoa = "";
        private void frmDSSV_Load(object sender, EventArgs e)
        {
            LoadDSSV();//gọi tới hàm load sinh viên khi form đc load
        }

        private void LoadDSSV()
        {
            
            //khai báo list customparameter 
            List<CustomParameter> lstPara = new List<CustomParameter>();
            lstPara.Add(new CustomParameter()
            {
                key = "@tukhoa",
                value = tukhoa
            });
            var data= new Database().SelectData("SelectAllSinhVien", lstPara);
            dgvSinhVien.DataSource = data;

            //đặt tên cột
            dgvSinhVien.Columns["masinhvien"].HeaderText = "Mã SV";
            dgvSinhVien.Columns["hoten"].HeaderText = "Họ tên";
            dgvSinhVien.Columns["nsinh"].HeaderText = "Ngày sinh";
            dgvSinhVien.Columns["gt"].HeaderText = "Giới tính";
            dgvSinhVien.Columns["quequan"].HeaderText = "Quê quán";
            dgvSinhVien.Columns["diachi"].HeaderText = "Địa chỉ";
            dgvSinhVien.Columns["email"].HeaderText = "Email";
            dgvSinhVien.Columns["dienthoai"].HeaderText = "Điện thoại";
        }

        private void dgvSinhVien_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
            if (e.RowIndex >= 0)
            {
                var msv = dgvSinhVien.Rows[e.RowIndex].Cells["masinhvien"].Value.ToString(); //lay ma sv 
               
                new frmSinhVien(msv).ShowDialog();

                LoadDSSV();
            }
        }

        private void btnThemmoi_Click(object sender, EventArgs e)
        {
            new frmSinhVien(null).ShowDialog();//nếu thêm mới sinh viên -> mã sinh viên = null
            LoadDSSV();//load lại danh sách sinh viên khi thêm thành công
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            tukhoa = txtTukhoa.Text;
            LoadDSSV();
        }

        private void dgvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e) //xóa sinh viên
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == dgvSinhVien.Columns["btnDelete"].Index)
                {
                    if (
                       MessageBox.Show("Bạn chắc chắn muốn xóa sinh viên:" + dgvSinhVien.Rows[e.RowIndex].Cells["hoten"].Value.ToString() + "?",
                       "Xác nhận xóa!!!",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var maSV = dgvSinhVien.Rows[e.RowIndex].Cells["masinhvien"].Value.ToString(); //lấy mssv
                        var sql = "deleteSV";
                        var lstPara = new List<CustomParameter>()
                    {
                        new CustomParameter
                        {
                            key="@masinhvien",
                            value =maSV
                        }
                    };

                        var result = new Database().ExeCute(sql, lstPara);
                        if (result == 1)
                        {
                            MessageBox.Show("Xóa sinh vien thành công");
                            LoadDSSV();
                        }

                    }
                }
            }
        }
    }
}
