VAR beat = 0

-> dispatch

=== dispatch ===
{
    - beat == 0: -> sapaan
    - else: -> basa_basi
}

=== sapaan ===
Hei, kamu Pasuryan, kan? # speaker: brama
Yang katanya peringkat 1 paralel seangkatan itu? # speaker: brama
Beberapa pasang mata nengok. Ke arahku. # speaker: Surya # mode: batin # tensi: 20
Uh.. Entah kenapa waktu kayak melambat. Suara anak2 kelas kedengeran dari jauh. # speaker: Surya # mode: batin # jagatlakon

+ [tenang]
    Ah, topeng Tenang ini. Halus, sopan, terkontrol. # speaker: Surya # mode: batin
    Refleks yang jalan sendiri, tanpa pernah kusuruh. # speaker: Surya # mode: batin
    ** Bentul banget, namaku Pasuryan. # speaker: surya
    ** .....Iya. # speaker: surya
    ** Iya, betul. Itu aku. # speaker: surya
    -- Senyumku kepasang rapikan? Ah, aku bahkan nggak ngerasain mukaku sendiri. # speaker: Surya # mode: batin

- -> pujian

=== pujian ===
Wah, gila. Keren banget! # speaker: brama
Padahal SMP kamu bukan yang top ya? Gimana caranya bisa segitu? # speaker: brama
Huh? Dia nggak lagi nyindirkan? Ku rasa, dia beneran kagum. # speaker: Surya # mode: batin
Tapi.. justru itu yang bikin diriku kadang merasa jijk dan susah. # speaker: Surya # mode: batin # tensi: 25 # mh: -10

+ [Terima pujiannya]
    Makasih... # speaker: surya
    Bohong. Aku nggak pernah ngerasa pinter. Nggak sekali pun. # speaker: Surya # mode: batin # mh: -10

+ [Kecilin diri]
    Aku cuma beruntung kok. # speaker: surya
    Kalimat itu keluar sebelum sempat kupikir. Kayak alarm. # speaker: Surya # mode: batin # energy: -10

- Oh, dia ketawa. Dia percaya omonganku. # speaker: Surya # mode: batin # tensi: 15
-> bisikan

=== bisikan ===
Permisi, aku ke toilet dulu ya. # speaker: surya
Aku merasa di belakang punggungku dan di depan pintu sana ada omongan samar-samar dari anak-anak kelas. # speaker: Surya # mode: batin
Samar-samar, dari gerombolan deket pintu itu, ku dengar lirih... # speaker: Surya # mode: batin
"Sombong banget. Juara 1 di sekolah kayak gitu mah aku juga bisa." # speaker: Surya # mode: batin
Aku ngepalin tangan di dalam saku. # speaker: Surya # mode: batin # mh: -10
Aku nggak sombong. Aku cuma nggak tau caranya ngerespons manusia dengan bener. # speaker: Surya # mode: batin
Dadaku sesak. Udara di sekitarku kerasa nipis. # speaker: Surya # mode: batin # tensi: 40
~ beat = 1
-> DONE

=== basa_basi ===
Eh, udah baikan? # speaker: brama
-> DONE