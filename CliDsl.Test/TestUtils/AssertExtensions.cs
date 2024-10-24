namespace CliDsl.Test.TestUtils
{
    internal static class AssertExtensions
    {
        public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            Assert.IsTrue(expected.SequenceEqual(actual));
        }
    }
}
