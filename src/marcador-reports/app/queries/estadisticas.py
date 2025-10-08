from ..db import get_conn, rows_to_dicts
from . import TABLES

def jugador_header(jugador_id: int):
    sql = f"""
    SELECT j.Id,
           (j.Nombre + ' ' + j.Apellido) AS NombreCompleto,
           e.Nombre AS Equipo
    FROM {TABLES['Jugador']} j
    INNER JOIN {TABLES['Equipo']} e ON e.Id = j.EquipoId
    WHERE j.Id = ?;
    """
    with get_conn() as conn:
        cur = conn.cursor()
        cur.execute(sql, (jugador_id,))
        rows = rows_to_dicts(cur)
        return rows[0] if rows else None

def estadisticas_by_jugador(jugador_id: int, desde: str | None, hasta: str | None):
    where = "WHERE ejp.JugadorId = ?"
    params = [jugador_id]
    if desde:
        where += " AND p.FechaPartido >= ?"
        params.append(desde)
    if hasta:
        where += " AND p.FechaPartido < DATEADD(DAY, 1, ?)"
        params.append(hasta)

    sql_rows = f"""
    SET NOCOUNT ON;
    SELECT 
        CONVERT(varchar(10), p.FechaPartido, 120) AS Fecha,
        CASE WHEN p.EquipoLocalId = j.EquipoId THEN ev.Nombre ELSE el.Nombre END AS Rival,
        ejp.Puntos, ejp.Asistencias, ejp.RebotesOf, ejp.RebotesDef, ejp.Robos, ejp.Tapones, ejp.Faltas
    FROM EstadisticaJugadorPartido ejp
    INNER JOIN {TABLES['Partido']} p ON p.Id = ejp.PartidoId
    INNER JOIN {TABLES['Jugador']} j ON j.Id = ejp.JugadorId
    INNER JOIN {TABLES['Equipo']} el ON el.Id = p.EquipoLocalId
    INNER JOIN {TABLES['Equipo']} ev ON ev.Id = p.EquipoVisitanteId
    {where}
    ORDER BY p.FechaPartido DESC;
    """
    with get_conn() as conn:
        cur = conn.cursor()
        cur.execute(sql_rows, params)
        rows = rows_to_dicts(cur)

        sql_total = f"""
        SELECT 
            SUM(ejp.Puntos) AS Puntos,
            SUM(ejp.Asistencias) AS Asistencias,
            SUM(ejp.RebotesOf) AS RebotesOf,
            SUM(ejp.RebotesDef) AS RebotesDef,
            SUM(ejp.Robos) AS Robos,
            SUM(ejp.Tapones) AS Tapones,
            SUM(ejp.Faltas) AS Faltas
        FROM EstadisticaJugadorPartido ejp
        INNER JOIN {TABLES['Partido']} p ON p.Id = ejp.PartidoId
        {where};
        """
        cur = conn.cursor()
        cur.execute(sql_total, params)
        tot = rows_to_dicts(cur)
        total = tot[0] if tot else {"Puntos":0,"Asistencias":0,"RebotesOf":0,"RebotesDef":0,"Robos":0,"Tapones":0,"Faltas":0}
        return rows, total
