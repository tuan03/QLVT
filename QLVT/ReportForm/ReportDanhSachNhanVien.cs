﻿using DevExpress.XtraReports.UI;
using QLVT;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace QLTVT.ReportForm
{
    public partial class ReportDanhSachNhanVien : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportDanhSachNhanVien()
        {
            InitializeComponent();
            this.sqlDataSource1.Connection.ConnectionString = Program.connstr;
            this.sqlDataSource1.Fill();
        }

    }
}
