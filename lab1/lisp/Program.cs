Console.WriteLine(Fact(21));

ulong Fact(ulong number)
{
    if (number == 1) return 1;

    return number * Fact(number - 1);
}