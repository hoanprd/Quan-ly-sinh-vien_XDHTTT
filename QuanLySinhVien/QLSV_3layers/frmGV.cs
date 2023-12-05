using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV_3layers
{
    public partial class frmGV : Form
    {
        public frmGV(string mgv)
        {
            this.mgv = mgv;
            InitializeComponent();
        }
        private string mgv;
        private string nguoithucthi = "admin";
        private void frmGV_Load(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(mgv))                            //kiểm tra xem biến mgv có rỗng hoặc null
            {
                this.Text = "Thêm mới giáo viên";                      //Nếu mgv rỗng hoặc null-->Thêm mới giáo viên
            }
            else
            {
                this.Text = "Cập nhật giáo viên";
                var r = new Database().Select("selectGV '" + int.Parse(mgv) + "'"); //thực hiện việc tạo một truy vấn SQL từ cơ sở dữ liệu
                                                                                    //int.Parse(mgv) chuyển đổi giá trị trong biến mgv thành một số nguyên
                txtHo.Text = r["ho"].ToString();                                    
                txtTendem.Text = r["tendem"].ToString();
                txtTen.Text = r["ten"].ToString();
                rbtNam.Checked = r["gioitinh"].ToString() == "1" ? true : false;     //giá trị từ cột "gioitinh" được chuyển đổi thành chuỗi sau đó so sánh với chuỗi "1".

                mtbNgaysinh.Text = r["ngsinh"].ToString();
                txtDienthoai.Text = r["dienthoai"].ToString();
                txtEmail.Text = r["email"].ToString();
                txtDiachi.Text = r["diachi"].ToString();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql = "";
            DateTime ngaysinh;
            List<CustomParameter> lstPara = new List<CustomParameter>();
            try
            {
                ngaysinh = DateTime.ParseExact(mtbNgaysinh.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //CultureInfo.InvariantCulture  chuyển đổi ngày tháng hoặc định dạng số
            }
            catch 
            {
                MessageBox.Show("Ngày sinh không hợp lệ");
                mtbNgaysinh.Select();
                return;
            }

            if(string.IsNullOrEmpty(mgv))               //kiểm tra xem biến mgv có rỗng hoặc null hay không
            {                                           //nếu rong thi them moi gv
                sql = "insertGV";
                lstPara.Add(new CustomParameter()         //biến sql được thiết lập thành "insertGV" và một tham số nguoitao  được thêm vào danh sách lstPara
                {                                         //Tham số này có giá trị là nguoithucthi,
                    key = "@nguoitao",
                    value = nguoithucthi
                });
            }
            else
            {                                                 //Nếu mgv không rỗng hoặc null--> cập nhật giáo viên                                       
                sql = "updateGV";                              
                lstPara.Add(new CustomParameter()               
                {                                                //Tham số @nguoicapnhat được thiết lập để chứa giá trị nguoithucthi và tham số @magiaovien chứa giá trị mgv.
                    key = "@nguoicapnhat",
                    value = nguoithucthi
                });
                lstPara.Add(new CustomParameter() {
                    key = "@magiaovien",
                    value = mgv
                });
            }

            lstPara.Add(new CustomParameter()                  //thông tin gv do người dùng nhap
            {
                key = "@ho",
                value = txtHo.Text
            });

            lstPara.Add(new CustomParameter()
            {
                key = "@tendem",
                value = txtTendem.Text
            });
            lstPara.Add(new CustomParameter()
            {
                key = "@ten",
                value = txtTen.Text
            });

            lstPara.Add(new CustomParameter()
            {
                key = "@ngaysinh",
                value = ngaysinh.ToString("yyyy-MM-dd")
            });

            lstPara.Add(new CustomParameter()
            {
                key = "@gioitinh",
                value = rbtNam.Checked?"1":"0"
            });

            lstPara.Add(new CustomParameter()
            {
                key = "@email",
                value = txtEmail.Text
            });

            lstPara.Add(new CustomParameter()
            {
                key = "@dienthoai",
                value = txtDienthoai.Text
            });

            lstPara.Add(new CustomParameter()
            {
                key = "@diachi",
                value = txtDiachi.Text
            });


            var rs = new Database().ExeCute(sql,lstPara);
            if(rs == 1)
            {
                if(string.IsNullOrEmpty(mgv))
                {
                    MessageBox.Show("Thêm mới giáo viên thành công");
                }
                else
                {
                    MessageBox.Show("Cập nhật mới giáo viên thành công");
                }
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Thực thi truy vấn thất bại");
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
