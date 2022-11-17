namespace ConsoleApp2
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //1
            Console.WriteLine("Inizio l'elaborazione");

            //2
            //string[] lines = await File.ReadAllLinesAsync("TextFile1.txt");
            Task<string[]> t = File.ReadAllLinesAsync("TextFile1.txt");

            //Istruzioni del blocco arancio
            Console.WriteLine("Istruzioni del blocco arancio");

            Console.WriteLine("Elaboro il risultato");
            string[] lines = await t;
            foreach (string line in lines) Console.WriteLine(line);
            
            //3
            Console.WriteLine("Ho terminato");
            Console.ReadLine();
        }
    }
}