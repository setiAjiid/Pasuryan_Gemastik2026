VAR koridor_insiden = false
VAR bohong_ayah = false

=== day1 ===
Hari ini aku merasa telah melakukan suatu kejahatan besar.

{ koridor_insiden:
    Pagi tadi napasku hampir habis di koridor. Aku menenangkannya sendiri, dan itu bukan hal kecil.
}

{ bohong_ayah:
    Aku bilang tasku tertinggal di sekolah. Ayah percaya begitu saja.
- else:
    Aku pulang, dan tidak ada yang bertanya apa-apa.
}

* [redam: Tutup saja malam ini.]
    Masa bodoh dengan tas itu. Besok juga aku ambil sendiri. # mh: -10

* [pahami: Tulis kenapa kamu bohong.]
    Aku bohong karena tidak sanggup menjelaskan kenapa aku pulang selarut ini. # ketik
    Bohong terasa lebih murah daripada bercerita, tapi harganya menumpuk. # ketik # energy: -10

-
Lampu meja kumatikan.
-> DONE