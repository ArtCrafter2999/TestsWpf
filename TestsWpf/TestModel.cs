using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsWpf
{
    public class TestModel
    {
        public string Question { get; set; }
        public bool MultipleAnswer { get; set; } = false;
        public bool StrictAnswer { get; set; } = false;
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}
