from ..db import get_conn, rows_to_dicts
from . import TABLES

def list_partidos_historial(desde: str | None, hasta: str | None):
    where = "WHERE 1=1"
    params = []
    if desde:
        where += " AND p.FechaPartido >= ?"
        params.append(desde)
    if hasta:
        where += " AND p.FechaPartido < DATEADD(DAY, 1, ?)"
        params.append(hasta)

    sql = f"""
    SET NOCOUNT ON;
    SELECT 
        CONVERT(varchar(16), p.FechaPartido, 120) AS FechaHora,
        el.Nombre AS Local,
        ev.Nombre AS Visitante,
        p.MarcadorLocal,
        p.MarcadorVisitante,
        p.Id AS PartidoId
    FROM {TABLES['Partido']} p
    INNER JOIN {TABLES['Equipo']} el ON el.Id = p.EquipoLocalId
    INNER JOIN {TABLES['Equipo']} ev ON ev.Id = p.EquipoVisitanteId
    {where}
    ORDER BY p.FechaPartido DESC;
    """
    with get_conn() as conn:
        cur = conn.cursor()
        cur.execute(sql, params)
        return rows_to_dicts(cur)

def partido_header(partido_id: int):
    sql = f"""
    SELECT p.Id,
           el.Nombre AS Local,
           ev.Nombre AS Visitante,
           CONVERT(varchar(16), p.FechaPartido, 120) AS FechaHora
    FROM {TABLES['Partido']} p
    INNER JOIN {TABLES['Equipo']} el ON el.Id = p.EquipoLocalId
    INNER JOIN {TABLES['Equipo']} ev ON ev.Id = p.EquipoVisitanteId
    WHERE p.Id = ?;
    """
    with get_conn() as conn:
        cur = conn.cursor()
        cur.execute(sql, (partido_id,))
        rows = rows_to_dicts(cur)
        return rows[0] if rows else None

def roster_por_partido(partido_id: int):
    sql_local = f"""
    SELECT j.Dorsal,
           (j.Nombre + ' ' + j.Apellido) AS NombreCompleto
    FROM {TABLES['PartidoJugador']} pj
    INNER JOIN {TABLES['Jugador']} j ON j.Id = pj.JugadorId
    INNER JOIN {TABLES['Partido']} p ON p.Id = pj.PartidoId
    WHERE pj.PartidoId = ? AND j.EquipoId = p.EquipoLocalId
    ORDER BY j.Dorsal, j.Apellido;
    """
    sql_visita = f"""
    SELECT j.Dorsal,
           (j.Nombre + ' ' + j.Apellido) AS NombreCompleto
    FROM {TABLES['PartidoJugador']} pj
    INNER JOIN {TABLES['Jugador']} j ON j.Id = pj.JugadorId
    INNER JOIN {TABLES['Partido']} p ON p.Id = pj.PartidoId
    WHERE pj.PartidoId = ? AND j.EquipoId = p.EquipoVisitanteId
    ORDER BY j.Dorsal, j.Apellido;
    """
    with get_conn() as conn:
        cur = conn.cursor()
        cur.execute(sql_local, (partido_id,))
        local = rows_to_dicts(cur)
        cur.execute(sql_visita, (partido_id,))
        visita = rows_to_dicts(cur)
        return local, visita
