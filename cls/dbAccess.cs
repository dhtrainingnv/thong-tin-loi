using DevExpress.XtraGrid.Views.Grid;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DHIssues.cls
{
    class dbAccess
    {
        NpgsqlConnection con = LibraryApp.ClsConnection.conn;
        DataTable dt;
        NpgsqlDataAdapter adap;
        NpgsqlCommand command;
        string mabv = "";

        public DataTable GetDataTable(string sql)
        {
            try
            {
                adap = new NpgsqlDataAdapter(sql, con);
                dt = new DataTable("Data");
                adap.Fill(dt);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dt;
        }

        public object Get_Object(string sql)
        {
            object value = null;
            dt = new DataTable();
            dt = this.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                value = dt.Rows[0][0];
            }
            else
            {
                value = null;
            }

            dt.Dispose();
            return value;
        }

        public int Excute(string sql)
        {
            int value = -1;
            command = new NpgsqlCommand(sql, con);
            value = command.ExecuteNonQuery();

            return value;
        }

        public object GetRowCellValue(GridView gridView, string fieldName)
        {
            object value = gridView.GetRowCellValue(gridView.FocusedRowHandle, fieldName);
            return value;
        }

        public string StringIssues_BN()
        {
            string Chitietloiphatsinh = "###### ![chi_tiet_loi](https://img.shields.io/badge/Chi%20tiết%20lỗi%20phát%20sinh-:-blue?style=for-the-badge&logo=github)";
            string Chitietbenhnhan = "-  ![loi_benh_nhan](https://img.shields.io/badge/Chi%20tiết%20bệnh%20nhân-:-blue?style=plastic&logo=github)";
            string strBN = "> _**MaBN**_: `" + Mabn + "` -";
                    strBN += "_**MaKB**_: `"+Makb+ "` - ";
                    strBN += "_**MaBA**_: `" + Maba + "` - ";
                    strBN += "_**NgayKCB**_: `" + Ngaykcb + "` - ";
                    strBN += "_**MaPhong**_: `" + Maphong + "` - ";
                    strBN += "_**MaDV**_: `" + Madv + "` - ";
                    strBN += "_**ThangKT**_: `" + Thangkt + "` - ";
                    strBN += "_**NamKT**_: `" + Namkt + "`";

            return Chitietloiphatsinh + "\n\n" + Chitietbenhnhan + "\n" + strBN;
        }

        public string StringIssues_Thuoc()
        {
            string Chitietthuoc = "\n-  ![loi_thuoc](https://img.shields.io/badge/Chi%20tiết%20thuốc-:-blue?style=plastic&logo=github) \n";

            string strThuoc = "> _**KhoChan**_: `"+Khochan+"` - ";
            strThuoc += "_**KhoLe**_: `" + Khole + "` - ";
            strThuoc += "_**MaHH**_: `" + Mahh + "` - ";
            strThuoc += "_**SoHD**_: `" + Sohd + "` - ";
            strThuoc += "_*NgayHD**_: `" + Ngayhd + "` - ";
            strThuoc += "_**ThangKT**_: `" + T_thangkt + "` - ";
            strThuoc += "_**NamKT**_: `" + T_namkt + "`";           

            return Chitietthuoc + "\n" + strThuoc;
        }

        public string StringIssues_CLS()
        {
            string ChitietCLS = "\n- ![loi_cls](https://img.shields.io/badge/Chi%20tiết%20CLS-:-blue?style=plastic&logo=github) \n";

            string strCLS = "> _**MaCLS**_: `"+Macls+"` - ";
                    strCLS += "_**PhieuYC**_: `"+ Phieuyc + "`";
            return ChitietCLS + "\n" + strCLS;
        }

        public string StringIssues_Khac()
        {
            string Chitietbosung = " \n-  ![loi_bo_sung](https://img.shields.io/badge/Chi%20tiết%20bổ%20sung-:-blue?style=plastic&logo=github)\n";

            string strKhac = "> _**Khac**_: `"+ Khac + "`";
            
            return Chitietbosung + "\n" + strKhac;
        }





        string mabn;
        string makb;
        string maba;
        string ngaykcb;
        string hoten;
        string madv;
        string maphong;
        string thangkt;
        string namkt;
        string bant;



        #region Field
        public string Mabn
        {
            get
            {
                return mabn;
            }

            set
            {
                mabn = value;
            }
        }

        public string Maba
        {
            get
            {
                return maba;
            }

            set
            {
                maba = value;
            }
        }

        public string Ngaykcb
        {
            get
            {
                return ngaykcb;
            }

            set
            {
                ngaykcb = value;
            }
        }

        public string Hoten
        {
            get
            {
                return hoten;
            }

            set
            {
                hoten = value;
            }
        }

        public string Madv
        {
            get
            {
                return madv;
            }

            set
            {
                madv = value;
            }
        }

        public string Maphong
        {
            get
            {
                return maphong;
            }

            set
            {
                maphong = value;
            }
        }

        public string Thangkt
        {
            get
            {
                return thangkt;
            }

            set
            {
                thangkt = value;
            }
        }

        public string Namkt
        {
            get
            {
                return namkt;
            }

            set
            {
                namkt = value;
            }
        }

        public string Khochan
        {
            get
            {
                return khochan;
            }

            set
            {
                khochan = value;
            }
        }

        public string Khole
        {
            get
            {
                return khole;
            }

            set
            {
                khole = value;
            }
        }

        public string Mahh
        {
            get
            {
                return mahh;
            }

            set
            {
                mahh = value;
            }
        }

        public string Sohd
        {
            get
            {
                return sohd;
            }

            set
            {
                sohd = value;
            }
        }

        public string Ngayhd
        {
            get
            {
                return ngayhd;
            }

            set
            {
                ngayhd = value;
            }
        }

        public string T_thangkt
        {
            get
            {
                return t_thangkt;
            }

            set
            {
                t_thangkt = value;
            }
        }

        public string T_namkt
        {
            get
            {
                return t_namkt;
            }

            set
            {
                t_namkt = value;
            }
        }

        public string Macls
        {
            get
            {
                return macls;
            }

            set
            {
                macls = value;
            }
        }

        public string Phieuyc
        {
            get
            {
                return phieuyc;
            }

            set
            {
                phieuyc = value;
            }
        }

        public string Khac
        {
            get
            {
                return khac;
            }

            set
            {
                khac = value;
            }
        }

        public string Makb
        {
            get
            {
                return makb;
            }

            set
            {
                makb = value;
            }
        }

        public string Bant
        {
            get
            {
                return bant;
            }

            set
            {
                bant = value;
            }
        }
        #endregion

        string khochan;
        string khole;
        string mahh;
        string sohd;
        string ngayhd;
        string t_thangkt;
        string t_namkt;
        string macls;
        string phieuyc;
        string khac;
    }
}
