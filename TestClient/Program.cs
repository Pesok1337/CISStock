namespace TestClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceReference2.Service1Client client = new ServiceReference2.Service1Client();

            Console.WriteLine(client.GetTest());

        }

    }
}