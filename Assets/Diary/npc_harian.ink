=== satpam ===
{&
    - Kamu kok pucet gitu? Sakit? # speaker: satpam
      Gapapa kok, Pak. # speaker: surya
    - Ada yang bisa dibantu? # speaker: satpam
      Nggak, Pak. Makasih. # speaker: surya
    - Selamat pagi lagi. # speaker: satpam
      ...pagi lagi, Pak. # speaker: surya
}
-> DONE

=== darsono ===
{&
    - Tanganmu gemeteran, Nak. # speaker: darsono
      Gapapa, Pak. # speaker: surya
    - Topeng itu bukan buat sembunyi. # speaker: darsono
      Terus buat apa, Pak? # speaker: surya
    - Mampir lagi kalau tanganmu lagi nganggur. # speaker: darsono
      Iya, Pak. # speaker: surya
}
-> DONE

=== ayah ===
{&
    - Kamu kenapa? # speaker: ayah
      Gapapa. # speaker: surya
    - Udah makan? # speaker: ayah
      Udah. # speaker: surya
    - Ayah pulang telat lagi nanti. # speaker: ayah
      Iya, Yah. # speaker: surya
}
-> DONE

=== laras ===
{&
    - -> capek
    - -> hari_pertama
    - -> betah
}

= capek
Kamu keliatan capek. Baik-baik aja? # speaker: laras
+ [Jujur]
    Kurang tidur, Kak. # speaker: surya
    Jangan dipaksa. Mampir PMR kalau pusing. # speaker: laras
+ [Aman]
    Gapapa kok, Kak. # speaker: surya
    Ya udah. Tapi jangan dipaksain. # speaker: laras
+ [Balik nanya]
    Kelihatan banget ya? # speaker: surya
    Banget. Muka kamu jujur banget, Sur. # speaker: laras
- -> DONE

= hari_pertama
Eh, anak baru! Gimana hari pertamamu? # speaker: laras
+ [Jujur]
    Rame banget. Aku belum kebiasa. # speaker: surya
    Wajar. Aku dulu bengong seminggu. # speaker: laras
+ [Aman]
    Lancar kok, Kak. # speaker: surya
    Syukur deh. # speaker: laras
+ [Balik nanya]
    Kakak dulu gimana? # speaker: surya
    Aku? Nyasar ke kelas orang. Dua kali. # speaker: laras
- -> DONE

= betah
Masih betah di sini? # speaker: laras
+ [Jujur]
    Belum tau, Kak. # speaker: surya
    Nggak apa-apa. Nggak ada yang betah di minggu pertama. # speaker: laras
+ [Aman]
    Betah kok. # speaker: surya
    Bagus kalau gitu. # speaker: laras
+ [Balik nanya]
    Kakak betah? # speaker: surya
    Sekarang iya. Dulu enggak. # speaker: laras
- -> DONE