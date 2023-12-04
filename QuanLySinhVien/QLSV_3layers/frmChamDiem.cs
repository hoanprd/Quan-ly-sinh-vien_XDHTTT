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
    public partial class frmChamDiem : Form
    {
        public frmChamDiem(string malophoc, string magv)// danh sách tham số truyền vào form
        {
            this.malophoc = malophoc;// lưu malophoc được truyền qua
            this.magv = magv;
            InitializeComponent();
        }
        private string malophoc;// khai báo biến để lưu malophoc được truyền vào
        private string magv;
        private void LoadDSSV()
        {
            List<CustomParameter> lstPara = new List<CustomParameter>();
            lstPara.Add(new CustomParameter()
            {
                key= "@malophoc",                  //cho phép truyền giá trị của biến malophoc vào câu truy vấn SQL để tải danh sách sinh viên của lớp học cụ thể.
                value = malophoc
            });
            lstPara.Add(new CustomParameter()
            {
                key = "@tukhoa",                            //@tukhoa là giá trị nhập từ TextBox
                value = txtTuKhoa.Text                      //tìm kiếm trong danh sách sinh viên dựa trên từ khóa mà người dùng nhập vào TextBox
            });

            dgvDSSV.DataSource = new Database().SelectData("dssv", lstPara);           
        }

        private void frmChamDiem_Load(object sender, EventArgs e)
        {
            //cho gọi hàm load dssv ngay khi form đc load
            LoadDSSV();
           
            dgvDSSV.Columns["masinhvien"].HeaderText = "MSV"; //column index = 0 -- chỉ số cột
            dgvDSSV.Columns["hoten"].HeaderText = "Họ và tên";//column index = 1
            dgvDSSV.Columns["lanhoc"].HeaderText = "Lần học";//columnindex = 2
            dgvDSSV.Columns["diemthilan1"].HeaderText = "Điểm lần 1";//column index = 3
            dgvDSSV.Columns["diemthilan2"].HeaderText = "Điểm lần 2";//column index = 4

           
            for (int i = 0; i<=2; i++)// không cho sửa 3 cột phía trước
            {
                dgvDSSV.Columns[i].ReadOnly = true;
            }
        }

        private void btnTraCuu_Click(object sender, EventArgs e)
        {
            LoadDSSV();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // các điểm được chấm trên datagridview dgvDSSV sẽ được cập nhật vào csdl ( bảng tblDiem)
            if(     DialogResult.Yes ==                
                    MessageBox.Show(
                            "Bạn muốn lưu bảng điểm?",
                            "Xác nhận thao tác",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question                    
                        )
               )
            {
                //storedprocedure chamdiem chỉ chấm cho 1 sinh viên-->để có thể lưu hết bảng điểmcần duyệt hết các dòng dữ liệu trên datagridview  -->vòng lặp
                
                var db = new Database();
                List<CustomParameter> lstPara;

                //bắt đầu duyệt
                int chk = 1;
                foreach(DataGridViewRow r in dgvDSSV.Rows)
                {
                    lstPara = new List<CustomParameter>();


                    lstPara = new List<CustomParameter>();
                    lstPara.Add(new CustomParameter() {
                        key= "@magiaovien",
                        value=magv
                    });
                    lstPara.Add(new CustomParameter()
                    {
                        key = "@malop",
                        value = malophoc
                    });
                    lstPara.Add(new CustomParameter()
                    {
                        key = "@masinhvien",
                        value = r.Cells["masinhvien"].Value.ToString()
                    });
                    lstPara.Add(new CustomParameter()
                    {
                        key = "@diemlan1",
                        value = r.Cells["diemthilan1"].Value.ToString()
                    });
                    lstPara.Add(new CustomParameter()
                    {
                        key = "@diemlan2",
                        value = r.Cells["diemthilan2"].Value.ToString()
                    });
                    //thực thi truy vấn
                    chk = db.ExeCute("chamdiem", lstPara);
                    if(chk !=1)//nếu chấm điểm thất bại
                    {
                        MessageBox.Show("Chấm điểm thất bại");
                        break;//thoát khỏi vòng lặp luôn mà không chạy các lần còn lại
                    }
                }//kết thúc duyệt

                if(chk==1)
                {
                    MessageBox.Show("Lưu bảng điểm thành công");
                }                
                
            }

        }

        private void btnKetThuc_Click(object sender, EventArgs e)
        {
            
            // kết thúc lớp học phần <=> trạng thái daketthuc của tblLopHoc sẽ chuyển từ 0 -> 1
            
            if(
                DialogResult.Yes ==
                MessageBox.Show(
                                    "Bạn thực sự muốn đóng lớp học phần này??",
                                    "Xác thực thao tác",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question
                                 )
                )
            {
                var lstPara = new List<CustomParameter>()
                {
                    new CustomParameter()
                    {
                        key = "@magiaovien",
                        value=magv,
                    },
                    new CustomParameter()
                    {
                        key = "@malop",
                        value=malophoc,
                    }
                };

                var rs = new Database().ExeCute("ketthuchocphan", lstPara);
                if(rs == 1)
                {
                    MessageBox.Show("Kết thúc lớp học phần thành công");
                    this.Dispose();//đóng form 
                }
            }
        }

        private void txtTuKhoa_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
