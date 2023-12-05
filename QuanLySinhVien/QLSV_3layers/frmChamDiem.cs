using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Words;
using ThuVienWinform.Report.AsposeWordExtension;
using Aspose.Words.Tables;

namespace QLSV_3layers
{
    public partial class frmChamDiem : Form
    {
        DataGridViewRow r;

        private int row = 0;
        private string dir, fileName;

        public frmChamDiem(string malophoc, string magv, string maMonHoc, string tenMonHoc)// danh sách tham số truyền vào form
        {
            this.malophoc = malophoc;// lưu malophoc được truyền qua
            this.magv = magv;
            this.maMonHoc = maMonHoc;
            this.tenMonHoc = tenMonHoc;
            InitializeComponent();
        }

        private string malophoc;// khai báo biến để lưu malophoc được truyền vào
        private string magv;
        private string maMonHoc;
        private string tenMonHoc;

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
            lblMH.Text = "Môn học: " + tenMonHoc + " - Mã môn học: " + maMonHoc;
            fileName = tenMonHoc + "_" + maMonHoc;

            dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

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

            row = dgvDSSV.Rows.Count;
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

        private void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            var homNay = DateTime.Now;

            Document baoCao = new Document("Mau_Bao_Cao_LopHoc.doc");

            baoCao.MailMerge.Execute(new[] { "Ngay_Thang_Nam_BC" }, new[] { homNay.ToString() });
            baoCao.MailMerge.Execute(new[] { "Ten_Lop_Hoc" }, new[] { lblMH.Text });

            Table bangThongTin = baoCao.GetChild(NodeType.Table, 1, true) as Table;
            int hangHienTai = 1;
            bangThongTin.InsertRows(hangHienTai, hangHienTai, row);
            for (int i = 1; i <= row; i++)
            {
                r = dgvDSSV.Rows[hangHienTai - 1];

                bangThongTin.PutValue(hangHienTai, 0, r.Cells["masinhvien"].Value.ToString());
                bangThongTin.PutValue(hangHienTai, 1, r.Cells["hoten"].Value.ToString());
                bangThongTin.PutValue(hangHienTai, 2, r.Cells["lanhoc"].Value.ToString());
                bangThongTin.PutValue(hangHienTai, 3, r.Cells["diemthilan1"].Value.ToString());
                bangThongTin.PutValue(hangHienTai, 4, r.Cells["diemthilan2"].Value.ToString());
                hangHienTai++;
            }

            try
            {
                baoCao.Save("BaoCaoLopHoc_" + fileName + ".docx");
                string temp = Application.StartupPath.ToString() + "/BaoCaoLopHoc_" + fileName + ".docx";
                string sourceFilePath = temp;
                string outputFilePath = dir + "/BaoCaoLopHoc_" + fileName + ".pdf";

                ConvertToPdf(sourceFilePath, outputFilePath);
                System.Diagnostics.Process.Start(outputFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ConvertToPdf(string sourceFilePath, string outputFilePath)
        {
            Document doc = new Document(sourceFilePath);

            doc.Save(outputFilePath, SaveFormat.Pdf);
        }
    }
}
