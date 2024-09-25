// Copyright © 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.TransportWeb.VehicleRouting.Common;
using Cencora.TransportWeb.VehicleRouting.Model.Places;

namespace Cencora.TransportWeb.VehicleRouting.Model.Vehicles;

/// <summary>
/// A builder for creating a shift.
/// </summary>
public sealed class ShiftBuilder
{
    private ValueRange _shiftTimeWindow;
    private Driver? _driver;
    private HashSet<Break> _breaks = new HashSet<Break>();
    private Location? _startLocation;
    private Location? _endLocation;
    private long? _maxDuration;
    private long? _maxDistance;

    /// <summary>
    /// Adds a time window to the shift.
    /// </summary>
    /// <param name="shiftTimeWindow">The time window to add.</param>
    /// <returns>The builder.</returns>
    public ShiftBuilder WithShiftTimeWindow(ValueRange shiftTimeWindow)
    {
        _shiftTimeWindow = shiftTimeWindow;
        return this;
    }

    /// <summary>
    /// Adds a driver to the shift.
    /// </summary>
    /// <param name="driver">The driver to add.</param>
    /// <returns>The builder.</returns>
    public ShiftBuilder WithDriver(Driver? driver)
    {
        _driver = driver;
        return this;
    }

    /// <summary>
    /// Removes the driver from the shift.
    /// </summary>
    /// <returns>The builder.</returns>
    public ShiftBuilder WithoutDriver()
    {
        _driver = null;
        return this;
    }

    /// <summary>
    /// Adds a break to the shift.
    /// </summary>
    /// <param name="breakToAdd">The break to add.</param>
    /// <returns>The builder.</returns>
    public ShiftBuilder WithBreak(Break breakToAdd)
    {
        _breaks.Add(breakToAdd);
        return this;
    }

    /// <summary>
    /// Adds breaks to the shift.
    /// </summary>
    /// <param name="breaks">The breaks to add.</param>
    /// <returns>The builder.</returns>
    public ShiftBuilder WithBreaks(IEnumerable<Break> breaks)
    {
        foreach (var currentBreak in breaks)
        {
            _breaks.Add(currentBreak);
        }

        return this;
    }

    /// <summary>
    /// Removes a break from the shift.
    /// </summary>
    /// <param name="breakToRemove">The break to remove.</param>
    /// <returns>The builder.</returns>
    public ShiftBuilder WithoutBreak(Break breakToRemove)
    {
        _breaks.Remove(breakToRemove);
        return this;
    }

    /// <summary>
    /// Removes breaks from the shift.
    /// </summary>
    /// <param name="breaks">The breaks to remove.</param>
    /// <returns>The builder.</returns>
    public ShiftBuilder WithoutBreaks(IEnumerable<Break> breaks)
    {
        foreach (var currentBreak in breaks)
        {
            _breaks.Remove(currentBreak);
        }

        return this;
    }

    /// <summary>
    /// Removes all breaks from the shift.
    /// </summary>
    /// <returns>The builder.</returns>
    public ShiftBuilder WithoutBreaks()
    {
        _breaks.Clear();
        return this;
    }

    /// <summary>
    /// Adds a start location to the shift.
    /// </summary>
    /// <param name="startLocation">The start location to add.</param>
    /// <returns>The builder.</returns>
    public ShiftBuilder WithStartLocation(Location startLocation)
    {
        _startLocation = startLocation;
        return this;
    }

    /// <summary>
    /// Removes the start location from the shift.
    /// </summary>
    /// <returns>The builder.</returns>
    public ShiftBuilder WithoutStartLocation()
    {
        _startLocation = null;
        return this;
    }

    /// <summary>
    /// Adds an end location to the shift.
    /// </summary>
    /// <param name="endLocation">The end location to add.</param>
    /// <returns>The builder.</returns>
    public ShiftBuilder WithEndLocation(Location endLocation)
    {
        _endLocation = endLocation;
        return this;
    }

    /// <summary>
    /// Removes the end location from the shift.
    /// </summary>
    /// <returns>The builder.</returns>
    public ShiftBuilder WithoutEndLocation()
    {
        _endLocation = null;
        return this;
    }

    /// <summary>
    /// Adds a maximum duration to the shift.
    /// </summary>
    /// <param name="maxDuration">The maximum duration to add.</param>
    /// <returns>The builder.</returns>
    public ShiftBuilder WithMaxDuration(long maxDuration)
    {
        _maxDuration = maxDuration;
        return this;
    }

    /// <summary>
    /// Removes the maximum duration from the shift.
    /// </summary>
    /// <returns>The builder.</returns>
    public ShiftBuilder WithoutMaxDuration()
    {
        _maxDuration = null;
        return this;
    }

    /// <summary>
    /// Adds a maximum distance to the shift.
    /// </summary>
    /// <param name="maxDistance">The maximum distance to add.</param>
    /// <returns>The builder.</returns>
    public ShiftBuilder WithMaxDistance(long maxDistance)
    {
        _maxDistance = maxDistance;
        return this;
    }

    /// <summary>
    /// Removes the maximum distance from the shift.
    /// </summary>
    /// <returns>The builder.</returns>
    public ShiftBuilder WithoutMaxDistance()
    {
        _maxDistance = null;
        return this;
    }

    /// <summary>
    /// Builds the shift.
    /// </summary>
    /// <returns>The shift.</returns>
    public Shift Build()
    {
        return new Shift(_shiftTimeWindow, _driver, _breaks, _startLocation, _endLocation, _maxDuration, _maxDistance);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return "ShiftBuilder";
    }
}
