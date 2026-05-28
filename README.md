# Planerprodukt

To projekt oparty na wieloplatformowym systemie zarządzania zadaniami.

## Opis projektu

System umożliwia zarządzanie zadaniami na trzech platformach: webowej, desktopowej i mobilnej. Wszystkie aplikacje komunikują się ze wspólnym backendem ASP.NET Core Web API.

## Platformy
- **Backend:** ASP.NET Core Web API (.NET 10)
- **Web:** Angular
- **Desktop:** WPF (.NET)
- **Mobile:** .NET MAUI

## Funkcje
- Rejestracja i logowanie (JWT)
- CRUD zadań (tworzenie, edycja, usuwanie)
- Filtrowanie po statusie, priorytecie
- Statystyki z wykresami
- Powiadomienia o terminach
- Eksport do CSV (desktop)
- Swipe do oznaczania wykonanych (mobile)
- Widok "Zadania na dziś" (mobile)
- Testy jednostkowe (xUnit)

## Struktura projektu
Planerprodukt/
├── backend/ # ASP.NET Core Web API
│ └── PlanerproduktAPI/
├── web/ # Aplikacja Angular
│ └── task-planner/
├── desktop/ # Aplikacja WPF
│ └── TaskPlannerDesktop/
├── mobile/ # Aplikacja .NET MAUI
│ └── TaskPlannerMobile/
├── docs/ # Dokumentacja
└── .github/workflows/ # CI/CD

## Uruchomienie

### Backend

bash
cd backend/PlanerproduktAPI
dotnet run
Swagger dostępny pod adresem: http://localhost:5056/swagger

## Aplikacja Webowa 
cd web/task-planner
npm install
ng serve
Strona dostępna pod adresem: http://localhost:4200

## Aplikacja desktopowa
- Otwórz desktop/TaskPlannerDesktop.sln w Visual Studio
- Uruchom projekt (F5)
- Zaloguj się  (np: test / Test123!)

## Aplikacja mobilna

- Otwórz mobile/TaskPlannerMobile.sln w Visual Studio
- Wybierz Windows Machine jako cel uruchomienia
- Uruchom projekt (F5)
- Zarejestruj się lub zaloguj

## Testy
Testy jednostkowe znajdują się w projekcie PlanerproduktAPI.Tests.
cd backend
dotnet test

## CI/CD
Projekt posiada konfigurację GitHub Actions (.github/workflows/main.yml), która automatycznie buduje i testuje backend przy każdym pushu.

Autor
Radosław Gapiński 

Wersja
v1.0 – 28 maja 2026
