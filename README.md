# Parserv2

1. [Opis](#Opis)
2. [Formaty plików tekstowych](#Formaty-plików-tekstowych)
   - [Nowszy format](#Nowszy-format)
   - [Stary format](#Stary-format)
3. [Formaty plików json](#Formaty-plików-json)
   - [Pierwotny](#Pierwotny)
   - [Proponowany](#Proponowany)

# Opis
Program służy do parsowania plików txt (stary format) oraz pwzim (nowszy format) oraz aktualizacji planu na serwerze.

Funkcje programu to:
 - generowanie pliku .json z plików txt oraz pwzim 
   - pliki te można ładować jednoczeście a wynikiem bedzie json zawierający zajęcia z obu typów plików
 - można też załadować do programu json'a bezpośrednio
 - wgrany/wygenerowany (wynikowy) plik json można wysłać na serwer za pomocą przycisku UPLOAD
 - pozwala na nadpisanie daty w wynikowym pliku na date teraźniejszą
 - pobranie aktualnego pliku json z serwera (w celu backup'u)

--------------------------------------------------------------------------------------------------------------------------
# Formaty plików tekstowych
## Nowszy format
Rozszerzenie .pwzim
Jest format bardziej kompaktowy od starszego. Każdy plik reprezentuje jeden rocznik studentów na danym kierunku.
Plik jest podzielony na sekcje, które zaczynają się linią:
 - `GROUPS` - zakończona `end_groups.` - informuje o mapowaniu grup do danych specjalizacji
 - `INFO` - zakończona `end_info.` - linia informacyjna o roczniku
 - oraz dni tyg - każda z nich zakończone `end_day.`:
   - `PN` - poniedziałek
   - `WT` - wtorek
   - `ŚR` - środa
   - `CZW` - czwartek
   - `PT` - piątek
   - `SO` - sobota
   - `NIE` - niedziela

Każda z sekcji jest opcjonalna oprócz sekcji `INFO`

### Sekcja `GROUPS`
W niej tworzymy prostą mapę numerów grup studenckich do ich specjalizacji np.:
```
1; ISI
2; ISK
```
Więc grupa 1 jest na specjalizacji ISI, a grupa druga na specjalizacji ISK. W przypadku dwóch grup o tej samej specjalizacji można dodać nr np.:
```
1; ISI_1
2; ISI_2
3; ISK
```
Sekcje kończymy linią `end_groups.`

### Sekcja `INFO`
linia w niej wygląda następująco:\
`<rok akademicki>; <wydział>; <typ studiów [ST/NSTA/NSTB]>; <kierunek>; <stopień>; <semestr>; <rok>`\
na przykład:\
`2019/2020; WZIM; NSTB; Inf; inż; 7; 4`\
Sekcje kończymy linią `end_info.`

### Sekcje dnia tygodnia
W niej listujemy zajęcia w formacie:
```
<tytuł zajęć>; <typ zajęć>
<numery grup>
<start>-<koniec>
<sala>; <budynek>
<prowadzący>
<uwagi>
end_event.
```
gdzie:\
`typ zajęć` - można wpisać tam wszystko jednak są pewnie skróty jak:\
 - ćw - ćwiczenia
 - F - fakultet
 - lab - laboratoria
 - w - wykład
 
`numery grup` - numery grup studenckich, dla których sa przeznaczone zajęcia oddzielone `;`\
`<start>-<koniec>` - okres godzinowy trwania zajęć w formacie HH:mm\
`<budynek>` - numer budynku (ten parametr można pominąć - jego domyślna wartość to 34)

przykład:
```
Wstęp do programowania; w
1;2;3;4;5
16:00-17:45
Aula IV
Waldemar Karwowski
6 zjazdów normalnie, 2 zjazdy 15 minut krócej
end_event.
```

Plik należy zakończyć linią `end.`

--------------------------------------------------------------------------------------------------------------------------

## Stary format
Rozszerzenie .txt
Każdy plik reprezentuje jedną grupe studencką i jej zajęcia.

Nagłówek pliku (linia 0 oraz 1)
```
<data DD.MM.RRRR> <godzina HH:mm>
<wydział>, <rok>, <sem jesienny/wiosenny [J/W]>, <typ studiów [ST/NSTA/NSTB]>, <kierunek>, <stopień>, R<rok stud>, S<semestr>, gr<grupa>, <specjalizacja>;
```
gdzie <specjalizacja> jest parametrem opcjonalnym (jeśli podany zastępuje nr grupy)

przykład:
```
18.10.2019 17:34
WZIM, 2019, J, ST, IiE, lic, R2, S3, gr3, ISI-1;
```

Następnie jest linia oddzielająca\
`------------------------------------------------`\
Nastepnie każde z zajęć jest reprezentowane następująco:
```
ZJ_<numer zajęć>
<Tytuł zajęć> [<typ zajęć>]
d<numer dnia tyg>, <start>-<koniec>
<sala>, <budynek>
<prowadzący>
U: <uwagi>
------------------------------------------------
```
gdzie:\
`numer zajęć` - dwa znaki przeznaczone na inkrementowany index zajęc (poczynająć od 01)\
`typ zajęć` - można wpisać tam wszystko jednak są pewnie skróty jak:\
 - ćw - ćwiczenia
 - F - fakultet
 - lab - laboratoria
 - w - wykład
 
`numer dnia tyg`:
 - 1 - poniedziałek
 - 2 - wtorek
 - itd...
 
`<start>-<koniec>` - okres godzinowy trwania zajęć w formacie HH:mm\
`<budynek>` - numer budynku (ten parametr można pominąć - jego domyślna wartość to 34)

przykład:
```
ZJ_03
Podstawy ekonometrii [Lab]
d1, 15:00-16:30
2066, 23
Jolanta Kotlarska
U: przykładowe uwagi
------------------------------------------------
```

Plik należy zakończyć linią `.end`

# Formaty plików .json
## Pierwotny
Jest to format używany do teraz.
Każde z wydarzeń z wydarzeń jest reprezentowane przez obiekt, a zbiór tych wydarzeń w tablicy `events`.
```js
{
 "department": string,
 "fieldOfStudy": string,
 "mode": string,
 "year": string,
 "semester": string,
 "group": string,
 "isFaculty": string,
 "facultyGroup": string,
 "specialization": string,
 "degree": string,
 "name": string,
 "dayOfWeek": string,
 "startTime": string,
 "endTime": string,
 "building": string,
 "room": string,
 "lecturers": stringArray,
 "type": string,
 "remarks": string,
 "academicYear": string
}
```
Przykład:
```js
{
 "date":"2020-02-20T22:22:00",
 "events":[
  {
   "department":"WZIM",
   "fieldOfStudy":"BDA",
   "mode":"Stacjonarne",
   "year":"1",
   "semester":"2",
   "group":"1",
   "isFaculty":false,
   "facultyGroup":"",
   "specialization":"",
   "degree":"Mgr",
   "name":"Oracle Databases",
   "dayOfWeek":"CZW",
   "startTime":"11:45",
   "endTime":"12:30",
   "building":"34",
   "room":"3/11",
   "lecturers":["Krzysztof Karpio"],
   "type":"wykład",
   "remarks":"",
   "academicYear":"2019/2020"
  },
  //...
 ]
}
```
## Proponowany
Hierarchiczny obiekt
Wraz ze wzrostem wydarzeń wielkość pliku rośnie logarytmicznie (chyba), a nie liniowo
przykład:
```js
{
  "Date": "2019-10-17T22:12:00",
  "Departments": [
    {
      "Department": "WZIM",
      "Modes": [
        {
          "Mode": "Stacjonarne",
          "Fields": [
            {
              "FieldOfStudy": "BDA",
              "Semesters": [
                {
                  "Semester": 1,
                  "Year": 1,
                  "Degree": "mgr",
                  "Days": [
                    {
                      "DayOfWeek": 1,
                      "Events": [
                        {
                          "Name": "Networks Services",
                          "Type": "wykład",
                          "IsFaculty": false,
                          "Groups": [{ "Group": 1, "Specialization": "" }],
                          "FacultyGroups": [""],
                          "StartTime": "2019-10-19T09:30:00",
                          "EndTime": "2019-10-19T10:15:00",
                          "Building": "34",
                          "Room": "3/84",
                          "Lecturers": ["Jarosław Kurek"],
                          "Remarks": ""
                        },
                        {
                          "Name": "Networks Services",
                          "Type": "laboratorium",
                          "IsFaculty": false,
                          "Groups": [{ "Group": 1, "Specialization": "" }],
                          "FacultyGroups": [""],
                          "StartTime": "2019-10-19T10:30:00",
                          "EndTime": "2019-10-19T12:00:00",
                          "Building": "34",
                          "Room": "3/84",
                          "Lecturers": ["Jarosław Kurek"],
                          "Remarks": ""
                        }
                      ]
                    },
                    // itd
```
