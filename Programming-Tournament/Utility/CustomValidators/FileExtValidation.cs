using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Utility.CustomValidators
{
    public class FileExtValidation : ValidationAttribute
    {
        private List<string> Exts { get; set; }
        private bool AllowNull { get; set; }

        public FileExtValidation(string ext, string errorText, bool alowNull)
        {
            Exts = new List<string>();
            Exts.Add(ext.ToLower());
            AllowNull = alowNull;
            ErrorMessage = errorText;
        }

        public FileExtValidation(string[] exts, string errorText, bool alowNull)
        {
            Exts = new List<string>(exts);
            Exts.ForEach(x => x = x.ToLower());
            AllowNull = alowNull;
            ErrorMessage = errorText;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                if (AllowNull)
                    return true;
                else
                    return false;

            var file = (IFormFile)value;
            var ext = Path.GetExtension(file.FileName).ToLower().Replace(".", "");

            return Exts.Contains(ext);
        }
    }
}
