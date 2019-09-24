using System;
using System.ComponentModel.DataAnnotations;

namespace archives.gateway.Models
{
    public class EditArchiveModel : LoginUserModel
    {
        public EditArchiveModel(LoginUserModel lum)
       : base(lum)            //子类的构造函数  
        {
        }

        [Required]
        public string Id { get; set; }
    }
}