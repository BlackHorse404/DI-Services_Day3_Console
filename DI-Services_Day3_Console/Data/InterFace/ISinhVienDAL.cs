using System.Collections.Generic;
using DI_Services_Day3_Console.Models;

namespace DI_Services_Day3_Console.Data.InterFace
{
    public interface ISinhVienDAL
    {
        //connect database
        void ConnectDataBaseSinhVien();
        //get list SinhVien read data from database
        List<SinhVien> GetAllSinhVien();
        //get list MonHoc read data from database
        List<MonHoc> GetAllMonHoc();
        //get list BangDiem read data from database
        List<BangDiem> GetAllBangDiem();
        //get all sinhvien,BangDiem,MonHoc read data from database
        void GetAllInOne(ref List<SinhVien> lsv, ref List<MonHoc> lmh, ref List<BangDiem> lbd);
        //update score when edit to database
        void updateScoreDB(string maSV, string maMH, float diemQT, float diemTP);

    }
}
