using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace QLVT.ReportForm
{
    public partial class ReportDonHangKhongPhieuNhap : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportDonHangKhongPhieuNhap()
        {
            InitializeComponent();
            this.txtLapBaoCaoBoi.Text = Program.staff;
            this.txtMaNguoiLapBaoCao.Text = Program.userName;
            this.sqlDataSource1.Connection.ConnectionString = Program.connstr;
            this.sqlDataSource1.Fill();
        }

    }
}
