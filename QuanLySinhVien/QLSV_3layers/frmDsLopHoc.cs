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
    public partial class frmDsLopHoc : Form
    {
        public frmDsLopHoc()
        {
            InitializeComponent();
        }
        private string tukhoa = "";
        private void frmDsLopHoc_Load(object sender, EventArgs e)
        {
            loadDSLH();
        }
        private void loadDSLH()
        {
            string sql = "allLopHoc";                                   //Chuỗi truy vấn SQL để lấy tất cả các lớp học
            List<CustomParameter> lstPara = new List<CustomParameter>() //Danh sách các tham số(@tukhoa)
            {
                new CustomParameter()
                {
                    key = "@tukhoa",
                    value = tukhoa  //tukhoa là biến cho từ khóa tìm kiếm
                }
            };
            dgvLopHoc.DataSource = new Database().SelectData(sql,lstPara);  // dụng lớp Database để thực hiện truy vấn và lấy dữ liệu
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            tukhoa = txtTimKiem.Text;
            loadDSLH();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            new frmLopHoc(null).ShowDialog();
            loadDSLH();
        }

        private void dgvLopHoc_CellDoubleClick(object sender, DataGridViewCellEventArgs e) //sua lh
        {
            if (e.RowIndex >= 0)
            {
                new frmLopHoc(dgvLopHoc.Rows[e.RowIndex].Cells["malophoc"].Value.ToString()).ShowDialog(); //lấy ma lop hoc
                loadDSLH();
            }
        }
    }
}
