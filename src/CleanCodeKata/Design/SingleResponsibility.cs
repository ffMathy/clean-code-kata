namespace CleanCodeKata.Design;

public class UserController
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IProductService _productService;

    public UserController(
        IUserRepository userRepository,
        IEmailService emailService,
        IProductService productService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _productService = productService;
    }

    public void CreateUser(string email, string password)
    {
        var hashedPassword = GenerateHash(password);
        _userRepository.CreateUser(email, hashedPassword);
        _emailService.SendEmail(email, "You have signed up!");
    }

    private string GenerateHash(string text)
    {
        return "this is totally hashed - nothing to see here, carry on: " + text;
    }

    public void CreateOrderForUserByEmail(string email, OrderLine[] orderLines)
    {
        var user = _userRepository.GetUserByEmail(email);
        
        var totalAmount = orderLines.Sum(orderLine => orderLine.Amount);
        user.Balance -= totalAmount;

        DecreaseStockForProductsInOrder(orderLines);

        SendOutReceiptEmail(user, orderLines);
    }

    private void SendOutReceiptEmail(User user, OrderLine[] orderLines)
    {
        var emailMessage = FormatOrderSummaryEmailText(orderLines);
        _emailService.SendEmail(user.Email, emailMessage);
    }

    private void DecreaseStockForProductsInOrder(OrderLine[] orderLines)
    {
        foreach (var orderLine in orderLines)
        {
            _productService.ReduceStockAmount(orderLine.ProductName, orderLine.Amount);
        }
    }

    private static string FormatOrderSummaryEmailText(OrderLine[] orderLines)
    {
        var emailMessage = "You bought: ";
        foreach (var orderLine in orderLines)
        {
            emailMessage += orderLine.Amount + "X " + orderLine.ProductName + ", ";
        }

        return emailMessage;
    }
}

#region Outside scope

public interface IProductService
{
    void ReduceStockAmount(string productName, double amount);
}

public record OrderLine(
    double Amount, 
    string ProductName);

public interface IEmailService
{
    void SendEmail(string email, string contents);
}

public class User
{
    public double Balance { get; set; }
    public string Email { get; set; }
}

public interface IUserRepository
{
    void CreateUser(string email, string hashedPassword);
    User GetUserByEmail(string email);
}

#endregion