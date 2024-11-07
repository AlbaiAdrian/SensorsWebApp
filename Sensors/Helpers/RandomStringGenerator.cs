namespace Sensors.Helpers
{
    public static class RandomStringGenerator
    {
        private static readonly Random _random = new Random();
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%&*=";

        public static string Generate(int length = 6)
        {
            return new string(Enumerable.Repeat(_chars, length).Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
