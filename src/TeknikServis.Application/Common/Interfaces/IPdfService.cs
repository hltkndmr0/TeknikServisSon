using TeknikServis.Domain.Entities;

namespace TeknikServis.Application.Common.Interfaces;

public interface IPdfService
{
    byte[] GenerateTeklifPdf(Teklif teklif, Musteri musteri, Cihaz cihaz, Dukkan dukkan);
    byte[] GenerateFaturaPdf(Fatura fatura, Musteri musteri, Cihaz cihaz, Dukkan dukkan);
}