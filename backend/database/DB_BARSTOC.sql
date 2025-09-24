CREATE DATABASE DB_BARSTOC
COLLATE Latin1_General_100_CI_AI_SC_UTF8;
GO

USE DB_BARSTOC;
GO

--------------------------------------------------------------------------------------------------------
--------------------------- Eliminar tablas en orden correcto (dependencias) ---------------------------
--------------------------------------------------------------------------------------------------------

-- Primero eliminar triggers (si existen)
DROP TRIGGER IF EXISTS TR_After_Insert_DetallePedido;
DROP TRIGGER IF EXISTS TR_After_Update_Pedido_Cancelado;
DROP TRIGGER IF EXISTS TR_After_Update_Pedido_Estado;
DROP TRIGGER IF EXISTS TR_After_Insert_Pedido;
DROP TRIGGER IF EXISTS TR_Before_Insert_Pedido;
GO

-- Luego eliminar procedures
DROP PROCEDURE IF EXISTS SP_ActualizarInventario;
DROP PROCEDURE IF EXISTS SP_ReporteVentasPorFecha;
DROP PROCEDURE IF EXISTS SP_CancelarPedido;
DROP PROCEDURE IF EXISTS SP_CerrarPedido;
DROP PROCEDURE IF EXISTS SP_AgregarProductoPedido;
DROP PROCEDURE IF EXISTS SP_CrearPedido;
DROP PROCEDURE IF EXISTS SP_FormatearPrecio;
GO

-- Luego eliminar views
DROP VIEW IF EXISTS VW_Pedidos_Activos;
DROP VIEW IF EXISTS VW_Inventario_Actual;
DROP VIEW IF EXISTS VW_Reporte_Ventas;
DROP VIEW IF EXISTS VW_Productos_Precios_Formateados;
GO

-- Finalmente eliminar tablas en orden inverso de dependencias
DROP TABLE IF EXISTS TBL_Detalle_Pedidos;
DROP TABLE IF EXISTS TBL_Pedidos;
DROP TABLE IF EXISTS TBL_Movimientos_Inventario;
DROP TABLE IF EXISTS TBL_Sesiones_Usuario;
DROP TABLE IF EXISTS TBL_Inventario;
DROP TABLE IF EXISTS TBL_MenuRol;
DROP TABLE IF EXISTS TBL_Menu;
DROP TABLE IF EXISTS TBL_Usuarios;
DROP TABLE IF EXISTS TBL_Mesas;
DROP TABLE IF EXISTS TBL_Producto;
DROP TABLE IF EXISTS TBL_Categoria;
DROP TABLE IF EXISTS TBL_Roles;
DROP TABLE IF EXISTS TBL_Sedes;
DROP TABLE IF EXISTS TBL_Numero_Pedido;
GO

-------------------------------------------------------------------------
-------------------------- TABLAS PRINCIPALES ---------------------------
-------------------------------------------------------------------------

-- Tabla de Sedes
CREATE TABLE TBL_Sedes (
    idSede INT PRIMARY KEY IDENTITY(1,1),
    nombreSede NVARCHAR(100) NOT NULL,
    direccion NVARCHAR(200),
    telefono VARCHAR(20),
    estado VARCHAR(10) DEFAULT 'activa' CHECK (estado IN ('activa', 'inactiva')),
    fechaCreacion DATETIME2 DEFAULT GETDATE()
);
GO

-- Tabla de Categorías de Productos
CREATE TABLE TBL_Categoria (
    idCategoria INT PRIMARY KEY IDENTITY(1,1),
    nombreCategoria NVARCHAR(100) NOT NULL,
    estado VARCHAR(10) DEFAULT 'activa' CHECK (estado IN ('activa', 'inactiva')),
    fechaCreacion DATETIME2 DEFAULT GETDATE()
);
GO

