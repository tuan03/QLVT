using QLVT.SubForm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class FormNhanVien : Form
    {
        /* vị trí của con trỏ trên grid view*/
        int viTri = 0;
        bool dangThemMoi = false;
                    
        String maChiNhanh = "";
        Stack undoList = new Stack();
        private static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }

        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }
        public FormNhanVien()
        {
            InitializeComponent();
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void nhanVienBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsNhanVien.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet);

        }
 
        private void FormNhanVien_Load(object sender, EventArgs e)
        {

            dataSet.EnforceConstraints = false;

            this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.nhanVienTableAdapter.Fill(this.dataSet.NhanVien);
            
            this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
            this.datHangTableAdapter.Fill(this.dataSet.DatHang);
            
            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuNhapTableAdapter.Fill(this.dataSet.PhieuNhap);
           
            this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuXuatTableAdapter.Fill(this.dataSet.PhieuXuat);


            maChiNhanh = ((DataRowView)bdsNhanVien[0])["MACN"].ToString();
            /*Step 2*/
            cmbChiNhanh.DataSource = Program.bindingSource;
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.brand;
            
            /*Step 3*/
            /*CONG TY chi xem du lieu*/
            if( Program.role == "CongTy")
            {
                cmbChiNhanh.Enabled = true;

                this.btnTHEM.Enabled = false;
                this.btnXOA.Enabled = false;
                this.btnGHI.Enabled = false;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnCHUYENChiNhanh.Enabled = false;
                this.btnTHOAT.Enabled = true;

                this.panelNhapLieu.Enabled = false;
            }

            /* CHI NHANH & User co the xem - xoa - sua du lieu nhung khong the 
             chuyen sang chi nhanh khac*/
            if( Program.role == "ChiNhanh" || Program.role == "User")
            {
                cmbChiNhanh.Enabled = false;

                this.btnTHEM.Enabled = true;
                this.btnXOA.Enabled = true;
                this.btnGHI.Enabled = true;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnCHUYENChiNhanh.Enabled = true;
                this.btnTHOAT.Enabled = true;

                this.panelNhapLieu.Enabled = true;
                this.txtMANV.Enabled = false;
            }

        }

        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {

        }


        private void btnTHEM_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 1*/
            /*lấy vị trí hiện tại của con trỏ*/
            viTri = bdsNhanVien.Position;
            this.panelNhapLieu.Enabled = true;
            dangThemMoi = true;


            /*Step 2*/
            /*AddNew tự động nhảy xuống cuối thêm 1 dòng mới*/
            bdsNhanVien.AddNew();
            txtMACN.Text = maChiNhanh;
            dteNGAYSINH.EditValue = "2000-05-01";
            txtLUONG.Value = 4000000;// dat san muc luong toi thieu
            txtMANV.Text = "1";

            /*Step 3*/
            this.txtMANV.Enabled = true;
            this.btnTHEM.Enabled = false;
            this.btnXOA.Enabled = false;
            this.btnGHI.Enabled = true;

            this.btnHOANTAC.Enabled = true;
            this.btnLAMMOI.Enabled = false;
            this.btnCHUYENChiNhanh.Enabled = false;
            this.btnTHOAT.Enabled = false;
            this.trangThaiXoaCheckBox.Checked = false;
            this.trangThaiXoaCheckBox.Enabled = false;

            this.gcNhanVien.Enabled = false;
            this.panelNhapLieu.Enabled = true;
        }

        private void btnHOANTAC_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /* Step 0 - */
            if ( dangThemMoi == true && this.btnTHEM.Enabled == false)
            {
                dangThemMoi = false;

                this.txtMANV.Enabled = false;
                this.btnTHEM.Enabled = true;
                this.btnXOA.Enabled = true;
                this.btnGHI.Enabled = true;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnCHUYENChiNhanh.Enabled = true;
                this.btnTHOAT.Enabled = true;
                this.trangThaiXoaCheckBox.Checked = false;

                this.gcNhanVien.Enabled = true;
                this.panelNhapLieu.Enabled = true;

                bdsNhanVien.CancelEdit();
                /*xoa dong hien tai*/
                bdsNhanVien.RemoveCurrent();
                /* trở về lúc đầu con trỏ đang đứng*/
                bdsNhanVien.Position = viTri;
                return;
            }


            /*Step 1*/
            if ( undoList.Count == 0)
            {
                MessageBox.Show("Không còn thao tác nào để khôi phục" , "Thông báo", MessageBoxButtons.OK);
                btnHOANTAC.Enabled = false;
                return;
            }

            /*Step 2*/
            bdsNhanVien.CancelEdit();
            String cauTruyVanHoanTac = undoList.Pop().ToString();
            //Console.WriteLine(cauTruyVanHoanTac);

            /*Step 2.1*/
            if ( cauTruyVanHoanTac.Contains("sp_ChuyenChiNhanh") )
            {
                try
                {
                    String ChiNhanhHienTai = Program.serverName;
                    String ChiNhanhChuyenToi = Program.serverNameLeft;

                    Program.serverName = ChiNhanhChuyenToi;
                    Program.loginName = Program.remoteLogin;
                    Program.loginPassword = Program.remotePassword;

                    if (Program.KetNoi() == 0)
                    {
                        return;
                    }


                    int n = Program.ExecSqlNonQuery(cauTruyVanHoanTac);

                    MessageBox.Show("Chuyển nhân viên trở lại thành công", "Thông báo", MessageBoxButtons.OK);
                    Program.serverName = ChiNhanhHienTai;
                    Program.loginName = Program.currentLogin;
                    Program.loginPassword = Program.currentPassword;
                }
                catch( Exception ex)
                {
                    MessageBox.Show("Chuyển nhân viên thất bại \n"+ex.Message, "Thông báo", MessageBoxButtons.OK);
                    return;
                }
                
            }
            /*Step 2.2*/
            else
            {
                if (Program.KetNoi() == 0)
                {
                    return;
                }
                int n = Program.ExecSqlNonQuery(cauTruyVanHoanTac);
                
            }
            this.nhanVienTableAdapter.Fill(this.dataSet.NhanVien);
        }

        private void btnLAMMOI_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           try
           {
                // do du lieu moi tu dataSet vao gridControl NHANVIEN
                this.nhanVienTableAdapter.Fill(this.dataSet.NhanVien);
                this.gcNhanVien.Enabled = true;
           }
           catch(Exception ex)
           {
                MessageBox.Show("Lỗi Làm mới" + ex.Message,"Thông báo", MessageBoxButtons.OK);
                return;
           }
        }
         private void btnXOA_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String tenNV = ((DataRowView)bdsNhanVien[bdsNhanVien.Position])["MANV"].ToString();
            /*Step 1*/

            // khong cho xoa nguoi dang dang nhap ke ca nguoi do khong co don hang - phieu nhap - phieu xuat
            if(tenNV == Program.userName)
            {
                MessageBox.Show("Không thể xóa chính tài khoản đang đăng nhập", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            if ( bdsNhanVien.Count == 0)
            {
                btnXOA.Enabled = false;
            }

            if( bdsDatHang.Count > 0)
            {
                MessageBox.Show("Không thể xóa nhân viên này vì đã lập đơn đặt hàng", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            if (bdsPhieuNhap.Count > 0)
            {
                MessageBox.Show("Không thể xóa nhân viên này vì đã lập phiếu nhập", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            if (bdsPhieuXuat.Count > 0)
            {
                MessageBox.Show("Không thể xóa nhân viên này vì đã lập phiếu xuất", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            /* Phần này phục vụ tính năng hoàn tác
                    * Đưa câu truy vấn hoàn tác vào undoList 
                    * để nếu chẳng may người dùng ấn hoàn tác thì quất luôn*/
            int trangThai = (trangThaiXoaCheckBox.Checked == true) ? 1 : 0;
            /*Lấy ngày sinh trong grid view*/
            DateTime NGAYSINH = (DateTime)((DataRowView)bdsNhanVien[bdsNhanVien.Position])["NGAYSINH"];
            

            string cauTruyVanHoanTac =
                string.Format("INSERT INTO DBO.NHANVIEN( MANV,HO,TEN,SOCMND,DIACHI,NGAYSINH,LUONG,MACN,TrangThaiXoa)" +
            "VALUES({0},'{1}','{2}','{3}','{4}',CAST({5} AS DATETIME), {6},'{7}', {8})", txtMANV.Text, txtHO.Text, txtTEN.Text, txtCMND.Text, txtDIACHI.Text, NGAYSINH.ToString("yyyy-MM-dd"), txtLUONG.Value, txtMACN.Text.Trim(), trangThai);

            Console.WriteLine(cauTruyVanHoanTac);
            undoList.Push(cauTruyVanHoanTac);


            /*Step 2*/
            if ( MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này không ?","Thông báo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    /*Step 3*/
                    viTri = bdsNhanVien.Position;
                    bdsNhanVien.RemoveCurrent();

                    this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.nhanVienTableAdapter.Update(this.dataSet.NhanVien);

                    MessageBox.Show("Xóa thành công ", "Thông báo", MessageBoxButtons.OK);
                    this.btnHOANTAC.Enabled = true;
                }
                catch(Exception ex)
                {
                    /*Step 4*/
                    MessageBox.Show("Lỗi xóa nhân viên. Hãy thử lại\n" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                    this.nhanVienTableAdapter.Fill(this.dataSet.NhanVien);
                    // tro ve vi tri cua nhan vien dang bi loi
                    bdsNhanVien.Position = viTri;
                    //bdsNhanVien.Position = bdsNhanVien.Find("MANV", manv);
                    return;
                }
            }
            else
            {
                undoList.Pop();
            }    
        }

        private void cmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            /*Neu combobox khong co so lieu thi ket thuc luon*/
            if (cmbChiNhanh.SelectedValue.ToString() == "System.Data.DataRowView")
                return;

            Program.serverName = cmbChiNhanh.SelectedValue.ToString();

            /*Neu chon sang chi nhanh khac voi chi nhanh hien tai*/
            if( cmbChiNhanh.SelectedIndex != Program.brand )
            {
                Program.loginName = Program.remoteLogin;
                Program.loginPassword = Program.remotePassword;
            }
            /*Neu chon trung voi chi nhanh dang dang nhap o formDangNhap*/
            else
            {
                Program.loginName = Program.currentLogin;
                Program.loginPassword = Program.currentPassword;
            }

            if( Program.KetNoi() == 0)
            {
                MessageBox.Show("Xảy ra lỗi kết nối với chi nhánh hiện tại","Thông báo",MessageBoxButtons.OK);
            }
            else
            {
                /*Do du lieu tu dataSet vao grid Control*/
                this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                this.nhanVienTableAdapter.Fill(this.dataSet.NhanVien);

                this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
                this.datHangTableAdapter.Fill(this.dataSet.DatHang);

                this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuNhapTableAdapter.Fill(this.dataSet.PhieuNhap);

                this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuXuatTableAdapter.Fill(this.dataSet.PhieuXuat);
                /*Tu dong lay maChiNhanh hien tai - phuc vu cho phan btnTHEM*/
                /*Cho dong nay chay thi bi loi*/
                //maChiNhanh = ((DataRowView)bdsNhanVien[0])["MACN"].ToString().Trim();
            }
        }

        private void bdsPhieuNhap_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void dteNGAYSINH_EditValueChanged(object sender, EventArgs e)
        {
            //dteNGAYSINH.ShowPopup();
        }

        private void trangThaiXoaCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private bool kiemTraDuLieuDauVao()
        {
            /*kiem tra txtMANV*/
            if (txtMANV.Text == "")
            {
                MessageBox.Show("Không bỏ trống mã nhân viên", "Thông báo", MessageBoxButtons.OK);
                txtMANV.Focus();
                return false;
            }

            if (Regex.IsMatch(txtMANV.Text, @"^[0-9]+$") == false)
            {
                MessageBox.Show("Mã nhân viên chỉ chấp nhận số", "Thông báo", MessageBoxButtons.OK);
                txtMANV.Focus();
                return false;
            }
            /*kiem tra txtHO*/
            if (txtHO.Text == "")
            {
                MessageBox.Show("Không bỏ trống họ và tên", "Thông báo", MessageBoxButtons.OK);
                txtHO.Focus();
                return false;
            }
            //"^[0-9A-Za-z ]+$"
            if ( Regex.IsMatch(txtHO.Text, @"^[\p{L} ]+$") == false)
            {
                MessageBox.Show("Họ của người chỉ có chữ cái và khoảng trắng", "Thông báo", MessageBoxButtons.OK);
                txtHO.Focus();
                return false;
            }
            if (txtHO.Text.Length > 40)
            {
                MessageBox.Show("Họ không thể lớn hơn 40 kí tự", "Thông báo", MessageBoxButtons.OK);
                txtHO.Focus();
                return false;
            }


            /*kiem tra txtCMND*/
            if (txtCMND.Text == "")
            {
                MessageBox.Show("Không bỏ trống CMND", "Thông báo", MessageBoxButtons.OK);
                txtCMND.Focus();
                return false;
            }
       
            if (Regex.IsMatch(txtCMND.Text, @"^[0-9]+$") == false)
            {
                MessageBox.Show("CMND chỉ chứa kí tự số", "Thông báo", MessageBoxButtons.OK);
                txtCMND.Focus();
                return false;
            }
            if (txtCMND.Text.Length > 20)
            {
                MessageBox.Show("CMND không thể lớn hơn 20 kí tự", "Thông báo", MessageBoxButtons.OK);
                txtCMND.Focus();
                return false;
            }
            /*kiem tra txtTEN*/
            if (txtTEN.Text == "")
            {
                MessageBox.Show("Không bỏ trống họ và tên", "Thông báo", MessageBoxButtons.OK);
                txtTEN.Focus();
                return false;
            }

            if (Regex.IsMatch(txtTEN.Text, @"^[\p{L} ]+$") == false)
            {
                MessageBox.Show("Tên người chỉ có chữ cái và khoảng trắng", "Thông báo", MessageBoxButtons.OK);
                txtTEN.Focus();
                return false;
            }

            if (txtTEN.Text.Length > 10)
            {
                MessageBox.Show("Tên không thể lớn hơn 10 kí tự", "Thông báo", MessageBoxButtons.OK);
                txtTEN.Focus();
                return false;
            }
            /*kiem tra txtDIACHI*/
            if (txtDIACHI.Text == "")
            {
                MessageBox.Show("Không bỏ trống địa chỉ", "Thông báo", MessageBoxButtons.OK);
                txtDIACHI.Focus();
                return false;
            }

            if (Regex.IsMatch(txtDIACHI.Text, @"^[\p{L}0-9, ]+$") == false)
            {
                MessageBox.Show("Địa chỉ chỉ chấp nhận chữ cái, số và khoảng trắng", "Thông báo", MessageBoxButtons.OK);
                txtDIACHI.Focus();
                return false;
            }

            if (txtDIACHI.Text.Length > 100)
            {
                MessageBox.Show("Không bỏ trống địa chỉ", "Thông báo", MessageBoxButtons.OK);
                txtDIACHI.Focus();
                return false;
            }
            /*kiem tra dteNGAYSINH va txtLUONG*/
            if (CalculateAge(dteNGAYSINH.DateTime) < 18)
            {
                MessageBox.Show("Nhân viên chưa đủ 18 tuổi", "Thông báo", MessageBoxButtons.OK);
                dteNGAYSINH.Focus();
                return false;
            }

            if (txtLUONG.Value < 4000000 || txtLUONG.Value == 0)
            {
                MessageBox.Show("Mức lương không thể bỏ trống & tối thiểu 4.000.000 đồng", "Thông báo", MessageBoxButtons.OK);
                txtLUONG.Focus();
                return false;
            }
            return true;
        }

        private void btnGHI_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /* Step 0 */
            bool ketQua = kiemTraDuLieuDauVao();
            if (ketQua == false)
                return;



            /*Step 1*/
            /*Lay du lieu truoc khi chon btnGHI - phuc vu btnHOANTAC - sau khi OK thi da la du lieu moi*/
            String maNhanVien = txtMANV.Text.Trim();// Trim() de loai bo khoang trang thua
            DataRowView drv = ((DataRowView)bdsNhanVien[bdsNhanVien.Position]);
            String ho = drv["HO"].ToString();
            String ten = drv["TEN"].ToString();
            String cmnd = drv["SOCMND"].ToString();

            String diaChi = drv["DIACHI"].ToString();

            DateTime ngaySinh = ((DateTime)drv["NGAYSINH"]);
            Console.WriteLine(ngaySinh);

            int luong = int.Parse(drv["LUONG"].ToString());
            String maChiNhanh = drv["MACN"].ToString();
            int trangThai = (trangThaiXoaCheckBox.Checked == true) ? 1 : 0;


            /*declare @returnedResult int
              exec @returnedResult = sp_TraCuu_KiemTraMaNhanVien '20'
              select @returnedResult*/
            String cauTruyVan =
                    "DECLARE	@result int " +
                    "EXEC @result = [dbo].[sp_TraCuu_KiemTraMaNhanVien] '" +
                    maNhanVien + "' " +
                    "SELECT 'Value' = @result"; ;
            SqlCommand sqlCommand = new SqlCommand(cauTruyVan, Program.conn);
            try
            {
                Program.myReader = Program.ExecSqlDataReader(cauTruyVan);
                /*khong co ket qua tra ve thi ket thuc luon*/
                if (Program.myReader == null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thực thi database thất bại!\n\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
                return;
            }
            Program.myReader.Read();
            int result = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();



            /*Step 2*/
            int viTriConTro = bdsNhanVien.Position;
            int viTriMaNhanVien = bdsNhanVien.Find("MANV", txtMANV.Text);
            
            if ( result == 1 && viTriConTro != viTriMaNhanVien)
            {
                MessageBox.Show("Mã nhân viên này đã được sử dụng !", "Thông báo", MessageBoxButtons.OK);
                return; 
            }
            else/*them moi | sua nhan vien*/
            {
                DialogResult dr = MessageBox.Show("Bạn có chắc muốn ghi dữ liệu vào cơ sở dữ liệu ?", "Thông báo",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if ( dr == DialogResult.OK)
                {
                    try
                    {
                        /*bật các nút về ban đầu*/
                        btnTHEM.Enabled = true;
                        btnXOA.Enabled = true;
                        btnGHI.Enabled = true;
                        btnHOANTAC.Enabled = true;

                        btnLAMMOI.Enabled = true;
                        btnCHUYENChiNhanh.Enabled = true;
                        btnTHOAT.Enabled = true;

                        this.txtMANV.Enabled = false;
                        this.bdsNhanVien.EndEdit();
                        this.nhanVienTableAdapter.Update(this.dataSet.NhanVien);
                        this.gcNhanVien.Enabled = true;

                        /*lưu 1 câu truy vấn để hoàn tác yêu cầu*/
                        String cauTruyVanHoanTac = "";
                        /*trước khi ấn btnGHI là btnTHEM*/
                        if( dangThemMoi == true)
                        {
                            cauTruyVanHoanTac = "" +
                                "DELETE DBO.NHANVIEN " +
                                "WHERE MANV = " + txtMANV.Text.Trim();
                        }
                        /*trước khi ấn btnGHI là sửa thông tin nhân viên*/
                        else
                        {
                            

                            cauTruyVanHoanTac = 
                                "UPDATE DBO.NhanVien "+
                                "SET " +
                                "HO = N'" + ho + "'," +
                                "TEN = N'" + ten + "'," +
                                "SOCMND = '" + cmnd + "'," +
                                "DIACHI = N'" + diaChi + "'," +
                                "NGAYSINH = CAST('" + ngaySinh.ToString("yyyy-MM-dd") + "' AS DATETIME)," +
                                "LUONG = '" + luong + "',"+
                                "TrangThaiXoa = " + trangThai + " " +
                                "WHERE MANV = '" + maNhanVien + "'";
                        }
                        Console.WriteLine(cauTruyVanHoanTac);

                        /*Đưa câu truy vấn hoàn tác vào undoList 
                         * để nếu chẳng may người dùng ấn hoàn tác thì quất luôn*/
                        undoList.Push(cauTruyVanHoanTac);
                        /*cập nhật lại trạng thái thêm mới cho chắc*/
                        dangThemMoi = false;
                        MessageBox.Show("Ghi thành công", "Thông báo", MessageBoxButtons.OK);
                    }
                    catch(Exception ex)
                    {

                        bdsNhanVien.RemoveCurrent();
                        MessageBox.Show("Thất bại. Vui lòng kiểm tra lại!\n" + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            
        }

        private void dteNGAYSINH_Validating(object sender, CancelEventArgs e)
        {
            
        }

        public void chuyenChiNhanh(String ChiNhanh )
        {
            /*Step 1*/
            if ( Program.serverName == ChiNhanh)
            {
                MessageBox.Show("Hãy chọn chi nhánh khác chi nhánh bạn đang đăng nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            /*Step 2*/
            String maChiNhanhMoi = "";
            int viTriHienTai = bdsNhanVien.Position;
            String maNhanVien = ((DataRowView)bdsNhanVien[viTriHienTai])["MANV"].ToString();

            if (ChiNhanh.Contains("1") )
            {
                maChiNhanhMoi = "CN1";
            }
            else
            {
                if (ChiNhanh.Contains("2"))
                {
                    maChiNhanhMoi = "CN2";
                }
            }

            /*Step 4*/
            try
            {
                String cauTruyVan = "EXEC sp_ChuyenChiNhanh " + maNhanVien + ",'" + maChiNhanhMoi + "'";
            SqlCommand sqlcommand = new SqlCommand(cauTruyVan, Program.conn);
                Program.myReader = Program.ExecSqlDataReader(cauTruyVan);
                MessageBox.Show("Chuyển chi nhánh thành công", "thông báo", MessageBoxButtons.OK);
                if (Program.myReader == null)
                {
                    return;/*khong co ket qua tra ve thi ket thuc luon*/
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("thực thi database thất bại!\n\n" + ex.Message, "thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
                return;
            }
            Program.myReader.Close();
            this.nhanVienTableAdapter.Update(this.dataSet.NhanVien);


        }
        private void btnCHUYENChiNhanh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {


            int viTriHienTai = bdsNhanVien.Position;
            bool trangThaiXoa = (bool)(( (DataRowView) (bdsNhanVien[viTriHienTai]) )["TrangThaiXoa"]);
            string maNhanVien = ((DataRowView)(bdsNhanVien[viTriHienTai]))["MANV"].ToString();

            if( maNhanVien == Program.userName)
            {
                MessageBox.Show("Không thể chuyển chính người đang đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }    

            if ( trangThaiXoa == true )
            {
                MessageBox.Show("Nhân viên này không có ở chi nhánh này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            /*Step 2 Kiem tra xem form da co trong bo nho chua*/
            Form f = this.CheckExists(typeof(FormChuyenChiNhanh));
            if (f != null)
            {
                f.Activate();
            }
            FormChuyenChiNhanh form = new FormChuyenChiNhanh();
            form.Show();
            form.branchTransfer = new FormChuyenChiNhanh.MyDelegate(chuyenChiNhanh);
        }
    }
}
