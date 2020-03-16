namespace REVUnit.Crlib
{
    public delegate bool TryParser<in TSrc, T>(TSrc value, out T result);
}