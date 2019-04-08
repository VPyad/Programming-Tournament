using Programming_Tournament.Areas.Identity.Models;
using Programming_Tournament.Models.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming_Tournament.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (!context.Faculties.Any())
            {
                var faculties = new Faculty[]
                {
                    new Faculty { Name = "ПММ" },
                    new Faculty { Name = "ФКН" }
                };

                foreach (var item in faculties)
                    context.Faculties.Add(item);

                context.SaveChanges();
            }

            if (!context.Lecterns.Any())
            {
                // ПММ
                context.Lecterns.Add(new Lectern { Name = "Математическое Обеспечение ЭВМ" });
                context.Lecterns.Add(new Lectern { Name = "Математические Методы Исследования Операций" });
                context.Lecterns.Add(new Lectern { Name = "Математический и Прикладной Анализ" });

                // ФКН
                context.Lecterns.Add(new Lectern { Name = "Информационные системы" });
                context.Lecterns.Add(new Lectern { Name = "Программирование и информационные системы" });
                context.Lecterns.Add(new Lectern { Name = "Цифровые технологии" });

                context.SaveChanges();
            }

            if (!context.Сurriculums.Any())
            {
                // ПММ
                context.Сurriculums.Add(new Curriculum { Name = "ФИИТ" });
                context.Сurriculums.Add(new Curriculum { Name = "ПМИ" });
                context.Сurriculums.Add(new Curriculum { Name = "Бизнес информатика" });

                // ФКН
                context.Сurriculums.Add(new Curriculum { Name = "Корпоративные информационные системы с базами данных" });
                context.Сurriculums.Add(new Curriculum { Name = "Параллельные и распределенные вычислительные системы" });
                context.Сurriculums.Add(new Curriculum { Name = "Технические средства и методы защиты информации" });

                context.SaveChanges();
            }

            if (!context.SupportedProgrammingLanguages.Any())
            {
                context.SupportedProgrammingLanguages.Add(new Models.Domain.Tournaments.SupportedProgrammingLanguage { Name = "C", Code = "C" });
                context.SupportedProgrammingLanguages.Add(new Models.Domain.Tournaments.SupportedProgrammingLanguage { Name = "C++", Code = "CPP" });
                context.SupportedProgrammingLanguages.Add(new Models.Domain.Tournaments.SupportedProgrammingLanguage { Name = "Java", Code = "Java" });
                context.SupportedProgrammingLanguages.Add(new Models.Domain.Tournaments.SupportedProgrammingLanguage { Name = "C#", Code = "CSharp" });
                context.SupportedProgrammingLanguages.Add(new Models.Domain.Tournaments.SupportedProgrammingLanguage { Name = "FreePascal", Code = "FreePascal" });
                context.SupportedProgrammingLanguages.Add(new Models.Domain.Tournaments.SupportedProgrammingLanguage { Name = "Delphi", Code = "Delphi" });
                context.SupportedProgrammingLanguages.Add(new Models.Domain.Tournaments.SupportedProgrammingLanguage { Name = "ObjPascal", Code = "ObjPascal" });

                context.SaveChanges();
            }
        }
    }
}
