using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace OAuth_Google
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Books API Sample: List MyLibrary");
            Console.WriteLine("================================");
            try
            {
                new Program().Run().Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("ERROR: " + e.Message);
                }
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private async Task Run()
        {
            UserCredential credential;
            using (var stream = new FileStream("secretshit.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { BooksService.Scope.Books },
                    "user", CancellationToken.None, new FileDataStore("Books.ListMyLibrary"));
            }

            // Create the service.
            var service = new BooksService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Books API Sample",
            });

            var bookshelves = await service.Mylibrary.Bookshelves.List().ExecuteAsync();
        }
    }
}
