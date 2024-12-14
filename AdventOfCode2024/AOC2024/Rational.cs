using System.Diagnostics.CodeAnalysis;
using System.Numerics;

public struct Rational
{
    public int Denominator { get; private set; }
    public int Numerator { get; private set; }
    public Rational()
    {
        Numerator = 0;
        Denominator = 1;
    }
    public Rational(int value)
    {
        Numerator = value;
        Denominator = 1;
    }
    public Rational(int nume, int denom)
    {
        Denominator = denom;
        Numerator = nume;
        Normalize();
    }
    public static Rational Zero => new Rational(0);
    public static Rational One => new Rational(1);
    private void Normalize()
    {
        if (Numerator == 0)
        {
            Denominator = 1;
            return;
        }
        int gcd = Gcd(Numerator, Denominator);
        int sign = Math.Sign(Denominator);
        Denominator /= gcd * sign;
        Numerator /= gcd * sign;
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
    public decimal ToDecimal() => ((decimal)Numerator) / Denominator;
    public double ToDouble() => ((double)Numerator) / Denominator;
    public bool IsInteger() => Denominator == 1;
    public static Rational operator +(Rational a, Rational b)
    {
        int lcm = Lcm(a.Denominator, b.Denominator);
        int amult = lcm / a.Denominator;
        int bmult = lcm / b.Denominator;
        int nom = a.Numerator * amult + b.Numerator * bmult;
        return new Rational(nom, lcm);
    }
    public static Rational operator -(Rational a, Rational b)
    => a + (new Rational(-1) * b);
    public static Rational operator *(Rational a, Rational b)
            => new Rational(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
    public static Rational operator /(Rational a, Rational b)
            => a * new Rational(b.Denominator, b.Numerator);
    public static bool operator ==(Rational a, Rational b)
        => a.Numerator == b.Numerator && a.Denominator == b.Denominator;
    public static bool operator !=(Rational a, Rational b)
        => !(a == b);
    public static bool operator <(Rational a, Rational b)
    {
        int lcm = Lcm(a.Denominator, b.Denominator);
        int amult = lcm / a.Denominator;
        int bmult = lcm / b.Denominator;
        return a.Numerator * amult < b.Numerator * bmult;
    }
    public static bool operator >(Rational a, Rational b)
    {
        int lcm = Lcm(a.Denominator, b.Denominator);
        int amult = lcm / a.Denominator;
        int bmult = lcm / b.Denominator;
        return a.Numerator * amult > b.Numerator * bmult;
    }
    public override bool Equals([NotNullWhen(true)] object obj)
    {
        if (obj is Rational)
        {
            return (Rational)obj == this;
        }
        throw new Exception($"Called {nameof(Rational)}.{nameof(Equals)} with a non {nameof(Rational)} object");
    }
    public bool Equals(Rational other) => this == other;
    public override int GetHashCode() => Numerator + Denominator;
    public override string ToString() => ToString(false);
    public string ToString(bool Float)
        => Float ? (((double)Numerator) / Denominator).ToString() : $"{Numerator} / {Denominator}";
}
public struct LongRational
{
    public long Denominator { get; private set; }
    public long Numerator { get; private set; }
    public LongRational()
    {
        Numerator = 0;
        Denominator = 1;
    }
    public LongRational(long value)
    {
        Numerator = value;
        Denominator = 1;
    }
    public LongRational(long nume, long denom)
    {
        Denominator = denom;
        Numerator = nume;
        Normalize();
    }
    public static LongRational Zero => new LongRational(0);
    public static LongRational One => new LongRational(1);
    private void Normalize()
    {
        if (Numerator == 0)
        {
            Denominator = 1;
            return;
        }
        long gcd = Gcd(Numerator, Denominator);
        long sign = Math.Sign(Denominator);
        Denominator /= gcd * sign;
        Numerator /= gcd * sign;
    }
    private static long Gcd(long a, long b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);
        long min = Math.Min(a, b);
        long max = Math.Max(a, b);
        if (min == 0) return max;
        if (min == 1)
        {
            return 1;
        }
        max = Math.Max(a, b);
        long mod = max % min;
        return mod == 0 ? min : Gcd(min, mod);
    }
    public static long Lcm(long a, long b)
    {
        return checked(a * (b / Gcd(a, b)));
    }
    public decimal ToDecimal() => ((decimal)Numerator) / Denominator;
    public double ToDouble() => ((double)Numerator) / Denominator;
    public bool IsInteger() => Denominator == 1;
    public static LongRational operator +(LongRational a, LongRational b)
    {
        long lcm = Lcm(a.Denominator, b.Denominator);
        long amult = lcm / a.Denominator;
        long bmult = lcm / b.Denominator;
        long nom = a.Numerator * amult + b.Numerator * bmult;
        return new LongRational(nom, lcm);
    }
    public static LongRational operator -(LongRational a, LongRational b)
    => a + (new LongRational(-1) * b);
    public static LongRational operator *(LongRational a, LongRational b)
            => new LongRational(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
    public static LongRational operator /(LongRational a, LongRational b)
            => a * new LongRational(b.Denominator, b.Numerator);
    public static bool operator ==(LongRational a, LongRational b)
        => a.Numerator == b.Numerator && a.Denominator == b.Denominator;
    public static bool operator !=(LongRational a, LongRational b)
        => !(a == b);
    public static bool operator <(LongRational a, LongRational b)
    {
        long lcm = Lcm(a.Denominator, b.Denominator);
        long amult = lcm / a.Denominator;
        long bmult = lcm / b.Denominator;
        return a.Numerator * amult < b.Numerator * bmult;
    }
    public static bool operator >(LongRational a, LongRational b)
    {
        long lcm = Lcm(a.Denominator, b.Denominator);
        long amult = lcm / a.Denominator;
        long bmult = lcm / b.Denominator;
        return a.Numerator * amult > b.Numerator * bmult;
    }
    public override bool Equals([NotNullWhen(true)] object obj)
    {
        if (obj is LongRational)
        {
            return (LongRational)obj == this;
        }
        throw new Exception($"Called {nameof(LongRational)}.{nameof(Equals)} with a non {nameof(LongRational)} object");
    }
    public bool Equals(LongRational other) => this == other;
    public override int GetHashCode() => Numerator.GetHashCode() + Denominator.GetHashCode();
    public override string ToString() => ToString(false);
    public string ToString(bool Float)
        => Float ? (((double)Numerator) / Denominator).ToString() : $"{Numerator} / {Denominator}";
}
public struct BigRational
{
    public BigInteger Denominator { get; private set; }
    public BigInteger Numerator { get; private set; }
    public BigRational()
    {
        Numerator = 0;
        Denominator = 1;
    }
    public BigRational(BigInteger value)
    {
        Numerator = value;
        Denominator = 1;
    }
    public BigRational(BigInteger nume, BigInteger denom)
    {
        Denominator = denom;
        Numerator = nume;
        Normalize();
    }
    public static BigRational Zero => new BigRational(0);
    public static BigRational One => new BigRational(1);
    private void Normalize()
    {
        if (Numerator == 0)
        {
            Denominator = 1;
            return;
        }
        BigInteger gcd = BigInteger.GreatestCommonDivisor(Numerator, Denominator);
        BigInteger sign = Denominator.Sign;
        Denominator /= gcd * sign;
        Numerator /= gcd * sign;
    }
    public static BigInteger Lcm(BigInteger a, BigInteger b)
    {
        return checked(a * (b / BigInteger.GreatestCommonDivisor(a, b)));
    }
    public decimal ToDecimal() => ((decimal)Numerator) / (decimal)Denominator;
    public double ToDouble() => ((double)Numerator) / (double)Denominator;
    public bool IsInteger() => Denominator == 1;
    public static BigRational operator +(BigRational a, BigRational b)
    {
        BigInteger lcm = Lcm(a.Denominator, b.Denominator);
        BigInteger amult = lcm / a.Denominator;
        BigInteger bmult = lcm / b.Denominator;
        BigInteger nom = a.Numerator * amult + b.Numerator * bmult;
        return new BigRational(nom, lcm);
    }
    public static BigRational operator -(BigRational a, BigRational b)
    => a + (new BigRational(-1) * b);
    public static BigRational operator *(BigRational a, BigRational b)
            => new BigRational(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
    public static BigRational operator /(BigRational a, BigRational b)
            => a * new BigRational(b.Denominator, b.Numerator);
    public static bool operator ==(BigRational a, BigRational b)
        => a.Numerator == b.Numerator && a.Denominator == b.Denominator;
    public static bool operator !=(BigRational a, BigRational b)
        => !(a == b);
    public static bool operator <(BigRational a, BigRational b)
    {
        BigInteger lcm = Lcm(a.Denominator, b.Denominator);
        BigInteger amult = lcm / a.Denominator;
        BigInteger bmult = lcm / b.Denominator;
        return a.Numerator * amult < b.Numerator * bmult;
    }
    public static bool operator >(BigRational a, BigRational b)
    {
        BigInteger lcm = Lcm(a.Denominator, b.Denominator);
        BigInteger amult = lcm / a.Denominator;
        BigInteger bmult = lcm / b.Denominator;
        return a.Numerator * amult > b.Numerator * bmult;
    }
    public override bool Equals([NotNullWhen(true)] object obj)
    {
        if (obj is BigRational)
        {
            return (BigRational)obj == this;
        }
        throw new Exception($"Called {nameof(BigRational)}.{nameof(Equals)} with a non {nameof(BigRational)} object");
    }
    public bool Equals(BigRational other) => this == other;
    public override int GetHashCode() => Numerator.GetHashCode() + Denominator.GetHashCode();
    public override string ToString() => ToString(false);
    public string ToString(bool Float)
        => Float ? (((double)Numerator) / (double)Denominator).ToString() : $"{Numerator} / {Denominator}";
}