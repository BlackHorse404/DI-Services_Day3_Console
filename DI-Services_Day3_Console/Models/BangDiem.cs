using System;
using DI_Services_Day3_Console.Utilities;

namespace DI_Services_Day3_Console.Models
{
    public class BangDiem
    {
        //thuộc tính môn học
        #region thuộc tính
        private string _maMonHoc;
        private string _maSinhVien;
        private string _loaiMon;
        private float _diemQuaTrinh;
        private float _diemThanhPhan;
        private float _diemTong;
        #endregion

        //property
        #region Property
        public string MaMonHoc { get { return _maMonHoc; } set { } }
        public string MaSinhVien { get { return _maSinhVien; } set { } }
        public float DiemQuaTrinh { get { return _diemQuaTrinh; } set { } }
        public float DiemThanhPhan { get { return _diemThanhPhan; } set { } }
        #endregion

        //constructor Mon Hoc khởi tạo dữ liệu cho các thuộc tính MonHoc
        #region Constructor
        //mặc định
        public BangDiem()
        {
            _maMonHoc = "MKC";
            _maSinhVien = "200000000";
            _loaiMon = "LT";
            _diemQuaTrinh = 0f;
            _diemThanhPhan = 0f;
        }
        //truyền tham số khởi tạo đối tượng BangDiem
        public BangDiem(string maMonHoc, string maSinhVien, string loaiMon, float diemQuaTrinh, float diemThanhPhan)
        {
            
            _maMonHoc = maMonHoc;
            _maSinhVien = maSinhVien;
            _loaiMon = loaiMon;
            if (0 <= diemQuaTrinh && diemQuaTrinh <= 10)
                _diemQuaTrinh = diemQuaTrinh;
            else
                ExceptionNotice.ExceptionParameters();
            if (0 <= diemThanhPhan && diemThanhPhan <= 10)
                _diemThanhPhan = diemThanhPhan;
            else
                ExceptionNotice.ExceptionParameters();
        }
        #endregion

        //phương thức môn học
        #region Phương Thức
        //nhập điểm quá trình và thành phần
        public void setScore()
        {
            bool check;
            do
            {
                Console.Write("Nhập vào điểm quá trình: ");
                check = float.TryParse(Console.ReadLine(), out _diemQuaTrinh);
                if (_diemQuaTrinh < 0)
                    check = false;
            } while (!check);
            do
            {
                Console.Write("Nhập vào điểm thành phần: ");
                check = float.TryParse(Console.ReadLine(), out _diemThanhPhan);
                if (_diemThanhPhan < 0)
                    check = false;
            } while (!check);
        }
        // tính theo tỉ lệ - Thực hành 50% : 50% - Lý thuyết (Thành Phần) 70% - (Quá Trình) 30%
        // lấy ra điểm tổng của 1 môn học
        public float DiemTongKetMon()
        {
            if (string.Compare(_loaiMon, "TH") == 0)
                _diemTong = _diemQuaTrinh * 0.5f + _diemThanhPhan * 0.5f;
            else
                _diemTong = _diemQuaTrinh * 0.3f + _diemThanhPhan * 0.7f;
            return _diemTong;
        }
        #endregion
    }
}