-- Tabla de Productos
CREATE TABLE TBL_Producto (
    idProducto INT PRIMARY KEY IDENTITY(1,1),
    nombreProducto NVARCHAR(200) NOT NULL,
    idCategoria INT NOT NULL,
    costo DECIMAL(12,2) NOT NULL CHECK (costo >= 0),
    precioVenta DECIMAL(12,2) NOT NULL CHECK (precioVenta >= 0),
    estado VARCHAR(10) DEFAULT 'activo' CHECK (estado IN ('activo', 'inactivo')),
    fechaCreacion DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_Producto_Categoria FOREIGN KEY (idCategoria) REFERENCES TBL_Categoria(idCategoria),
    CONSTRAINT CHK_Precio_Mayor_Costo CHECK (precioVenta >= costo)
);
GO

-- Tabla de Mesas
CREATE TABLE TBL_Mesas (
    idMesa INT PRIMARY KEY IDENTITY(1,1),
    idSede INT NOT NULL,
    numeroMesa VARCHAR(10) NOT NULL,
    capacidad INT DEFAULT 4 CHECK (capacidad > 0),
    estado VARCHAR(10) DEFAULT 'libre' CHECK (estado IN ('libre', 'ocupada')),
    fechaCreacion DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_Mesas_Sedes FOREIGN KEY (idSede) REFERENCES TBL_Sedes(idSede),
    CONSTRAINT UK_Mesa_Sede UNIQUE (idSede, numeroMesa)
);
GO

-- Tabla de Roles
CREATE TABLE TBL_Roles (
    idRol INT PRIMARY KEY IDENTITY(1,1),
    nombreRol NVARCHAR(50) NOT NULL,
    fechaCreacion DATETIME2 DEFAULT GETDATE()
);
GO

-- Tabla de Menus
CREATE TABLE TBL_Menu (
    idMenu INT PRIMARY KEY IDENTITY(1,1),
    nombreMenu NVARCHAR(50) NOT NULL,
    icono NVARCHAR(50),
    url NVARCHAR(100)
);
GO

-- Tabla de Menu de Roles
CREATE TABLE TBL_MenuRol (
    idMenuRol INT PRIMARY KEY IDENTITY(1,1),
    idMenu INT NOT NULL,
    idRol INT NOT NULL,
    fechaCreacion DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_MenuRol_Menu FOREIGN KEY (idMenu) REFERENCES TBL_Menu(idMenu),
    CONSTRAINT FK_MenuRol_Rol FOREIGN KEY (idRol) REFERENCES TBL_Roles(idRol)
);
GO

-- Tabla de Usuarios
CREATE TABLE TBL_Usuarios (
    idUsuario INT PRIMARY KEY IDENTITY(1,1),
    numeroDocumento VARCHAR(20) UNIQUE NOT NULL,
    idSede INT NOT NULL,
    nombreUsuario NVARCHAR(100) NOT NULL,
    apellidoUsuario NVARCHAR(100) NOT NULL,
    correo VARCHAR(150) UNIQUE NOT NULL,
    idRol INT NOT NULL,
    usuarioLogin VARCHAR(50) UNIQUE NOT NULL,
    passwordHash VARCHAR(255) NOT NULL,
    estado VARCHAR(10) DEFAULT 'activo' CHECK (estado IN ('activo', 'inactivo')),
    fechaCreacion DATETIME2 DEFAULT GETDATE(),
    fechaUltimoLogin DATETIME2 NULL,
    CONSTRAINT FK_Usuarios_Sedes FOREIGN KEY (idSede) REFERENCES TBL_Sedes(idSede),
    CONSTRAINT FK_Usuarios_Roles FOREIGN KEY (idRol) REFERENCES TBL_Roles(idRol)
);
GO

-- Tabla de Inventario por Sede
CREATE TABLE TBL_Inventario (
    idInventario INT PRIMARY KEY IDENTITY(1,1),
    idSede INT NOT NULL,
    idProducto INT NOT NULL,
    cantidadDisponible INT NOT NULL DEFAULT 0 CHECK (cantidadDisponible >= 0),
    ultimaActualizacion DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_Inventario_Sedes FOREIGN KEY (idSede) REFERENCES TBL_Sedes(idSede),
    CONSTRAINT FK_Inventario_Producto FOREIGN KEY (idProducto) REFERENCES TBL_Producto(idProducto),
    CONSTRAINT UK_Producto_Sede UNIQUE (idSede, idProducto)
);
GO

