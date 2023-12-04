using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV_3layers
{
    public partial class frmDangKy : Form
    {
        public frmDangKy()
        {
            InitializeComponent();
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenDangNhap.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtMatKhau.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Select();
                return;
            }
            if (txtMatKhau.Text.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất từ 6 ký tự trở lên!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Select();
                return;
            }
            if (string.IsNullOrEmpty(txtNhapLaiMK.Text))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Select();
                return;
            }
            if (txtMatKhau.Text != txtNhapLaiMK.Text)
            {
                MessageBox.Show("Nhập lại mật khẩu không khớp!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Select();
                return;
            }

            string sql = "themTaiKhoanAdmin";
            List<CustomParameter> lstPara = new List<CustomParameter>();

            lstPara.Add(new CustomParameter()
            {
                key = "@tentaikhoan",
                value = txtTenDangNhap.Text
            });
            lstPara.Add(new CustomParameter()
            {
                key = "@matkhau",
                value = txtMatKhau.Text
            });

            var rs = new Database().ExeCute(sql, lstPara);//truyền 2 tham số là câu lệnh sql  và danh sách các tham số

            if (rs == 1)//nếu thuc thi thành công
            {
                string duongdan = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string checkDK = duongdan + @"\QLSV\checkdk.txt";

                File.WriteAllText(checkDK, "dadangky");
                MessageBox.Show("Tạo tài khoản admin thành công");
            }
            else//nếu không thành công
            {
                MessageBox.Show("Tạo tài khoản thất bại!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.Hide();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            frmMain.thoatDangNhap = true;
            Application.Exit();
        }
    }
}
