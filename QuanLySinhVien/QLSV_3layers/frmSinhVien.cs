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
    public partial class frmSinhVien : Form
    {
        public frmSinhVien(string msv)
        {
            this.msv = msv;//truyền lại mã sinh viên khi form chạy
            InitializeComponent();
        }
        private string msv;

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(msv))//nếu msv không có => thêm mới sinh viên
            {
                this.Text = "Thêm mới sinh viên";
            }
            else
            {
                this.Text = "Cập nhật thông tin sinh viên";
               
                var r = new Database().Select("selectSV '"+msv+"'");
             

                txtHo.Text = r["ho"].ToString();
                txtTendem.Text = r["tendem"].ToString();
                txtTen.Text = r["ten"].ToString();
                mtbNgaysinh.Text = r["ngsinh"].ToString();
                if(int.Parse(r["gioitinh"].ToString()) == 1)
                {
                    rbtNam.Checked = true;
                }
                else
                {
                    rbtNu.Checked = true;
                }

                txtQuequan.Text = r["quequan"].ToString();
                txtDiachi.Text = r["diachi"].ToString();
                txtDienthoai.Text = r["dienthoai"].ToString();
                txtEmail.Text = r["email"].ToString();

            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
           

            string sql = "";
            string ho = txtHo.Text;
            string tendem = txtTendem.Text;
            string ten = txtTen.Text;
            DateTime ngaysinh;
            try
            {
                ngaysinh = DateTime.ParseExact(mtbNgaysinh.Text,"dd/MM/yyyy",CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                MessageBox.Show("Ngày sinh không hợp lệ");
                mtbNgaysinh.Select();//trỏ chuột về mtbNgaysinh
                return;
            }
            //vì ngày sinh ở masketbox set theo dạng dd/mm/yyy
            //nhưng trong csdl lại lưu dưới dạng yyyy-mm-dd
            


            string gioitinh = rbtNam.Checked ? "1" : "0";
            //nếu radiobutton Nam đc check thì chọn giá trị 1
            //ngược lại chọn giá trị 0 -> phù hợp với giá trị đã được lưu ở csdl

            string quequan = txtQuequan.Text;
            string diachi = txtDiachi.Text;
            string dienthoai = txtDienthoai.Text;
            string email = txtEmail.Text;

            //khai báo một danh sách tham sô = class CustomParameter 
            List<CustomParameter> lstPara = new List<CustomParameter>();
           if(string.IsNullOrEmpty(msv))//nếu thêm mới sinh viên
            {
                sql = "ThemMoiSV";
               
            }
            else//nếu cập nhật sinh viên
            {
                sql = "updateSV";
                lstPara.Add(new CustomParameter()
                {
                    key = "@masinhvien",
                    value = msv
                });
            }

            lstPara.Add(new CustomParameter() {
                key = "@ho",
                value = ho
            });
            lstPara.Add(new CustomParameter()
            {
                key = "@tendem",
                value = tendem
            });
            lstPara.Add(new CustomParameter()
            {
                key = "@ten",
                value = ten
            });
            lstPara.Add(new CustomParameter()
            {
                key = "@ngaysinh",
                value = ngaysinh.ToString("yyyy-MM-dd")
            });
            lstPara.Add(new CustomParameter()
            {
                key = "@gioitinh",
                value = gioitinh
            });
            lstPara.Add(new CustomParameter()
            {
                key = "@quequan",
                value = quequan
            });
            lstPara.Add(new CustomParameter()
            {
                key = "@diachi",
                value = diachi
            });
            lstPara.Add(new CustomParameter()
            {
                key = "@dienthoai",
                value = dienthoai
            });
            lstPara.Add(new CustomParameter()
            {
                key = "@email",
                value = email
            });


            var rs = new Database().ExeCute(sql,lstPara);//truyền 2 tham số là câu lệnh sql  và danh sách các tham số

            if (rs == 1)//nếu thuc thi thành công
            {
                if(string.IsNullOrEmpty(msv))//nếu thêm mới
                {
                    MessageBox.Show("Thêm mới sinh viên thành công");
                }else//nếu cập nhật
                {
                    MessageBox.Show("Cập nhật thông tin sinh viên thành công");
                }
                this.Dispose();//đóng form sau khi thêm mới/cập nhật thành công
            }else//nếu không thành công
            {
                MessageBox.Show("Thực thi thất bại");
            }

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
