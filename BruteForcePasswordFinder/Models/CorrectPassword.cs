namespace BruteForcePasswordFinder.Models
{
    public class CorrectPassword
    {
        public string Content { get; private set; }

        public CorrectPassword(string pwd)
        {
            Content = pwd;
        }

        public static implicit operator CorrectPassword(string value)
        {
            if (value == null)
                return null;

            return new CorrectPassword(value);
        }

        public static implicit operator string(CorrectPassword obj) => obj?.Content;
        public static implicit operator bool(CorrectPassword obj) => obj?.Content != null;

        public override string ToString() => Content;
    }
}
