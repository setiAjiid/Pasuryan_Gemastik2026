# Pasuryan

A 2D narrative game about the masks we wear to get through the day. Built in Unity 6 for **GEMASTIK 2026** (Game Application Development).

You play as Surya, a new student at SMA 01 Anggeran. Every conversation asks you to put on a "mask" (calm, cheerful, firm, sad, or wise), and every mask costs energy. Hide too much and your mental health slips. Let the pressure build and it turns into a crisis you have to work through.

## Gameplay systems

- **Masks.** Five masks mapped to basic emotions. Choosing one in a dialogue spends energy and shapes how the scene plays out.
- **Inner state.** Three stats (mental health, energy, tension) shown as UI bars and changed by dialogue choices through Ink tags.
- **Crises.** When tension maxes out, a crisis starts and mental health slowly drains until you resolve it through a minigame: guided breathing, piecing a broken mask back together, or restoring a fractured scene.
- **Diary.** At the end of each day Surya writes a journal entry. For each memory you either suppress it or try to understand it, and that choice carries a cost.
- **Dialogue and exploration.** Branching dialogue written in [Ink](https://www.inklestudios.com/ink/), with speech bubbles, narration boxes, and interactable doors and objects across the house, classroom, and school corridor.

## Tech

- Unity 6 (6000.0.47f1), URP 2D
- C#, with a state machine for the player and an event-driven stat system
- Ink for narrative scripting
- New Input System
- Git LFS for art and audio

---

## Development notes (Bahasa Indonesia)

### Setup

Dibuat dengan **Unity 6000.0.47f1**. Sebisa mungkin samain version unitynya kalau mau edit/extend. Kalau dibuka pakai versi Unity yang berbeda, Unity akan otomatis meng-upgrade file di `ProjectSettings/`, dan perubahan itu ikut ke-commit. Ujung-ujungnya konflik yang nggak jelas asalnya dan susah buat dilacak.

Cara mulai:
1. Clone this repo.
2. Buka Unity Hub, Add project dari disk, pilih folder hasil clone.
3. Buka pakai 6000.0.47f1. Import pertama agak lama (Unity bikin folder `Library/` dari nol), jadi tinggal ditinggal :b
4. Scene utama ada di `Assets/Scenes/`.

### Git LFS

Repo ini pakai Git LFS untuk file berat (sprite, audio, model). Sebelum clone atau commit pertama, jalankan sekali:

```
git lfs install
```

Kalau dilewat, LFS nggak akan jalan, tapi Git juga nggak akan protes :v. Gambar-gambar bakal masuk mentah ke history dan repo-nya membengkak permanen. Aturan file mana yang lewat LFS sudah diatur di `.gitattributes`.

### Struktur

Yang di-commit cuma tiga folder:

- `Assets/` — semua isi project
- `Packages/` — daftar package Unity
- `ProjectSettings/` — versi Unity, physics, tag, layer, render pipeline

`Library/`, `Temp/`, `obj/`, `Logs/` sengaja diabaikan. Itu cache, tiap komputer generate sendiri, ukurannya bisa giga-an. Tapi aman se harusnya, ini udah otomatis masuk .gitignore dipasangin ama unitynya sendiri

### Soal file .meta

Jangan pernah hapus file `.meta`, dan pastikan selalu ikut ke-commit bareng file aslinya. Unity nyimpen referensi antar-file pakai GUID yang ada di dalam `.meta`, bukan pakai nama atau path file. So, kalau `.meta`-nya hilang, Unity bikin GUID baru, dan semua referensi ke file itu putus, which is biasanya bakal bikin error kek, `Missing (Mono Script)` di Inspector, sprite jadi kotak putih, prefab rusak, dll.

### Stack
- Unity 6 (URP, setup 2D)
- Input System (newest, bukan yg legacy)
