-- Sepet Tablosu Oluşturma
CREATE TABLE Sepet (
    SepetID INT PRIMARY KEY IDENTITY(1,1),
    UrunAd NVARCHAR(200),
    UrunFiyat DECIMAL(10,2),
    UrunResim NVARCHAR(500),
    Adet INT DEFAULT 1,
    EklenmeTarihi DATETIME DEFAULT GETDATE()
);
