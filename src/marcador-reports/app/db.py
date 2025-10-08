import os
import pyodbc
from contextlib import contextmanager

def _conn_str() -> str:
    host = os.getenv("SQLSERVER__HOST", "localhost")
    port = os.getenv("SQLSERVER__PORT", "1433")
    db = os.getenv("SQLSERVER__DB", "MarcadorDb")
    user = os.getenv("SQLSERVER__USER", "sa")
    pwd = os.getenv("SQLSERVER__PASSWORD", "")
    return f"DRIVER={{ODBC Driver 18 for SQL Server}};SERVER={host},{port};DATABASE={db};UID={user};PWD={pwd};Encrypt=No;TrustServerCertificate=Yes"

@contextmanager
def get_conn():
    conn = pyodbc.connect(_conn_str(), timeout=5)
    try:
        yield conn
    finally:
        conn.close()

def rows_to_dicts(cursor):
    cols = [c[0] for c in cursor.description]
    return [dict(zip(cols, row)) for row in cursor.fetchall()]
