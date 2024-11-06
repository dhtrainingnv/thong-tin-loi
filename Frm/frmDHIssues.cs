using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DHIssues.cls;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DHIssues.Frm
{
    public partial class frmDHIssues : Form
    {
        NpgsqlConnection con = LibraryApp.ClsConnection.conn;
        LibraryApp.FrmCauHinh frmCauHinh;
        dbAccess access = new dbAccess();
        DataTable dt;
        

        public frmDHIssues()
        {
            InitializeComponent();
        }

        private void frmDHIssues_Load(object sender, EventArgs e)
        {
            this.textEdit_Makb.Focus();
            
            this.groupControl_khambenh.Size = new Size(787, 120);
            this.groupControl_thuoc.Size = new Size(787, 120);
            this.groupControl_CLS.Size = new Size(787, 90);
            this.gridControl_khambenh.Visible = false;
            this.gridControl_thuoc.Visible = false;
            this.gridControl_CLS.Visible = false;
        }

        
        private void textEdit_Makb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {                
                if (this.textEdit_Makb.Text != "")
                {
                    LoadBN(this.textEdit_Makb.Text.Trim(),"","");
                }
            }
        }

        private void LoadBN(string makb, string mabn, string maba)
        {
            try
            {


                string sql = "SELECT dk.mabn, dk.makb, COALESCE(dk.maba,'') AS maba, ";
                sql += " to_char(kb.ngaykcb, 'DD/MM/YYYY') AS ngaykcb, bn.holot || ' ' || bn.ten AS hoten, ";
                sql += "  CASE WHEN COALESCE(dk.maba, '') = '' THEN kb.madv ";
                sql += "  WHEN COALESCE(dk.maba) != '' AND dk.bant = 1 THEN kb.madv ELSE nt.madv END AS madv, ";
                sql += "  CASE WHEN COALESCE(dk.maba) != '' AND dk.bant = 0 THEN '' ELSE kb.maphong END AS maphong, dk.thangkt, dk.namkt,dk.bant ";
                sql += "  FROM current.psdangky dk LEFT JOIN ";
                sql += "  current.khambenh kb ON kb.mabn = dk.mabn AND kb.makb = dk.makb LEFT JOIN ";
                sql += "  current.dmbenhnhan bn ON bn.mabn = dk.mabn LEFT JOIN ";
                sql += "   current.bnnoitru nt ON nt.mabn = dk.mabn AND nt.makb = dk.makb AND nt.maba = COALESCE(dk.maba) ";
                sql += "  WHERE dk.makb = '" + makb + "'";

                if (mabn != "")
                {
                    sql += "  AND dk.mabn = '" + mabn + "'";
                }

                if (maba != "")
                {
                    sql += "  AND COALESCE(dk.maba,'') = '" + maba + "'";
                }



                dt = new DataTable();
                dt = access.GetDataTable(sql);
                if (dt.Rows.Count == 1)
                {
                    this.groupControl_khambenh.Size = new Size(787, 115);
                    this.gridControl_khambenh.Visible = false;

                    SetTextBN(dt.Rows[0]["mabn"].ToString().Trim(),
                        dt.Rows[0]["maba"].ToString().Trim(),
                        dt.Rows[0]["ngaykcb"].ToString().Trim(),
                        dt.Rows[0]["hoten"].ToString().Trim(),
                        dt.Rows[0]["madv"].ToString().Trim(),
                        dt.Rows[0]["maphong"].ToString().Trim(),
                        dt.Rows[0]["thangkt"].ToString().Trim(),
                        dt.Rows[0]["namkt"].ToString().Trim(),
                        dt.Rows[0]["bant"].ToString().Trim());


                }
                else if (dt.Rows.Count > 1)
                {
                    this.groupControl_khambenh.Size = new Size(787, 195);
                    //this.gridControl_khambenh.DataSource = dt;
                    this.gridControl_khambenh.Visible = true;

                    //if (this.gridView_khambenh.RowCount > 0)
                    //{
                    //    this.gridView_khambenh.FocusedRowHandle = 0;
                    //}
                }
                else
                {
                    this.groupControl_khambenh.Size = new Size(787, 115);
                    this.gridControl_khambenh.Visible = false;
                    SetTextBN("", "", "", "", "", "", "", "", "");
                }

                this.gridControl_khambenh.DataSource = dt;
                if (this.gridView_khambenh.RowCount > 0)
                {
                    this.gridView_khambenh.FocusedRowHandle = 0;
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetAll();
            }
        }

        private void SetTextBN(string mabn, string maba, string ngaykcb, string hoten, string madv, string maphong, string thangkt, string namkt,string bant)
        {
            this.textEdit_Mabn.Text = mabn;
            this.textEdit_Maba.Text = maba;
            this.textEdit_hoten.Text = hoten;
            this.textEdit_ngaykcb.Text = ngaykcb;
            this.textEdit_maphong.Text = maphong;
            this.textEdit_madv.Text = madv;
            this.textEdit_thangkt.Text = thangkt;
            this.textEdit_namkt.Text = namkt;
            this.textEdit_bant.Text = bant;
        }

       

        private void barButtonItem_CauHinhDuLieu_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmCauHinh = new LibraryApp.FrmCauHinh();
            frmCauHinh.ShowDialog();
        }

        private void textEdit_Makb_EditValueChanged(object sender, EventArgs e)
        {
            if (this.textEdit_Makb.Text.Trim() == "")
            {
                ResetAll();
            }
        }

        private void gridView_khambenh_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (this.gridView_khambenh.FocusedRowHandle >=0)
            {
                #region SetText
                if (access.GetRowCellValue(this.gridView_khambenh,"mabn") != null)
                {
                    access.Mabn = access.GetRowCellValue(this.gridView_khambenh, "mabn").ToString().Trim();
                }
                else
                {
                    access.Mabn = "";
                }

                if (access.GetRowCellValue(this.gridView_khambenh, "maba") != null)
                {
                    access.Maba = access.GetRowCellValue(this.gridView_khambenh, "maba").ToString().Trim();
                }
                else
                {
                    access.Maba = "";
                }

                if (access.GetRowCellValue(this.gridView_khambenh, "hoten") != null)
                {
                    access.Hoten = access.GetRowCellValue(this.gridView_khambenh, "hoten").ToString().Trim();
                }
                else
                {
                    access.Hoten = "";
                }
                if (access.GetRowCellValue(this.gridView_khambenh, "ngaykcb") != null)
                {
                    access.Ngaykcb = access.GetRowCellValue(this.gridView_khambenh, "ngaykcb").ToString().Trim();
                }
                else
                {
                    access.Ngaykcb = "";
                }
                if (access.GetRowCellValue(this.gridView_khambenh, "maphong") != null)
                {
                    access.Maphong = access.GetRowCellValue(this.gridView_khambenh, "maphong").ToString().Trim();
                }
                else
                {
                    access.Maphong = "";
                }
                if (access.GetRowCellValue(this.gridView_khambenh, "madv") != null)
                {
                    access.Madv = access.GetRowCellValue(this.gridView_khambenh, "madv").ToString().Trim();
                }
                else
                {
                    access.Madv = "";
                }

                if (access.GetRowCellValue(this.gridView_khambenh, "thangkt") != null)
                {
                    access.Thangkt = access.GetRowCellValue(this.gridView_khambenh, "thangkt").ToString().Trim();
                }
                else
                {
                    access.Thangkt = "";
                }

                if (access.GetRowCellValue(this.gridView_khambenh, "namkt") != null)
                {
                    access.Namkt = access.GetRowCellValue(this.gridView_khambenh, "namkt").ToString().Trim();
                }
                else
                {
                    access.Namkt = "";
                }

                if (access.GetRowCellValue(this.gridView_khambenh, "bant") != null)
                {
                    access.Bant = access.GetRowCellValue(this.gridView_khambenh, "bant").ToString().Trim();
                }
                else
                {
                    access.Bant = "";
                }
                #endregion
                SetTextBN(access.Mabn, access.Maba, access.Ngaykcb, access.Hoten, access.Madv, access.Maphong, access.Thangkt, access.Namkt, access.Bant);
            }
        }

        private void dropDownButton_Tao_Click(object sender, EventArgs e)
        {
            #region BENH_NHAN
            if (this.textEdit_Mabn.Text.Trim() == "")
            {
                access.Mabn = "MaBN";
            }else
            {
                access.Mabn = this.textEdit_Mabn.Text.Trim();
            }

            if (this.textEdit_Makb.Text.Trim() == "")
            {
                access.Makb = "MaKB";
            }
            else
            {
                access.Makb = this.textEdit_Makb.Text.Trim();
            }

            if (this.textEdit_Maba.Text.Trim() == "")
            {
                access.Maba = "MaBA";
            }
            else
            {
                access.Maba = this.textEdit_Maba.Text.Trim();
            }

            if (this.textEdit_ngaykcb.Text.Trim() == "")
            {
                access.Ngaykcb = "NgayKCB";
            }
            else
            {
                access.Ngaykcb = this.textEdit_ngaykcb.Text.Trim();
            }

            if (this.textEdit_maphong.Text.Trim() == "")
            {
                access.Maphong = "Phong";
            }
            else
            {
                access.Maphong = this.textEdit_maphong.Text.Trim();
            }

            if (this.textEdit_madv.Text.Trim() == "")
            {
                access.Madv = "MaDV";
            }
            else
            {
                access.Madv = this.textEdit_madv.Text.Trim();
            }

            if (this.textEdit_thangkt.Text.Trim() == "")
            {
                access.Thangkt = "ThangKT";
            }
            else
            {
                access.Thangkt = this.textEdit_thangkt.Text.Trim();
            }

            if (this.textEdit_namkt.Text.Trim() == "")
            {
                access.Namkt = "NamKT";
            }
            else
            {
                access.Namkt = this.textEdit_namkt.Text.Trim();
            }
            #endregion

            #region THUOC
            if (this.textEdit_khochan.Text.Trim() == "")
            {
                access.Khochan = "KhoChan";
            }
            else
            {
                access.Khochan = this.textEdit_khochan.Text.Trim();
            }

            if (this.textEdit_khole.Text.Trim() == "")
            {
                access.Khole = "KhoLe";
            }
            else
            {
                access.Khole = this.textEdit_khole.Text.Trim();
            }

            if (this.textEdit_mahh.Text.Trim() == "")
            {
                access.Mahh = "MaHH";
            }
            else
            {
                access.Mahh = this.textEdit_mahh.Text.Trim();
            }

            if (this.textEdit_sohd.Text.Trim() == "")
            {
                access.Sohd = "SoHD";
            }
            else
            {
                access.Sohd = this.textEdit_sohd.Text.Trim();
            }

            if (this.textEdit_ngayhd.Text.Trim() == "")
            {
                access.Ngayhd = "NgayHD";
            }
            else
            {
                access.Ngayhd = this.textEdit_ngayhd.Text.Trim();
            }

            if (this.textEdit_hdthangkt.Text.Trim() == "")
            {
                access.T_thangkt = "ThangKT";
            }
            else
            {
                access.T_thangkt = this.textEdit_hdthangkt.Text.Trim();
            }

            if (this.textEdit_hdnamkt.Text.Trim() == "")
            {
                access.T_namkt = "NamKT";
            }
            else
            {
                access.T_namkt = this.textEdit_hdnamkt.Text.Trim();
            }

            #endregion

            #region CLS + KHAC
            if (this.textEdit_macls.Text.Trim() == "")
            {
                access.Macls = "MaCLS";
            }
            else
            {
                access.Macls = this.textEdit_macls.Text.Trim();
            }

            if (this.textEdit_phieuyc.Text.Trim() == "")
            {
                access.Phieuyc = "PhieuYC";
            }
            else
            {
                access.Phieuyc = this.textEdit_phieuyc.Text.Trim();
            }

            if (this.richTextBox_bosung.Text.Trim() == "")
            {
                access.Khac = "Khac";
            }
            else
            {
                access.Khac = this.richTextBox_bosung.Text.Trim();
            }

            #endregion


            this.richTextBox_issuestring.ResetText();

            string result = access.StringIssues_BN(access.Mabn, access.Makb, access.Maba,access.Ngaykcb, access.Maphong, access.Madv, access.Thangkt, access.Namkt);
            result += access.StringIssues_Thuoc(access.Khochan, access.Khole, access.Mahh, access.Sohd, access.Ngayhd, access.T_thangkt, access.T_namkt);
            result += access.StringIssues_CLS(access.Macls, access.Phieuyc);

            result += access.StringIssues_Khac(access.Khac);

            this.richTextBox_issuestring.Text = result;
        }

        private void checkBox_thuoc_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox_thuoc.Checked == true )
            {
                if (this.checkBox_benhnhan.Checked == true & this.textEdit_ngaykcb.Text.Trim() != "")
                {
                    
                    string kh = "";
                    if (this.textEdit_Maba.Text.Trim() != "")
                    {
                        if (this.textEdit_bant.Text.Trim() == "1")
                        {
                            kh = this.textEdit_Makb.Text.Trim();
                        }
                        else
                        {
                            kh = this.textEdit_Maba.Text.Trim();
                        }
                    }
                    
                    LoadThuoc(this.textEdit_Mabn.Text.Trim(),kh,"");
                    this.groupControl_thuoc.Size = new Size(787, 195);
                    this.gridControl_thuoc.Visible = true;
                }
                else
                {

                    this.checkBox_CLS.Checked = false;
                                  
                }

            }
            else
            {               
              
                this.groupControl_thuoc.Size = new Size(787, 120);
                this.gridControl_thuoc.Visible = false;
                SetTextThuoc("","","","","","","");
            }
        }

        private void LoadThuoc(string mabn, string makh, string sohd)
        {
            
            string sqlThuoc = "";
            //lấy thuốc theo đợt khám
            if (this.checkBox_benhnhan.Checked == true)
            {                
                sqlThuoc = "SELECT ct.mabn,ct.makh, ct.sohd, to_char(ct.ngayhd,'DD/MM/YYYY') AS ngayhd, ct.thangkt, ct.namkt,";
                sqlThuoc += " xn.mahh, dm.tenhh, dm.dvt, xn.soluong, ct.loaixn,ct.khochan, ct.khole,ct.noitru";
                sqlThuoc += " FROM current.chungtu ct LEFT JOIN";
                sqlThuoc += " current.pshdxn xn ON xn.sohd = ct.sohd LEFT JOIN";
                sqlThuoc += " current.dmthuoc dm ON dm.mahh = xn.mahh";
                sqlThuoc += " WHERE ct.makh = '" + makh + "' AND ct.mabn = '" + mabn + "'";
                sqlThuoc += " AND xn.xoa = 0";
                sqlThuoc += " AND ct.xoa = 0";
                sqlThuoc += " ORDER BY ct.ngayhd, xn.stt ";
            }

            //lấy theo chứng từ
            if (sohd != "")
            {
                sqlThuoc = "SELECT ct.mabn,ct.makh, ct.sohd, to_char(ct.ngayhd,'DD/MM/YYYY') AS ngayhd, ct.thangkt, ct.namkt,";
                sqlThuoc += " xn.mahh, dm.tenhh, dm.dvt, xn.soluong, ct.loaixn, ct.khochan, ct.khole, ct.noitru";
                sqlThuoc += " FROM current.chungtu ct LEFT JOIN";
                sqlThuoc += " current.pshdxn xn ON xn.sohd = ct.sohd LEFT JOIN";
                sqlThuoc += " current.dmthuoc dm ON dm.mahh = xn.mahh";
                sqlThuoc += " WHERE ct.sohd = '" + sohd + "'";
                sqlThuoc += " AND xn.xoa = 0";
                sqlThuoc += " AND ct.xoa = 0";
                sqlThuoc += " ORDER BY ct.ngayhd, xn.stt ";
            }

            //lấy theo tồn kho
            if (mabn == "" & makh == "" & sohd == "")
            {
                

            }

            dt = new DataTable("THUOC");
            dt = access.GetDataTable(sqlThuoc);
            this.gridControl_thuoc.DataSource = dt;
        }

        private void checkBox_benhnhan_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox_benhnhan.Checked == true)
            {
                
            }
            else
            {
                this.textEdit_Makb.Text = "";
                this.groupControl_khambenh.Size = new Size(787, 115);
                this.gridControl_khambenh.Visible = false;
                SetTextBN("","","","","","","","","");
                
                this.groupControl_CLS.Size = new Size(1000,90);
                this.gridControl_CLS.Visible = false;
                this.checkBox_thuoc.Checked = false;
                this.checkBox_CLS.Checked = false;
            }
        }

        private void gridView_thuoc_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (this.gridView_thuoc.RowCount > 0)
                {
                    if (this.gridView_thuoc.FocusedRowHandle >= 0)
                    {
                        #region SET PARA
                        if (access.GetRowCellValue(this.gridView_thuoc, "khochan") != null)
                        {
                            access.Khochan = access.GetRowCellValue(this.gridView_thuoc, "khochan").ToString().Trim();
                        }
                        else
                        {
                            access.Khochan = "";
                        }

                        if (access.GetRowCellValue(this.gridView_thuoc, "khole") != null)
                        {
                            access.Khole = access.GetRowCellValue(this.gridView_thuoc, "khole").ToString().Trim();
                        }
                        else
                        {
                            access.Khole = "";
                        }

                        if (access.GetRowCellValue(this.gridView_thuoc, "mahh") != null)
                        {
                            access.Mahh = access.GetRowCellValue(this.gridView_thuoc, "mahh").ToString().Trim();
                        }
                        else
                        {
                            access.Mahh = "";
                        }

                        if (access.GetRowCellValue(this.gridView_thuoc, "sohd") != null)
                        {
                            access.Sohd = access.GetRowCellValue(this.gridView_thuoc, "sohd").ToString().Trim();
                        }
                        else
                        {
                            access.Sohd = "";
                        }

                        if (access.GetRowCellValue(this.gridView_thuoc, "ngayhd") != null)
                        {
                            access.Ngayhd = access.GetRowCellValue(this.gridView_thuoc, "ngayhd").ToString().Trim();
                        }
                        else
                        {
                            access.Ngayhd = "";
                        }

                        if (access.GetRowCellValue(this.gridView_thuoc, "thangkt") != null)
                        {
                            access.T_thangkt = access.GetRowCellValue(this.gridView_thuoc, "thangkt").ToString().Trim();
                        }
                        else
                        {
                            access.T_thangkt = "";
                        }

                        if (access.GetRowCellValue(this.gridView_thuoc, "namkt") != null)
                        {
                            access.T_namkt = access.GetRowCellValue(this.gridView_thuoc, "namkt").ToString().Trim();
                        }
                        else
                        {
                            access.T_namkt = "";
                        }
                        #endregion

                        SetTextThuoc(access.Khochan, access.Khole, access.Mahh, access.Sohd, access.Ngayhd, access.T_thangkt, access.T_namkt);
                        if (access.GetRowCellValue(this.gridView_thuoc, "mabn") != null & access.GetRowCellValue(this.gridView_thuoc, "makh") != null & access.GetRowCellValue(this.gridView_thuoc, "noitru") != null)
                        {
                            
                        }

                    }

                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetAll();
            }
        }

        private void SetTextThuoc(string khochan, string khole, string mahh, string sohd, string ngayhd, string thangkt, string namkt)
        {
            this.textEdit_khochan.Text = khochan;
            this.textEdit_khole.Text = khole;
            this.textEdit_mahh.Text = mahh;
            this.textEdit_sohd.Text = sohd;
            this.textEdit_ngayhd.Text = ngayhd;
            this.textEdit_hdthangkt.Text = thangkt;
            this.textEdit_hdnamkt.Text = namkt;
        }

        private void textEdit_sohd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.textEdit_sohd.Text.Trim() != "")
                {
                    this.groupControl_thuoc.Size = new Size(787, 195);
                    this.gridControl_thuoc.Visible = true;
                    LoadThuoc("","", this.textEdit_sohd.Text.Trim());

                }
            }
        }

        private void checkBox_CLS_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox_CLS.Checked == true)
            {
                if (this.checkBox_benhnhan.Checked == true)
                {

                    this.groupControl_CLS.Size = new Size(787, 185);
                    this.gridControl_CLS.Visible = true;
                    //Load CLS
                    LoadCLS(this.textEdit_Mabn.Text.Trim(),this.textEdit_Makb.Text.Trim(),"");
                }
                else
                {
                    
                }
            }
            else
            {
                this.groupControl_CLS.Size = new Size(787, 90);
                this.gridControl_CLS.Visible = false;
            }
        }

        private void LoadCLS(string mabn, string makb, string macls)
        {
            try
            {
                string sqlCLS = "SELECT cd.macls, dm.tencls, cd.soluong, cd.dongia, cd.giabh, cd.thanhtien, ";
                sqlCLS += " to_char(cd.ngaykcb, 'DD/MM/YYYY HH24:MI:SS') AS ngaykcd, cd.noitru";
                sqlCLS += " FROM current.chidinhcls cd LEFT JOIN";
                sqlCLS += " current.dmcls dm ON dm.macls = cd.macls";
                sqlCLS += " WHERE cd.makb = '2410001574';";
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetAll();
            }
            
        }

        private void textEdit_mahh_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void simpleButton_reset_Click(object sender, EventArgs e)
        {
            ResetAll();
        }

        private void ResetAll()
        {
            SetTextBN("", "", "", "", "", "", "", "", "");
            SetTextThuoc("", "", "", "", "", "", "");
            this.richTextBox_issuestring.Text = "";
            this.richTextBox_bosung.Text = "";
            this.textEdit_Makb.Text = "";


            this.groupControl_khambenh.Size = new Size(787, 120);
            this.groupControl_thuoc.Size = new Size(787, 120);
            this.groupControl_CLS.Size = new Size(787, 90);
            this.gridControl_khambenh.Visible = false;
            this.gridControl_thuoc.Visible = false;
            this.gridControl_CLS.Visible = false;
            this.textEdit_Makb.Focus();

            this.gridView_khambenh.Columns.Clear();
            this.gridView_thuoc.Columns.Clear();
            this.gridView_CLS.Columns.Clear();

            this.gridControl_khambenh.DataSource = null;
            this.gridControl_thuoc.DataSource = null;
            this.gridControl_CLS.DataSource = null;

            





        }

        private void gridView_CLS_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (this.gridView_CLS.RowCount > 0)
            {
                if (access.GetRowCellValue(this.gridView_CLS,"macls") != null)
                {
                    this.textEdit_macls.Text = access.GetRowCellValue(this.gridView_CLS, "macls").ToString().Trim();
                }
            }
        }
    }
}
