using System.Diagnostics.CodeAnalysis;

public struct Rational
{
    public int Denominator { get; private set; }
    public int Nominator { get; private set; }
    public Rational()
    {
        Nominator = 0;
        Denominator = 1;
    }
    public Rational(int value)
    {
        Nominator = value;
        Denominator = 1;
    }
    public Rational(int nomi, int denom)
    {
        Denominator = denom;
        Nominator = nomi;
        Normalize();
    }
    public static Rational Zero => new Rational(0);
    public static Rational One => new Rational(1);
    private void Normalize()
    {
        if (Nominator == 0)
        {
            Denominator = 1;
            return;
        }
        int gcd = Gcd(Nominator, Denominator);
        Denominator /= gcd;
        Nominator /= gcd;
    }
    private static int Gcd(int a, int b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);
        int min = Math.Min(a, b);
        int max = Math.Max(a, b);
        if (min == 0) return max;
        if (min == 1)
        {
            return 1;
        }
        max = Math.Max(a, b);
        int mod = max % min;
        return mod == 0 ? min : Gcd(min, mod);
    }
    public static int Lcm(int a, int b)
    {
        return checked(a * (b / Gcd(a, b)));
    }
    public decimal ToDecimal() => ((decimal)Nominator) / Denominator;
    public double ToDouble() => ((double)Nominator) / Denominator;
    public static Rational operator +(Rational a, Rational b)
    {
        int lcm = Lcm(a.Denominator, b.Denominator);
        int amult = lcm / a.Denominator;
        int bmult = lcm / b.Denominator;
        int nom = a.Nominator * amult + b.Nominator * bmult;
        return new Rational(nom, lcm);
    }
    public static Rational operator -(Rational a, Rational b)
    => a + (new Rational(-1) * b);
    public static Rational operator *(Rational a, Rational b)
            => new Rational(a.Nominator * b.Nominator, a.Denominator * b.Denominator);
    public static Rational operator /(Rational a, Rational b)
            => a * new Rational(b.Denominator, b.Nominator);
    public static bool operator ==(Rational a, Rational b)
        => a.Nominator == b.Nominator && a.Denominator == b.Denominator;
    public static bool operator !=(Rational a, Rational b)
        => !(a == b);
    public static bool operator <(Rational a, Rational b)
    {
        int lcm = Lcm(a.Denominator, b.Denominator);
        int amult = lcm / a.Denominator;
        int bmult = lcm / b.Denominator;
        return a.Nominator * amult < b.Nominator * bmult;
    }
    public static bool operator >(Rational a, Rational b)
    {
        int lcm = Lcm(a.Denominator, b.Denominator);
        int amult = lcm / a.Denominator;
        int bmult = lcm / b.Denominator;
        return a.Nominator * amult > b.Nominator * bmult;
    }
    public override bool Equals([NotNullWhen(true)] object obj)
    {
        if (obj is Rational)
        {
            return (Rational)obj == this;
        }
        throw new Exception();
    }
    public bool Equals(Rational other) => this == other;
    public override int GetHashCode() => Nominator + Denominator;
    public override string ToString() => ToString(false);
    public string ToString(bool Float)
        => Float ? (((double)Nominator) / Denominator).ToString() : $"{Nominator} / {Denominator}";
}