# Portal-Pracowniczy-Client
## Cel:

Głównym celem działania aplikacji jest informowanie użytkownika, o której pojawił się w pracy i jak długo pracował danego dnia. 
Projekt został stworzony pod wieloplatformowość. Podczas praktyk był rozwijany tylko na system Android. 

## Sposób działania:

Aplikacja wykrywa Wi-Fi zakładu pracy. Użytkownik otrzymuje powiadomienie z zapytaniem, czy potwierdza przyjście do pracy. Najpierw użytkownik logouje się, wpisując login i hasło. Aplikacja zapamiętuje, kiedy użytkownik się zalogował i oblicza czas pracy. Gdy użytkownik utraci łączność z zakładowym wifi aplikacja wysyła powiadomienie z zapytaniem, czy użytkownik zakończył pracę danego dnia. Jeśli użytkownik kliknął w powiadomienie, aplikacja zapamiętuje datę wyjścia z pracy, a następnie wyskakuje kolejne powiadomienie z podsumowaniem dnia pracy i jeżeli były wyjścia podczas pracy to również ich liczbę.

## Narzędzia:

- Xamarin.Native Android
- Xamarin.Android.Support 27.0.2.1
- ReactiveUI 9.18.2 

## Wymagania dla wersji Android:

- Minimalne SDK:23 
- Docelowe SDK:27

## Uwagi

Aby program działał w należyty sposób, konieczne jest posiadanie włączonej funkcji Wi-Fi oraz program należy przetrzymywać "w tle". W przypadku zmiany zakładowego Wi-Fi, należy w pliku WifiScanReceiver.cs zmienić nazwę użytą w kodzie w zmiennej _availableSsids na właściwą.

Projekt był realizowany w parze podczas miesięcznych praktyk w firmie Rekord SI.
