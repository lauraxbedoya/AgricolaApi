namespace AgricolaApi.Domain;

public class InvalidCredentialException : Exception
{
    public InvalidCredentialException(string Message) : base(Message)
    { }
}
