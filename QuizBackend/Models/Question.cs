namespace QuizBackend.Models
{
    public class Question
    {
        public Question ( string text, string answer1, string answer2, string answer3)
        {
            _text = text;
            Answer1 = answer1;
            Answer2 = answer2;
            Answer3 = answer3;
        }
        private string _text = String.Empty;
        
        private int _id;

        public int Id { 
            get => _id;
            set
            {
                if (value > 0)
                    _id = value;
                else
                    throw new ArgumentException("Id must be greater than zero");
            }
        }

        public string Text { 
            get => _text;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _text = value;
                }
                else
                {
                    throw new ArgumentException("Text cannot be null nor empty.");
                }
            }
        }

        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
    }
}
