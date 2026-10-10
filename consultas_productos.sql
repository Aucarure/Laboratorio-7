CREATE OR ALTER PROCEDURE sp_Productos_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT idproducto, nombreProducto, cantidadPorUnidad, precioUnidad,
           unidadesEnExistencia, categoriaProducto, suspendido
    FROM dbo.productos
    WHERE suspendido = 0 OR suspendido IS NULL
    ORDER BY nombreProducto;
END
GO

CREATE OR ALTER PROCEDURE sp_Productos_Buscar
    @texto VARCHAR(40)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT idproducto, nombreProducto, cantidadPorUnidad, precioUnidad,
           unidadesEnExistencia, categoriaProducto, suspendido
    FROM dbo.productos
    WHERE (suspendido = 0 OR suspendido IS NULL)
      AND (nombreProducto LIKE '%' + @texto + '%'
           OR CAST(idproducto AS VARCHAR(10)) = @texto)
    ORDER BY nombreProducto;
END
GO

CREATE OR ALTER PROCEDURE sp_Productos_ObtenerPorId
    @idproducto INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT idproducto, nombreProducto, cantidadPorUnidad, precioUnidad,
           unidadesEnExistencia, categoriaProducto, suspendido
    FROM dbo.productos
    WHERE idproducto = @idproducto AND (suspendido = 0 OR suspendido IS NULL);
END
GO

CREATE OR ALTER PROCEDURE sp_Productos_Insertar
    @nombreProducto       VARCHAR(40),
    @cantidadPorUnidad    VARCHAR(20),
    @precioUnidad         DECIMAL(18,0),
    @unidadesEnExistencia SMALLINT,
    @categoriaProducto    VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @nuevoId INT;

    BEGIN TRANSACTION;

    SELECT @nuevoId = ISNULL(MAX(idproducto), 0) + 1
    FROM dbo.productos WITH (UPDLOCK, HOLDLOCK);

    INSERT INTO dbo.productos
        (idproducto, nombreProducto, cantidadPorUnidad, precioUnidad,
         unidadesEnExistencia, categoriaProducto, suspendido)
    VALUES
        (@nuevoId, @nombreProducto, @cantidadPorUnidad, @precioUnidad,
         @unidadesEnExistencia, @categoriaProducto, 0);

    COMMIT TRANSACTION;

    SELECT @nuevoId;
END
GO

CREATE OR ALTER PROCEDURE sp_Productos_Actualizar
    @idproducto           INT,
    @nombreProducto       VARCHAR(40),
    @cantidadPorUnidad    VARCHAR(20),
    @precioUnidad         DECIMAL(18,0),
    @unidadesEnExistencia SMALLINT,
    @categoriaProducto    VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.productos
    SET nombreProducto = @nombreProducto,
        cantidadPorUnidad = @cantidadPorUnidad,
        precioUnidad = @precioUnidad,
        unidadesEnExistencia = @unidadesEnExistencia,
        categoriaProducto = @categoriaProducto
    WHERE idproducto = @idproducto AND (suspendido = 0 OR suspendido IS NULL);
END
GO

CREATE OR ALTER PROCEDURE sp_Productos_Eliminar
    @idproducto INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.productos SET suspendido = 1 WHERE idproducto = @idproducto;
END
GO

SELECT TOP 1 *
FROM dbo.productos
ORDER BY idproducto DESC;

SELECT idproducto, nombreProducto, suspendido
FROM dbo.productos
WHERE suspendido = 1;