-- Tabla de Numero de Pedido
CREATE TABLE TBL_Numero_Pedido (
    idNumeroPedido INT PRIMARY KEY IDENTITY(1,1),
    ultimoNumero INT NOT NULL DEFAULT 0 CHECK (ultimoNumero >= 0),
    prefijo VARCHAR(10) DEFAULT 'PED-',
    fechaCreacion DATETIME2 DEFAULT GETDATE()
);
GO

-- Tabla de Pedidos
CREATE TABLE TBL_Pedidos (
    idPedido INT PRIMARY KEY IDENTITY(1,1),
    idMesa INT NOT NULL,
    idUsuarioMesero INT NOT NULL,
    idSede INT NOT NULL,
    numeroPedido VARCHAR(50) UNIQUE NOT NULL,
    estadoPedido VARCHAR(10) DEFAULT 'activo' CHECK (estadoPedido IN ('activo', 'cancelado', 'pagado')),
    fechaApertura DATETIME2 DEFAULT GETDATE(),
    fechaCierre DATETIME2 NULL,
    totalPedido DECIMAL(12,2) DEFAULT 0 CHECK (totalPedido >= 0),
    metodoPago VARCHAR(10) DEFAULT 'efectivo' CHECK (metodoPago IN ('efectivo')),
    observaciones NVARCHAR(500),
    CONSTRAINT FK_Pedidos_Mesas FOREIGN KEY (idMesa) REFERENCES TBL_Mesas(idMesa),
    CONSTRAINT FK_Pedidos_Usuarios FOREIGN KEY (idUsuarioMesero) REFERENCES TBL_Usuarios(idUsuario),
    CONSTRAINT FK_Pedidos_Sedes FOREIGN KEY (idSede) REFERENCES TBL_Sedes(idSede)
);
GO

-- Tabla de Detalles de Pedido 
CREATE TABLE TBL_Detalle_Pedidos (
    idDetallePedido INT PRIMARY KEY IDENTITY(1,1),
    idPedido INT NOT NULL,
    idProducto INT NOT NULL,
    cantidad INT NOT NULL CHECK (cantidad > 0),
    precioUnitario DECIMAL(12,2) NOT NULL CHECK (precioUnitario >= 0),
    subtotal DECIMAL(12,2) NOT NULL CHECK (subtotal >= 0),
    fechaCreacion DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_DetallePedidos_Pedido FOREIGN KEY (idPedido) REFERENCES TBL_Pedidos(idPedido) ON DELETE CASCADE,
    CONSTRAINT FK_DetallePedidos_Producto FOREIGN KEY (idProducto) REFERENCES TBL_Producto(idProducto)
);
GO

-- Tabla de Movimientos de Inventario
CREATE TABLE TBL_Movimientos_Inventario (
    idMovimientoInventario INT PRIMARY KEY IDENTITY(1,1),
    idSede INT NOT NULL,
    idProducto INT NOT NULL,
    tipoMovimiento VARCHAR(10) CHECK (tipoMovimiento IN ('entrada', 'salida', 'ajuste')),
    cantidad INT NOT NULL,
    cantidadAnterior INT NOT NULL,
    cantidadNueva INT NOT NULL,
    idUsuario INT NOT NULL,
    fechaMovimiento DATETIME2 DEFAULT GETDATE(),
    observaciones NVARCHAR(500),
    CONSTRAINT FK_Movimientos_Sedes FOREIGN KEY (idSede) REFERENCES TBL_Sedes(idSede),
    CONSTRAINT FK_Movimientos_Productos FOREIGN KEY (idProducto) REFERENCES TBL_Producto(idProducto),
    CONSTRAINT FK_Movimientos_Usuarios FOREIGN KEY (idUsuario) REFERENCES TBL_Usuarios(idUsuario)
);
GO

