using System.Text.RegularExpressions;

namespace KattisSearch
{
    public class Problem
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Text { get; set; }
        public string Difficulty { get; set; }

        public Problem() { }

        public Problem(string Name, string Id, string Difficulty, string Text)
        {
            this.Name = Name;
            this.Id = Id;
            this.Difficulty = Difficulty;
            this.Text = Regex.Replace(Text, @"[’‘]", "'");
            this.Text = this.Text.Replace("\"", "\\\"");
            this.Text = this.Text.Replace("\\", "");
        }

        public override string ToString()
        {
            return "{ " + Name + ", " + Id + ", " + Difficulty + " }";
        }
    }
}