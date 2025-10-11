# Script PowerShell para corregir la codificacion de materiales

$serverName = "localhost"
$databaseName = "NegocioBiblioteca"

$connectionString = "Server=$serverName;Database=$databaseName;Integrated Security=True;TrustServerCertificate=True"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)

try {
    $connection.Open()
    Write-Host "Conexion establecida" -ForegroundColor Green

    $updates = @(
        @{Where="Titulo LIKE '%Cien A%'"; Titulo="Cien Años de Soledad"; Autor="Gabriel García Márquez"; Editorial="Editorial Sudamericana"},
        @{Where="Titulo LIKE '%Cr%nica%'"; Titulo="Crónica de una Muerte Anunciada"; Autor="Gabriel García Márquez"; Editorial="Editorial Sudamericana"},
        @{Where="Titulo LIKE '%Dr%cula%'"; Titulo="Drácula"; Autor="Bram Stoker"; Editorial="Editorial Penguin"},
        @{Where="Titulo LIKE '%C%digo%'"; Titulo="El Código Da Vinci"; Autor="Dan Brown"; Editorial="Editorial Planeta"},
        @{Where="Autor LIKE '%Exup%'"; Titulo="El Principito"; Autor="Antoine de Saint-Exupéry"; Editorial="Editorial Salamandra"},
        @{Where="Titulo LIKE '%Romeo%'"; Titulo="Romeo y Julieta"; Autor="William Shakespeare"; Editorial="Editorial Cátedra"},
        @{Where="Titulo LIKE '%Hamlet%'"; Titulo="Hamlet"; Autor="William Shakespeare"; Editorial="Editorial Cátedra"},
        @{Where="Titulo LIKE '%Los Miserables%'"; Titulo="Los Miserables"; Autor="Victor Hugo"; Editorial="Editorial Porrúa"},
        @{Where="Titulo LIKE '%Gram%tica%'"; Titulo="Manual de Gramática Española"; Autor="Real Academia Española"; Editorial="Espasa"},
        @{Where="Titulo LIKE '%Matem%ticas%'"; Titulo="Manual de Matemáticas Secundaria"; Autor="Varios Autores"; Editorial="Editorial Santillana"}
    )

    foreach ($update in $updates) {
        $sql = "UPDATE Material SET Titulo = @Titulo, Autor = @Autor, Editorial = @Editorial WHERE " + $update.Where
        $command = New-Object System.Data.SqlClient.SqlCommand($sql, $connection)
        $command.Parameters.AddWithValue("@Titulo", $update.Titulo) | Out-Null
        $command.Parameters.AddWithValue("@Autor", $update.Autor) | Out-Null
        $command.Parameters.AddWithValue("@Editorial", $update.Editorial) | Out-Null
        $rowsAffected = $command.ExecuteNonQuery()
        Write-Host "Actualizado:" $update.Titulo "- filas:" $rowsAffected
    }

    $sql = "SELECT Titulo, Autor, Editorial FROM Material ORDER BY Titulo"
    $command = New-Object System.Data.SqlClient.SqlCommand($sql, $connection)
    $reader = $command.ExecuteReader()

    Write-Host ""
    Write-Host "MATERIALES:" -ForegroundColor Green

    while ($reader.Read()) {
        $titulo = $reader["Titulo"]
        $autor = $reader["Autor"]
        $editorial = $reader["Editorial"]
        Write-Host $titulo "-" $autor "-" $editorial
    }

    $reader.Close()
    Write-Host ""
    Write-Host "Completado" -ForegroundColor Green

} catch {
    Write-Host "Error:" $_ -ForegroundColor Red
} finally {
    if ($connection.State -eq 'Open') {
        $connection.Close()
    }
}
