VAR beat = 0

-> dispatch

=== dispatch ===
{
    - beat == 0: -> perkenalan
    - beat == 1: -> ajakan_nongkrong
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

=== basa_basi ===
Hati-hati di jalan ya, Sur. # speaker: brama
-> DONE