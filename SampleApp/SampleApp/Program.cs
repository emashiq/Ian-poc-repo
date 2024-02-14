using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;

PdfDocument outputPdf = new PdfDocument();

// Open the existing PDF document
PdfDocument inputPdf = PdfReader.Open("path/to/input.pdf", PdfDocumentOpenMode.Import);

// Iterate through the pages and extract images
foreach (PdfPage page in inputPdf.Pages)
{
    

    // Add the image to the output PDF document
    PdfPage outputPage = outputPdf.AddPage();
    XGraphics gfx = XGraphics.FromPdfPage(outputPage);
    gfx.DrawImage(image, 0, 0);
}

// Save the output PDF document
outputPdf.Save("path/to/output.pdf");