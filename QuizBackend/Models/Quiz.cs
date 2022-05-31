namespace QuizBackend.Models
{
    public class Quiz
    {
        public Quiz ( string title )
        {
            _title = title;
            Questions = new List<Question> ();

        }
        private string _title = String.Empty;
        
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

        public string Title { 
            get => _title;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _title = value;
                }
                else
                {
                    throw new ArgumentException("Title cannot be null nor empty.");
                }
            }
        }

        public virtual ICollection<Question> Questions { get; set; }

    }
}
