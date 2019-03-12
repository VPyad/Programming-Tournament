using Programming_Tournament.Areas.Identity.Models;
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
                context.Faculties.Add(new Faculty { Name = "ПММ" });
                context.Faculties.Add(new Faculty { Name = "ФКН" });

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
                context.Сurriculums.Add(new Сurriculum { Name = "ФИИТ" });
                context.Сurriculums.Add(new Сurriculum { Name = "ПМИ" });
                context.Сurriculums.Add(new Сurriculum { Name = "Бизнес информатика" });
                
                // ФКН
                context.Сurriculums.Add(new Сurriculum { Name = "Корпоративные информационные системы с базами данных" });
                context.Сurriculums.Add(new Сurriculum { Name = "Параллельные и распределенные вычислительные системы" });
                context.Сurriculums.Add(new Сurriculum { Name = "Технические средства и методы защиты информации" });

                context.SaveChanges();
            }
        }
    }
}
