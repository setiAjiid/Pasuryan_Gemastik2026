VAR beat = 0

-> dispatch

=== dispatch ===
{
    - beat == 0: -> perkenalan
    - beat == 1: -> ajakan_nongkrong
    - beat == 2: -> konflik_brama
    - else: -> basa_basi
}

=== perkenalan ===
Ruang tamu masih sepi. Cuma ada suara jam dinding. # speaker: Naratif # mode: narasi
Halo, anak baru! Selamat datang di kampung. # speaker: brama
Bahu gue tiba-tiba berat. # speaker: Surya # mode: batin # tensi: 20
Nggak apa-apa kok, Pak. # speaker: surya
~ beat = 1
-> DONE

=== ajakan_nongkrong ===
Eh Sur, ntar sore ikut nongkrong ga? # speaker: brama
* [Ikut aja lah]
    Boleh, gue ikut. # speaker: surya
    Padahal capek banget. # speaker: Surya # mode: batin # energy: -10
* [Nolak halus]
    Duh, ntar dulu deh. # speaker: surya
    Lega. Tapi kok rasanya bersalah. # speaker: Surya # mode: batin # mh: -5
* [Diam aja]
    ... # speaker: surya # tensi: 15
- Yaudah, kabarin aja ya. # speaker: brama
~ beat = 2
-> DONE

=== konflik_brama ===
Eh, muka lo kenapa sih dari tadi? # speaker: brama
Dada gue mulai panas. # speaker: Surya # mode: batin # tensi: 15 # jagatlakon
* [tenang]
    Napas gue tarik pelan-pelan. # speaker: Surya # mode: batin
    ** Gue oke kok. Cuma butuh napas bentar. # speaker: surya
    ** Tenang, gue nggak apa-apa. Beneran. # speaker: surya
    ** Nggak usah khawatir, gue cuma capek. # speaker: surya
    -- Brama ngangguk pelan. Ga maksa. # speaker: Naratif # mode: narasi # tensi: -20
* [tegas]
    Ada yang naik sampai tenggorokan. # speaker: Surya # mode: batin
    ** Bisa ga sih lo ga usah ikut campur? # speaker: surya
    ** Urus aja urusan lo sendiri. # speaker: surya
    ** Berisik. Gue lagi males. # speaker: surya
    -- Brama diem. Suasananya jadi kaku. # speaker: Naratif # mode: narasi # mh: -10
- Bel masuk bunyi. # speaker: Naratif # mode: narasi
~ beat = 3
-> DONE

=== basa_basi ===
Hati-hati di jalan ya, Sur. # speaker: brama
-> DONE