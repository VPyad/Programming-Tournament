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

        public static SupportedLanguage Map(string code)
        {
            throw new NotImplementedException();
        }

        public static SupportedLanguage Map(SupportedProgrammingLanguage language)
        {
            if (language == null)
                throw new NullReferenceException();

            return Map(language.Code);
        }
    }
}