-- Tabla de Sesiones de Usuario (CORREGIDA: Cambiar TEXT por NVARCHAR(MAX))
CREATE TABLE TBL_Sesiones_Usuario (
    idSesionUsuario VARCHAR(100) PRIMARY KEY,
    idUsuario INT NOT NULL,
    fechaInicio DATETIME2 DEFAULT GETDATE(),
    fechaUltimaActividad DATETIME2 DEFAULT GETDATE(),
    userAgent NVARCHAR(MAX), -- Cambiado de TEXT a NVARCHAR(MAX)
    estado VARCHAR(10) DEFAULT 'activa' CHECK (estado IN ('activa', 'expirada')),
    CONSTRAINT FK_Sesiones_Usuarios FOREIGN KEY (idUsuario) REFERENCES TBL_Usuarios(idUsuario)
);
GO

-------------------------------------------------------------------------
--------------------------- ÍNDICES OPTIMIZACIÓN -------------------------
-------------------------------------------------------------------------

CREATE INDEX IX_Usuarios_Sede ON TBL_Usuarios(idSede);
CREATE INDEX IX_Usuarios_Rol ON TBL_Usuarios(idRol);
CREATE INDEX IX_Pedidos_Mesa ON TBL_Pedidos(idMesa);
CREATE INDEX IX_Pedidos_Usuario ON TBL_Pedidos(idUsuarioMesero);
CREATE INDEX IX_Pedidos_Sede ON TBL_Pedidos(idSede);
CREATE INDEX IX_Pedidos_Estado ON TBL_Pedidos(estadoPedido);
CREATE INDEX IX_DetallePedidos_Pedido ON TBL_Detalle_Pedidos(idPedido);
CREATE INDEX IX_DetallePedidos_Producto ON TBL_Detalle_Pedidos(idProducto);
CREATE INDEX IX_Inventario_Sede_Producto ON TBL_Inventario(idSede, idProducto);
CREATE INDEX IX_Movimientos_Fecha ON TBL_Movimientos_Inventario(fechaMovimiento);
CREATE INDEX IX_Sesiones_Usuario ON TBL_Sesiones_Usuario(idUsuario);
CREATE INDEX IX_Sesiones_Estado ON TBL_Sesiones_Usuario(estado);
GO

-------------------------------------------------------------------------
------------------------------ TRIGGERS ---------------------------------
-------------------------------------------------------------------------

-- 1. Trigger para generar número de pedido automático ANTES de insertar
CREATE TRIGGER TR_Before_Insert_Pedido
ON TBL_Pedidos
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @nuevoNumero INT;
    DECLARE @numeroPedido VARCHAR(50);
    
    -- Obtener o crear el registro de números de pedido
    MERGE TBL_Numero_Pedido AS target
    USING (SELECT 1 AS dummy) AS source
    ON 1=1
    WHEN NOT MATCHED THEN
        INSERT (ultimoNumero) VALUES (0);
    
    -- Actualizar el último número de manera segura
    UPDATE TBL_Numero_Pedido 
    SET @nuevoNumero = ultimoNumero + 1,
        ultimoNumero = ultimoNumero + 1,
        fechaCreacion = GETDATE();
    
    SET @numeroPedido = 'PED-' + FORMAT(@nuevoNumero, '000000');
    
    -- Insertar el pedido con el número generado
    INSERT INTO TBL_Pedidos (
        idMesa, idUsuarioMesero, idSede, numeroPedido, 
        estadoPedido, fechaApertura, totalPedido, metodoPago, observaciones
    )
    SELECT 
        idMesa, idUsuarioMesero, idSede, @numeroPedido,
        estadoPedido, fechaApertura, totalPedido, metodoPago, observaciones
    FROM inserted;
END;
GO

-- 2. Trigger para actualizar estado de mesa DESPUÉS de insertar pedido
CREATE TRIGGER TR_After_Insert_Pedido
ON TBL_Pedidos
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE TBL_Mesas 
    SET estado = 'ocupada' 
    FROM TBL_Mesas m
    INNER JOIN inserted i ON m.idMesa = i.idMesa
    WHERE i.estadoPedido = 'activo';
END;
GO

