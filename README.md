# 📚 Kütüphane Otomasyon Sistemi

Bu proje, bir kütüphane otomasyon sistemi olup, kullanıcıların kitapları ödünç almasını ve iade etmesini sağlar.  
Admin paneli üzerinden kitapları yönetebilir, kullanıcıları listeleyebilir ve ödünç alınan kitapları görüntüleyebilirsiniz.

## 🚀 Özellikler
- 📖 **Kitap Yönetimi:** Kitap ekleme, düzenleme, silme işlemleri (Admin)
- 📥 **Ödünç Alma:** Kullanıcıların kitap ödünç alabilmesi
- 📤 **İade Etme:** Kullanıcıların ödünç aldığı kitapları iade edebilmesi
- 📋 **Ödünç Listeleme:** Kullanıcıların ve adminin ödünç alınan kitapları görebilmesi
- 👥 **Kullanıcı Yönetimi:** Admin tarafından kullanıcıları görüntüleme ve silme işlemleri
- 🔐 **Yetkilendirme:** Admin ve Kullanıcı rollerine göre yetkilendirme
- 🎨 **Bootstrap ile Şık Arayüz**

## 🛠 Kurulum
### 1️⃣ Projeyi Klonlayın
```sh
git clone https://github.com/kullanici/kutuphane-otomasyon.git
cd kutuphane-otomasyon
```

### 2️⃣ Gerekli Bağımlılıkları Yükleyin
```sh
dotnet restore
```

### 3️⃣ Veritabanını Güncelleyin
```sh
dotnet ef database update
```

### 4️⃣ Projeyi Çalıştırın
```sh
dotnet run
```

## 👥 Kullanıcı Rolleri
| Kullanıcı | Email                 | Şifre      | Rol    |
|-----------|----------------------|------------|--------|
| Admin     | admin@library.com    | Admin123!  | Admin  |
| Kullanıcı | user@library.com     | User123!   | User   |

## 📸 Ekran Görüntüleri
### 📖 Kitap Listesi
![KitapListesi](https://github.com/user-attachments/assets/fd93b4c2-205d-4803-9606-a676afb99a0a)


