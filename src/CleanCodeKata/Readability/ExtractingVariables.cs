namespace CleanCodeKata.Readability;

public class ExtractingVariables
{
    public static void DoSomething()
    {
        //TODO: what type of number is this, and who is it for?
        SendSomething(27644942, "Hello world");

        //TODO: what is this if-statement doing?
        if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || (22 >= DateTime.Now.Hour && 8 < DateTime.Now.Hour) || DateTime.Now.DayOfWeek == DayOfWeek.Friday)
        {
            SendSomething(13371337, "Knock knock!");
        }
    }

    private static void SendSomething(int phoneNumber, string content)
    {
    }
}