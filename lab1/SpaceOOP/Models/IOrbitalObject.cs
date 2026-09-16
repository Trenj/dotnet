namespace SpaceOOP.Models;

/// <summary>Поведение объектов с орбитальными характеристиками.</summary>
public interface IOrbitalObject
{
    double OrbitalPeriod { get; }

    double OrbitRadius { get; }

    void ShowOrbitInfo();
}
