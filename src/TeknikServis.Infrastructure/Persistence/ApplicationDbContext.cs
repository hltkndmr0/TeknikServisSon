using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Domain.Entities;

namespace TeknikServis.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Dukkan> Dukkanlar => Set<Dukkan>();
    public DbSet<Kullanici> Kullanicilar => Set<Kullanici>();
    public DbSet<Musteri> Musteriler => Set<Musteri>();
    public DbSet<DukkanMusteri> DukkanMusterileri => Set<DukkanMusteri>();
    public DbSet<CihazKategori> CihazKategorileri => Set<CihazKategori>();
    public DbSet<CihazTanim> CihazTanimlari => Set<CihazTanim>();
    public DbSet<Cihaz> Cihazlar => Set<Cihaz>();
    public DbSet<IsDurumu> IsDurumlari => Set<IsDurumu>();
    public DbSet<IsEmri> IsEmirleri => Set<IsEmri>();
    public DbSet<IsNotu> IsNotlari => Set<IsNotu>();
    public DbSet<IsDurumGecmisi> IsDurumGecmisleri => Set<IsDurumGecmisi>();
    public DbSet<Teklif> Teklifler => Set<Teklif>();
    public DbSet<TeklifKalem> TeklifKalemleri => Set<TeklifKalem>();
    public DbSet<Fatura> Faturalar => Set<Fatura>();
    public DbSet<FaturaKalem> FaturaKalemleri => Set<FaturaKalem>();
    public DbSet<KayitTalep> KayitTalepleri => Set<KayitTalep>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}