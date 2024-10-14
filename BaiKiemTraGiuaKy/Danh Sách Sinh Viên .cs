using BaiKiemTraGiuaKy.NewFolder1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiKiemTraGiuaKy
{
    public partial class Form1 : Form
    {
        Model1 model1;
        private List<SinhVien> originalStudentList;
        public Form1()
        {
            InitializeComponent();
            model1 = new Model1();
            List<SinhVien> listStudent = model1.SinhViens.ToList();
            List<Lop> listLop = model1.Lops.ToList();
            FillLopCombobox(listLop);
            BindGrid(listStudent);
            originalStudentList = model1.SinhViens.ToList();
            cmbLop.SelectedIndex = 0;
        }
        private void BindGrid(List<SinhVien> listStudent)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.StudentID;
                dataGridView1.Rows[index].Cells[1].Value = item.FullName;
                dataGridView1.Rows[index].Cells[2].Value = item.NgaySinh;
                dataGridView1.Rows[index].Cells[3].Value = item.Lop.TenLop;
            }
        }

        private void FillLopCombobox(List<Lop> listLop)
        {
            this.cmbLop.DataSource = listLop;
            this.cmbLop.DisplayMember = "TenLop";
            this.cmbLop.ValueMember = "MaLop";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                List<SinhVien> SinhVienList = model1.SinhViens.ToList();

               
                if (SinhVienList.Any(s => s.StudentID == txtMaSV.Text))
                {
                    MessageBox.Show("Mã SV đã tồn tại!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                DateTime ngaySinh = dateTimePicker1.Value;

                var newSinhVien = new SinhVien
                    {
                        StudentID = txtMaSV.Text,  
                        FullName = txtName.Text,   
                        NgaySinh = ngaySinh,       
                        MaLop = cmbLop.SelectedValue.ToString()  
                    };

                    
                    model1.SinhViens.Add(newSinhVien);
                    model1.SaveChanges();  
 
                    BindGrid(model1.SinhViens.ToList());
                    MessageBox.Show("Thêm sinh viên thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                
                List<SinhVien> SinhVienList = model1.SinhViens.ToList();

                
                var Sinhvien = SinhVienList.FirstOrDefault(s => s.StudentID == txtMaSV.Text);
                if (Sinhvien == null)
                {
                    MessageBox.Show("Mã SV không tồn tại!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                
                DateTime ngaySinh = dateTimePicker1.Value;

                var selectedLop = cmbLop.SelectedValue;
                if (selectedLop != null)
                {
                    Sinhvien.MaLop = selectedLop.ToString(); 
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn lớp cho sinh viên.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; 
                }

                
                Sinhvien.FullName = txtName.Text;
                Sinhvien.NgaySinh = ngaySinh;
               
                model1.SaveChanges();
               
                BindGrid(model1.SinhViens.ToList());

                MessageBox.Show("Cập nhật sinh viên thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtMaSV.Text = row.Cells[0].Value?.ToString();
                txtName.Text = row.Cells[1].Value?.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells[2].Value);
                string tenLop = row.Cells[3].Value?.ToString();
                var selectedLop = model1.Lops.FirstOrDefault(l => l.TenLop == tenLop);

                
                if (selectedLop != null)
                {
                    cmbLop.SelectedValue = selectedLop.MaLop; 
                }
                else
                {
                    cmbLop.SelectedValue = null;
                }
            }
        }

        private void Xóa_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này không?", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No) return;

                List<SinhVien> SinhVienList = model1.SinhViens.ToList();
                var Sinhvien = SinhVienList.FirstOrDefault(s => s.StudentID == txtMaSV.Text);
                if (Sinhvien != null)
                {
                    model1.SinhViens.Remove(Sinhvien);
                    model1.SaveChanges();
                    BindGrid(model1.SinhViens.ToList());
                    MessageBox.Show("Xóa Sinh Viên Thành Công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Sinh Viên Không Tồn Tại!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnThoat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close(); 
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string searchTerm = txtTimKiem.Text.ToLower().Trim();

            
            var filteredList = originalStudentList
                .Where(s => s.FullName.ToLower().Contains(searchTerm) )
                .ToList();

            BindGrid(filteredList);
        }
    }

   
}
