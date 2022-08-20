using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSorting
{
    public class PersonalInfoModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Fathername { get; set; }
        public string? MeliCode { get; set; }
        public int PersonalCode { get; set; }
        public string? DirPath { get; set; }
        public int FileCount { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }

    }
}
