namespace QuizBackend.Models
{
    public class Question
    {
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
    }
}
