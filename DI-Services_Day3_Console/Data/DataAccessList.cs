using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Text;
using DI_Services_Day3_Console.Models;
using DI_Services_Day3_Console.Utilities;
using DI_Services_Day3_Console.Data.InterFace;

namespace DI_Services_Day3_Console.Data
{
    public class DataAccessList : ISinhVienDAL
    {
        //thuộc tính 
        #region attribute
        private List<SinhVien> _lSinhVien = new List<SinhVien>();
        private List<MonHoc> _lMonHoc = new List<MonHoc>();
        private List<BangDiem> _lBangDiem = new List<BangDiem>();
        private DataTable _TableBangDiem;
        private string ServerName = "Phat-MSI";
        private string DatabaseName = "DatabaseSinhVien";
        #endregion

        //constructor
        #region Constructor
        public DataAccessList()
        {
            ConnectDataBaseSinhVien();
        }
        #endregion

        //hàm kết nối tới Database
        #region Kết nối database + truy xuất dữ liệu
        //Hàm kết nối sqlDataBase
        public void ConnectDataBaseSinhVien()
        {
            try
            {
                //nhập vào Server Name, Database Name để lấy thông tin kết nối
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[!] Để trống ServerName nếu muốn lấy tên Server mặc định tự nhận theo máy !");
                Console.ForegroundColor = ConsoleColor.Green; Console.Write("[>] "); Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Nhập vào Server Name: ");
                ServerName = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Green; Console.Write("[>] "); Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Nhập vào Database Name (DatabaseSinhVien): ");
                DatabaseName = Console.ReadLine();
                Console.ResetColor();
                //thông báo đang kết nối
                ExceptionNotice.WarningConnectingDB();
                //kết nối tới database và mở kết nối
                SqlConnection sqlCon = new SqlConnection(@"Data Source=" + ServerName + ";Initial Catalog=" + DatabaseName + ";Integrated Security=True");
                if (sqlCon.ConnectionTimeout > 15)//kiểm tra kết nối database quá 20 đơn vị thời gian sẽ ngắt kết nối và báo lỗi không thể kết nối
                    throw ExceptionNotice.ExceptionConnectDatabase();
                sqlCon.Open();
                //thông báo kết nối khi thành công 
                ExceptionNotice.SuccessConnectDatabase();Thread.Sleep(2000);
                //thực hiện truy vấn lấy dữ liệu từ các bảng dữ liệu SinhVien, MonHoc, BangDiem
                //truy vấn Bảng SinhVien
                SqlDataAdapter sqlquery1 = new SqlDataAdapter("Select * from SinhVien", sqlCon);
                //truy vấn Bảng Monhoc
                SqlDataAdapter sqlquery2 = new SqlDataAdapter("Select * from MonHoc", sqlCon);
                //truy vấn Bảng BangDiem
                SqlDataAdapter sqlquery3 = new SqlDataAdapter("Select BD.MaMonHoc,BD.MaSinhVien,MH.LoaiMon,BD.DiemQuaTrinh,BD.DiemThanhPhan from BangDiem as BD,MonHoc as MH WHERE MH.MaMonHoc = BD.MaMonHoc", sqlCon);
                //tạo các datatable để chứa dữ liệu từ truy vấn dữ liệu bảng
                DataTable dataTable1 = new DataTable();//bảng sinh viên
                DataTable dataTable2 = new DataTable();//bảng môn học
                DataTable dataTable3 = new DataTable();//bảng điểm
                //nạp dữ liệu vào bảng dataTable
                sqlquery1.Fill(dataTable1);//bảng sinh viên 
                sqlquery2.Fill(dataTable2);//bảng môn học
                sqlquery3.Fill(dataTable3);//bảng điểm
                //duyệt và nạp dữ liệu vào chương trình ứng với các List SinhVien, MonHoc, BangDiem 
                foreach (DataRow row in dataTable1.Rows)
                {
                    //nạp data table SinhVien
                    SinhVien tempSV = new SinhVien(
                        Convert.ToString(row["MaSinhVien"]),
                        Convert.ToString(row["TenSinhVien"]),
                        Convert.ToBoolean(row["GioiTinh"]),
                        Convert.ToDateTime(row["NgaySinh"]),
                        Convert.ToString(row["Lop"]),
                        Convert.ToString(row["Khoa"]));
                    _lSinhVien.Add(tempSV);
                }
                foreach (DataRow row in dataTable2.Rows)
                {
                    //nạp data table MonHoc
                    MonHoc tempMH = new MonHoc(
                        Convert.ToString(row["MaMonHoc"]),
                        Convert.ToString(row["TenMonHoc"]),
                        Convert.ToString(row["SoTiet"]),
                        Convert.ToString(row["LoaiMon"]));
                    _lMonHoc.Add(tempMH);
                }
                foreach (DataRow row in dataTable3.Rows)
                {
                    //nạp data table BangDiem
                    BangDiem tempBD = new BangDiem(
                        Convert.ToString(row["MaMonHoc"]),
                        Convert.ToString(row["MaSinhvien"]),
                        Convert.ToString(row["LoaiMon"]),
                        float.Parse(Convert.ToString(row["DiemQuaTrinh"])),
                        float.Parse(Convert.ToString(row["DiemThanhPhan"]))
                        );
                    _lBangDiem.Add(tempBD);
                }
                //đóng kết nối database
                sqlCon.Close();
                Console.Clear();
            }
            //khi lỗi trong quá trình kết nối database xảy ra catch sẽ được thực thi thông báo lỗi
            catch (Exception e)
            {
                ExceptionNotice.ColorError();
                Console.WriteLine(e.Message);
                ExceptionNotice.InfoExit();
                Console.ReadLine();
                Environment.Exit(0);
            }
        }
        //Hàm Reset BangDiem mỗi khi có cập nhật dữ liệu
        public void resetDataTableBangDiem()
        {
            //reset lại dữ liệu bảng điểm
            _TableBangDiem.Reset();
            //kết nối database
            SqlConnection sqlCon = new SqlConnection(@"Data Source=" + ServerName + ";Initial Catalog=" + DatabaseName + ";Integrated Security=True");
            sqlCon.Open();
            //truy vấn dữ liệu từ bảng BangDiem để nạp lại dữ liệu mới khi đã có dữ liệu thay đổi
            SqlDataAdapter sqlquery3 = new SqlDataAdapter("Select BD.MaMonHoc,BD.MaSinhVien,MH.LoaiMon,BD.DiemQuaTrinh,BD.DiemThanhPhan from BangDiem as BD,MonHoc as MH WHERE MH.MaMonHoc = BD.MaMonHoc", sqlCon);
            _lBangDiem.Clear();
            sqlquery3.Fill(_TableBangDiem);
            foreach (DataRow row in _TableBangDiem.Rows)
            {
                //nạp data table BangDiem
                BangDiem tempBD = new BangDiem(
                    Convert.ToString(row["MaMonHoc"]),
                    Convert.ToString(row["MaSinhvien"]),
                    Convert.ToString(row["LoaiMon"]),
                    float.Parse(Convert.ToString(row["DiemQuaTrinh"])),
                    float.Parse(Convert.ToString(row["DiemThanhPhan"]))
                    );
                _lBangDiem.Add(tempBD);
            }
            //đóng kết nối database
            sqlCon.Close();
        }
        //update database
        public void updateScoreDB(string maSV, string maMH ,float diemQT, float diemTP)
        {
            //thực thi nhập điểm
            try
            {
                //cập nhật - chỉnh sửa điểm
                StringBuilder sqlQuery = new StringBuilder();
                sqlQuery.Append("UPDATE BangDiem SET DiemQuaTrinh = " + diemQT + ", DiemThanhPhan = " + diemTP + " WHERE MaMonHoc = '" + maMH + "' AND MaSinhVien ='" + maSV + "'");
                string Connect = "Data Source=" + ServerName + ";Initial Catalog=DatabaseSinhVien;Integrated Security=True;";
                SqlConnection ConnectDatabase = new SqlConnection(Connect);
                ConnectDatabase.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery.ToString(), ConnectDatabase))
                {
                    command.ExecuteNonQuery();
                }
                ConnectDatabase.Close();
            }
            catch
            {
                ExceptionNotice.ExceptionConnectDatabase();
                return;
            }
        }
        #endregion

        //method select get All
        #region Method Select Object
        //method select all sinh vien
        public List<SinhVien> GetAllSinhVien() => _lSinhVien;
        //method select all Mon Hoc
        public List<MonHoc> GetAllMonHoc() => _lMonHoc;
        //method select all Bang Diem
        public List<BangDiem> GetAllBangDiem() => _lBangDiem;
        //method select all in one
        public void GetAllInOne(ref List<SinhVien> lsv, ref List<MonHoc> lmh, ref List<BangDiem> lbd)
        {
            lsv = _lSinhVien;
            lmh = _lMonHoc;
            lbd = _lBangDiem;
        }
        #endregion

    }
}
