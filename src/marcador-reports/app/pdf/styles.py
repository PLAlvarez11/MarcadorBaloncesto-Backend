from reportlab.lib.pagesizes import A4
from reportlab.lib.units import cm
from reportlab.lib import colors
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.platypus import Paragraph, Spacer, Image, Table, TableStyle
from datetime import datetime
from io import BytesIO

styles = getSampleStyleSheet()
styles.add(ParagraphStyle(name="Subtle", fontSize=9, leading=11, textColor=colors.grey))

def header(flow, title: str, system_logo: bytes | None):
    if system_logo:
        try:
            flow.append(Image(BytesIO(system_logo), width=2.0*cm, height=2.0*cm))
        except Exception:
            pass
    flow.append(Paragraph(title, styles["Title"]))
    flow.append(Paragraph(datetime.now().strftime("%Y-%m-%d %H:%M"), styles["Subtle"]))
    flow.append(Spacer(1, 0.5*cm))

def build_table(data, col_widths=None):
    t = Table(data, repeatRows=1, colWidths=col_widths)
    t.setStyle(TableStyle([
        ('BACKGROUND', (0,0), (-1,0), colors.HexColor("#E5E7EB")),
        ('TEXTCOLOR', (0,0), (-1,0), colors.HexColor("#111827")),
        ('GRID', (0,0), (-1,-1), 0.25, colors.grey),
        ('VALIGN', (0,0), (-1,-1), 'MIDDLE'),
    ]))
    return t
