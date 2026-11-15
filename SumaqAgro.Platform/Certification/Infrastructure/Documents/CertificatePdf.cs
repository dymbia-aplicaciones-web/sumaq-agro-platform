using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using QRCoder;
using SumaqAgro.Platform.Certification.Domain.Model.Aggregates;

namespace SumaqAgro.Platform.Certification.Infrastructure.Documents;

public sealed class CertificatePdf
{
    static CertificatePdf()
    {
        if (OperatingSystem.IsWindows())
            GlobalFontSettings.UseWindowsFontsUnderWindows = true;
        else
            GlobalFontSettings.FontResolver = new LinuxCertificateFonts();
    }

    private sealed class LinuxCertificateFonts : IFontResolver
    {
        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic) =>
            new(isBold ? "DejaVuSans-Bold.ttf" : "DejaVuSans.ttf");

        public byte[] GetFont(string faceName) =>
            File.ReadAllBytes(Path.Combine("/usr/share/fonts/truetype/dejavu", faceName));
    }

    public byte[] Render(QualityCertificate certificate, HarvestBatch batch, string verificationUrl)
    {
        using var qrData = QRCodeGenerator.GenerateQrCode(verificationUrl, QRCodeGenerator.ECCLevel.M);
        using var document = new PdfDocument();
        document.Info.Title = $"SumaqAgro - {certificate.CertificateNumber}";
        var page = document.AddPage();

        using (var graphics = XGraphics.FromPdfPage(page))
        {
            var title = new XFont("Arial", 21, XFontStyleEx.Bold);
            var body = new XFont("Arial", 12);
            graphics.DrawString("SumaqAgro | Quality Certificate", title, XBrushes.Black, 45, 60);
            string[] lines =
            [
                $"Certificate: {certificate.CertificateNumber}",
                $"Batch: {batch.BatchCode}",
                $"Crop: {batch.CropType}",
                $"Category: {certificate.QualityCategory}",
                FormattableString.Invariant($"Score: {batch.QualityScore}"),
                $"Harvest date: {batch.HarvestedOn:yyyy-MM-dd}",
                $"Issued by: {certificate.IssuedByUserId}",
                $"Issued at (UTC): {certificate.IssuedAt.UtcDateTime:u}",
                $"Status: {certificate.Status}"
            ];
            for (var index = 0; index < lines.Length; index++)
                graphics.DrawString(lines[index], body, XBrushes.Black, 45, 105 + index * 20);

            graphics.DrawString($"SHA-256: {certificate.CertificateHash}", new XFont("Arial", 9), XBrushes.Black, 45, 305);
            const double moduleSize = 3;
            var modules = qrData.ModuleMatrix;
            for (var row = 0; row < modules.Count; row++)
            {
                for (var column = 0; column < modules[row].Length; column++)
                {
                    if (modules[row][column])
                        graphics.DrawRectangle(XBrushes.Black, 45 + column * moduleSize,
                            330 + row * moduleSize, moduleSize, moduleSize);
                }
            }
        }

        using var output = new MemoryStream();
        document.Save(output, false);
        return output.ToArray();
    }
}
