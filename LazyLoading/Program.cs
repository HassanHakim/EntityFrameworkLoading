using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyLoading
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Context context = new Context())
            {
                Course course = new Course() { Name = ".NET Bridging" };
                Student student1 = new Student() { Name = "Joe" };
                Student student2 = new Student() { Name = "Jane" };
                Student student3 = new Student() { Name = "Sally" };
                course.Students = new List<Student>() { student1, student2, student3 };
                context.Courses.Add(course);
                context.SaveChanges();


                Course LoadedCourse = context                           
                    .Courses.Where(c => c.Name == ".NET Bridging")
                    .FirstOrDefault();

                // Lazy Loaded - Students are not loaded until required
                foreach (Student student in LoadedCourse.Students)
                {
                    Console.WriteLine(student.Name);
                }


                Console.ReadLine();
            }
        }
    }

    class Context : DbContext
    {
        public Context()
        {
            Database.Log = Console.Write;
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
    }

    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // forgein key refereces are marked as virtual
        //    so EF can override and provide the lazy load behaviour 
        public virtual ICollection<Student> Students { get; set; } 
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // forgein key refereces are marked as virtual
        //    so EF can override and provide the lazy load behaviour 
        public virtual Course Course { get; set; }
    }
}