-- 3. Trigger para manejar cambios de estado del pedido (pago/cancelación)
CREATE TRIGGER TR_After_Update_Pedido_Estado
ON TBL_Pedidos
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    IF UPDATE(estadoPedido)
    BEGIN
        -- Liberar mesa cuando el pedido se paga
        UPDATE TBL_Mesas 
        SET estado = 'libre' 
        FROM TBL_Mesas m
        INNER JOIN inserted i ON m.idMesa = i.idMesa
        INNER JOIN deleted d ON i.idPedido = d.idPedido
        WHERE i.estadoPedido = 'pagado' AND d.estadoPedido != 'pagado';
        
        -- Liberar mesa cuando se cancela el pedido
        UPDATE TBL_Mesas 
        SET estado = 'libre' 
        FROM TBL_Mesas m
        INNER JOIN inserted i ON m.idMesa = i.idMesa
        INNER JOIN deleted d ON i.idPedido = d.idPedido
        WHERE i.estadoPedido = 'cancelado' AND d.estadoPedido != 'cancelado';
    END
END;
GO

-- 4. Trigger para actualizar inventario cuando se CANCELA un pedido
CREATE TRIGGER TR_After_Update_Pedido_Cancelado
ON TBL_Pedidos
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    IF UPDATE(estadoPedido)
    BEGIN
        -- Solo procesar si el pedido fue cancelado
        IF EXISTS (SELECT 1 FROM inserted i 
                  INNER JOIN deleted d ON i.idPedido = d.idPedido 
                  WHERE i.estadoPedido = 'cancelado' AND d.estadoPedido != 'cancelado')
        BEGIN
            -- Registrar movimiento de inventario por cancelación
            INSERT INTO TBL_Movimientos_Inventario (
                idSede, idProducto, tipoMovimiento, cantidad, 
                cantidadAnterior, cantidadNueva, idUsuario, observaciones
            )
            SELECT 
                i.idSede,
                dp.idProducto,
                'entrada' as tipoMovimiento,
                dp.cantidad,
                inv.cantidadDisponible as cantidadAnterior,
                inv.cantidadDisponible + dp.cantidad as cantidadNueva,
                i.idUsuarioMesero,
                'Reintegro por cancelación de pedido: ' + i.numeroPedido
            FROM inserted i
            INNER JOIN deleted d ON i.idPedido = d.idPedido
            INNER JOIN TBL_Detalle_Pedidos dp ON i.idPedido = dp.idPedido
            INNER JOIN TBL_Inventario inv ON i.idSede = inv.idSede AND dp.idProducto = inv.idProducto
            WHERE i.estadoPedido = 'cancelado' AND d.estadoPedido != 'cancelado';
            
            -- Actualizar las cantidades en inventario
            UPDATE inv
            SET inv.cantidadDisponible = inv.cantidadDisponible + dp.cantidad,
                inv.ultimaActualizacion = GETDATE()
            FROM TBL_Inventario inv
            INNER JOIN inserted i ON inv.idSede = i.idSede
            INNER JOIN TBL_Detalle_Pedidos dp ON i.idPedido = dp.idPedido AND inv.idProducto = dp.idProducto
            WHERE i.estadoPedido = 'cancelado';
        END
    END
END;
GO

-- 5. Trigger para actualizar inventario cuando se AGREGA producto a pedido
CREATE TRIGGER TR_After_Insert_DetallePedido
ON TBL_Detalle_Pedidos
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Actualizar inventario cuando se agregan productos a un pedido activo
    INSERT INTO TBL_Movimientos_Inventario (
        idSede, idProducto, tipoMovimiento, cantidad, 
        cantidadAnterior, cantidadNueva, idUsuario, observaciones
    )
    SELECT 
        p.idSede,
        i.idProducto,
        'salida' as tipoMovimiento,
        i.cantidad,
        inv.cantidadDisponible as cantidadAnterior,
        inv.cantidadDisponible - i.cantidad as cantidadNueva,
        p.idUsuarioMesero,
        'Venta por pedido: ' + p.numeroPedido
    FROM inserted i
    INNER JOIN TBL_Pedidos p ON i.idPedido = p.idPedido
    INNER JOIN TBL_Inventario inv ON p.idSede = inv.idSede AND i.idProducto = inv.idProducto
    WHERE p.estadoPedido = 'activo';
    
    -- Actualizar las cantidades en inventario
    UPDATE inv
    SET inv.cantidadDisponible = inv.cantidadDisponible - i.cantidad,
        inv.ultimaActualizacion = GETDATE()
    FROM TBL_Inventario inv
    INNER JOIN inserted i ON inv.idProducto = i.idProducto
    INNER JOIN TBL_Pedidos p ON i.idPedido = p.idPedido AND inv.idSede = p.idSede
    WHERE p.estadoPedido = 'activo';
