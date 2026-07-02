import pyodbc

from app.core.config import get_database_settings


def get_connection():

    settings = get_database_settings()

    connection_parts = [
        f"DRIVER={{{settings['driver']}}}",
        f"SERVER={settings['server']}",
        f"DATABASE={settings['database']}",
        "TrustServerCertificate=YES",
        f"Connection Timeout={settings['timeout']}",
    ]

    if settings["trusted_connection"]:
        connection_parts.append("Trusted_Connection=yes")
    else:
        if not settings["username"] or not settings["password"]:
            raise EnvironmentError(
                "DB_USER and DB_PASSWORD are required when DB_TRUSTED_CONNECTION is false."
            )
        connection_parts.append(f"UID={settings['username']}")
        connection_parts.append(f"PWD={settings['password']}")

    connection_string = ";".join(connection_parts)

    return pyodbc.connect(connection_string)