using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TriviaQuestions
{
    public class Questions
    {
        public int Difficulty { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public Category Category { get; set; }
    }

    public class Category
    {
        public string Title { get; set; }
    }
}