END;
GO

-------------------------------------------------------------------------
-------------------------------- VIEWS ----------------------------------
-------------------------------------------------------------------------

-- Vista para reportes de ventas con precios formateados
CREATE VIEW VW_Reporte_Ventas AS
SELECT 
    p.fechaApertura as fecha,
    p.numeroPedido,
    s.nombreSede as sede,
    pr.nombreProducto,
    c.nombreCategoria,
    dp.cantidad,
    pr.costo,
    dp.precioUnitario as precioVenta,
    (dp.precioUnitario - pr.costo) as margenGanancia,
    (dp.precioUnitario - pr.costo) * dp.cantidad as gananciaTotal,
    dp.subtotal,
    p.totalPedido,
    u.nombreUsuario + ' ' + u.apellidoUsuario as mesero
FROM TBL_Pedidos p
JOIN TBL_Detalle_Pedidos dp ON p.idPedido = dp.idPedido
JOIN TBL_Producto pr ON dp.idProducto = pr.idProducto
JOIN TBL_Categoria c ON pr.idCategoria = c.idCategoria
JOIN TBL_Sedes s ON p.idSede = s.idSede
JOIN TBL_Usuarios u ON p.idUsuarioMesero = u.idUsuario
WHERE p.estadoPedido = 'pagado';
GO

-- Vista para inventario actual con precios formateados
CREATE VIEW VW_Inventario_Actual AS
SELECT 
    s.nombreSede,
    p.nombreProducto,
    c.nombreCategoria,
    inv.cantidadDisponible,
    inv.ultimaActualizacion,
    p.precioVenta,
    p.costo
FROM TBL_Inventario inv
JOIN TBL_Sedes s ON inv.idSede = s.idSede
JOIN TBL_Producto p ON inv.idProducto = p.idProducto
JOIN TBL_Categoria c ON p.idCategoria = c.idCategoria
WHERE p.estado = 'activo' AND s.estado = 'activa';
GO

-- Vista para pedidos activos
CREATE VIEW VW_Pedidos_Activos AS
SELECT 
    p.idPedido,
    p.numeroPedido,
    m.numeroMesa,
    s.nombreSede,
    u.nombreUsuario + ' ' + u.apellidoUsuario as mesero,
    p.fechaApertura,
    p.totalPedido,
    p.observaciones
FROM TBL_Pedidos p
JOIN TBL_Mesas m ON p.idMesa = m.idMesa
JOIN TBL_Sedes s ON p.idSede = s.idSede
JOIN TBL_Usuarios u ON p.idUsuarioMesero = u.idUsuario
WHERE p.estadoPedido = 'activo';
GO

-- Vista para productos con precios formateados en pesos colombianos
CREATE VIEW VW_Productos_Precios_Formateados AS
SELECT 
    p.idProducto,
    p.nombreProducto,
    c.nombreCategoria,
    p.costo,
    p.precioVenta,
    -- Formatear precios para mostrar en formato colombiano
    FORMAT(p.costo, 'C', 'es-CO') as costoFormateado,
    FORMAT(p.precioVenta, 'C', 'es-CO') as precioVentaFormateado,
    (p.precioVenta - p.costo) as margen,
    FORMAT((p.precioVenta - p.costo), 'C', 'es-CO') as margenFormateado,
    p.estado
FROM TBL_Producto p
JOIN TBL_Categoria c ON p.idCategoria = c.idCategoria;
GO

-------------------------------------------------------------------------
--------------------------- PROCEDURES ----------------------------------
-------------------------------------------------------------------------

