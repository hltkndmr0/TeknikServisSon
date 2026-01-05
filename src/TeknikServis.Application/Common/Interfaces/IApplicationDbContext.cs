using Microsoft.EntityFrameworkCore;
using TeknikServis.Domain.Entities;

namespace TeknikServis.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Dukkan> Dukkanlar { get; }
    DbSet<Kullanici> Kullanicilar { get; }
    DbSet<Musteri> Musteriler { get; }
    DbSet<DukkanMusteri> DukkanMusterileri { get; }
    DbSet<CihazKategori> CihazKategorileri { get; }
    DbSet<CihazTanim> CihazTanimlari { get; }
    DbSet<Cihaz> Cihazlar { get; }
    DbSet<IsDurumu> IsDurumlari { get; }
    DbSet<IsEmri> IsEmirleri { get; }
    DbSet<IsNotu> IsNotlari { get; }
    DbSet<IsDurumGecmisi> IsDurumGecmisleri { get; }
    DbSet<Teklif> Teklifler { get; }
    DbSet<TeklifKalem> TeklifKalemleri { get; }
    DbSet<Fatura> Faturalar { get; }
    DbSet<FaturaKalem> FaturaKalemleri { get; }
    DbSet<KayitTalep> KayitTalepleri { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}