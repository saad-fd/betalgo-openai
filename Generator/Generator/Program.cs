using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.GPT3.Extensions;
using OpenAI.GPT3.Interfaces;
using OpenAI.Playground.TestHelpers;
#if NET6_0_OR_GREATER
//using LaserCatEyes.HttpClientListener;
#endif

var builder = new ConfigurationBuilder()
    .AddJsonFile("ApiSettings.json")
    .AddUserSecrets<Program>();

IConfiguration configuration = builder.Build();
var serviceCollection = new ServiceCollection();
serviceCollection.AddScoped(_ => configuration);

serviceCollection.AddOpenAIService();

var serviceProvider = serviceCollection.BuildServiceProvider();
var sdk = serviceProvider.GetRequiredService<IOpenAIService>();

List<string> topics = new List<string>
        {
            "Book", "Reserve", "Schedule", "Modify", "Change", "Cancel", "Status",
            "Departure", "Arrival", "Delay", "Cancellation", "Update", "Allowance", "Restrictions", "Fees",
            "Lost", "Damaged", "Class", "Assistance", "Unaccompanied", "Minors", "Account", "Miles",
            "Redeem", "Offers", "Promotions", "Refund", "Compensation", "Policy", "Process", "Visa",
            "Passport", "Validity", "Documents", "Check-in", "Boarding", "Online", "Mobile", "Passes",
            "Meals", "Entertainment", "Wi-Fi", "Specials", "Pets", "Questions", "Feedback", "Complaints",
            "Suggestions", "Discounts", "Promotions", "Medical", "Wheelchair", "Group"
        };

Random random = new Random();
for (int i = 0; i < 2; i++)  
{
    string randomTopic = topics[random.Next(topics.Count)];

    string prompt = $"Create an example of a conversation between customer service and a customer who ask/complain/give/inquire about {randomTopic}. The customer service is an operator of Merpati Airline. Create a minimum of 2000 words. Customer uses an informal language style, while customer service uses a formal style. The conversation should be written in JSONL format as follows:\r\n{{\"role\": \"customer\", \"message\": \"any text\"}}\r\n{{\"role\": \"cs\", \"message\": \"any text\"}}";

    var apiResponse = await OpenAICall.CallOpenAIAPI(sdk, prompt);

    int numbering = i + 1;
    string formattedNumber = numbering.ToString("D3");  
    string currentDirectory = Directory.GetCurrentDirectory();
    string outputPath = Path.Combine(currentDirectory, "Output");

    // Create the "Output" directory if it doesn't exist
    if (!Directory.Exists(outputPath))
        Directory.CreateDirectory(outputPath);

    File.WriteAllText(Path.Combine(outputPath, $"Conversation_{formattedNumber}.jsonl"), apiResponse);

    Console.WriteLine($"File 'Conversation_{formattedNumber}.jsonl' has been saved");

}