# Pasuryan

Project untuk Gemastik 2026.

## Setup

Dibuat dengan **Unity 6000.0.47f1**. Sebisa mungkin samain version unitynya kalau mau edit/extend. Kalau dibuka pakai versi Unity yang berbeda, Unity akan otomatis meng-upgrade file di `ProjectSettings/`, dan perubahan itu ikut ke-commit. Ujung-ujungnya konflik yang nggak jelas asalnya dan susah buat dilacak.

Cara mulai:
1. Clone this repo.
2. Buka Unity Hub, Add project dari disk, pilih folder hasil clone.
3. Buka pakai 6000.0.47f1. Import pertama agak lama (Unity bikin folder `Library/` dari nol), jadi tinggal ditinggal :b
4. Scene utama ada di `Assets/Scenes/`.

## Git LFS

Repo ini pakai Git LFS untuk file berat (sprite, audio, model). Sebelum clone atau commit pertama, jalankan sekali:

```
git lfs install
```

Kalau dilewat, LFS nggak akan jalan, tapi Git juga nggak akan protes :v. Gambar-gambar bakal masuk mentah ke history dan repo-nya membengkak permanen. Aturan file mana yang lewat LFS sudah diatur di `.gitattributes`.

## Struktur

Yang di-commit cuma tiga folder:

- `Assets/` — semua isi project
- `Packages/` — daftar package Unity
- `ProjectSettings/` — versi Unity, physics, tag, layer, render pipeline

`Library/`, `Temp/`, `obj/`, `Logs/` sengaja diabaikan. Itu cache, tiap komputer generate sendiri, ukurannya bisa giga-an. Tapi aman se harusnya, ini udah otomatis masuk .gitignore dipasangin ama unitynya sendiri

## Soal file .meta

Jangan pernah hapus file `.meta`, dan pastikan selalu ikut ke-commit bareng file aslinya. Unity nyimpen referensi antar-file pakai GUID yang ada di dalam `.meta`, bukan pakai nama atau path file. So, kalau `.meta`-nya hilang, Unity bikin GUID baru, dan semua referensi ke file itu putus, which is biasanya bakal bikin error kek, `Missing (Mono Script)` di Inspector, sprite jadi kotak putih, prefab rusak, dll.

## Stack
- Unity 6 (URP, setup 2D)
- Input System (newest, bukan yg legacy)
