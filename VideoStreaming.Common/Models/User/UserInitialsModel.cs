namespace VideoStreaming.Common.Models.User;

public class UserInitialsModel
{
    public string ThumbUrl { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Initials
    {
        get => InitialsHelper.Get(FirstName, LastName);
    }
}

public static class InitialsHelper
{
    public static string Get(string firstName, string lastName)
    {
        var result = string.Empty;
        if (!string.IsNullOrWhiteSpace(firstName))
        {
            result += firstName[0];
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            result += lastName[0];
        }
        else if (firstName?.Length > 1)
        {
            result += firstName[1];
        }

        return result.ToUpper();
    }
}
