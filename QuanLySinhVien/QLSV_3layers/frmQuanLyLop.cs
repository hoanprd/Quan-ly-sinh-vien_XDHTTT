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
    public partial class frmQuanLyLop : Form
    {
        public frmQuanLyLop(string mgv)//khai báo tham số để truyền mã giáo viên ( tài khoản đăng nhập) giữa 2 form main và quản lý lớp
        {
            this.mgv = mgv;
            InitializeComponent();
        }

        private string mgv;//khai báo biến để lưu tham số mgv được truyền vào
        private void LoadDSLop()
        {
            List<CustomParameter> lstPara = new List<CustomParameter>();
            lstPara.Add(new CustomParameter() {
                key= "@magiaovien",
                value=mgv
            });
            lstPara.Add(new CustomParameter()
            {
                key = "@tukhoa",
                value = txtTuKhoa.Text
            });
            dgvDSLop.DataSource = new Database().SelectData("tracuulop", lstPara);
        }

        private void frmQuanLyLop_Load(object sender, EventArgs e)
        {
            LoadDSLop();
            //đặt lại tiêu đề cột cho datagridview dgvDSLop
            dgvDSLop.Columns["malophoc"].HeaderText = "Mã lớp";
            dgvDSLop.Columns["mamonhoc"].HeaderText = "Mã học phần";
            dgvDSLop.Columns["tenmonhoc"].HeaderText = "Tên học phần";
            dgvDSLop.Columns["sotinchi"].HeaderText = "Số TC";
            dgvDSLop.Columns["siso"].HeaderText = "Sĩ số";
        }

        private void btnTraCuu_Click(object sender, EventArgs e)
        {
            LoadDSLop();
        }

        private void dgvDSLop_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
            if(dgvDSLop.Rows[e.RowIndex].Index >=0)// chỉ số hàng của datagridview được tính bắt đầu từ 0
            {
                //MessageBox.Show("Bạn vừa chọn lớp: "+  dgvDSLop.Rows[e.RowIndex].Cells["malophoc"].Value.ToString()   );
                new frmChamDiem(
                        dgvDSLop.Rows[e.RowIndex].Cells["malophoc"].Value.ToString(),//mã lớp học
                        mgv//mã giáo viên                    
                    ).ShowDialog();
                LoadDSLop();
            }
        }
    }
}
