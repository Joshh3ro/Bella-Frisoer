# Bella-Frisoer
Second Semester Exam, 2025

# Migration and Update Database
dotnet ef migrations add InitialCreate --project "BellaFrisoer.Infrastructure" --startup-project "BellaFrisoer.WebUi"

dotnet ef database update --project BellaFrisoer.Infrastructure --startup-project BellaFrisoer.WebUi