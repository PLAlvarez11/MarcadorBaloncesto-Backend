from ..db import get_conn, rows_to_dicts
from . import TABLES

def equipo_by_id(equipo_id: int):
    sql = f"SELECT Id, Nombre FROM {TABLES['Equipo']} WHERE Id = ?;"
    with get_conn() as conn:
        cur = conn.cursor()
        cur.execute(sql, (equipo_id,))
        rows = rows_to_dicts(cur)
        return rows[0] if rows else None

def list_jugadores_by_equipo(equipo_id: int):
    sql = f"""
    SET NOCOUNT ON;
    SELECT 
        j.Id AS JugadorId,
        (j.Nombre + ' ' + j.Apellido) AS NombreCompleto,
        j.Dorsal,
        j.Posicion,
        DATEDIFF(YEAR, j.FechaNacimiento, GETDATE()) AS Edad,
        j.EstaturaCm,
        j.Nacionalidad
    FROM {TABLES['Jugador']} j
    WHERE j.EquipoId = ?
    ORDER BY j.Dorsal, j.Apellido, j.Nombre;
    """
    with get_conn() as conn:
        cur = conn.cursor()
        cur.execute(sql, (equipo_id,))
        return rows_to_dicts(cur)
