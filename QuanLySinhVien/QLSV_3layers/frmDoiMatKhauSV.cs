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
    public partial class frmDoiMatKhauSV : Form
    {
        List<CustomParameter> lstPara = new List<CustomParameter>();
        string sql = "";

        public frmDoiMatKhauSV()
        {
            InitializeComponent();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMatKhauHienTai.Text != frmDangnhap.matKhauThayHienTai)
            {
                MessageBox.Show("Mật khẩu hiện tại không đúng!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauHienTai.Select();
                return;
            }

            if (string.IsNullOrEmpty(txtMatKhauMoi.Text))//nếu thêm mới sinh viên
            {
                MessageBox.Show("Mật khẩu mới không được phép trống!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauMoi.Select();
                return;
            }
            else if (txtMatKhauMoi.Text.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất từ 6 ký tự trở lên!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauMoi.Select();
                return;
            }
            else if (txtMatKhauMoi.Text == txtMatKhauHienTai.Text)
            {
                MessageBox.Show("Mật khẩu mới không được trùng với mật khẩu hiện tại!", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhauMoi.Select();
                return;
            }

            try
            {
                string maSV = frmDangnhap.maSVIndex;

                sql = "updateMatKhauSV";
                lstPara.Add(new CustomParameter()
                {
                    key = "@masinhvien",
                    value = maSV
                });

                lstPara.Add(new CustomParameter()
                {
                    key = "@matkhau",
                    value = txtMatKhauMoi.Text
                });

                var rs = new Database().ExeCute(sql, lstPara);//truyền 2 tham số là câu lệnh sql  và danh sách các tham số

                if (rs == 1)//nếu thuc thi thành công
                {
                    frmDangnhap.matKhauThayHienTai = txtMatKhauMoi.Text;
                    MessageBox.Show("Cập nhật mật khẩu sinh viên thành công");
                    this.Dispose();//đóng form sau khi thêm mới/cập nhật thành công
                }

                txtMatKhauHienTai.Text = null;
                txtMatKhauMoi.Text = null;
            }
            catch
            {
                MessageBox.Show("Đổi thất bại!");
            }
        }
    }
}