-- Procedimiento para formatear precios en pesos colombianos
CREATE PROCEDURE SP_FormatearPrecio
    @precio DECIMAL(12,2),
    @precioFormateado NVARCHAR(50) OUTPUT
AS
BEGIN
    SET @precioFormateado = FORMAT(@precio, 'C', 'es-CO');
END;
GO

-- Procedimiento para crear un nuevo pedido
CREATE PROCEDURE SP_CrearPedido
    @idMesa INT,
    @idUsuarioMesero INT,
    @observaciones NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @idSede INT;
    DECLARE @numeroPedido VARCHAR(50);
    DECLARE @idPedido INT;
    
    -- Obtener la sede de la mesa
    SELECT @idSede = idSede FROM TBL_Mesas WHERE idMesa = @idMesa;
    
    -- Generar el número de pedido manualmente (ya que el trigger es INSTEAD OF)
    DECLARE @nuevoNumero INT;
    
    -- Obtener o crear el registro de números de pedido
    MERGE TBL_Numero_Pedido AS target
    USING (SELECT 1 AS dummy) AS source
    ON 1=1
    WHEN NOT MATCHED THEN
        INSERT (ultimoNumero) VALUES (0);
    
    -- Actualizar el último número
    UPDATE TBL_Numero_Pedido 
    SET @nuevoNumero = ultimoNumero + 1,
        ultimoNumero = ultimoNumero + 1;
    
    SET @numeroPedido = 'PED-' + FORMAT(@nuevoNumero, '000000');
    
    -- Insertar directamente en la tabla (el trigger no interferirá)
    INSERT INTO TBL_Pedidos (
        idMesa, idUsuarioMesero, idSede, numeroPedido, 
        estadoPedido, fechaApertura, observaciones
    )
    VALUES (@idMesa, @idUsuarioMesero, @idSede, @numeroPedido, 'activo', GETDATE(), @observaciones);
    
    -- Obtener el ID del pedido creado
    SET @idPedido = SCOPE_IDENTITY();
    
    -- Actualizar el estado de la mesa
    UPDATE TBL_Mesas SET estado = 'ocupada' WHERE idMesa = @idMesa;
    
    -- Retornar el ID del pedido creado
    SELECT @idPedido as idPedido;
END;
GO

-- Procedimiento para agregar producto a pedido
CREATE PROCEDURE SP_AgregarProductoPedido
    @idPedido INT,
    @idProducto INT,
    @cantidad INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @precioVenta DECIMAL(12,2);
    DECLARE @subtotal DECIMAL(12,2);
    DECLARE @cantidadDisponible INT;
    DECLARE @idSede INT;
    DECLARE @estadoPedido VARCHAR(10);
    
    -- Verificar que el pedido existe y está activo
    SELECT @idSede = idSede, @estadoPedido = estadoPedido 
    FROM TBL_Pedidos 
    WHERE idPedido = @idPedido;
    
    IF @idSede IS NULL
    BEGIN
        RAISERROR('El pedido especificado no existe', 16, 1);
        RETURN;
    END
    
    IF @estadoPedido != 'activo'
    BEGIN
        RAISERROR('No se pueden agregar productos a un pedido cerrado o cancelado', 16, 1);
        RETURN;
    END
    
    -- Verificar stock disponible
    SELECT @cantidadDisponible = cantidadDisponible
    FROM TBL_Inventario 
    WHERE idSede = @idSede AND idProducto = @idProducto;
    
    IF @cantidadDisponible < @cantidad
    BEGIN
        RAISERROR('Stock insuficiente para el producto solicitado', 16, 1);
        RETURN;
    END
    
    -- Obtener el precio de venta del producto
    SELECT @precioVenta = precioVenta FROM TBL_Producto WHERE idProducto = @idProducto;
    
    SET @subtotal = @precioVenta * @cantidad;
    
    -- Insertar el detalle del pedido
    INSERT INTO TBL_Detalle_Pedidos (idPedido, idProducto, cantidad, precioUnitario, subtotal)
    VALUES (@idPedido, @idProducto, @cantidad, @precioVenta, @subtotal);
    
    -- Actualizar el total del pedido
    UPDATE TBL_Pedidos 
    SET totalPedido = (
        SELECT SUM(subtotal) FROM TBL_Detalle_Pedidos WHERE idPedido = @idPedido
    )
    WHERE idPedido = @idPedido;
