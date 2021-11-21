using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCreator
{
    public class Answer
    {
        public Answer(string text)
        {
            Text = text;
        }
        public string Text { get; set;}
        public bool IsCorrect { get; set; }
        public override string ToString()
        {
            return Text;
        }
    }
}
