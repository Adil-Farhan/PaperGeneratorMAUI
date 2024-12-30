using Microsoft.Maui.Graphics.Pdf;

namespace QuestionPaperMaker.Services
{
    public class PrintService : IPrintService
    {
        public async Task PrintAsync(QuestionPaper paper, IEnumerable<Question> questions)
        {
            try
            {
                // Create PDF document
                using var stream = new MemoryStream();
                using var pdf = new PdfDocument();
                var page = pdf.AddPage();
                var canvas = page.Canvas;

                // Set up fonts and styles
                var titleFont = new Font("Arial", 24, FontStyle.Bold);
                var headerFont = new Font("Arial", 14);
                var normalFont = new Font("Arial", 12);
                var codeFont = new Font("Courier New", 11);

                float y = 50;
                float margin = 50;
                float width = page.Width - (2 * margin);

                // Draw header
                canvas.DrawString(paper.Title, titleFont, Colors.Black, margin, y, HorizontalAlignment.Center, width);
                y += 40;

                if (!string.IsNullOrEmpty(paper.Description))
                {
                    canvas.DrawString(paper.Description, headerFont, Colors.Gray, margin, y, HorizontalAlignment.Center, width);
                    y += 30;
                }

                canvas.DrawString($"Total Questions: {questions.Count()}", headerFont, Colors.Black, margin, y, HorizontalAlignment.Right, width);
                y += 40;

                // Draw questions
                int questionNumber = 1;
                foreach (var question in questions)
                {
                    // Check if we need a new page
                    if (y > page.Height - 100)
                    {
                        page = pdf.AddPage();
                        canvas = page.Canvas;
                        y = 50;
                    }

                    // Question header
                    canvas.DrawString($"Q{questionNumber}. {question.Text}", normalFont, Colors.Black, margin, y);
                    y += 30;

                    // Question type specific content
                    switch (question.QuestionType.Name.ToLower())
                    {
                        case "multiple_choice":
                            foreach (var option in question.Options)
                            {
                                canvas.DrawString($"□ {option.Value}", normalFont, Colors.Black, margin + 20, y);
                                y += 25;
                            }
                            break;

                        case "code":
                            if (!string.IsNullOrEmpty(question.CodeSnippet))
                            {
                                var codeRect = new RectF(margin + 20, y, width - 20, y + 100);
                                canvas.SetFillColor(Colors.LightGray);
                                canvas.FillRectangle(codeRect);
                                canvas.DrawString(question.CodeSnippet, codeFont, Colors.Black, margin + 30, y + 10);
                                y += 120;

                                canvas.DrawString($"Language: {question.Language}", normalFont, Colors.Gray, margin + 20, y);
                                y += 20;
                            }
                            break;
                    }

                    // Answer space
                    var answerRect = new RectF(margin + 20, y, width - 20, y + 100);
                    canvas.SetStrokeColor(Colors.Gray);
                    canvas.SetStrokePattern(new float[] { 5, 5 });
                    canvas.DrawRectangle(answerRect);
                    y += 120;

                    questionNumber++;
                }

                // Save PDF
                pdf.SaveToStream(stream);

                // Get the file path
                var filePath = Path.Combine(FileSystem.CacheDirectory, $"QuestionPaper_{DateTime.Now:yyyyMMddHHmmss}.pdf");
                File.WriteAllBytes(filePath, stream.ToArray());

                // Share the file
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = paper.Title,
                    File = new ShareFile(filePath)
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to generate PDF", ex);
            }
        }
    }
} 