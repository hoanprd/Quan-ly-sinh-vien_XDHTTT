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
    public partial class frmDoiMatKhau : Form
    {
        List<CustomParameter> lstPara = new List<CustomParameter>();
        string sql = "";
        private string maIndex, matKhauHienTai, loaiTK;

        public frmDoiMatKhau(string maIndex, string matKhauHienTai, string loaiTK)
        {
            InitializeComponent();
            this.maIndex = maIndex;
            this.matKhauHienTai = matKhauHienTai;
            this.loaiTK = loaiTK;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMatKhauHienTai.Text != matKhauHienTai)
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
                if (loaiTK == "sv")
                {
                    sql = "updateMatKhauSV";
                    lstPara.Add(new CustomParameter()
                    {
                        key = "@masinhvien",
                        value = maIndex
                    });

                    lstPara.Add(new CustomParameter()
                    {
                        key = "@matkhau",
                        value = txtMatKhauMoi.Text
                    });

                    var rs = new Database().ExeCute(sql, lstPara);//truyền 2 tham số là câu lệnh sql  và danh sách các tham số

                    if (rs == 1)//nếu thuc thi thành công
                    {
                        matKhauHienTai = txtMatKhauMoi.Text;
                        MessageBox.Show("Cập nhật mật khẩu sinh viên thành công");
                        this.Dispose();//đóng form sau khi thêm mới/cập nhật thành công
                    }
                }
                else if (loaiTK == "gv")
                {
                    sql = "updateMatKhauGV";
                    lstPara.Add(new CustomParameter()
                    {
                        key = "@magiaovien",
                        value = maIndex
                    });

                    lstPara.Add(new CustomParameter()
                    {
                        key = "@matkhau",
                        value = txtMatKhauMoi.Text
                    });

                    var rs = new Database().ExeCute(sql, lstPara);

                    if (rs == 1)
                    {
                        matKhauHienTai = txtMatKhauMoi.Text;
                        MessageBox.Show("Cập nhật mật khẩu giáo viên thành công");
                        this.Dispose();
                    }
                }
                else if (loaiTK == "admin")
                {
                    sql = "updateMatKhauAdmin";
                    lstPara.Add(new CustomParameter()
                    {
                        key = "@tentaikhoan",
                        value = maIndex
                    });

                    lstPara.Add(new CustomParameter()
                    {
                        key = "@matkhau",
                        value = txtMatKhauMoi.Text
                    });

                    var rs = new Database().ExeCute(sql, lstPara);

                    if (rs == 1)
                    {
                        matKhauHienTai = txtMatKhauMoi.Text;
                        MessageBox.Show("Cập nhật mật khẩu quản trị viên thành công");
                        this.Dispose();
                    }
                }

                txtMatKhauHienTai.Text = null;
                txtMatKhauMoi.Text = null;
            }
            catch
            {
                MessageBox.Show("Đổi mật khẩu thất bại!", "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
