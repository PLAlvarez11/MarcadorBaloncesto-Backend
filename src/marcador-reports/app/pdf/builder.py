from io import BytesIO
from reportlab.platypus import SimpleDocTemplate, Paragraph, Spacer, Image
from reportlab.lib.pagesizes import A4
from reportlab.lib.units import cm
from .styles import styles, header, build_table
from typing import List, Dict, Optional

def _build_pdf(story_builder) -> bytes:
    buf = BytesIO()
    doc = SimpleDocTemplate(buf, pagesize=A4, leftMargin=1.8*cm, rightMargin=1.2*cm, topMargin=1.2*cm, bottomMargin=1.2*cm)
    story = []
    story_builder(story)
    doc.build(story)
    buf.seek(0)
    return buf.getvalue()

def equipos_pdf(rows: List[Dict], system_logo: Optional[bytes]) -> bytes:
    def _b(flow):
        header(flow, "Equipos Registrados", system_logo)
        data = [["Nombre", "Ciudad", "Logo"]]
        for r in rows:
            logo_flow = ""
            if r.get("LogoBytes"):
                try:
                    logo_flow = Image(BytesIO(r["LogoBytes"]), width=1.3*cm, height=1.3*cm)
                except Exception:
                    logo_flow = ""
            data.append([r.get("Nombre",""), r.get("Ciudad",""), logo_flow])
        flow.append(build_table(data, col_widths=[8*cm, 6*cm, 3*cm]))
    return _build_pdf(_b)

def jugadores_por_equipo_pdf(equipo: Dict, rows: List[Dict], system_logo: Optional[bytes]) -> bytes:
    def _b(flow):
        titulo = f"Jugadores del Equipo: {equipo.get('Nombre','(sin nombre)')}"
        header(flow, titulo, system_logo)
        data = [["Nombre completo", "Dorsal", "Posición", "Edad", "Estatura (cm)", "Nacionalidad"]]
        for r in rows:
            data.append([
                r.get("NombreCompleto",""),
                r.get("Dorsal",""),
                r.get("Posicion",""),
                r.get("Edad",""),
                r.get("EstaturaCm",""),
                r.get("Nacionalidad",""),
            ])
        flow.append(build_table(data, col_widths=[7*cm, 2*cm, 3*cm, 2*cm, 3*cm, 3*cm]))
    return _build_pdf(_b)

def historial_partidos_pdf(rows: List[Dict], system_logo: Optional[bytes], rango: str) -> bytes:
    def _b(flow):
        header(flow, f"Historial de Partidos {rango}", system_logo)
        data = [["Fecha/Hora", "Local", "Visitante", "Marcador"]]
        for r in rows:
            marcador = f"{r.get('MarcadorLocal',0)} - {r.get('MarcadorVisitante',0)}"
            data.append([r.get("FechaHora",""), r.get("Local",""), r.get("Visitante",""), marcador])
        flow.append(build_table(data, col_widths=[4*cm, 6*cm, 6*cm, 3*cm]))
    return _build_pdf(_b)

def roster_por_partido_pdf(partido: Dict, roster_local: List[Dict], roster_visita: List[Dict], system_logo: Optional[bytes]) -> bytes:
    def _b(flow):
        title = f"Roster por Partido - {partido.get('Local','')} vs {partido.get('Visitante','')}"
        header(flow, title, system_logo)
        flow.append(Paragraph("Equipo Local", styles['Heading2']))
        dataL = [["Dorsal", "Nombre"]]
        for r in roster_local:
            dataL.append([r.get("Dorsal",""), r.get("NombreCompleto","")])
        flow.append(build_table(dataL, col_widths=[2*cm, 12*cm]))
        flow.append(Spacer(1, 0.5*cm))

        flow.append(Paragraph("Equipo Visitante", styles['Heading2']))
        dataV = [["Dorsal", "Nombre"]]
        for r in roster_visita:
            dataV.append([r.get("Dorsal",""), r.get("NombreCompleto","")])
        flow.append(build_table(dataV, col_widths=[2*cm, 12*cm]))
    return _build_pdf(_b)

def estadisticas_jugador_pdf(jugador: Dict, rows: List[Dict], total: Dict, system_logo: Optional[bytes], rango: str) -> bytes:
    def _b(flow):
        header(flow, f"Estadísticas de {jugador.get('NombreCompleto','')} {rango}", system_logo)
        data = [["Fecha", "Rival", "Puntos", "Asist.", "Reb.Of", "Reb.Def", "Robos", "Tapones", "Faltas"]]
        for r in rows:
            data.append([
                r.get("Fecha",""),
                r.get("Rival",""),
                r.get("Puntos",0),
                r.get("Asistencias",0),
                r.get("RebotesOf",0),
                r.get("RebotesDef",0),
                r.get("Robos",0),
                r.get("Tapones",0),
                r.get("Faltas",0),
            ])
        flow.append(build_table(data, col_widths=[2.5*cm, 5*cm, 2*cm, 2*cm, 2*cm, 2*cm, 2*cm, 2*cm, 2*cm]))
        flow.append(Spacer(1, 0.5*cm))
        dataT = [["TOTAL", total.get("Puntos",0), total.get("Asistencias",0), total.get("RebotesOf",0),
                  total.get("RebotesDef",0), total.get("Robos",0), total.get("Tapones",0), total.get("Faltas",0)]]
        flow.append(build_table(dataT, col_widths=[7.5*cm, 2*cm, 2*cm, 2*cm, 2*cm, 2*cm, 2*cm, 2*cm]))
    return _build_pdf(_b)
