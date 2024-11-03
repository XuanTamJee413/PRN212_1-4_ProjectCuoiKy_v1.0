using System;
using System.Linq;
using ConsoleAppTestDb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("jsconfig1.json", optional: true, reloadOnChange: true)
    .Build();

var optionsBuilder = new DbContextOptionsBuilder<EnglishVocabularyQuizContext>();
optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

using (var context = new EnglishVocabularyQuizContext(optionsBuilder.Options))
{
    // Lấy danh sách từ vựng
    var vocabularies = context.Vocabularies.ToList();

    foreach (var vocab in vocabularies)
    {
        // Tìm các từ vựng khác có cùng WordType
        var options = vocabularies
            .Where(v => v.WordType == vocab.WordType && v.WordId != vocab.WordId)
            .OrderBy(v => Guid.NewGuid()) // Xáo trộn danh sách
            .Take(3) // Lấy 3 đáp án ngẫu nhiên
            .ToList();

        // Thêm đáp án đúng vào danh sách đáp án
        options.Insert(new Random().Next(0, 4), vocab); // Chèn đáp án đúng vào vị trí ngẫu nhiên

        Console.WriteLine($"Câu hỏi: {vocab.EnglishWord} (Dịch: {vocab.VietnameseWord})");
        Console.WriteLine("Các đáp án:");

        // In ra các đáp án
        char optionLabel = 'A'; // Để đánh số đáp án từ A
        foreach (var option in options)
        {
            Console.WriteLine($"{optionLabel}. {option.VietnameseWord}"); // In ra đáp án
            optionLabel++; // Tăng chỉ số đáp án
        }
        Console.WriteLine(); // Dòng trống giữa các câu hỏi
    }
}
