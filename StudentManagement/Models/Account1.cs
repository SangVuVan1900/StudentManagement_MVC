using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentManagement.Models
{
    public class Account1
    {
        public int Id { get; set; }

        [DisplayName("Tên đăng nhập")]
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Tên đăng nhập phải chứa ít nhất 6-30 kí tự")]
        public string Username { get; set; }
         
        [DisplayName("Mật khẩu")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Mật khẩu phải chứa ít nhất 6-30 kí tự")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Xác nhận mật khẩu")]  
        [Required(ErrorMessage = "Xác nhận mật khẩu sai")]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu sai")]
        public string ConfirmPassword { get; set; }

        [DisplayName("Câu hỏi bảo mật")]
        [Required(ErrorMessage = "Bạn phải điền hỏi bảo mật")]
        public string Question { get; set; }
        
    }
}