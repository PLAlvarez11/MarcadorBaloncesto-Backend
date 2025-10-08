from fastapi import FastAPI, Depends, Query, HTTPException
from fastapi.responses import StreamingResponse
from .security import require_admin
from .pdf.builder import (
    equipos_pdf, jugadores_por_equipo_pdf, historial_partidos_pdf,
    roster_por_partido_pdf, estadisticas_jugador_pdf
)
from .queries.equipos import list_equipos_with_logo, get_system_logo_bytes
from .queries.jugadores import list_jugadores_by_equipo, equipo_by_id
from .queries.partidos import list_partidos_historial, roster_por_partido, partido_header
from .queries.estadisticas import jugador_header, estadisticas_by_jugador
from io import BytesIO

app = FastAPI(title="Marcador Reports", version="1.0.0")

@app.get("/reports/equipos.pdf", response_class=StreamingResponse)
def report_equipos(_: dict = Depends(require_admin)):
    rows = list_equipos_with_logo()
    system_logo = get_system_logo_bytes()
    pdf_bytes = equipos_pdf(rows, system_logo)
    return StreamingResponse(BytesIO(pdf_bytes), media_type="application/pdf",
                             headers={"Content-Disposition": "attachment; filename=equipos.pdf"})

@app.get("/reports/equipos/{equipo_id}/jugadores.pdf", response_class=StreamingResponse)
def report_jugadores_equipo(equipo_id: int, _: dict = Depends(require_admin)):
    equipo = equipo_by_id(equipo_id)
    if not equipo:
        raise HTTPException(status_code=404, detail="Equipo no encontrado")
    rows = list_jugadores_by_equipo(equipo_id)
    system_logo = get_system_logo_bytes()
    pdf_bytes = jugadores_por_equipo_pdf(equipo, rows, system_logo)
    fn = f"jugadores_equipo_{equipo_id}.pdf"
    return StreamingResponse(BytesIO(pdf_bytes), media_type="application/pdf",
                             headers={"Content-Disposition": f"attachment; filename={fn}"})


@app.get("/reports/partidos/historial.pdf", response_class=StreamingResponse)
def report_historial_partidos(desde: str | None = Query(None, description="YYYY-MM-DD"),
                              hasta: str | None = Query(None, description="YYYY-MM-DD"),
                              _: dict = Depends(require_admin)):
    rows = list_partidos_historial(desde, hasta)
    system_logo = get_system_logo_bytes()
    rango = ""
    if desde or hasta:
        rango = f"({desde or '...'} a {hasta or '...'})"
    pdf_bytes = historial_partidos_pdf(rows, system_logo, rango)
    return StreamingResponse(BytesIO(pdf_bytes), media_type="application/pdf",
                             headers={"Content-Disposition": "attachment; filename=historial_partidos.pdf"})

@app.get("/reports/partidos/{partido_id}/roster.pdf", response_class=StreamingResponse)
def report_roster_partido(partido_id: int, _: dict = Depends(require_admin)):
    header = partido_header(partido_id)
    if not header:
        raise HTTPException(status_code=404, detail="Partido no encontrado")
    local, visita = roster_por_partido(partido_id)
    system_logo = get_system_logo_bytes()
    pdf_bytes = roster_por_partido_pdf(header, local, visita, system_logo)
    fn = f"roster_partido_{partido_id}.pdf"
    return StreamingResponse(BytesIO(pdf_bytes), media_type="application/pdf",
                             headers={"Content-Disposition": f"attachment; filename={fn}"})

@app.get("/reports/jugadores/{jugador_id}/estadisticas.pdf", response_class=StreamingResponse)
def report_estadisticas_jugador(jugador_id: int,
                                desde: str | None = Query(None, description="YYYY-MM-DD"),
                                hasta: str | None = Query(None, description="YYYY-MM-DD"),
                                _: dict = Depends(require_admin)):
    jugador = jugador_header(jugador_id)
    if not jugador:
        raise HTTPException(status_code=404, detail="Jugador no encontrado")
    rows, total = estadisticas_by_jugador(jugador_id, desde, hasta)
    system_logo = get_system_logo_bytes()
    rango = ""
    if desde or hasta:
        rango = f"({desde or '...'} a {hasta or '...'})"
    pdf_bytes = estadisticas_jugador_pdf(jugador, rows, total, system_logo, rango)
    fn = f"estadisticas_jugador_{jugador_id}.pdf"
    return StreamingResponse(BytesIO(pdf_bytes), media_type="application/pdf",
                             headers={"Content-Disposition": f"attachment; filename={fn}"})
