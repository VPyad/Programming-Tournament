using Programming_Tournament.Data.Repositories.Core;
using Programming_Tournament.Models.Domain.Tournaments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Data.Repositories.SupportedLanguages
{
    public class SupportedLanguageRepository : Repository<SupportedProgrammingLanguage>, ISupportedLanguageRepository
    {
        public SupportedLanguageRepository(ApplicationDbContext context) : base(context) { }
    }
}
