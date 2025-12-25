using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Infrastructure
{
    public class OMockData
    {
        public string value { get; set; }
        public string label { get; set; }
    }
    public class OMockQuyDinhGioLamViec
    {
        public double SoGioLamViecHanhChinh { get; set; }
        public double GioBatDau { get; set; }
        public double GioKetThuc { get; set; }
    }
    public static class MockData
    {
        public static OMockQuyDinhGioLamViec QuyDinhGioLamViec = new OMockQuyDinhGioLamViec
        {
            SoGioLamViecHanhChinh = 8,
            GioBatDau = 7.30,
            GioKetThuc = 19.30
        };
        //public static OMockData[] TinhThanhPho = {
        //    new OMockData { value= "86b520e3-56f4-46f6-bd1b-1b987ed4b9c2", label= "Thành phố Hồ Chí Minh" }
        //};
        //public static OMockData[] QuanHuyen = {
        //    new OMockData { value = "16b520e3-56f4-46f6-bd1b-1b987ed4b9c2", label= "Củ Chi" }
        //};
        //public static OMockData[] PhuongXa = {
        //    new OMockData { value= "a6b520e3-56f4-46f6-bd1b-1b987ed4b901", label= "Thị trấn Củ Chi" },
        //    new OMockData { value= "a6b520e3-56f4-46f6-bd1b-1b987ed4b902", label= "Xã Phú Mỹ Hưng" },
        //    new OMockData { value= "a6b520e3-56f4-46f6-bd1b-1b987ed4b903", label= "Xã An Phú" },
        //    new OMockData { value= "a6b520e3-56f4-46f6-bd1b-1b987ed4b904", label= "Xã Trung Lập Thượng" },
        //    new OMockData { value= "a6b520e3-56f4-46f6-bd1b-1b987ed4b905", label= "Xã An Nhơn Tây" },
        //    new OMockData { value= "a6b520e3-56f4-46f6-bd1b-1b987ed4b906", label= "Xã Nhuận Đức" },
        //    new OMockData { value= "a6b520e3-56f4-46f6-bd1b-1b987ed4b907", label= "Xã Phạm Văn Cội" },
        //    new OMockData { value= "a6b520e3-56f4-46f6-bd1b-1b987ed4b908", label= "Xã Phú Hòa Đông" },
        //    new OMockData { value= "a6b520e3-56f4-46f6-bd1b-1b987ed4b909", label= "Xã Trung Lập Hạ" },
        //    new OMockData { value= "a6b520e3-56f4-46f6-bd1b-1b987ed4b910", label= "Xã Trung An" },
        //    new OMockData { value= "a6b520e3-56f4-46f6-bd1b-1b987ed4b911", label= "Xã Phước Thạnh" },
        //    new OMockData { value= "a6b520e3-56f4-46f6-bd1b-1b987ed4b912", label= "Xã Phước Hiệp" },
        //    new OMockData { value= "a6b520e3-56f4-46f6-bd1b-1b987ed4b913", label= "Xã Tân An Hội" },
        //    new OMockData { value= "a6b520e3-56f4-46f6-bd1b-1b987ed4b914", label= "Xã Phước Vĩnh An" },
        //    new OMockData { value= "a6b520e3-56f4-46f6-bd1b-1b987ed4b915", label= "Xã Thái Mỹ" },
        //    new OMockData { value= "a6b520e3-56f4-46f6-bd1b-1b987ed4b916", label= "Xã Tân Thạnh Tây" },
        //    new OMockData { value= "a6b520e3-56f4-46f6-bd1b-1b987ed4b917", label= "Xã Hòa Phú" },
        //    new OMockData { value= "a6b520e3-56f4-46f6-bd1b-1b987ed4b918", label= "Xã Tân Thạnh Đông" },
        //    new OMockData { value= "a6b520e3-56f4-46f6-bd1b-1b987ed4b919", label= "Xã Bình Mỹ" }
        //};
    }
}
