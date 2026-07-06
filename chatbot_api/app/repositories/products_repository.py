from app.database.connection import get_connection


def buscar_producto(texto_busqueda):
    conn = get_connection()
    cursor = conn.cursor()

    cursor.execute(
        "EXEC dbo.SP_ListarGeneralProducts_Filtro @01_FilterText = ?",
        texto_busqueda,
    )

    columns = [column[0] for column in cursor.description] if cursor.description else []
    productos = []

    for row in cursor.fetchall():
        productos.append({column: value for column, value in zip(columns, row)})

    conn.close()
    return productos
