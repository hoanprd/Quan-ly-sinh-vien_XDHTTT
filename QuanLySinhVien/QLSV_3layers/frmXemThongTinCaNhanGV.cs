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
    public partial class frmXemThongTinCaNhanGV : Form
    {
        DataGridViewRow r;

        private string maGV;

        public frmXemThongTinCaNhanGV(string maGV)
        {
            InitializeComponent();
            this.maGV = maGV;
        }

        private void frmXemThongTinCaNhanGV_Load(object sender, EventArgs e)
        {
            loadDSGV();
        }

        private void loadDSGV()
        {
            string sql = "xemThongTinCaNhanGV";          //Lấy danh sách giáo viên từ cơ sở dữ liệu.
            List<CustomParameter> lstPara = new List<CustomParameter>();     // tạo một danh sách  rỗng để lưu trữ các tham số tùy chỉnh
            lstPara.Add(new CustomParameter()                              // sau đó thêm nó vào danh sách lstPara
            {                                       // tạo một đối tượng tham số tùy chỉnh với tên là @tukhoa và giá trị là biến tukhoa
                key = "@magiaovien",
                value = maGV
            });
            dgvGiaoVien.DataSource = new Database().SelectData(sql, lstPara);  // đẩy dữ liệu lên DataGridView

            //đặt tên cột
            dgvGiaoVien.Columns["magiaovien"].HeaderText = "Mã SV";
            dgvGiaoVien.Columns["hoten"].HeaderText = "Họ tên";
            dgvGiaoVien.Columns["ngaysinh"].HeaderText = "Ngày sinh";
            dgvGiaoVien.Columns["gt"].HeaderText = "Giới tính";
            dgvGiaoVien.Columns["dienthoai"].HeaderText = "Điện thoại";
            dgvGiaoVien.Columns["email"].HeaderText = "Email";
            dgvGiaoVien.Columns["diachi"].HeaderText = "Địa chỉ";

            r = dgvGiaoVien.Rows[0];
            txtMaGV.Text = r.Cells["magiaovien"].Value.ToString();
            txtHoTen.Text = r.Cells["hoten"].Value.ToString();
            txtNgaySinh.Text = r.Cells["ngaysinh"].Value.ToString();
            txtGioiTinh.Text = r.Cells["gt"].Value.ToString();
            txtDiaChi.Text = r.Cells["diachi"].Value.ToString();
            txtDienThoai.Text = r.Cells["email"].Value.ToString();
            txtEmail.Text = r.Cells["dienthoai"].Value.ToString();
        }
    }
}