END;
GO

-- Procedimiento para cerrar pedido (pagar)
CREATE PROCEDURE SP_CerrarPedido
    @idPedido INT,
    @metodoPago VARCHAR(10) = 'efectivo'
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE TBL_Pedidos 
    SET estadoPedido = 'pagado',
        metodoPago = @metodoPago,
        fechaCierre = GETDATE()
    WHERE idPedido = @idPedido;
END;
GO

-- Procedimiento para cancelar pedido
CREATE PROCEDURE SP_CancelarPedido
    @idPedido INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE TBL_Pedidos 
    SET estadoPedido = 'cancelado',
        fechaCierre = GETDATE()
    WHERE idPedido = @idPedido;
END;
GO

-- Procedimiento para obtener reporte de ventas por fecha con precios formateados
CREATE PROCEDURE SP_ReporteVentasPorFecha
    @fechaInicio DATE,
    @fechaFin DATE,
    @idSede INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        fecha,
        numeroPedido,
        sede,
        nombreProducto,
        nombreCategoria,
        cantidad,
        FORMAT(costo, 'C', 'es-CO') as costoFormateado,
        FORMAT(precioVenta, 'C', 'es-CO') as precioVentaFormateado,
        FORMAT(margenGanancia, 'C', 'es-CO') as margenGananciaFormateado,
        FORMAT(gananciaTotal, 'C', 'es-CO') as gananciaTotalFormateado,
        FORMAT(subtotal, 'C', 'es-CO') as subtotalFormateado,
        FORMAT(totalPedido, 'C', 'es-CO') as totalPedidoFormateado,
        mesero,
        -- Valores numéricos originales para cálculos
        costo,
        precioVenta,
        margenGanancia,
        gananciaTotal,
        subtotal,
        totalPedido
    FROM VW_Reporte_Ventas
    WHERE fecha >= @fechaInicio 
        AND fecha < DATEADD(DAY, 1, @fechaFin)
        AND (@idSede IS NULL OR sede = (SELECT nombreSede FROM TBL_Sedes WHERE idSede = @idSede))
    ORDER BY fecha DESC, numeroPedido;
END;
GO

-- Procedimiento para actualizar inventario manualmente
CREATE PROCEDURE SP_ActualizarInventario
    @idSede INT,
    @idProducto INT,
    @nuevaCantidad INT,
    @idUsuario INT,
    @observaciones NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @cantidadAnterior INT;
    DECLARE @diferencia INT;
    
    -- Obtener cantidad anterior
    SELECT @cantidadAnterior = cantidadDisponible 
    FROM TBL_Inventario 
    WHERE idSede = @idSede AND idProducto = @idProducto;
    
    -- Si no existe registro, crearlo
    IF @cantidadAnterior IS NULL
    BEGIN
        INSERT INTO TBL_Inventario (idSede, idProducto, cantidadDisponible)
        VALUES (@idSede, @idProducto, @nuevaCantidad);
        
        SET @cantidadAnterior = 0;
    END
    ELSE
    BEGIN
        UPDATE TBL_Inventario 
        SET cantidadDisponible = @nuevaCantidad,
            ultimaActualizacion = GETDATE()
        WHERE idSede = @idSede AND idProducto = @idProducto;
    END
    
    SET @diferencia = @nuevaCantidad - @cantidadAnterior;
    
    -- Registrar movimiento
    INSERT INTO TBL_Movimientos_Inventario (
        idSede, idProducto, tipoMovimiento, cantidad, 
        cantidadAnterior, cantidadNueva, idUsuario, observaciones
    )
    VALUES (
        @idSede, @idProducto, 
        CASE WHEN @diferencia > 0 THEN 'entrada' ELSE 'salida' END,
        ABS(@diferencia), 
        @cantidadAnterior, @nuevaCantidad, @idUsuario,
        COALESCE(@observaciones, 'Ajuste manual de inventario')
    );
END;
GO