#Cel:

Głównym celem działania aplikacji jest informowanie użytkownika, o której pojawił się w pracy i jak długo pracował danego dnia.

#Sposób działania:

Aplikacja wykrywa wifi zakładu pracy. Użytkownik otrzymuje powiadomienie z zapytaniem, czy chce się zalogować.
Użytkownik potwierdza chęć zalogowania się, wpisuje login i hasło.
Aplikacja zapamiętuje, kiedy użytkownik się zalogował i oblicza czas pracy.
Gdy użytkownik utraci łączność z zakładowym wifi aplikacja wysyła powiadomienie z zapytaniem, czy użytkownik zakończył pracę danego dnia.
Jeśli użytkownik kliknął w powiadomienie, aplikacja zapamiętuje datę wyjścia z pracy.

#Narzędzia:

Xamarin.Native Android, Xamarin.Android.Support 27.0.2.1, framework: ReactiveUI 9.18.2
Minimalne SDK:23
Docelowe SDK:27

#Uwagi

Aby program działał w należyty sposób, konieczne jest posiadanie włączonej funkcji Wi-Fi oraz program należy przetrzymywać "w tle".
W przypadku zmiany zakładowego wifi, należy w pliku WifiScanReceiver.cs zmienić nazwę użytą w kodzie w zmiennej _availableSsids na właściwą.