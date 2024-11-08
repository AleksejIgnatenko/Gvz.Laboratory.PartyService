namespace Gvz.Laboratory.PartyService.Exceptions
{
    public class PartyValidationException : Exception
    {
        public Dictionary<string, string> Errors { get; set; }

        public PartyValidationException(Dictionary<string, string> errors)
        {
            Errors = errors;
        }
    }
}
