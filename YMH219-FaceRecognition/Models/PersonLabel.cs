namespace FaceRecognition.Models;

// Veri setindeki bir etiket (kişi / hal) ve örnek sayısı
public sealed record PersonLabel(string Name, int SampleCount, string FolderPath);
