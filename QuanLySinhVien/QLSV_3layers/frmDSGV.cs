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
    public partial class frmDSGV : Form
    {
        public frmDSGV()
        {
            InitializeComponent();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            tukhoa = txtTimKiem.Text;       //  lấy giá trị từ hộp văn bản  có tên txtTimKiem và gán giá trị này cho biến tukhoa
                                            // Biến tukhoa chứa từ khóa mà người dùng đã nhập để thực hiện tìm kiếm.
            loadDSGV();
        }
        private string tukhoa = "";         //tukhoa: Đây là tên của biến_->lưu trữ từ khóa tìm kiếm.
        private void loadDSGV()
        {
            string sql = "selectAllGV";          //Lấy danh sách giáo viên từ cơ sở dữ liệu.
            List<CustomParameter> lstPara = new List<CustomParameter>();     // tạo một danh sách  rỗng để lưu trữ các tham số tùy chỉnh
            lstPara.Add(new CustomParameter()                              // sau đó thêm nó vào danh sách lstPara
            {                                       // tạo một đối tượng tham số tùy chỉnh với tên là @tukhoa và giá trị là biến tukhoa
                key = "@tukhoa",
                value = tukhoa
            });
            dgvDSGV.DataSource = new Database().SelectData(sql, lstPara);  // đẩy dữ liệu lên DataGridView
        }

        private void frmDSGV_Load(object sender, EventArgs e)
        {
            loadDSGV();
        }

        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            new frmGV(null).ShowDialog();       // tạo một form để thêm mới thông tin giáo viên
            loadDSGV();
        }

        private void dgvDSGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e) //chỉnh sửa thông tin giáo viên
        {
            if (e.RowIndex >= 0)
            {
                var mgv = dgvDSGV.Rows[e.RowIndex].Cells["magiaovien"].Value.ToString(); // lấy giá trị từ cột có tên "magiaovien" của hàng được nhấp và gán giá trị đó cho biến mgv.
                                                                                        
                new frmGV(mgv).ShowDialog();
                loadDSGV();
            }
        }

        private void dgvDSGV_CellClick(object sender, DataGridViewCellEventArgs e)  //xóa giáo viên
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == dgvDSGV.Columns["btnDelete"].Index)
                {
                    if (
                        MessageBox.Show("Bạn chắc chắn muốn xóa giáo viên:" + dgvDSGV.Rows[e.RowIndex].Cells["hoten"].Value.ToString() + "?", 
                        "Xác nhận xóa!!!",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var maGV = dgvDSGV.Rows[e.RowIndex].Cells["magiaovien"].Value.ToString();  //lấy mã giáo viên
                        var sql = "deleteGV";
                        List<CustomParameter> lstPara = new List<CustomParameter>();

                        lstPara.Add(new CustomParameter()
                        {                                                //Tham số @nguoicapnhat được thiết lập để chứa giá trị nguoithucthi và tham số @magiaovien chứa giá trị mgv.
                            key = "@magiaovien",
                            value = maGV
                        });
                        /*var lstPara = new List<CustomParameter>()
                    {
                        new CustomParameter
                        {
                            key="@magiaovien",
                            value = maGV
                        }
                    };*/

                        new Database().ExeCute(sql, lstPara);
                        MessageBox.Show("Xóa giáo viên thành công");
                        loadDSGV();
                    }
                }
            }
        }
    }
}
