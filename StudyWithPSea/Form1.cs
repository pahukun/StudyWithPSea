using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudyWithPSea
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        GMapOverlay overlay = new GMapOverlay("testOverlay"); //เป็นการสร้างตัวแปร overlay ขึ้นมา ใน "" เป็นชื่ออะไรก็ได้
        Dictionary<GMapMarker, MarkerDetail> markers = new Dictionary<GMapMarker, MarkerDetail>();
        bool isUpdate = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            map.MaxZoom = 15; //ระยะการซูมใกล้สูงสุด
            map.MinZoom = 5; //ระยะการซูมไกลสูงสุด
            map.Zoom = 8; //ระยะการซูมเริ้มต้น
            map.Manager.Mode = AccessMode.ServerOnly; //เป็นการเลือกโหมกว่าจะดึงแมพจากไหน Server หรือ เครื่องเราเอง
            map.MapProvider = GMapProviders.GoogleMap; //เป็นรูปแบบของแมพ

            map.Overlays.Add(overlay); //เป็นการสร้าง overlay ขึ้นมาบนแมพ 1 ชั้น
        }

        private void map_MouseClick(object sender, MouseEventArgs e)
        {
           isUpdate = false;
           
           PointLatLng point =  map.FromLocalToLatLng(e.X, e.Y); //ดึงค่า lat lng จาด map มาใส่ไว้ในตัวแปร
           if (e.Button == MouseButtons.Left) //ดัก Event คลิกซ้าย
           {
                Console.WriteLine(isUpdate);
                txtLat.Text = point.Lat.ToString();
                txtLng.Text = point.Lng.ToString();    
           }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            double lat = double.Parse(txtLat.Text);
            double lng = double.Parse(txtLng.Text);
            PointLatLng pointLatLng = new PointLatLng(lat, lng); //เอาค่า lat lng จาก txt มาใส่ในตัวแปร
            GMapMarker marker = new GMarkerGoogle(pointLatLng, GMarkerGoogleType.pink); //สร้างมาร์คเกอร์เข้าตัวแปร
            MarkerDetail detail = new MarkerDetail();
            detail.id = 0;
            detail.name = txtName.Text;
            markers.Add(marker, detail);
            marker.ToolTipText = detail.id + "\n" + detail.name; //ดึงข้อความมาใส่ใน marker.ToolTipText
            marker.ToolTipMode = MarkerTooltipMode.OnMouseOver; //ไว้แสดงข้อความตอนเมาส์ไป over ไว้
            overlay.Markers.Add(marker); //แอดมาร์คเกอร์ลงบน Overlay
            txtLat.Text = "";
            txtLng.Text = "";
            txtName.Text = "";
        }

        private void map_OnMarkerClick(GMapMarker item, MouseEventArgs e) //Event เมื่อเอาเม้าส์ไปคลิกที่ marker
        {
            isUpdate = true;
            Console.WriteLine(isUpdate);
            PointLatLng pointData = item.Position;
            if (e.Button == MouseButtons.Left) //ดัก Event คลิกซ้าย
            {
                txtLat.Text = pointData.Lat.ToString();
                txtLng.Text = pointData.Lng.ToString();
                txtName.Text = markers[item].name;
            }
        }
    }
}
