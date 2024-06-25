using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT.ReportForm
{
    public partial class FormTongHopNhapXuat : Form
    {
        public FormTongHopNhapXuat()
        {
            InitializeComponent();
        }

        private void FormTongHopNhapXuat_Load(object sender, EventArgs e)
        {
            /*Step 2*/
            cmbChiNhanh.DataSource = Program.bindingSource;/*sao chep bingding source tu form dang nhap*/
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.brand;

            dteTuNgay.EditValue = "01-05-2000";
            dteToiNgay.EditValue = DateTime.Today.ToString("dd-MM-yyyy");

            if (Program.role == "CongTy")
            {
                cmbChiNhanh.Enabled = true;
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
            if (cmbChiNhanh.SelectedIndex != Program.brand)
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

            if (Program.KetNoi() == 0)
            {
                MessageBox.Show("Xảy ra lỗi kết nối với chi nhánh hiện tại", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime fromDate = (DateTime)dteTuNgay.DateTime;
            DateTime toDate = (DateTime)dteToiNgay.DateTime;
            string ChiNhanh = cmbChiNhanh.SelectedValue.ToString().Contains("1") ? "Chi Nhánh 1" : "Chi Nhánh 2";

            ReportTongHopNhapXuat report = new ReportTongHopNhapXuat(fromDate, toDate);
            report.txtTuNgay.Text = dteTuNgay.EditValue.ToString();
            report.txtToiNgay.Text = dteToiNgay.EditValue.ToString();
            report.txtChiNhanh.Text = ChiNhanh;

            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreviewDialog();
        }
    }
}
