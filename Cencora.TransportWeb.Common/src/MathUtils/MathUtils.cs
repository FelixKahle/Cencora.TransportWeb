// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

// ReSharper disable RedundantOverflowCheckingContext

using System.Runtime.CompilerServices;

namespace Cencora.TransportWeb.Common.MathUtils;

/// <summary>
/// Provides a set of methods for mathematical operations.
/// </summary>
public static class MathUtils
{
    /// <summary>
    /// Adds two integers and returns the result. If an overflow occurs, the default value is returned.
    /// </summary>
    /// <param name="a">The first integer to add.</param>
    /// <param name="b">The second integer to add.</param>
    /// <param name="defaultValue">The default value to return if an overflow occurs.</param>
    /// <returns>The sum of the two integers or the default value if an overflow occurs.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long AddOrDefault(long a, long b, long defaultValue)
    {
        try
        {
            return checked(a + b);
        }
        catch (OverflowException)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Subtracts two integers and returns the result. If an overflow occurs, the default value is returned.
    /// </summary>
    /// <param name="a">The integer to subtract from.</param>
    /// <param name="b">The integer to subtract.</param>
    /// <param name="defaultValue">The default value to return if an overflow occurs.</param>
    /// <returns>The difference of the two integers or the default value if an overflow occurs.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long SubtractOrDefault(long a, long b, long defaultValue)
    {
        try
        {
            return checked(a - b);
        }
        catch (OverflowException)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Multiplies two integers and returns the result. If an overflow occurs, the default value is returned.
    /// </summary>
    /// <param name="a">The first integer to multiply.</param>
    /// <param name="b">The second integer to multiply.</param>
    /// <param name="defaultValue">The default value to return if an overflow occurs.</param>
    /// <returns>The product of the two integers or the default value if an overflow occurs.</returns>
    public static long MultiplyOrDefault(long a, long b, long defaultValue)
    {
        try
        {
            return checked(a * b);
        }
        catch (OverflowException)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Divides two integers and returns the result. If an overflow occurs, the default value is returned.
    /// </summary>
    /// <param name="a">The integer to divide.</param>
    /// <param name="b">The integer to divide by.</param>
    /// <param name="defaultValue">The default value to return if an overflow occurs.</param>
    /// <returns>The quotient of the two integers or the default value if an overflow occurs.</returns>
    /// <exception cref="DivideByZeroException">Thrown when <paramref name="b"/> is zero.</exception>
    public static long DivideOrDefault(long a, long b, long defaultValue)
    {
        try
        {
            return checked(a / b);
        }
        catch (OverflowException)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Divides two integers and returns the result. If an overflow occurs, the default value is returned.
    /// If <paramref name="b"/> is zero, <paramref name="divideByZeroDefaultValue"/> is returned.
    /// </summary>
    /// <param name="a">The integer to divide.</param>
    /// <param name="b">The integer to divide by.</param>
    /// <param name="defaultValue">The default value to return if an overflow occurs.</param>
    /// <param name="divideByZeroDefaultValue">The value to return if <paramref name="b"/> is zero.</param>
    /// <returns>The quotient of the two integers or the default value if an overflow occurs.</returns>
    public static long DivideOrDefault(long a, long b, long defaultValue, long divideByZeroDefaultValue)
    {
        if (b == 0)
        {
            return divideByZeroDefaultValue;
        }

        try
        {
            return checked(a / b);
        }
        catch (OverflowException)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Adds two decimals and returns the result. If an overflow occurs, the default value is returned.
    /// </summary>
    /// <param name="a">The first decimal to add.</param>
    /// <param name="b">The second decimal to add.</param>
    /// <param name="defaultValue">The default value to return if an overflow occurs.</param>
    /// <returns>The sum of the two decimals or the default value if an overflow occurs.</returns>
    public static decimal AddOrDefault(decimal a, decimal b, decimal defaultValue)
    {
        try
        {
            return checked(a + b);
        }
        catch (OverflowException)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Subtracts two decimals and returns the result. If an overflow occurs, the default value is returned.
    /// </summary>
    /// <param name="a">The decimal to subtract from.</param>
    /// <param name="b">The decimal to subtract.</param>
    /// <param name="defaultValue">The default value to return if an overflow occurs.</param>
    /// <returns>The difference of the two decimals or the default value if an overflow occurs.</returns>
    public static decimal SubtractOrDefault(decimal a, decimal b, decimal defaultValue)
    {
        try
        {
            return checked(a - b);
        }
        catch (OverflowException)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Multiplies two decimals and returns the result. If an overflow occurs, the default value is returned.
    /// </summary>
    /// <param name="a">The first decimal to multiply.</param>
    /// <param name="b">The second decimal to multiply.</param>
    /// <param name="defaultValue">The default value to return if an overflow occurs.</param>
    /// <returns>The product of the two decimals or the default value if an overflow occurs.</returns>
    public static decimal MultiplyOrDefault(decimal a, decimal b, decimal defaultValue)
    {
        try
        {
            return checked(a * b);
        }
        catch (OverflowException)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Divides two decimals and returns the result. If an overflow occurs, the default value is returned.
    /// </summary>
    /// <param name="a">The decimal to divide.</param>
    /// <param name="b">The decimal to divide by.</param>
    /// <param name="defaultValue">The default value to return if an overflow occurs.</param>
    /// <returns>The quotient of the two decimals or the default value if an overflow occurs.</returns>
    /// <exception cref="DivideByZeroException">Thrown when <paramref name="b"/> is zero.</exception>
    public static decimal DivideOrDefault(decimal a, decimal b, decimal defaultValue)
    {
        try
        {
            return checked(a / b);
        }
        catch (OverflowException)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Divides two decimals and returns the result. If an overflow occurs, the default value is returned.
    /// If <paramref name="b"/> is zero, <paramref name="divideByZeroDefaultValue"/> is returned.
    /// </summary>
    /// <param name="a">The decimal to divide.</param>
    /// <param name="b">The decimal to divide by.</param>
    /// <param name="defaultValue">The default value to return if an overflow occurs.</param>
    /// <param name="divideByZeroDefaultValue">The value to return if <paramref name="b"/> is zero.</param>
    /// <returns>The quotient of the two decimals or the default value if an overflow occurs.</returns>
    public static decimal DivideOrDefault(decimal a, decimal b, decimal defaultValue, decimal divideByZeroDefaultValue)
    {
        if (b == 0)
        {
            return divideByZeroDefaultValue;
        }

        try
        {
            return checked(a / b);
        }
        catch (OverflowException)
        {
            return defaultValue;
        }
    }
}
