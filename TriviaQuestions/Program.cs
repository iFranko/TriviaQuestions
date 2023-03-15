using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using TriviaQuestions;
using System.Reflection.Emit;

class Program
{
    static async Task Main(string[] args)
    {
        // Clear the text file before each run
        File.WriteAllText("results.txt", string.Empty);

        // Ask the user for the difficulty level
        Console.WriteLine("Enter difficulty level");
        int value = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine();
        // Ask the user number of questions max 100
        Console.WriteLine("Enter number of questions range (1 - 100)");
        int count = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine();

        // validate questions number user entered
        bool validateCount = true;

        while (validateCount)
        {
            //if true countinue...
            if (count > 0 && count <=100)
            {
                validateCount = false;
            }
            // otherwise ask the user again 
            else
            {
                validateCount = true;
                Console.WriteLine("Number of questions must be in range of (1 - 100)");
                Console.WriteLine();
                Console.WriteLine("Enter a limit for the number of records returned:");
                count = Convert.ToInt32(Console.ReadLine());
                
            }
        }


        // Define the URL for the API jservice.io with user input
        string apiUrl = $"https://jservice.io/api/clues?value={value}";

        // Create a HTTP client to retrieve the data from the jservice.io API
        using (HttpClient client = new HttpClient())
        {
            // Retrieve the response from the jservice.io API
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            string apiResponse = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response into a list of dictionaries
            List<Dictionary<string, object>> responseList = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(apiResponse);

            // Create a list of Questions objects to hold the data
            List<Questions> questionList = new List<Questions>();

            // Loop through each dictionary in the response list and create a Questions object
            for (int i = 0; i < count && i < responseList.Count; i++)
            {

                Dictionary<string, object> questionDict = responseList[i];
                Questions question = new Questions();
                question.Difficulty = Convert.ToInt32(questionDict["value"].ToString());
                question.Question = questionDict["question"].ToString();
                question.Answer = questionDict["answer"].ToString();
                Dictionary<string, object> categoryDict = JsonSerializer.Deserialize<Dictionary<string, object>>(questionDict["category"].ToString());
                Category category = new Category();
                category.Title = categoryDict["title"].ToString();
                question.Category = category;
                questionList.Add(question);
            }

            // Output the questions to the console 
            foreach (Questions question in questionList)
            {
                Console.WriteLine("Question: " + question.Question);
             
            }

            // Save the results to a text file
            using (StreamWriter writer = new StreamWriter("results.txt"))
            {
                // Write the header row
                writer.WriteLine("Difficulty,Question,Answer,Category");

                // Write each record
                foreach (Questions question in questionList)
                {
                    writer.WriteLine($"{question.Difficulty},{question.Question},{question.Answer},{question.Category.Title}");
                }
            }
        }

        // Wait for user input before closing the console window
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
