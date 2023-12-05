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
    public partial class frmXemThongTinCaNhanSV : Form
    {
        DataGridViewRow r;

        private string maSV;

        public frmXemThongTinCaNhanSV(string maSV)
        {
            InitializeComponent();
            this.maSV = maSV;
        }

        private void frmXemThongTinCaNhanSV_Load(object sender, EventArgs e)
        {
            LoadDSSV();
        }

        private void LoadDSSV()
        {
            //khai báo list customparameter 
            List<CustomParameter> lstPara = new List<CustomParameter>();
            lstPara.Add(new CustomParameter()
            {
                key = "@masinhvien",
                value = maSV
            });
            var data = new Database().SelectData("xemThongTinCaNhanSV", lstPara);
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

            r = dgvSinhVien.Rows[0];
            txtMaSV.Text= r.Cells["masinhvien"].Value.ToString();
            txtHoTen.Text = r.Cells["hoten"].Value.ToString();
            txtNgaySinh.Text = r.Cells["nsinh"].Value.ToString();
            txtGioiTinh.Text = r.Cells["gt"].Value.ToString();
            txtQueQuan.Text = r.Cells["quequan"].Value.ToString();
            txtDiaChi.Text = r.Cells["diachi"].Value.ToString();
            txtDienThoai.Text = r.Cells["email"].Value.ToString();
            txtEmail.Text = r.Cells["dienthoai"].Value.ToString();
        }
    }
}
