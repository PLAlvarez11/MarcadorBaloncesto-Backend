# Marcador Reports (FastAPI + ReportLab)

## Local (opcional)
python -m venv .venv
# Windows: .venv\Scripts\activate
# Linux/Mac:
# source .venv/bin/activate
pip install -r requirements.txt

# Variables
set SQLSERVER__HOST=localhost
set SQLSERVER__PORT=1433
set SQLSERVER__DB=MarcadorDb
set SQLSERVER__USER=sa
set SQLSERVER__PASSWORD=Baloncesto11!
set JWT__ISSUER=MarcadorApi
set JWT__AUDIENCE=MarcadorFrontend
set JWT__KEY=super_secret_dev_key_change_me

uvicorn app.main:app --host 0.0.0.0 --port 8081

## Docker
docker build -t marcador-reports:1.0 .
docker run --rm -p 8081:8081 ^
  -e SQLSERVER__HOST=host.docker.internal ^
  -e SQLSERVER__PORT=1433 ^
  -e SQLSERVER__DB=MarcadorDb ^
  -e SQLSERVER__USER=sa ^
  -e SQLSERVER__PASSWORD=Baloncesto11! ^
  -e JWT__ISSUER=MarcadorApi ^
  -e JWT__AUDIENCE=MarcadorFrontend ^
  -e JWT__KEY=super_secret_dev_key_change_me ^
  marcador-reports:1.0

## Endpoints (JWT Admin requerido)
GET /reports/equipos.pdf
GET /reports/equipos/{equipo_id}/jugadores.pdf
GET /reports/partidos/historial.pdf?desde=YYYY-MM-DD&hasta=YYYY-MM-DD
GET /reports/partidos/{partido_id}/roster.pdf
GET /reports/jugadores/{jugador_id}/estadisticas.pdf?desde=YYYY-MM-DD&hasta=YYYY-MM-DD

## Ajuste de nombres de tablas
Edita app/queries/__init__.py
