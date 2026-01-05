using TeknikServis.Application.Common.Interfaces;

namespace TeknikServis.Infrastructure.Services;

public class FirmaKoduGenerator : IFirmaKoduGenerator
{
    private readonly Random _random = new();

    public string Generate()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var kod = new char[6];

        for (int i = 0; i < 6; i++)
        {
            kod[i] = chars[_random.Next(chars.Length)];
        }

        return new string(kod);
    }
}