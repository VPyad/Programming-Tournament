using ProcessManagment.BuildSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Models.Domain.Tournaments
{
    public class SupportedProgrammingLanguage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupportedProgrammingLanguageId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public static SupportedLanguage Map(string code, out bool success)
        {
            success = true;
            switch (code)
            {
                case "C":
                    return SupportedLanguage.C;
                case "CPP":
                    return SupportedLanguage.CPP;
                case "Java":
                    return SupportedLanguage.Java;
                case "CSharp":
                    return SupportedLanguage.CSharp;
                case "FreePascal":
                    return SupportedLanguage.FreePascal;
                case "Delphi":
                    return SupportedLanguage.Delphi;
                case "ObjPascal":
                    return SupportedLanguage.ObjPascal;
                default:
                    success = false;
                    return SupportedLanguage.C;
            }
        }
    }
}
