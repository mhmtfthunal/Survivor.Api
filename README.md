# Survivor.Api

Survivor yarışması için hazırlanan **.NET 8 Web API** projesi. Uygulama, **Category (Kategori)** ve **Competitor (Yarışmacı)** varlıkları arasında **bire-çok (1–N)** ilişkisi kurar ve her ikisi için tam **CRUD** endpoint’leri sağlar. Veriler **EF Core** ile SQL Server üzerinde yönetilir ve **Swagger** ile test edilir.

---

## Teknolojiler

* .NET 8 (ASP.NET Core Web API)
* Entity Framework Core 8 (8.0.19)
* SQL Server / LocalDB
* Swagger (Swashbuckle)

---

## Mimari ve İçerik

```
Survivor.Api/
├─ Controllers/
│  ├─ CategoriesController.cs
│  └─ CompetitorsController.cs
├─ Data/
│  └─ SurvivorDbContext.cs
├─ Dtos/
│  ├─ CategoryDtos.cs
│  └─ CompetitorDtos.cs
├─ Entities/
│  ├─ BaseEntity.cs
│  ├─ Category.cs
│  └─ Competitor.cs
├─ appsettings.json
└─ Program.cs
```

### BaseEntity

Tüm tabloların miras aldığı ortak alanlar:

* `Id (int)`
* `CreatedDate (DateTime)`
* `ModifiedDate (DateTime)`
* `IsDeleted (bool)` — **soft delete** için kullanılır. DbContext’te global filter ile listelerden otomatik gizlenir.

### İlişki

* **Category 1 — N Competitor**
* `Competitor.CategoryId` zorunludur, `DeleteBehavior.Restrict` uygulanır.

---

## Kurulum

### 1) Bağımlılıklar

NuGet paketleri proje içinde tanımlıdır. Manuel kurulum için:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### 2) Bağlantı Ayarı

`appsettings.json` içerisinde bağlantı dizesi:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SurvivorDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

İsterseniz farklı bir SQL Server sunucusu kullanabilirsiniz.

### 3) Migration ve Veritabanı

**Terminal (CLI)** ile:

```bash
# proje klasörüne girin
dotnet ef migrations add InitialCreate --context SurvivorDbContext
dotnet ef database update --context SurvivorDbContext
```

**Visual Studio – Package Manager Console** ile:

```powershell
Add-Migration InitialCreate -Project Survivor.Api -StartupProject Survivor.Api -Context SurvivorDbContext
Update-Database -Project Survivor.Api -StartupProject Survivor.Api -Context SurvivorDbContext
```

### 4) Çalıştırma

```bash
dotnet run
```

Tarayıcıdan **/swagger** adresine giderek endpoint’leri test edin.

---

## Seed Veriler

`OnModelCreating` içinde örnek veriler tanımlıdır.

**Kategoriler**

* (1) Ünlüler
* (2) Gönüllüler

**Yarışmacılar (özet)**

* Ünlüler: Acun Ilıcalı, Aleyna Avcı, Hadise Açıkgöz, Sertan Bozkuş, Özge Açık, Kıvanç Tatlıtuğ
* Gönüllüler: Ahmet Yılmaz, Elif Demirtaş, Cem Öztürk, Ayşe Karaca

---

## DTO’lar

* `CategoryCreateUpdateDto { name }`
* `CompetitorCreateUpdateDto { firstName, lastName, categoryId }`

> JSON alanları **camelCase** olarak beklenir.

---

## Endpoint’ler

### CategoryController

* **GET** `/api/categories` → Tüm kategoriler
* **GET** `/api/categories/{id}` → Id ile kategori (yarışmacıları dahil)
* **POST** `/api/categories` → Yeni kategori oluşturur
* **PUT** `/api/categories/{id}` → Kategori günceller
* **DELETE** `/api/categories/{id}` → Kategoriyi soft delete yapar

#### Örnek istekler

```bash
# Listele
curl -s https://localhost:5001/api/categories

# Detay
curl -s https://localhost:5001/api/categories/1

# Oluştur
curl -s -X POST https://localhost:5001/api/categories \
  -H "Content-Type: application/json" \
  -d '{"name":"All-Star"}'

# Güncelle
curl -s -X PUT https://localhost:5001/api/categories/3 \
  -H "Content-Type: application/json" \
  -d '{"name":"All-Star 2024"}'

# Sil (soft delete)
curl -s -X DELETE https://localhost:5001/api/categories/3
```

### CompetitorsController

* **GET** `/api/competitors` → Tüm yarışmacılar (+ kategori)
* **GET** `/api/competitors/{id}` → Id ile yarışmacı
* **GET** `/api/competitors/categories/{categoryId}` → Kategoriye göre yarışmacılar
* **POST** `/api/competitors` → Yeni yarışmacı oluşturur
* **PUT** `/api/competitors/{id}` → Yarışmacı günceller
* **DELETE** `/api/competitors/{id}` → Yarışmacıyı soft delete yapar

#### Örnek istekler

```bash
# Listele
curl -s https://localhost:5001/api/competitors

# Detay
curl -s https://localhost:5001/api/competitors/1

# Kategoriye göre
curl -s https://localhost:5001/api/competitors/categories/2

# Oluştur
curl -s -X POST https://localhost:5001/api/competitors \
  -H "Content-Type: application/json" \
  -d '{"firstName":"Mehmet","lastName":"Ünal","categoryId":2}'

# Güncelle
curl -s -X PUT https://localhost:5001/api/competitors/10 \
  -H "Content-Type: application/json" \
  -d '{"firstName":"Ayşe","lastName":"Karaca","categoryId":2}'

# Sil (soft delete)
curl -s -X DELETE https://localhost:5001/api/competitors/10
```

---

## Notlar

* JSON döngülerini önlemek için `ReferenceHandler.IgnoreCycles` aktiftir.
* Silme işlemleri **soft delete** olarak uygulanır (`IsDeleted = true`). Global filtre nedeniyle listelerde görünmezler.
* `DeleteBehavior.Restrict` nedeniyle bir kategoriyi silmeden önce o kategoriye bağlı yarışmacılar soft delete yapılmalıdır.

---

