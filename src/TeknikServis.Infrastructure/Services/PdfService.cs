using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Domain.Entities;

namespace TeknikServis.Infrastructure.Services;

public class PdfService : IPdfService
{
    public PdfService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] GenerateTeklifPdf(Teklif teklif, Musteri musteri, Cihaz cihaz, Dukkan dukkan)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);

                page.Header().Column(col =>
                {
                    col.Item().Text(dukkan.Ad).FontSize(20).Bold();
                    col.Item().Text($"Tel: {dukkan.Telefon} | Email: {dukkan.Email}");
                    col.Item().Text(dukkan.Adres ?? "");
                    col.Item().PaddingVertical(10).LineHorizontal(1);
                });

                page.Content().Column(col =>
                {
                    col.Item().PaddingVertical(10).Text($"TEKLİF NO: {teklif.TeklifNo}").FontSize(16).Bold();
                    col.Item().Text($"Tarih: {teklif.OlusturmaTarihi:dd.MM.yyyy}");
                    col.Item().Text($"Geçerlilik: {teklif.GecerlilikTarihi:dd.MM.yyyy}");

                    col.Item().PaddingVertical(10).Text("MÜŞTERİ BİLGİLERİ").Bold();
                    col.Item().Text(musteri.AdSoyad ?? musteri.FirmaAdi ?? "");
                    col.Item().Text($"Tel: {musteri.Telefon}");
                    col.Item().Text(musteri.Email ?? "");

                    col.Item().PaddingVertical(10).Text("CİHAZ BİLGİLERİ").Bold();
                    col.Item().Text($"{cihaz.CihazTanim?.Marka} {cihaz.CihazTanim?.Model}");
                    col.Item().Text($"Seri No: {cihaz.SeriNo ?? "-"}");

                    col.Item().PaddingVertical(10).Text("TEKLİF KALEMLERİ").Bold();

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Açıklama").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Miktar").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Birim Fiyat").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Toplam").Bold();
                        });

                        foreach (var kalem in teklif.Kalemler)
                        {
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten1).Padding(5).Text(kalem.Aciklama);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten1).Padding(5).Text(kalem.Miktar.ToString());
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten1).Padding(5).Text($"{kalem.BirimFiyat:N2} ₺");
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten1).Padding(5).Text($"{kalem.ToplamFiyat:N2} ₺");
                        }
                    });

                    col.Item().PaddingTop(10).AlignRight().Text($"TOPLAM: {teklif.ToplamTutar:N2} ₺").FontSize(14).Bold();

                    if (!string.IsNullOrEmpty(teklif.Aciklama))
                    {
                        col.Item().PaddingVertical(10).Text("AÇIKLAMA").Bold();
                        col.Item().Text(teklif.Aciklama);
                    }
                });

                page.Footer().AlignCenter().Text($"Sayfa 1 / 1");
            });
        });

        return document.GeneratePdf();
    }

    public byte[] GenerateFaturaPdf(Fatura fatura, Musteri musteri, Cihaz cihaz, Dukkan dukkan)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);

                page.Header().Column(col =>
                {
                    col.Item().Text(dukkan.Ad).FontSize(20).Bold();
                    col.Item().Text($"Tel: {dukkan.Telefon} | Email: {dukkan.Email}");
                    col.Item().Text(dukkan.Adres ?? "");
                    if (!string.IsNullOrEmpty(dukkan.VergiNo))
                        col.Item().Text($"Vergi No: {dukkan.VergiNo}");
                    col.Item().PaddingVertical(10).LineHorizontal(1);
                });

                page.Content().Column(col =>
                {
                    col.Item().PaddingVertical(10).Text($"FATURA NO: {fatura.FaturaNo}").FontSize(16).Bold();
                    col.Item().Text($"Fatura Tarihi: {fatura.FaturaTarihi:dd.MM.yyyy}");

                    col.Item().PaddingVertical(10).Text("MÜŞTERİ BİLGİLERİ").Bold();
                    col.Item().Text(musteri.AdSoyad ?? musteri.FirmaAdi ?? "");
                    col.Item().Text($"Tel: {musteri.Telefon}");
                    col.Item().Text(musteri.Email ?? "");
                    if (!string.IsNullOrEmpty(musteri.VergiNo))
                        col.Item().Text($"Vergi No: {musteri.VergiNo}");

                    col.Item().PaddingVertical(10).Text("CİHAZ BİLGİLERİ").Bold();
                    col.Item().Text($"{cihaz.CihazTanim?.Marka} {cihaz.CihazTanim?.Model}");
                    col.Item().Text($"Seri No: {cihaz.SeriNo ?? "-"}");

                    col.Item().PaddingVertical(10).Text("FATURA KALEMLERİ").Bold();

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Açıklama").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Miktar").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Birim Fiyat").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Toplam").Bold();
                        });

                        foreach (var kalem in fatura.Kalemler)
                        {
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten1).Padding(5).Text(kalem.Aciklama);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten1).Padding(5).Text(kalem.Miktar.ToString());
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten1).Padding(5).Text($"{kalem.BirimFiyat:N2} ₺");
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten1).Padding(5).Text($"{kalem.ToplamFiyat:N2} ₺");
                        }
                    });

                    col.Item().PaddingTop(10).AlignRight().Column(totals =>
                    {
                        totals.Item().Text($"Ara Toplam: {fatura.ToplamTutar:N2} ₺");
                        totals.Item().Text($"KDV (%{fatura.KdvOrani}): {fatura.KdvTutar:N2} ₺");
                        totals.Item().Text($"GENEL TOPLAM: {fatura.GenelToplam:N2} ₺").FontSize(14).Bold();
                    });

                    if (!string.IsNullOrEmpty(fatura.Aciklama))
                    {
                        col.Item().PaddingVertical(10).Text("AÇIKLAMA").Bold();
                        col.Item().Text(fatura.Aciklama);
                    }
                });

                page.Footer().AlignCenter().Text($"Sayfa 1 / 1");
            });
        });

        return document.GeneratePdf();
    }
}