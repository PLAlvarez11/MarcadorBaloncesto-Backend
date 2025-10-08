from ..db import get_conn, rows_to_dicts
from . import TABLES

def get_system_logo_bytes():
    return None

def list_equipos_with_logo():
    sql = f"""
    SET NOCOUNT ON;
    SELECT e.Id AS EquipoId,
           e.Nombre,
           e.Ciudad,
           l.Data AS LogoBytes
    FROM {TABLES['Equipo']} e
    LEFT JOIN {TABLES['Logo']} l ON l.EquipoId = e.Id
    ORDER BY e.Nombre;
    """
    with get_conn() as conn:
        cur = conn.cursor()
        cur.execute(sql)
        return rows_to_dicts(cur)
