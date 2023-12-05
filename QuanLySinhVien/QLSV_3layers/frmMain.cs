using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV_3layers
{
    public partial class frmMain : Form
    {
        public static bool thoatDangNhap;

        public frmMain()
        {
            InitializeComponent();
        }

        private string taikhoan;
        private string matKhauHienTai;
        private string loaitk;

        private void frmMain_Load(object sender, EventArgs e)
        {
            var fn = new frmDangnhap();
            fn.ShowDialog();//load form đăng nhập khi formmain đc gọi

            //sau khi form đăng nhập đc tắt, lấy thông tin tài khoản đã đăng nhập
            taikhoan = fn.tendangnhap;
            matKhauHienTai = fn.matkhau;
            loaitk = fn.loaitk;

            string sql = null;
            List<CustomParameter> lstPara = new List<CustomParameter>();

            if (loaitk == "admin")
            {
                sql = "getTenTaiKhoanAdmin";
                lstPara.Add(new CustomParameter()
                {
                    key = "mataikhoan",
                    value = taikhoan
                });
            }
            else if (loaitk == "gv")
            {
                sql = "getTenTaiKhoanGV";
                lstPara.Add(new CustomParameter()
                {
                    key = "magiaovien",
                    value = taikhoan
                });
            }
            else if (loaitk == "sv")
            {
                sql = "getTenTaiKhoanSV";
                lstPara.Add(new CustomParameter()
                {
                    key = "masinhvien",
                    value = taikhoan
                });
            }

            var rs = new Database().ExeCuteString(sql, lstPara);

            string lbl = rs;
            lblXinChao.Text = "Xin chào: " + lbl;

            try
            {
                if (!thoatDangNhap)
                {
                    if (loaitk.Equals("admin"))
                    {
                        //nếu là admin
                        //ẩn 2 menu chấm điểm và đăng ký môn học
                        //chỉ để lại menu quản lý
                        chamDiemToolStripMenuItem.Visible = false;
                        chucNangToolStripMenuItem.Visible = false;
                        thongTinCaNhanGiaoVienToolStripMenuItem.Visible = false;
                        thongTinCaNhanSinhVienToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        //nếu không phải admin thì ẩn menu quản lý
                        quanLyToolStripMenuItem.Visible = false;
                        if (loaitk.Equals("gv"))//nếu là giáo viên
                        {
                            //ẩn menu đăng ký học -> cái này chỉ dành riêng cho sinh viên
                            chucNangToolStripMenuItem.Visible = false;
                            baoCaoToolStripMenuItem.Visible = false;
                            thongTinCaNhanSinhVienToolStripMenuItem.Visible = false;
                        }
                        else//chỉ còn lại trường hợp là sinh viên
                        {
                            chamDiemToolStripMenuItem.Visible = false;//ẩn menu chấm điểm<-chức năng của gv
                            baoCaoToolStripMenuItem.Visible = false;
                            thongTinCaNhanGiaoVienToolStripMenuItem.Visible = false;
                        }
                    }

                    frmWelcome f = new frmWelcome();
                    AddForm(f);
                }
                else
                {
                    Application.Exit();
                }
            }
            catch
            {
                return;
            }
        }

        private void AddForm(Form f)
        {
            this.pnlContent.Controls.Clear();               //xóa tất cả các control trước khi thêm Form mới
            f.TopLevel = false;
            f.AutoScroll = true;                            //bật tính năng tự động cuộn cho Form
            f.FormBorderStyle = FormBorderStyle.None;       // loại bỏ viền xung quanh Form,
            f.Dock = DockStyle.Fill;
            this.Text = f.Text;
            this.pnlContent.Controls.Add(f);
            f.Show();
        }

        private void giaoVienToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmDSGV f = new frmDSGV();
            AddForm(f);
        }

        private void sinhVienToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDSSV f = new frmDSSV();
            AddForm(f);
        }

        private void monHocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDSMH_ f = new frmDSMH_();
            AddForm(f);
        }

       

        private void lopHocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDsLopHoc f = new frmDsLopHoc();
            AddForm(f);
        }

        private void dangKyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new frmDsMHDaDky(taikhoan);
            AddForm(f);
        }

        private void traCuuDiemToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            var f = new frmKetQuaHocTap(taikhoan);//truyền tài khoản đăng nhập  = mã sinh viên
            AddForm(f);
        }

        private void quanLyLopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new frmQuanLyLop(taikhoan);//truyền tài khoản đăng nhập  = mã giao viên
            AddForm(f);
        }

        private void doiMatKhauToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDoiMatKhau f = new frmDoiMatKhau(taikhoan, matKhauHienTai, loaitk);
            f.ShowDialog();
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mànHìnhChínhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmWelcome f = new frmWelcome();
            AddForm(f);
        }

        private void hỗTrợToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.facebook.com/CongtacsinhvienUT");
        }

        private void báoCáoSinhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBaoCaoDSSV f = new frmBaoCaoDSSV();
            f.ShowDialog();
        }

        private void báoCáoGiáoViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBaoCaoDSGV f = new frmBaoCaoDSGV();
            f.ShowDialog();
        }

        private void thongTinCaNhanGiaoVienToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmXemThongTinCaNhanGV f = new frmXemThongTinCaNhanGV(taikhoan);
            AddForm(f);
        }

        private void thongTinCaNhanSinhVienToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmXemThongTinCaNhanSV f = new frmXemThongTinCaNhanSV(taikhoan);
            AddForm(f);
        }
    }
}
